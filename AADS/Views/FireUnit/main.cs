using GMap.NET;
using GMap.NET.WindowsForms;
using Net_GmapMarkerWithLabel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Demo.WindowsForms.Forms;
using GMap.NET.WindowsForms.Markers;

namespace AADS.Views.FireUnit
{
    public partial class MainFireunit : UserControl
    {
        //public static List<FireunitObject> objectList = new List<FireunitObject>();

        // Object to add
        string[] batteryID = { "Tiger", "Lion", "Snake", "Phoenix" };
        string[] number = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12" };
        string[] type = { "Infomation unit", "Education unit" };
        public static int temp = 0;
        public string status;
        public string typeStatus;
        public Bitmap bmpMarker;
        public Bitmap newImage;
        //
        public FireunitEvent handler;
        private static MainForm mainInstance = MainForm.GetInstance();
        private GMapControl map = mainInstance.GetmainMap();
        public static Dictionary<GMapMarker, MarkerDetail> markers = new Dictionary<GMapMarker, MarkerDetail>();
        public static MainFireunit InstanceFireUnit;

        public static event EventHandler DeleteEvent;
        public static event EventHandler EditEvent;
        public MainFireunit()
        {
            FireUnitDelete delete = new FireUnitDelete();
            handler = FireunitEvent.Instance;
            InitializeComponent();
        }

        public static MainFireunit GetInstanceza()
        {
            return InstanceFireUnit;
        }

        public Button getButton()
        {
            return btnDelete;
        }
        public Bitmap ResizeBitmap(Bitmap bmpMarker, int width, int height)
        {
            Bitmap result = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(result))
            {
                g.DrawImage(bmpMarker, 0, 0, width, height);
            }

            return result;
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            RadioButton_Process();

            Images_Process();

            MarkerAdd_Process();

        }

        GMapMarker CurrentMarkerSelected;
        int eventInt;
        int eventTag;
        private void main_Load(object sender, EventArgs e)
        {
            //FireUnitTag.tag.Add(0);
            StartUp();
            handler.onFireunitChanged += new FireunitChanged(Changed);
            //handler.onFireunitChanged2 += new FireunitCganged2(Changed2);
            mainInstance.RaiseDelete += MainInstance_RaiseDelete;
            mainInstance.HideDelete += MainInstance_HideDelete;
            mainInstance.ClearScreen += MainInstance_ClearScreen;
            mainInstance.CurrentMarker += MainInstance_CurrentMarker;
            mainInstance.isSelected += MainInstance_isSelected;
            mainInstance.TagNumber += MainInstance_TagNumber;
        }

        private void MainInstance_TagNumber(object sender, int e)
        {
            //MessageBox.Show(e.ToString(), "Send Tag Number here ");
            eventTag = e;
        }

        private void MainInstance_isSelected(object sender, int e)
        {
            //MessageBox.Show(e.ToString(), "Send index of tag here ( isSelected ) ");
            eventInt = e;
        }

        private void MainInstance_CurrentMarker(object sender, GMapMarker e)
        {
            CurrentMarkerSelected = e;
            //MessageBox.Show(CurrentMarkerSelected.Tag.ToString(),"za");
        }

        private void MainInstance_ClearScreen(object sender, EventArgs e)
        {
            //
        }

        private void MainInstance_HideDelete(object sender, EventArgs e)
        {
            
            btnDelete.Enabled = false;
            btnEdit.Enabled = false;
            cbbBatteryId.Text = "";
            cbbNumber.Text = "";
            cbbType.Text = "";
            txtLocation.Text = "";
            txtDetail.Text = "";
            status = "";
            rdoOp.Checked = false;
            rdoLimited.Checked = false;
            rdoNonOp.Checked = false;

            //var item = objectList.SingleOrDefault(x => x.Tag == e);
            //if (item != null)
            //{
            //    MessageBox.Show("remove");
            //    objectList.Remove(item);
            //}
        }

