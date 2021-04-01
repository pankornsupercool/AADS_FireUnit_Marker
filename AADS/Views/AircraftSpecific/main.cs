using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AADS.Views.AircraftSpecific
{
    public partial class main : Form
    {
        private bool mouseDown;
        private Point lastLocation;
        public main()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void panelTop_Paint(object sender, PaintEventArgs e)
        {

        }

        private void windowDragable_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDown = true;
            lastLocation = e.Location;
        }

        private void windowDragable_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                this.Location = new Point(
                    (this.Location.X - lastLocation.X) + e.X, (this.Location.Y - lastLocation.Y) + e.Y);

                this.Update();
            }
        }

        private void windowDragable_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private void checkBoxShowFt_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBoxEnableFt_CheckedChanged(object sender, EventArgs e)
        {
            groupBox1.Enabled = checkBoxEnableFt.Checked;
        }
    }
}
