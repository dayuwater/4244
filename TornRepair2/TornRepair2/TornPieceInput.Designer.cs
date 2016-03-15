namespace TornRepair2
{
    partial class TornPieceInput
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
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.Image = new System.Windows.Forms.DataGridViewImageColumn();
            this.label1 = new System.Windows.Forms.Label();
            this.imageCountDisplay = new System.Windows.Forms.Label();
            this.button5 = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.blackSelect = new System.Windows.Forms.RadioButton();
            this.whiteSelect = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(1008, 26);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(286, 80);
            this.button1.TabIndex = 0;
            this.button1.Text = "Add a Fragment";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(1008, 149);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(286, 80);
            this.button2.TabIndex = 1;
            this.button2.Text = "Remove Selected";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(1008, 522);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(286, 89);
            this.button3.TabIndex = 2;
            this.button3.Text = "Back";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(1008, 670);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(286, 89);
            this.button4.TabIndex = 3;
            this.button4.Text = "Next";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Image});
            this.dataGridView1.Location = new System.Drawing.Point(92, 26);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 33;
            this.dataGridView1.Size = new System.Drawing.Size(819, 733);
            this.dataGridView1.TabIndex = 4;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // Image
            // 
            this.Image.HeaderText = "Image";
            this.Image.Name = "Image";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(1009, 354);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(139, 25);
            this.label1.TabIndex = 5;
            this.label1.Text = "Image Count:";
            // 
            // imageCountDisplay
            // 
            this.imageCountDisplay.AutoSize = true;
            this.imageCountDisplay.Location = new System.Drawing.Point(1270, 354);
            this.imageCountDisplay.Name = "imageCountDisplay";
            this.imageCountDisplay.Size = new System.Drawing.Size(24, 25);
            this.imageCountDisplay.TabIndex = 6;
            this.imageCountDisplay.Text = "0";
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(1014, 389);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(280, 79);
            this.button5.TabIndex = 7;
            this.button5.Text = "refresh";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.whiteSelect);
            this.groupBox1.Controls.Add(this.blackSelect);
            this.groupBox1.Location = new System.Drawing.Point(1008, 235);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(286, 100);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "groupBox1";
            // 
            // blackSelect
            // 
            this.blackSelect.AutoSize = true;
            this.blackSelect.Checked = true;
            this.blackSelect.Location = new System.Drawing.Point(19, 30);
            this.blackSelect.Name = "blackSelect";
            this.blackSelect.Size = new System.Drawing.Size(96, 29);
            this.blackSelect.TabIndex = 0;
            this.blackSelect.TabStop = true;
            this.blackSelect.Text = "Black";
            this.blackSelect.UseVisualStyleBackColor = true;
            // 
            // whiteSelect
            // 
            this.whiteSelect.AutoSize = true;
            this.whiteSelect.Location = new System.Drawing.Point(19, 65);
            this.whiteSelect.Name = "whiteSelect";
            this.whiteSelect.Size = new System.Drawing.Size(98, 29);
            this.whiteSelect.TabIndex = 1;
            this.whiteSelect.Text = "White";
            this.whiteSelect.UseVisualStyleBackColor = true;
            // 
            // TornPieceInput
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1352, 797);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.imageCountDisplay);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Name = "TornPieceInput";
            this.Text = "Input Manager";
            this.Activated += new System.EventHandler(this.TornPieceInput_Activated);
            this.Enter += new System.EventHandler(this.TornPieceInput_Enter);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label imageCountDisplay;
        private System.Windows.Forms.DataGridViewImageColumn Image;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton whiteSelect;
        private System.Windows.Forms.RadioButton blackSelect;
    }
}