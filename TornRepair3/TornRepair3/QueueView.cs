using Emgu.CV;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TornRepair3
{
    public partial class QueueView : Form
    {
        private int num = 0;
        private int ind1 = -1; // the index of the first contour map
        private int ind2 = -1; // the index of the second contour map
        private ColorfulContourMap ctmap; // the current contour map
        public double confidence = 0;
        public double overlap = 0;
        public QueueView()
        {
            InitializeComponent();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void refresh()
        {
            dataGridView1.Rows.Clear();
            for (int i = 0; i < Form1.blackSourceImages.Count; i++)
            {
                if (/*Form1.matched[i] == false*/true)
                {
                    using (Mat thumbnail = generateThumbnail(Form1.blackSourceImages[i]))
                    {
                        DataGridViewRow row = dataGridView1.Rows[dataGridView1.Rows.Add()];
                        
                        row.Cells["SourceImage"].Value = thumbnail.Bitmap;
                        row.Height = 150;
                    }
                }
            }
            for (int i = 0; i < Form1.whiteSourceImages.Count; i++)
            {
                if (/*Form1.matched[i] == false*/true)
                {
                    using (Mat thumbnail = generateThumbnail(Form1.whiteSourceImages[i]))
                    {
                        DataGridViewRow row = dataGridView1.Rows[dataGridView1.Rows.Add()];
                        row.Cells["SourceImage"].Value = thumbnail.Bitmap;
                        row.Height = 150;
                    }
                }
            }
            ConfidenceView.Text = confidence.ToString();
            OverlapView.Text = overlap.ToString();
        }
        private Mat generateThumbnail(Mat input)
        {
            MatImage m1 = new MatImage(input);
            m1.ResizeTo(150, 150);
            return m1.Out();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            refresh();
        }

        private void QueueView_Activated(object sender, EventArgs e)
        {
            refresh();
        }
    }
}

