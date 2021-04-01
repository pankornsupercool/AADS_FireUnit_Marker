
namespace AADS.Views.DailyAirMovementReport
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
            this.Item_No = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Date_Time = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Details = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Remarks = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(546, 501);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(131, 39);
            this.btnSend.TabIndex = 109;
            this.btnSend.Text = "Send Selected Rows";
            this.btnSend.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(161, 168);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 13);
            this.label2.TabIndex = 108;
            this.label2.Text = "Message Type";
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Location = new System.Drawing.Point(398, 168);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(68, 17);
            this.radioButton1.TabIndex = 107;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "Outgoing";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // radioAD
            // 
            this.radioAD.AutoSize = true;
            this.radioAD.Location = new System.Drawing.Point(324, 168);
            this.radioAD.Name = "radioAD";
            this.radioAD.Size = new System.Drawing.Size(68, 17);
            this.radioAD.TabIndex = 106;
            this.radioAD.TabStop = true;
            this.radioAD.Text = "Incoming";
            this.radioAD.UseVisualStyleBackColor = true;
            // 
            // radioOP
            // 
            this.radioOP.AutoSize = true;
            this.radioOP.Location = new System.Drawing.Point(244, 168);
            this.radioOP.Name = "radioOP";
            this.radioOP.Size = new System.Drawing.Size(74, 17);
            this.radioOP.TabIndex = 105;
            this.radioOP.TabStop = true;
            this.radioOP.Text = "Inprogress";
            this.radioOP.UseVisualStyleBackColor = true;
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(728, 501);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(47, 39);
            this.btnClose.TabIndex = 104;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnPrint
            // 
            this.btnPrint.Location = new System.Drawing.Point(164, 438);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(47, 39);
            this.btnPrint.TabIndex = 103;
            this.btnPrint.Text = "Print";
            this.btnPrint.UseVisualStyleBackColor = true;
            // 
            // btnSum
            // 
            this.btnSum.Location = new System.Drawing.Point(164, 393);
            this.btnSum.Name = "btnSum";
            this.btnSum.Size = new System.Drawing.Size(47, 39);
            this.btnSum.TabIndex = 102;
            this.btnSum.Text = "Sum";
            this.btnSum.UseVisualStyleBackColor = true;
            // 
            // btnView
            // 
            this.btnView.Location = new System.Drawing.Point(164, 348);
            this.btnView.Name = "btnView";
            this.btnView.Size = new System.Drawing.Size(47, 39);
            this.btnView.TabIndex = 101;
            this.btnView.Text = "View";
            this.btnView.UseVisualStyleBackColor = true;
            // 
            // btnMod
            // 
            this.btnMod.Location = new System.Drawing.Point(164, 303);
            this.btnMod.Name = "btnMod";
            this.btnMod.Size = new System.Drawing.Size(47, 39);
            this.btnMod.TabIndex = 100;
            this.btnMod.Text = "Mod";
            this.btnMod.UseVisualStyleBackColor = true;
            // 
            // btnDel
            // 
            this.btnDel.Location = new System.Drawing.Point(164, 258);
            this.btnDel.Name = "btnDel";
            this.btnDel.Size = new System.Drawing.Size(47, 39);
            this.btnDel.TabIndex = 99;
            this.btnDel.Text = "Del";
            this.btnDel.UseVisualStyleBackColor = true;
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(164, 213);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(47, 39);
            this.btnAdd.TabIndex = 98;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Item_No,
            this.Date_Time,
            this.Details,
            this.Remarks});
            this.dataGridView1.Location = new System.Drawing.Point(242, 202);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(435, 275);
            this.dataGridView1.TabIndex = 97;
            // 
            // Item_No
            // 
            this.Item_No.HeaderText = "Item No";
            this.Item_No.Name = "Item_No";
            // 
            // Date_Time
            // 
            this.Date_Time.HeaderText = "Date & Time";
            this.Date_Time.Name = "Date_Time";
            // 
            // Details
            // 
            this.Details.HeaderText = "Details";
            this.Details.Name = "Details";
            // 
            // Remarks
            // 
            this.Remarks.HeaderText = "Remarks";
            this.Remarks.Name = "Remarks";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(835, 45);
            this.panel1.TabIndex = 96;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(588, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(163, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Daily Air Movement Report Table";
            // 
            // main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(835, 580);
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
        private System.Windows.Forms.DataGridViewTextBoxColumn Item_No;
        private System.Windows.Forms.DataGridViewTextBoxColumn Date_Time;
        private System.Windows.Forms.DataGridViewTextBoxColumn Details;
        private System.Windows.Forms.DataGridViewTextBoxColumn Remarks;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
    }
}
