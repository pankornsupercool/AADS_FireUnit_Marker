
namespace AADS.Views.Airport
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
            this.button1 = new System.Windows.Forms.Button();
            this.btnSummary = new System.Windows.Forms.Button();
            this.btnView = new System.Windows.Forms.Button();
            this.btnModify = new System.Windows.Forms.Button();
            this.btnDel = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioBtnBoth = new System.Windows.Forms.RadioButton();
            this.radioBtnNeighbor = new System.Windows.Forms.RadioButton();
            this.vaa = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(48, 318);
            this.button1.Margin = new System.Windows.Forms.Padding(2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(38, 45);
            this.button1.TabIndex = 32;
            this.button1.Text = "Print";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // btnSummary
            // 
            this.btnSummary.Location = new System.Drawing.Point(47, 262);
            this.btnSummary.Margin = new System.Windows.Forms.Padding(2);
            this.btnSummary.Name = "btnSummary";
            this.btnSummary.Size = new System.Drawing.Size(62, 52);
            this.btnSummary.TabIndex = 31;
            this.btnSummary.Text = "Summary";
            this.btnSummary.UseVisualStyleBackColor = true;
            // 
            // btnView
            // 
            this.btnView.Location = new System.Drawing.Point(48, 367);
            this.btnView.Margin = new System.Windows.Forms.Padding(2);
            this.btnView.Name = "btnView";
            this.btnView.Size = new System.Drawing.Size(47, 49);
            this.btnView.TabIndex = 30;
            this.btnView.Text = "View";
            this.btnView.UseVisualStyleBackColor = true;
            // 
            // btnModify
            // 
            this.btnModify.Location = new System.Drawing.Point(48, 222);
            this.btnModify.Margin = new System.Windows.Forms.Padding(2);
            this.btnModify.Name = "btnModify";
            this.btnModify.Size = new System.Drawing.Size(70, 36);
            this.btnModify.TabIndex = 29;
            this.btnModify.Text = "Modify";
            this.btnModify.UseVisualStyleBackColor = true;
            // 
            // btnDel
            // 
            this.btnDel.Location = new System.Drawing.Point(48, 420);
            this.btnDel.Margin = new System.Windows.Forms.Padding(2);
            this.btnDel.Name = "btnDel";
            this.btnDel.Size = new System.Drawing.Size(62, 31);
            this.btnDel.TabIndex = 28;
            this.btnDel.Text = "Del";
            this.btnDel.UseVisualStyleBackColor = true;
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(48, 181);
            this.btnAdd.Margin = new System.Windows.Forms.Padding(2);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(70, 37);
            this.btnAdd.TabIndex = 27;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(122, 181);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(2);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 82;
            this.dataGridView1.RowTemplate.Height = 33;
            this.dataGridView1.Size = new System.Drawing.Size(115, 270);
            this.dataGridView1.TabIndex = 26;
            // 
            // groupBox1
            // 
            this.groupBox1.Location = new System.Drawing.Point(11, 11);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox1.Size = new System.Drawing.Size(443, 61);
            this.groupBox1.TabIndex = 25;
            this.groupBox1.TabStop = false;
            // 
            // radioBtnBoth
            // 
            this.radioBtnBoth.AutoSize = true;
            this.radioBtnBoth.Location = new System.Drawing.Point(47, 94);
            this.radioBtnBoth.Margin = new System.Windows.Forms.Padding(2);
            this.radioBtnBoth.Name = "radioBtnBoth";
            this.radioBtnBoth.Size = new System.Drawing.Size(47, 17);
            this.radioBtnBoth.TabIndex = 2;
            this.radioBtnBoth.TabStop = true;
            this.radioBtnBoth.Text = "Both";
            this.radioBtnBoth.UseVisualStyleBackColor = true;
            // 
            // radioBtnNeighbor
            // 
            this.radioBtnNeighbor.AutoSize = true;
            this.radioBtnNeighbor.Location = new System.Drawing.Point(47, 115);
            this.radioBtnNeighbor.Margin = new System.Windows.Forms.Padding(2);
            this.radioBtnNeighbor.Name = "radioBtnNeighbor";
            this.radioBtnNeighbor.Size = new System.Drawing.Size(68, 17);
            this.radioBtnNeighbor.TabIndex = 1;
            this.radioBtnNeighbor.TabStop = true;
            this.radioBtnNeighbor.Text = "Neighbor";
            this.radioBtnNeighbor.UseVisualStyleBackColor = true;
            // 
            // vaa
            // 
            this.vaa.AutoSize = true;
            this.vaa.Location = new System.Drawing.Point(98, 94);
            this.vaa.Margin = new System.Windows.Forms.Padding(2);
            this.vaa.Name = "vaa";
            this.vaa.Size = new System.Drawing.Size(66, 17);
            this.vaa.TabIndex = 0;
            this.vaa.TabStop = true;
            this.vaa.Text = "Thailand";
            this.vaa.UseVisualStyleBackColor = true;
            // 
            // main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.radioBtnNeighbor);
            this.Controls.Add(this.radioBtnBoth);
            this.Controls.Add(this.vaa);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnSummary);
            this.Controls.Add(this.btnView);
            this.Controls.Add(this.btnModify);
            this.Controls.Add(this.btnDel);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.groupBox1);
            this.Name = "main";
            this.Size = new System.Drawing.Size(290, 559);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.Button button1;
        public System.Windows.Forms.Button btnSummary;
        public System.Windows.Forms.Button btnView;
        public System.Windows.Forms.Button btnModify;
        public System.Windows.Forms.Button btnDel;
        public System.Windows.Forms.Button btnAdd;
        public System.Windows.Forms.DataGridView dataGridView1;
        public System.Windows.Forms.GroupBox groupBox1;
        public System.Windows.Forms.RadioButton radioBtnBoth;
        public System.Windows.Forms.RadioButton radioBtnNeighbor;
        public System.Windows.Forms.RadioButton vaa;
    }
}
