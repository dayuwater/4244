namespace TornRepair2
{
    partial class MatchHistory
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.Image1 = new System.Windows.Forms.DataGridViewImageColumn();
            this.Image2 = new System.Windows.Forms.DataGridViewImageColumn();
            this.Confidence = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Overlap = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Image1,
            this.Image2,
            this.Confidence,
            this.Overlap});
            this.dataGridView1.Location = new System.Drawing.Point(124, 50);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 33;
            this.dataGridView1.Size = new System.Drawing.Size(1026, 835);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // Image1
            // 
            this.Image1.HeaderText = "Image1";
            this.Image1.Name = "Image1";
            // 
            // Image2
            // 
            this.Image2.HeaderText = "Image2";
            this.Image2.Name = "Image2";
            // 
            // Confidence
            // 
            this.Confidence.HeaderText = "Confidence";
            this.Confidence.Name = "Confidence";
            // 
            // Overlap
            // 
            this.Overlap.HeaderText = "Overlap";
            this.Overlap.Name = "Overlap";
            // 
            // MatchHistory
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1301, 972);
            this.Controls.Add(this.dataGridView1);
            this.Name = "MatchHistory";
            this.Text = "MatchHistory";
            this.Activated += new System.EventHandler(this.MatchHistory_Activated);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewImageColumn Image1;
        private System.Windows.Forms.DataGridViewImageColumn Image2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Confidence;
        private System.Windows.Forms.DataGridViewTextBoxColumn Overlap;
    }
}