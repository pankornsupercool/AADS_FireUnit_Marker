
namespace AADS.Views.VitalAsset
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
            this.btnPrint = new System.Windows.Forms.Button();
            this.btnSum = new System.Windows.Forms.Button();
            this.btnView = new System.Windows.Forms.Button();
            this.btnMod = new System.Windows.Forms.Button();
            this.btnDel = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Name_Vital = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Type = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Priority = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Area_Name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Province = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Asset_Size = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Unit_Responsible = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Unit_Status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Sim_Status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnPrint
            // 
            this.btnPrint.Location = new System.Drawing.Point(18, 351);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(47, 39);
            this.btnPrint.TabIndex = 62;
            this.btnPrint.Text = "Print";
            this.btnPrint.UseVisualStyleBackColor = true;
            // 
            // btnSum
            // 
            this.btnSum.Location = new System.Drawing.Point(18, 306);
            this.btnSum.Name = "btnSum";
            this.btnSum.Size = new System.Drawing.Size(47, 39);
            this.btnSum.TabIndex = 61;
            this.btnSum.Text = "Sum";
            this.btnSum.UseVisualStyleBackColor = true;
            // 
            // btnView
            // 
            this.btnView.Location = new System.Drawing.Point(18, 261);
            this.btnView.Name = "btnView";
            this.btnView.Size = new System.Drawing.Size(47, 39);
            this.btnView.TabIndex = 60;
            this.btnView.Text = "View";
            this.btnView.UseVisualStyleBackColor = true;
            // 
            // btnMod
            // 
            this.btnMod.Location = new System.Drawing.Point(18, 216);
            this.btnMod.Name = "btnMod";
            this.btnMod.Size = new System.Drawing.Size(47, 39);
            this.btnMod.TabIndex = 59;
            this.btnMod.Text = "Mod";
            this.btnMod.UseVisualStyleBackColor = true;
            // 
            // btnDel
            // 
            this.btnDel.Location = new System.Drawing.Point(18, 171);
            this.btnDel.Name = "btnDel";
            this.btnDel.Size = new System.Drawing.Size(47, 39);
            this.btnDel.TabIndex = 58;
            this.btnDel.Text = "Del";
            this.btnDel.UseVisualStyleBackColor = true;
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(18, 126);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(47, 39);
            this.btnAdd.TabIndex = 57;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ID,
            this.Name_Vital,
            this.Type,
            this.Priority,
            this.Area_Name,
            this.Province,
            this.Asset_Size,
            this.Unit_Responsible,
            this.Unit_Status,
            this.Sim_Status});
            this.dataGridView1.Location = new System.Drawing.Point(98, 126);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(1039, 275);
            this.dataGridView1.TabIndex = 56;
            // 
            // ID
            // 
            this.ID.HeaderText = "ID";
            this.ID.Name = "ID";
            // 
            // Name_Vital
            // 
            this.Name_Vital.HeaderText = "Name";
            this.Name_Vital.Name = "Name_Vital";
            // 
            // Type
            // 
            this.Type.HeaderText = "Type";
            this.Type.Name = "Type";
            // 
            // Priority
            // 
            this.Priority.HeaderText = "Priority";
            this.Priority.Name = "Priority";
            // 
            // Area_Name
            // 
            this.Area_Name.HeaderText = "Area Name";
            this.Area_Name.Name = "Area_Name";
            // 
            // Province
            // 
            this.Province.HeaderText = "Province";
            this.Province.Name = "Province";
            // 
            // Asset_Size
            // 
            this.Asset_Size.HeaderText = "Asset Size";
            this.Asset_Size.Name = "Asset_Size";
            // 
            // Unit_Responsible
            // 
            this.Unit_Responsible.HeaderText = "Unit Responsible";
            this.Unit_Responsible.Name = "Unit_Responsible";
            // 
            // Unit_Status
            // 
            this.Unit_Status.HeaderText = "Unit Status";
            this.Unit_Status.Name = "Unit_Status";
            // 
            // Sim_Status
            // 
            this.Sim_Status.HeaderText = "Sim Status";
            this.Sim_Status.Name = "Sim_Status";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1178, 45);
            this.panel1.TabIndex = 55;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(387, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Vital Asset";
            // 
            // main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1178, 485);
            this.Controls.Add(this.btnPrint);
            this.Controls.Add(this.btnSum);
            this.Controls.Add(this.btnView);
            this.Controls.Add(this.btnMod);
            this.Controls.Add(this.btnDel);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.panel1);
            this.Name = "main";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Button btnSum;
        private System.Windows.Forms.Button btnView;
        private System.Windows.Forms.Button btnMod;
        private System.Windows.Forms.Button btnDel;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn Name_Vital;
        private System.Windows.Forms.DataGridViewTextBoxColumn Type;
        private System.Windows.Forms.DataGridViewTextBoxColumn Priority;
        private System.Windows.Forms.DataGridViewTextBoxColumn Area_Name;
        private System.Windows.Forms.DataGridViewTextBoxColumn Province;
        private System.Windows.Forms.DataGridViewTextBoxColumn Asset_Size;
        private System.Windows.Forms.DataGridViewTextBoxColumn Unit_Responsible;
        private System.Windows.Forms.DataGridViewTextBoxColumn Unit_Status;
        private System.Windows.Forms.DataGridViewTextBoxColumn Sim_Status;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
    }
}
