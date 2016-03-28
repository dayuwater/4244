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
using Microsoft.Office;

namespace TornRepair2
{
    public partial class AddFile : Form
    {
        // if using this tool to input fragmented photo, status=0
        // if using this tool to select cover photo, status=1
        public int status = 0;
        private Image<Bgr, byte> img1;
        private bool loaded = false;
        private Capture _capture=null;
        private bool cameraOn = false;

        public AddFile()
        {
            InitializeComponent();
            if (status == 0)
            {
                StatusText.Text = "Fetching an input photo fragment";
            }
            else if (status == 1)
            {
                StatusText.Text = "Fetching an image cover";
            }
            try
            {
                _capture = new Capture();
                _capture.ImageGrabbed += ProcessFrame;
                
            }
            catch (NullReferenceException excpt)
            {
                MessageBox.Show(excpt.Message);
            }
        }
        private void ProcessFrame(object sender, EventArgs arg)
        {
            Image<Bgr, Byte> frame = _capture.RetrieveBgrFrame();
            pictureBox1.Image = frame.ToBitmap();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (loaded)
            {
                if (status == 0)
                {
                    Form1.sourceImages.Add(img1.Resize(0.3,INTER.CV_INTER_CUBIC));
                 
                }
                else
                {
                    Form1.coverImage = img1;
                }
            }
            Close();
            

            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog()==DialogResult.OK)
            {
                try {
                    img1 = new Image<Bgr, byte>(ofd.FileName);
                    pictureBox1.Image = img1.ToBitmap();
                    StatusText.Text = "Image from file detected";
                    loaded = true;
                }
                catch
                {

                    StatusText.Text = "You must input a image file";
                }

            }
        }

        private void AddFile_FormClosed(object sender, FormClosedEventArgs e)
        {
            Console.WriteLine("Image Count:" + Form1.sourceImages.Count);
            
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        // Camera capture function.
        // Only for demonstration, cannot use this as a real input for now
        private void button3_Click(object sender, EventArgs e)
        {
            if (cameraOn)
            {
                button3.Text = "Take another Photo";
                _capture.Stop();
                cameraOn = false;
                if (!loaded)
                {
                    loaded = true;
                }

                // seems unnessesary, but actually needed
                img1 = _capture.RetrieveBgrFrame();
                Bitmap bmp1 = img1.ToBitmap();
                img1 = new Image<Bgr, byte>(bmp1);

                button1.Enabled = true;
                button2.Enabled = true;
            }
            else
            {
                button3.Text = "Capture this photo";
                _capture.Start();
                cameraOn = true;
                button1.Enabled = false;
                button2.Enabled = false;
            }
        }
    }
}
