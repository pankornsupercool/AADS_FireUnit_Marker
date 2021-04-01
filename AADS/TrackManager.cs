using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AADS
{
    public delegate void TrackClear(List<TrackData> tracks, List<GMarkerTrack> markers, TrackEventArgs e);
    public delegate void TrackAdd(TrackData item, TrackEventArgs e);
    public delegate void TrackUpdate(TrackData item, TrackEventArgs e);
    public delegate void TrackRemove(TrackData item, TrackEventArgs e);
    public class TrackEventArgs : EventArgs
    {
        public DateTime TransactionTime { get; set; }
    }
    public class TrackManager
    {
        private Dictionary<string, TrackData> tracks = new Dictionary<string, TrackData>();
        public Dictionary<string, GMarkerTrack> trackMarkers = new Dictionary<string, GMarkerTrack>();
        public event TrackClear OnTrackClear;
        public event TrackAdd OnTrackAdd;
        public event TrackUpdate OnTrackUpdate;
        public event TrackRemove OnTrackRemove;
        public void Clear()
        {
            List<TrackData> tracks = new List<TrackData>(this.tracks.Values);
            List<GMarkerTrack> markers = new List<GMarkerTrack>(trackMarkers.Values);
            OnTrackClear?.Invoke(tracks, markers, new TrackEventArgs
            {
                TransactionTime = DateTime.Now
            });
            this.tracks.Clear();
            trackMarkers.Clear();
        }
        public List<TrackData> GetTracks()
        {
            return new List<TrackData>(tracks.Values);
        }
        public TrackData GetTrack(string key)
        {
            if (tracks.ContainsKey(key))
            {
                return tracks[key];
            }
            return null;
        }
        public void AddTrack(TrackData track)
        {
            var key = track.Key;
            if (tracks.ContainsKey(key))
            {
                tracks[key] = track;
                OnTrackUpdate?.Invoke(track, new TrackEventArgs
                {
                    TransactionTime = DateTime.Now
                });
            }
            else
            {
                tracks.Add(key, track);
                OnTrackAdd?.Invoke(track, new TrackEventArgs
                {
                    TransactionTime = DateTime.Now
                });
            }
        }
        public void RemoveTrack(string key)
        {
            if (tracks.ContainsKey(key))
            {
                var track = tracks[key];
                tracks.Remove(key);
                OnTrackRemove?.Invoke(track, new TrackEventArgs
                {
                    TransactionTime = DateTime.Now
                });
            }
        }
    }
}
