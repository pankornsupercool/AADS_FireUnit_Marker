using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace AADS
{
    public class RadarClient
    {
        public static Socket ClientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private static byte[] buffer;
        public static bool ConnectToServer(IPAddress ipAddr, int port)
        {
            int attempts = 0;
            bool connected = false;
            while (!ClientSocket.Connected && attempts < 1)
            {
                try
                {
                    attempts++;
                    Debug.WriteLine("Connection attempt " + attempts);
                    ClientSocket.Connect(ipAddr, port);
                    connected = true;
                }
                catch (SocketException e)
                {
                    Debug.WriteLine(e);
                }
            }
            if (connected)
            {
                Debug.WriteLine("Connected");
                SendString(JsonSerializer.Serialize(new RadarCommand
                {
                    Feature = RadarFeature.Track,
                    Operation = "GET"
                }));
                buffer = new byte[4];
                ClientSocket.BeginReceive(buffer, 0, 4, SocketFlags.None, ReceiveCallback, ClientSocket);
            }
            return connected;
        }

        private static void ReceiveCallback(IAsyncResult AR)
        {
            Socket current = (Socket)AR.AsyncState;
            int received;
            MainForm form = MainForm.GetInstance();
            TrackManager trackHandler = form.trackHandler;
            try
            {
                received = current.EndReceive(AR);
                int length = BitConverter.ToInt32(buffer, 0);
                buffer = new byte[length];
                current.Receive(buffer, 0, length, SocketFlags.None);
                byte[] dataSent = new byte[length];
                Array.Copy(buffer, dataSent, length);
                string text = Encoding.ASCII.GetString(dataSent);
                var command = JsonSerializer.Deserialize<RadarCommand>(text);
                if (command.Feature == RadarFeature.Track)
                {
                    if (command.Operation == "ADD" || command.Operation == "UPDATE")
                    {
                        var args = JsonSerializer.Deserialize<TrackCommandArgs>(command.Args.ToString());
                        var track = args.Track;
                        trackHandler.AddTrack(track);
                    }
                    else if (command.Operation == "REMOVE")
                    {
                        var args = JsonSerializer.Deserialize<TrackCommandArgs>(command.Args.ToString());
                        var track = args.Track;
                        trackHandler.RemoveTrack(track.Key);
                    }
                    else if (command.Operation == "CLEAR")
                    {
                        trackHandler.Clear();
                    }
                    else if (command.Operation == "SYNC")
                    {
                        var args = JsonSerializer.Deserialize<TrackSyncArgs>(command.Args.ToString());
                        var tracks = args.Tracks;
                        trackHandler.Clear();
                        foreach (var track in tracks)
                        {
                            trackHandler.AddTrack(track);
                        }
                    }
                }
                buffer = new byte[4];
                ClientSocket.BeginReceive(buffer, 0, 4, SocketFlags.None, ReceiveCallback, ClientSocket);
            }
            catch (SocketException e)
            {

            }
            catch (ObjectDisposedException e)
            {

            }
        }

        /// <summary>
        /// Close socket and exit program.
        /// </summary>
        public static void Exit()
        {
            SendString(JsonSerializer.Serialize(new RadarCommand
            {
                Operation = "EXIT"
            })); // Tell the server we are exiting
            ClientSocket.Shutdown(SocketShutdown.Both);
            ClientSocket.Close();
        }

        /// <summary>
        /// Sends a string to the server with ASCII encoding.
        /// </summary>
        public static void SendString(string text)
        {
            byte[] buffer = Encoding.ASCII.GetBytes(text);
            var header = BitConverter.GetBytes(buffer.Length);
            ClientSocket.Send(header, 0, header.Length, SocketFlags.None);
            ClientSocket.BeginSend(buffer, 0, buffer.Length, 0, SendCallback, ClientSocket);
        }
        private static void SendCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.
                Socket handler = (Socket)ar.AsyncState;

                // Complete sending the data to the remote device.
                try
                {
                    handler.EndSend(ar);
                }
                catch (SocketException)
                {

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}
