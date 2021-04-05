
namespace AADS.Views.FireUnit
{
    partial class MainFireunit
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.txtDetail = new System.Windows.Forms.TextBox();
            this.detailLabel = new System.Windows.Forms.Label();
            this.btnSubmit = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtLocation = new System.Windows.Forms.TextBox();
            this.cbbType = new System.Windows.Forms.ComboBox();
            this.cbbNumber = new System.Windows.Forms.ComboBox();
            this.cbbBatteryId = new System.Windows.Forms.ComboBox();
            this.rdoNonOp = new System.Windows.Forms.RadioButton();
            this.rdoLimited = new System.Windows.Forms.RadioButton();
            this.rdoOp = new System.Windows.Forms.RadioButton();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.Controls.Add(this.btnEdit);
            this.panel1.Controls.Add(this.btnDelete);
            this.panel1.Controls.Add(this.txtDetail);
            this.panel1.Controls.Add(this.detailLabel);
            this.panel1.Controls.Add(this.btnSubmit);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.txtLocation);
            this.panel1.Controls.Add(this.cbbType);
            this.panel1.Controls.Add(this.cbbNumber);
            this.panel1.Controls.Add(this.cbbBatteryId);
            this.panel1.Controls.Add(this.rdoNonOp);
            this.panel1.Controls.Add(this.rdoLimited);
            this.panel1.Controls.Add(this.rdoOp);
            this.panel1.Location = new System.Drawing.Point(4, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(338, 503);
            this.panel1.TabIndex = 0;
            // 
            // btnEdit
            // 
            this.btnEdit.Enabled = false;
            this.btnEdit.Location = new System.Drawing.Point(84, 365);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(170, 33);
            this.btnEdit.TabIndex = 16;
            this.btnEdit.Text = "Edit";
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Enabled = false;
            this.btnDelete.Location = new System.Drawing.Point(84, 326);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(170, 33);
            this.btnDelete.TabIndex = 15;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // txtDetail
            // 
            this.txtDetail.Location = new System.Drawing.Point(84, 198);
            this.txtDetail.Name = "txtDetail";
            this.txtDetail.Size = new System.Drawing.Size(176, 20);
            this.txtDetail.TabIndex = 14;
            // 
            // detailLabel
            // 
            this.detailLabel.AutoSize = true;
            this.detailLabel.Location = new System.Drawing.Point(9, 201);
            this.detailLabel.Name = "detailLabel";
            this.detailLabel.Size = new System.Drawing.Size(34, 13);
            this.detailLabel.TabIndex = 13;
            this.detailLabel.Text = "Detail";
            // 
            // btnSubmit
            // 
            this.btnSubmit.Location = new System.Drawing.Point(84, 287);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(170, 33);
            this.btnSubmit.TabIndex = 12;
            this.btnSubmit.Text = "Submit";
            this.btnSubmit.UseVisualStyleBackColor = true;
            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(9, 233);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(37, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "Status";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 163);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(48, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Location";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 122);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(31, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Type";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 72);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(44, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Number";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Battery ID";
            // 
            // txtLocation
            // 
            this.txtLocation.Location = new System.Drawing.Point(84, 160);
            this.txtLocation.Name = "txtLocation";
            this.txtLocation.Size = new System.Drawing.Size(176, 20);
            this.txtLocation.TabIndex = 6;
            // 
            // cbbType
            // 
            this.cbbType.FormattingEnabled = true;
            this.cbbType.Location = new System.Drawing.Point(84, 119);
            this.cbbType.Name = "cbbType";
            this.cbbType.Size = new System.Drawing.Size(121, 21);
            this.cbbType.TabIndex = 5;
            // 
            // cbbNumber
            // 
            this.cbbNumber.FormattingEnabled = true;
            this.cbbNumber.Location = new System.Drawing.Point(84, 69);
            this.cbbNumber.Name = "cbbNumber";
            this.cbbNumber.Size = new System.Drawing.Size(121, 21);
            this.cbbNumber.TabIndex = 4;
            // 
            // cbbBatteryId
            // 
            this.cbbBatteryId.FormattingEnabled = true;
            this.cbbBatteryId.Location = new System.Drawing.Point(84, 24);
            this.cbbBatteryId.Name = "cbbBatteryId";
            this.cbbBatteryId.Size = new System.Drawing.Size(121, 21);
            this.cbbBatteryId.TabIndex = 3;
            // 
            // rdoNonOp
            // 
            this.rdoNonOp.AutoSize = true;
            this.rdoNonOp.Location = new System.Drawing.Point(194, 231);
            this.rdoNonOp.Name = "rdoNonOp";
            this.rdoNonOp.Size = new System.Drawing.Size(60, 17);
            this.rdoNonOp.TabIndex = 2;
            this.rdoNonOp.TabStop = true;
            this.rdoNonOp.Text = "NonOP";
            this.rdoNonOp.UseVisualStyleBackColor = true;
            // 
            // rdoLimited
            // 
            this.rdoLimited.AutoSize = true;
            this.rdoLimited.Location = new System.Drawing.Point(130, 231);
            this.rdoLimited.Name = "rdoLimited";
            this.rdoLimited.Size = new System.Drawing.Size(58, 17);
            this.rdoLimited.TabIndex = 1;
            this.rdoLimited.TabStop = true;
            this.rdoLimited.Text = "Limited";
            this.rdoLimited.UseVisualStyleBackColor = true;
            // 
            // rdoOp
            // 
            this.rdoOp.AutoSize = true;
            this.rdoOp.Location = new System.Drawing.Point(84, 231);
            this.rdoOp.Name = "rdoOp";
            this.rdoOp.Size = new System.Drawing.Size(40, 17);
            this.rdoOp.TabIndex = 0;
            this.rdoOp.TabStop = true;
            this.rdoOp.Text = "OP";
            this.rdoOp.UseVisualStyleBackColor = true;
            // 
            // MainFireunit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Name = "MainFireunit";
            this.Size = new System.Drawing.Size(545, 539);
            this.Load += new System.EventHandler(this.main_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.RadioButton rdoLimited;
        private System.Windows.Forms.RadioButton rdoOp;
        private System.Windows.Forms.RadioButton rdoNonOp;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtLocation;
        private System.Windows.Forms.ComboBox cbbType;
        private System.Windows.Forms.ComboBox cbbNumber;
        private System.Windows.Forms.ComboBox cbbBatteryId;
        private System.Windows.Forms.Button btnSubmit;
        private System.Windows.Forms.TextBox txtDetail;
        private System.Windows.Forms.Label detailLabel;
        public System.Windows.Forms.Button btnDelete;
        public System.Windows.Forms.Button btnEdit;
        public System.Windows.Forms.Panel panel1;
    }
}
