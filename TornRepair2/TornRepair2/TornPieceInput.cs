using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;

namespace TornRepair2
{
   
    public partial class TornPieceInput : Form
    {

        public int status = 0; // 0 for photo repair, 1 for document repair
        public TornPieceInput()
        {
            InitializeComponent();
            imageCountDisplay.Text = Form1.sourceImages.Count.ToString();
            for (int i = 0; i < Form1.sourceImages.Count; i++)
            {
                
                using (Image<Bgr, Byte> thumbnail = Form1.sourceImages[i].Resize(150, 150, INTER.CV_INTER_CUBIC, true))
                {
                    DataGridViewRow row = dataGridView1.Rows[dataGridView1.Rows.Add()];
                    row.Cells["Image"].Value = thumbnail.ToBitmap();
                    row.Height = 150;
                }
            }
            

        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form1 f1 = new Form1();
            f1.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Hide();

            AddFile add = new AddFile { status = 0 };
            add.Show();





        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (Form1.sourceImages.Count != 0)
            {
                DisplayBestMatch bestMatchView = new DisplayBestMatch();
                QueueView qv = new QueueView();

                // extract the contour maps, send the result into queueview
                int index = 0;
                foreach (Image<Bgr, byte> image in Form1.sourceImages)
                {
                    List<ColorfulContourMap> cmap;
                    if (blackSelect.Checked == true)
                    {
                        cmap = ColorfulContourMap.getAllContourMap(image, index, 0);
                    }
                    else
                    {
                        cmap = ColorfulContourMap.getAllContourMap(image, index, 1);
                    }
                    Form1.contourMaps.AddRange(cmap);
                    index++;
                }
                if (blackSelect.Checked)
                {
                    Form1.BKG_WHITE = true;
                }
                else
                {
                    Form1.BKG_WHITE = false;
                }

                Hide();

                bestMatchView.Show();
                qv.Show();
            }
            else
            {
                MessageBox.Show("Please input at least one image.");
                
            }
        }

       

        private void button5_Click(object sender, EventArgs e)
        {
            refresh();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DataGridViewRow currentRow=dataGridView1.CurrentRow;
            if (currentRow != null)
            {
                Form1.sourceImages.RemoveAt(currentRow.Index);
                
                refresh();
            }

        }
        private void refresh()
        {
            dataGridView1.Rows.Clear();
            for (int i = 0; i < Form1.sourceImages.Count; i++)
            {

                using (Image<Bgr, Byte> thumbnail = Form1.sourceImages[i].Resize(150, 150, INTER.CV_INTER_CUBIC, true))
                {
                    DataGridViewRow row = dataGridView1.Rows[dataGridView1.Rows.Add()];
                    row.Cells["Image"].Value = thumbnail.ToBitmap();
                    row.Height = 150;
                }
            }
        }

        private void TornPieceInput_Enter(object sender, EventArgs e)
        {
            refresh();
        }

        private void TornPieceInput_Activated(object sender, EventArgs e)
        {
            refresh();
        }
    }
}
