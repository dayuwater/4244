namespace TornRepair2
{
    partial class DocumentConfirm
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
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.PageNumDisplay = new System.Windows.Forms.Label();
            this.PageTotalNumDisplay = new System.Windows.Forms.Label();
            this.button6 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(55, 37);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(854, 722);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = "";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(1023, 98);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(344, 100);
            this.button1.TabIndex = 1;
            this.button1.Text = "Export to HTML";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(1023, 284);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(344, 100);
            this.button2.TabIndex = 2;
            this.button2.Text = "Export to MS Word";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(1023, 670);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(344, 100);
            this.button3.TabIndex = 3;
            this.button3.Text = "Share Online";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(55, 778);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(209, 48);
            this.button4.TabIndex = 4;
            this.button4.Text = "Previous Page";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(600, 778);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(209, 48);
            this.button5.TabIndex = 5;
            this.button5.Text = "Next Page";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(296, 790);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 25);
            this.label1.TabIndex = 6;
            this.label1.Text = "Page";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(423, 790);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(30, 25);
            this.label2.TabIndex = 7;
            this.label2.Text = "of";
            // 
            // PageNumDisplay
            // 
            this.PageNumDisplay.AutoSize = true;
            this.PageNumDisplay.Location = new System.Drawing.Point(364, 790);
            this.PageNumDisplay.Name = "PageNumDisplay";
            this.PageNumDisplay.Size = new System.Drawing.Size(56, 25);
            this.PageNumDisplay.TabIndex = 8;
            this.PageNumDisplay.Text = "Num";
            // 
            // PageTotalNumDisplay
            // 
            this.PageTotalNumDisplay.AutoSize = true;
            this.PageTotalNumDisplay.Location = new System.Drawing.Point(472, 790);
            this.PageTotalNumDisplay.Name = "PageTotalNumDisplay";
            this.PageTotalNumDisplay.Size = new System.Drawing.Size(104, 25);
            this.PageTotalNumDisplay.TabIndex = 9;
            this.PageTotalNumDisplay.Text = "TotalNum";
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(1023, 496);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(344, 90);
            this.button6.TabIndex = 10;
            this.button6.Text = "Export to PDF";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(834, 778);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(121, 48);
            this.button7.TabIndex = 11;
            this.button7.Text = "Save";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // DocumentConfirm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1477, 849);
            this.Controls.Add(this.button7);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.PageTotalNumDisplay);
            this.Controls.Add(this.PageNumDisplay);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.richTextBox1);
            this.Name = "DocumentConfirm";
            this.Text = "Document Confirm and Save";
            this.Activated += new System.EventHandler(this.DocumentConfirm_Activated);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.DocumentConfirm_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label PageNumDisplay;
        private System.Windows.Forms.Label PageTotalNumDisplay;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button7;
    }
}