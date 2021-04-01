using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AADS.Views.WeatherForecast
{
    public partial class main : Form
    {
        public main()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            panelActive.Height = btn.Height;
            panelActive.Top = btn.Top;
            controlContainer.Controls.Clear();
            UserControl ctr = null;
            if (btn == btnAdd)
            {
                ctr = new add();
            }
            else if (btn == btnSum)
            {
                ctr = new summary();
            }
            controlContainer.Controls.Add(ctr);
        }

        private void main_Load(object sender, EventArgs e)
        {
            controlContainer.Controls.Clear();
            add a = new add();
            controlContainer.Controls.Add(a);
        }
    }
}