        private void MainInstance_RaiseDelete(object sender, int e)
        {
            //MessageBox.Show(e.ToString(),"Raise Delete event index");

            cbbBatteryId.Text = "";
            cbbNumber.Text = "";
            cbbType.Text = "";
            txtLocation.Text = "";
            txtDetail.Text = "";
            status = "";
            rdoOp.Checked = false;
            rdoLimited.Checked = false;
            rdoNonOp.Checked = false;

            //foreach (var items in FireunitList.Detail)
            //{
            //    MessageBox.Show(items.ToString(), "LIST ITEMS IN RAISE");
            //}

            btnDelete.Enabled = true;
            btnEdit.Enabled = true;
            cbbBatteryId.Text = FireunitList.BatteryID[e];
            cbbNumber.Text = FireunitList.Number[e];
            cbbType.Text = FireunitList.Type[e];
            txtLocation.Text = FireunitList.Location[e];
            txtDetail.Text = FireunitList.Detail[e];
            status = FireunitList.Status[e];

            if (status == "OP")
            {
                rdoOp.Checked = true;
            }
            else if (status == "Limited")
            {
                rdoLimited.Checked = true;
            }
            else if (status == "NonOP")
            {
                rdoNonOp.Checked = true;
            }

        }

        private void StartUp()
        {
            foreach (var items in batteryID)
            {
                cbbBatteryId.Items.Add(items);
            }
            foreach (var items in number)
            {
                cbbNumber.Items.Add(items);
            }
            foreach (var items in type)
            {
                cbbType.Items.Add(items);
            }

        }
        private void Changed(FireunitEventArgs args)
        {
            txtLocation.Text = args.LatLng;
        }

        //private void Changed2(FireunitEventArgs2 args2)
        //{
        //    txtLocation.Text = args2.Location;
        //    txtDetail.Text = args2.Detail;
        //    btnDelete.Enabled = true;
        //    btnEdit.Enabled = true;
        //}


        private void btnDelete_Click(object sender, EventArgs e)
        {
            DeleteEvent?.Invoke(this, EventArgs.Empty);
        }


        private void btnEdit_Click(object sender, EventArgs e)
        {
            //MessageBox.Show(eventTag.ToString());
            //MessageBox.Show(eventInt.ToString());
            //MessageBox.Show(CurrentMarkerSelected.Tag.ToString());

            RadioButton_Process();

            Images_Process();

            bmpMarker = ResizeBitmap(bmpMarker, 30, 30);

            GMapOverlay markerOverlay = MainForm.GetInstance().GetOverlay("markersP");
            markerOverlay.Markers.Remove(CurrentMarkerSelected);
            PointLatLng point = new PointLatLng(GlobalFireUnit.Lat, GlobalFireUnit.Lng);
            GMapMarker marker = new GMarkerGoogle(point, bmpMarker);


            string full_detail = "BatteryID : " + cbbBatteryId.SelectedItem.ToString() + System.Environment.NewLine + "Number : " + cbbNumber.SelectedItem.ToString() + System.Environment.NewLine + "Type : " + cbbType.SelectedItem.ToString()
            + System.Environment.NewLine + "Status : " + status.ToString() + System.Environment.NewLine + "Detail : " + txtDetail.Text;
            string BatteryId = cbbBatteryId.SelectedItem.ToString();
            string Number = cbbNumber.SelectedItem.ToString();
            string Type = cbbType.SelectedItem.ToString();
            string Location = point.Lat.ToString() + ", " + point.Lng.ToString();
            string Detail = txtDetail.Text;
            string Status = status;

            try
            {
                FireunitList.BatteryID[eventInt] = BatteryId;
                FireunitList.Number[eventInt] = Number;
                FireunitList.Type[eventInt] = Type;
                FireunitList.Location[eventInt] = Location;
                FireunitList.Detail[eventInt] = Detail;
                FireunitList.Status[eventInt] = Status;
            }
            finally
            {
                //
                //foreach(var items in FireunitList.BatteryID)
                //{
                //    MessageBox.Show(items.ToString(),"LIST ITEMS");
                //}
            }


            
            marker.Tag = eventTag;
            marker.ToolTipText = full_detail;
            marker.ToolTipMode = MarkerTooltipMode.OnMouseOver;
            markerOverlay.Markers.Add(marker);
            map.Overlays.Add(markerOverlay);


            //cbbBatteryId.Text = "";
            //cbbNumber.Text = "";
            //cbbType.Text = "";
            //txtLocation.Text = "";
            //txtDetail.Text = "";
            //status = "";
            //rdoOp.Checked = false;
            //rdoLimited.Checked = false;
            //rdoNonOp.Checked = false;

            // EditEvent?.Invoke(this, EventArgs.Empty);
        }

