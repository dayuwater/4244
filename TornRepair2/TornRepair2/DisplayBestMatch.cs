using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TornRepair2
{
    public partial class DisplayBestMatch : Form
    {
        public DisplayBestMatch()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (Form1.status == 0)
            {
                ImageConfirm imageConfirm = new ImageConfirm();
                imageConfirm.Show();
                Hide();
            }
            else if (Form1.status == 1)
            {
                DocumentConfirm docConfirm = new DocumentConfirm();
                int index = 1;
                foreach(Image<Bgr,byte> img in Form1.finalImages)
                {
                    // mock the output, will substitute this with the result from OCR
                    docConfirm.content.Add(OCR.ReadText(img) + index.ToString());
                    index++;
                }
                
                docConfirm.Show();
                Hide();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
        private void refresh()
        {
            // refresh the grid view
            dataGridView1.Rows.Clear();
            for (int i = 0; i < Form1.finalImages.Count; i++)
            {

                using (Image<Bgr, byte> thumbnail = Form1.finalImages[i].Resize(150, 150, INTER.CV_INTER_CUBIC, true))
                {
                    DataGridViewRow row = dataGridView1.Rows[dataGridView1.Rows.Add()];
                    row.Cells["Image"].Value = thumbnail.ToBitmap();
                    row.Height = 150;
                }
            }
            // refresh the candidate selection
            listBox1.Items.Clear();
            int index = 0;
            foreach (List<Image<Bgr,byte>> img in Form1.candidateImages)
            {

                listBox1.Items.Add("Candidate " + (index+1));
                index++;

            }

        }

        private void DisplayBestMatch_Activated(object sender, EventArgs e)
        {
            refresh();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            refresh();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            dataGridView2.Rows.Clear();
            for (int i = 0; i < Form1.candidateImages[listBox1.SelectedIndex].Count; i++)
            {

                using (Image<Bgr, byte> thumbnail = Form1.candidateImages[listBox1.SelectedIndex][i].Resize(150, 150, INTER.CV_INTER_CUBIC, true))
                {
                    DataGridViewRow row = dataGridView2.Rows[dataGridView2.Rows.Add()];
                    row.Cells["CandidateImage"].Value = thumbnail.ToBitmap();
                    row.Height = 150;
                }
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            List<Image<Bgr, byte>> t = Form1.finalImages;
            try {
                Form1.finalImages = Form1.candidateImages[listBox1.SelectedIndex];
                Form1.candidateImages[listBox1.SelectedIndex] = t;

            }
            catch
            {
                MessageBox.Show("Please select a candidate");
            }
            refresh();
                    
        }
    }
}
