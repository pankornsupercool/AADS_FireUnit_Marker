
namespace AADS.Views.FreeTextMessage
{
    partial class main
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnSend = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.radioAD = new System.Windows.Forms.RadioButton();
            this.radioOP = new System.Windows.Forms.RadioButton();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnPrint = new System.Windows.Forms.Button();
            this.btnSum = new System.Windows.Forms.Button();
            this.btnView = new System.Windows.Forms.Button();
            this.btnMod = new System.Windows.Forms.Button();
            this.btnDel = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.Site_Index = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Functionnal_Mode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Comm_Status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Operational_Status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Local_Weapons_CtrlStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Rgnl_Weapons_CtrlStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Sim_Local_Weapons_CtrlStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SIM_Rgnl_Weapons_CtrlStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(517, 448);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(131, 39);
            this.btnSend.TabIndex = 95;
            this.btnSend.Text = "Send Selected Rows";
            this.btnSend.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(132, 115);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 13);
            this.label2.TabIndex = 94;
            this.label2.Text = "Message Type";
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Location = new System.Drawing.Point(369, 115);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(68, 17);
            this.radioButton1.TabIndex = 93;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "Outgoing";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // radioAD
            // 
            this.radioAD.AutoSize = true;
            this.radioAD.Location = new System.Drawing.Point(295, 115);
            this.radioAD.Name = "radioAD";
            this.radioAD.Size = new System.Drawing.Size(68, 17);
            this.radioAD.TabIndex = 92;
            this.radioAD.TabStop = true;
            this.radioAD.Text = "Incoming";
            this.radioAD.UseVisualStyleBackColor = true;
            // 
            // radioOP
            // 
            this.radioOP.AutoSize = true;
            this.radioOP.Location = new System.Drawing.Point(215, 115);
            this.radioOP.Name = "radioOP";
            this.radioOP.Size = new System.Drawing.Size(74, 17);
            this.radioOP.TabIndex = 91;
            this.radioOP.TabStop = true;
            this.radioOP.Text = "Inprogress";
            this.radioOP.UseVisualStyleBackColor = true;
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(699, 448);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(47, 39);
            this.btnClose.TabIndex = 90;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnPrint
            // 
            this.btnPrint.Location = new System.Drawing.Point(135, 385);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(47, 39);
            this.btnPrint.TabIndex = 89;
            this.btnPrint.Text = "Print";
            this.btnPrint.UseVisualStyleBackColor = true;
            // 
            // btnSum
            // 
            this.btnSum.Location = new System.Drawing.Point(135, 340);
            this.btnSum.Name = "btnSum";
            this.btnSum.Size = new System.Drawing.Size(47, 39);
            this.btnSum.TabIndex = 88;
            this.btnSum.Text = "Sum";
            this.btnSum.UseVisualStyleBackColor = true;
            // 
            // btnView
            // 
            this.btnView.Location = new System.Drawing.Point(135, 295);
            this.btnView.Name = "btnView";
            this.btnView.Size = new System.Drawing.Size(47, 39);
            this.btnView.TabIndex = 87;
            this.btnView.Text = "View";
            this.btnView.UseVisualStyleBackColor = true;
            // 
            // btnMod
            // 
            this.btnMod.Location = new System.Drawing.Point(135, 250);
            this.btnMod.Name = "btnMod";
            this.btnMod.Size = new System.Drawing.Size(47, 39);
            this.btnMod.TabIndex = 86;
            this.btnMod.Text = "Mod";
            this.btnMod.UseVisualStyleBackColor = true;
            // 
            // btnDel
            // 
            this.btnDel.Location = new System.Drawing.Point(135, 205);
            this.btnDel.Name = "btnDel";
            this.btnDel.Size = new System.Drawing.Size(47, 39);
            this.btnDel.TabIndex = 85;
            this.btnDel.Text = "Del";
            this.btnDel.UseVisualStyleBackColor = true;
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(135, 160);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(47, 39);
            this.btnAdd.TabIndex = 84;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Site_Index,
            this.Functionnal_Mode,
            this.Comm_Status,
            this.Operational_Status,
            this.Local_Weapons_CtrlStatus,
            this.Rgnl_Weapons_CtrlStatus,
            this.Sim_Local_Weapons_CtrlStatus,
            this.SIM_Rgnl_Weapons_CtrlStatus});
            this.dataGridView1.Location = new System.Drawing.Point(213, 149);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(842, 275);
            this.dataGridView1.TabIndex = 83;
            // 
            // Site_Index
            // 
            this.Site_Index.HeaderText = "Site Index";
            this.Site_Index.Name = "Site_Index";
            // 
            // Functionnal_Mode
            // 
            this.Functionnal_Mode.HeaderText = "Functional Mode";
            this.Functionnal_Mode.Name = "Functionnal_Mode";
            // 
            // Comm_Status
            // 
            this.Comm_Status.HeaderText = "Comm Status";
            this.Comm_Status.Name = "Comm_Status";
            // 
            // Operational_Status
            // 
            this.Operational_Status.HeaderText = "Operational Status";
            this.Operational_Status.Name = "Operational_Status";
            // 
            // Local_Weapons_CtrlStatus
            // 
            this.Local_Weapons_CtrlStatus.HeaderText = "Local Weapons";
            this.Local_Weapons_CtrlStatus.Name = "Local_Weapons_CtrlStatus";
            // 
            // Rgnl_Weapons_CtrlStatus
            // 
            this.Rgnl_Weapons_CtrlStatus.HeaderText = "Rgnl.Weapons Ctrl.Status";
            this.Rgnl_Weapons_CtrlStatus.Name = "Rgnl_Weapons_CtrlStatus";
            // 
            // Sim_Local_Weapons_CtrlStatus
            // 
            this.Sim_Local_Weapons_CtrlStatus.HeaderText = "Sim Local Weapons Ctrl.Status";
            this.Sim_Local_Weapons_CtrlStatus.Name = "Sim_Local_Weapons_CtrlStatus";
            // 
            // SIM_Rgnl_Weapons_CtrlStatus
            // 
            this.SIM_Rgnl_Weapons_CtrlStatus.HeaderText = "Sim Rgnl.Weapons Ctrl.Status";
            this.SIM_Rgnl_Weapons_CtrlStatus.Name = "SIM_Rgnl_Weapons_CtrlStatus";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1083, 45);
            this.panel1.TabIndex = 82;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(549, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(128, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Free Text Message Table";
            // 
            // main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1083, 496);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.radioButton1);
            this.Controls.Add(this.radioAD);
            this.Controls.Add(this.radioOP);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnPrint);
            this.Controls.Add(this.btnSum);
            this.Controls.Add(this.btnView);
            this.Controls.Add(this.btnMod);
            this.Controls.Add(this.btnDel);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.RadioButton radioAD;
        private System.Windows.Forms.RadioButton radioOP;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Button btnSum;
        private System.Windows.Forms.Button btnView;
        private System.Windows.Forms.Button btnMod;
        private System.Windows.Forms.Button btnDel;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Site_Index;
        private System.Windows.Forms.DataGridViewTextBoxColumn Functionnal_Mode;
        private System.Windows.Forms.DataGridViewTextBoxColumn Comm_Status;
        private System.Windows.Forms.DataGridViewTextBoxColumn Operational_Status;
        private System.Windows.Forms.DataGridViewTextBoxColumn Local_Weapons_CtrlStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn Rgnl_Weapons_CtrlStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn Sim_Local_Weapons_CtrlStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn SIM_Rgnl_Weapons_CtrlStatus;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
    }
}