        public void RadioButton_Process()
        {
            if (rdoLimited.Checked == true)
            {
                status = rdoLimited.Text;
            }
            else if (rdoNonOp.Checked == true)
            {
                status = rdoNonOp.Text;
            }
            else if (rdoOp.Checked == true)
            {
                status = rdoOp.Text;
            }
        }

        public void Images_Process()
        {
            if (cbbType.SelectedIndex == 0)
            {
                bmpMarker = (Bitmap)Image.FromFile("images/018-information.png");
            }
            else if (cbbType.SelectedIndex == 1)
            {
                bmpMarker = (Bitmap)Image.FromFile("images/011-education.png");
            }
        }

        public void MarkerAdd_Process()
        {
            if (txtLocation.Text == "" || txtDetail.Text == "" || cbbBatteryId.SelectedItem == null || cbbNumber.SelectedItem == null || cbbType.SelectedItem == null || status == null)
            {
                MessageBox.Show("Location and Detail needed");
            }
            else
            {
                try
                {
                    newImage = ResizeBitmap(bmpMarker, 40, 40);
                    PointLatLng point = new PointLatLng(GlobalFireUnit.Lat, GlobalFireUnit.Lng);
                    var marker = new GMarkerGoogle(point, newImage);
                    GMapOverlay overlay = MainForm.GetInstance().GetOverlay("markersP");
                    MarkerDetail detail = new MarkerDetail();

                    string full_detail = "BatteryID : " + cbbBatteryId.SelectedItem.ToString() + System.Environment.NewLine + "Number : " + cbbNumber.SelectedItem.ToString() + System.Environment.NewLine + "Type : " + cbbType.SelectedItem.ToString()
                        + System.Environment.NewLine + "Status : " + status.ToString() + System.Environment.NewLine + "Detail : " + txtDetail.Text;
                    string BatteryId = cbbBatteryId.SelectedItem.ToString();
                    string Number = cbbNumber.SelectedItem.ToString();
                    string Type = cbbType.SelectedItem.ToString();
                    string Location = point.Lat.ToString() + ", " + point.Lng.ToString();
                    string Detail = txtDetail.Text;
                    string Status = status;




                    //detail.id = temp;
                    ////detail.name = txtDetail.Text + System.Environment.NewLine + "za";
                    //detail.name = full_detail;


                    markers.Add(marker, detail);
                    marker.Tag = temp += 1;
                    marker.ToolTipText = full_detail;
                    marker.ToolTipMode = MarkerTooltipMode.OnMouseOver;

                    FireunitList.BatteryID.Add(BatteryId);
                    FireunitList.Number.Add(Number);
                    FireunitList.Type.Add(Type);
                    FireunitList.Location.Add(Location);
                    FireunitList.Detail.Add(Detail);
                    FireunitList.Status.Add(Status);
                    FireunitList.Tag.Add(Convert.ToInt32(marker.Tag));




                    //objectList.Add(new FireunitObject());
                    //objectList[tempOb].BatteryID = BatteryId;
                    //objectList[tempOb].Number = Number;
                    //objectList[tempOb].Type = Type;
                    //objectList[tempOb].Location = Location;
                    //objectList[tempOb].Detail = Detail;
                    //objectList[tempOb].Status = Status;
                    //objectList[tempOb].Tag = temp;


                    //foreach (FireunitObject items in objectList)
                    //{
                    //    MessageBox.Show(items.BatteryID);
                    //}

                    overlay.Markers.Add(marker);
                    map.Overlays.Add(overlay);

                    cbbBatteryId.SelectedItem = null;
                    cbbNumber.SelectedItem = null;
                    cbbType.SelectedItem = null;
                    txtLocation.Text = "";
                    txtDetail.Text = "";
                    rdoLimited.Checked = false;
                    rdoNonOp.Checked = false;
                    rdoOp.Checked = false;
                }
                catch (Exception E)
                {
                    MessageBox.Show(E.Message);
                }

                //var Mykey = markers.FirstOrDefault(x => x.Value.id == 0).Key;
                //MessageBox.Show(Mykey.ToString());
            }
        }
    }

}
