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
    public partial class MatchHistory : Form
    {
        public MatchHistory()
        {
            InitializeComponent();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void MatchHistory_Activated(object sender, EventArgs e)
        {
            refresh();
        }

        private void refresh()
        {
            dataGridView1.Rows.Clear();
            for (int i = 0; i < Form1.matchHistory.Count; i++)
            {

                Image<Bgr, Byte> thumbnail1 = Form1.matchHistory[i].img1.Resize(150, 150, INTER.CV_INTER_CUBIC, true);
                Image<Bgr, Byte> thumbnail2 = Form1.matchHistory[i].img2.Resize(150, 150, INTER.CV_INTER_CUBIC, true);
                double confidence = Form1.matchHistory[i].confident;
                double overlap = Form1.matchHistory[i].overlap;



                DataGridViewRow row = dataGridView1.Rows[dataGridView1.Rows.Add()];
                row.Cells["Image1"].Value = thumbnail1.ToBitmap();
                row.Cells["Image2"].Value = thumbnail2.ToBitmap();
                row.Cells["Confidence"].Value = confidence;
                row.Cells["Overlap"].Value = overlap;
                row.Height = 150;
                
               

                

            }
        }
    }
}
