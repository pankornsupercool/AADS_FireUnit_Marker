using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AADS.Views.ShowCategory
{
    public partial class Marker : UserControl
    {
        public Marker()
        {
            InitializeComponent();
            
        }

        private void btnShowAiport_Click(object sender, EventArgs e)
        {
            var Airportpage = new Views.Airport.main();
            panelShowDetail.Controls.Clear();
            panelShowDetail.Controls.Add(Airportpage);
        }

        private void btnShowCity_Click(object sender, EventArgs e)
        {
            var Citypage = new Views.City.main();
            panelShowDetail.Controls.Clear();
            panelShowDetail.Controls.Add(Citypage);
        }

        private void btnShowFixedPoint_Click(object sender, EventArgs e)
        {
           
        }

        private void btnShowFireUnit_Click(object sender, EventArgs e)
        {
            var Fireunitpage = new Views.FireUnit.main();
            panelShowDetail.Controls.Clear();
            panelShowDetail.Controls.Add(Fireunitpage);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var Landmarkpage = new Views.Landmark.main();
            panelShowDetail.Controls.Clear();
            panelShowDetail.Controls.Add(Landmarkpage);
        }

        private void btnShowVitalAsset_Click(object sender, EventArgs e)
        {
            var VitalAssetpage = new Views.VitalAsset.main();
            panelShowDetail.Controls.Clear();
            panelShowDetail.Controls.Add(VitalAssetpage);
        }

        private void btnShowWeaponBattery_Click(object sender, EventArgs e)
        {
            var WeaponsBatterypage = new Views.WeaponsBattery.main();
            panelShowDetail.Controls.Clear();
            panelShowDetail.Controls.Add(WeaponsBatterypage);
        }
    }
}
