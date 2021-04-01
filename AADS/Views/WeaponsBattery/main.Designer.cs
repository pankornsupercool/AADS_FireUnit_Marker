
namespace AADS.Views.WeaponsBattery
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
            this.WB_ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Type = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Higher_Unit_ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Defended_Area_ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Operational_status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Control_Status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Sim_Status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnPrint
            // 
            this.btnPrint.Location = new System.Drawing.Point(139, 397);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(47, 39);
            this.btnPrint.TabIndex = 34;
            this.btnPrint.Text = "Print";
            this.btnPrint.UseVisualStyleBackColor = true;
            // 
            // btnSum
            // 
            this.btnSum.Location = new System.Drawing.Point(139, 352);
            this.btnSum.Name = "btnSum";
            this.btnSum.Size = new System.Drawing.Size(47, 39);
            this.btnSum.TabIndex = 33;
            this.btnSum.Text = "Sum";
            this.btnSum.UseVisualStyleBackColor = true;
            // 
            // btnView
            // 
            this.btnView.Location = new System.Drawing.Point(139, 307);
            this.btnView.Name = "btnView";
            this.btnView.Size = new System.Drawing.Size(47, 39);
            this.btnView.TabIndex = 32;
            this.btnView.Text = "View";
            this.btnView.UseVisualStyleBackColor = true;
            // 
            // btnMod
            // 
            this.btnMod.Location = new System.Drawing.Point(139, 262);
            this.btnMod.Name = "btnMod";
            this.btnMod.Size = new System.Drawing.Size(47, 39);
            this.btnMod.TabIndex = 31;
            this.btnMod.Text = "Mod";
            this.btnMod.UseVisualStyleBackColor = true;
            // 
            // btnDel
            // 
            this.btnDel.Location = new System.Drawing.Point(139, 217);
            this.btnDel.Name = "btnDel";
            this.btnDel.Size = new System.Drawing.Size(47, 39);
            this.btnDel.TabIndex = 30;
            this.btnDel.Text = "Del";
            this.btnDel.UseVisualStyleBackColor = true;
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(139, 172);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(47, 39);
            this.btnAdd.TabIndex = 29;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.WB_ID,
            this.Type,
            this.Higher_Unit_ID,
            this.Defended_Area_ID,
            this.Operational_status,
            this.Control_Status,
            this.Sim_Status});
            this.dataGridView1.Location = new System.Drawing.Point(217, 161);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(729, 297);
            this.dataGridView1.TabIndex = 28;
            // 
            // WB_ID
            // 
            this.WB_ID.HeaderText = "WB ID";
            this.WB_ID.Name = "WB_ID";
            // 
            // Type
            // 
            this.Type.HeaderText = "Type";
            this.Type.Name = "Type";
            // 
            // Higher_Unit_ID
            // 
            this.Higher_Unit_ID.HeaderText = "Unit ID";
            this.Higher_Unit_ID.Name = "Higher_Unit_ID";
            // 
            // Defended_Area_ID
            // 
            this.Defended_Area_ID.HeaderText = "DA ID";
            this.Defended_Area_ID.Name = "Defended_Area_ID";
            // 
            // Operational_status
            // 
            this.Operational_status.HeaderText = "Operational Status";
            this.Operational_status.Name = "Operational_status";
            // 
            // Control_Status
            // 
            this.Control_Status.HeaderText = "Control Status";
            this.Control_Status.Name = "Control_Status";
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
            this.panel1.Size = new System.Drawing.Size(1169, 45);
            this.panel1.TabIndex = 27;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(444, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(122, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Weapons Bateery Table";
            // 
            // main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1169, 557);
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
        private System.Windows.Forms.DataGridViewTextBoxColumn WB_ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn Type;
        private System.Windows.Forms.DataGridViewTextBoxColumn Higher_Unit_ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn Defended_Area_ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn Operational_status;
        private System.Windows.Forms.DataGridViewTextBoxColumn Control_Status;
        private System.Windows.Forms.DataGridViewTextBoxColumn Sim_Status;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
    }
}
