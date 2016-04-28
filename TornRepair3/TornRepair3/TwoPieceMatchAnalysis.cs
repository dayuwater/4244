using Emgu.CV;
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

namespace TornRepair3
{
    public partial class TwoPieceMatchAnalysis : Form
    {
        public ColorfulContourMap map1; // the first piece
        public ColorfulContourMap map2; // the second piece
        public Mat pic1;
        public Mat pic2;
        private Mat pic1Copy;
        private Mat pic2Copy;
        private Mat joined;
        private Mat mask1;
        private Mat mask2;
        private Mat joined_mask;
        private Match edgeMatch;
        private Point centroid1;
        private Point centroid2;
        private double angle;
        private bool blackOrWhite = false;

        private List<Phi> DNA1;
        private List<Phi> DNA2;
        private double confidence = 0;
        private double overlap = 0;

        private Point p1Tweak = new Point(0, 0);
        private Point p2Tweak = new Point(0, 0);
        public TwoPieceMatchAnalysis()
        {
            InitializeComponent();
            label7.Text = p1Tweak.ToString();
            label8.Text = p2Tweak.ToString();
        }

        private void refresh(bool colored)
        {
            pic1Copy = pic1.Clone();

            pic2Copy = pic2.Clone();
            if (!colored)
            {
                map1.DrawTo(pic1Copy);
                map2.DrawTo(pic2Copy);
            }
            else
            {
                map1.DrawColorTo(pic1Copy);
                map2.DrawColorTo(pic2Copy);
            }
            MatImage m1 = new MatImage(pic1Copy);
            m1.ResizeTo(pictureBox1.Width, pictureBox1.Height);
            pictureBox1.Image = m1.Out().Bitmap;
            MatImage m2 = new MatImage(pic2Copy);
            m2.ResizeTo(pictureBox2.Width, pictureBox2.Height);
            pictureBox2.Image = m2.Out().Bitmap;
        }

        private void TwoPieceMatchAnalysis_Load(object sender, EventArgs e)
        {
            refresh(false);


            DNA1 = map1.extractDNA();
            DNA2 = map2.extractDNA();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                refresh(false);
            }
            else
            {
                refresh(true);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                pic1Copy = pic1.Clone();
                pic2Copy = pic2.Clone();
                map1.DrawTo(pic1);
                map2.DrawTo(pic2);

                edgeMatch = DNAUtil.partialMatch(DNA1, DNA2);
                List<Point> pointToDraw1 = new List<Point>();
                List<Point> pointToDraw2 = new List<Point>();
                for (int i = edgeMatch.t11; i < edgeMatch.t12; i++)
                {
                    pointToDraw1.Add(new Point((int)DNA1[i].x, (int)DNA1[i].y));

                }
                for (int i = edgeMatch.t21; i < edgeMatch.t22; i++)
                {
                    pointToDraw2.Add(new Point((int)DNA2[i].x, (int)DNA2[i].y));

                }
                CvInvoke.Polylines(pic1Copy, pointToDraw1.ToArray(), false, new Bgr(0, 255, 0).MCvScalar, 2);
                CvInvoke.Polylines(pic2Copy, pointToDraw2.ToArray(), false, new Bgr(0, 255, 0).MCvScalar, 2);

                MatImage m1 = new MatImage(pic1Copy);
                m1.ResizeTo(pictureBox1.Width, pictureBox1.Height);
                pictureBox1.Image = m1.Out().Bitmap;
                MatImage m2 = new MatImage(pic2Copy);
                m2.ResizeTo(pictureBox2.Width, pictureBox2.Height);
                pictureBox2.Image = m2.Out().Bitmap;
            }
            else
            {
                pic1Copy = pic1.Clone();
                pic2Copy = pic2.Clone();
                map1.DrawTo(pic1);
                map2.DrawTo(pic2);

                edgeMatch = DNAUtil.partialColorMatch(DNA1, DNA2);
                List<Point> pointToDraw1 = new List<Point>();
                List<Point> pointToDraw2 = new List<Point>();


                if (edgeMatch.t11 > edgeMatch.t12)
                {
                    for (int i = edgeMatch.t12; i < edgeMatch.t11; i++)
                    {
                        pointToDraw1.Add(new Point((int)DNA1[i].x, (int)DNA1[i].y));

                    }
                }
                else
                {

                    for (int i = edgeMatch.t11; i < edgeMatch.t12; i++)
                    {
                        pointToDraw1.Add(new Point((int)DNA1[i].x, (int)DNA1[i].y));

                    }
                }

               

                if (edgeMatch.t21 > edgeMatch.t22)
                {
                    
                    for (int i = edgeMatch.t22; i < edgeMatch.t21; i++)
                    {
                        pointToDraw2.Add(new Point((int)DNA2[i].x, (int)DNA2[i].y));

                    }
                }
                else
                {

                    for (int i = edgeMatch.t21; i < edgeMatch.t22; i++)
                    {
                        pointToDraw2.Add(new Point((int)DNA2[i].x, (int)DNA2[i].y));

                    }
                }

                CvInvoke.Polylines(pic1Copy, pointToDraw1.ToArray(), false, new Bgr(0, 255, 0).MCvScalar, 2);
                CvInvoke.Polylines(pic2Copy, pointToDraw2.ToArray(), false, new Bgr(0, 255, 0).MCvScalar, 2);

                MatImage m1 = new MatImage(pic1Copy);
                m1.ResizeTo(pictureBox1.Width, pictureBox1.Height);
                pictureBox1.Image = m1.Out().Bitmap;
                MatImage m2 = new MatImage(pic2Copy);
                m2.ResizeTo(pictureBox2.Width, pictureBox2.Height);
                pictureBox2.Image = m2.Out().Bitmap;
            }
        }
    }

       
}
