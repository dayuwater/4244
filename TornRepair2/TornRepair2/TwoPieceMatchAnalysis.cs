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
    public partial class TwoPieceMatchAnalysis : Form
    {
        public ColorfulContourMap map1; // the first piece
        public ColorfulContourMap map2; // the second piece
        private Image<Bgr, byte> pic1;
        private Image<Bgr, byte> pic2;
        private Image<Bgr, byte> joined;
        private Image<Bgr, byte> mask1;
        private Image<Bgr, byte> mask2;
        private Image<Bgr, byte> joined_mask;
        private Match edgeMatch;
        private Point centroid1;
        private Point centroid2;
        private double angle;

        private List<Phi> DNA1;
        private List<Phi> DNA2;
        private double confidence=0;
        private double overlap=0;

        private Point p1Tweak=new Point(0,0);
        private Point p2Tweak=new Point(0,0);
        
        public TwoPieceMatchAnalysis()
        {
            InitializeComponent();
            label7.Text = p1Tweak.ToString();
            label8.Text = p2Tweak.ToString();
           

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                pic1 = Form1.sourceImages[map1.imageIndex].Clone();
                pic2 = Form1.sourceImages[map2.imageIndex].Clone();
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
                pic1.DrawPolyline(pointToDraw1.ToArray(), false, new Bgr(0, 255, 0), 2);
                pic2.DrawPolyline(pointToDraw2.ToArray(), false, new Bgr(0, 255, 0), 2);
                pictureBox1.Image = pic1.Resize(pictureBox1.Width, pictureBox1.Height, INTER.CV_INTER_LINEAR).ToBitmap();
                pictureBox2.Image = pic2.Resize(pictureBox2.Width, pictureBox2.Height, INTER.CV_INTER_LINEAR).ToBitmap();
            }
            else
            {
                pic1 = Form1.sourceImages[map1.imageIndex].Clone();
                pic2 = Form1.sourceImages[map2.imageIndex].Clone();
                map1.DrawTo(pic1);
                map2.DrawTo(pic2);

                // add color matching algorithm here
                edgeMatch = DNAUtil.partialColorMatch(DNA1, DNA2);
                List<Point> pointToDraw1 = new List<Point>();
                List<Point> pointToDraw2 = new List<Point>();
               
                
                if (edgeMatch.t11 > edgeMatch.t12)
                {
                    /*for(int i=edgeMatch.t11; i < DNA1.Count; i++)
                    {
                        pointToDraw1.Add(new Point((int)DNA1[i].x, (int)DNA1[i].y));
                    }
                    for(int i=0; i<=edgeMatch.t12; i++)
                    {
                        pointToDraw1.Add(new Point((int)DNA1[i].x, (int)DNA1[i].y));
                    }*/
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

                // the piece 2 should draw reversely
                /*if (edgeMatch.t21 > edgeMatch.t22)
                {
                    for (int i = edgeMatch.t22; i > -1; i--)
                    {
                        pointToDraw2.Add(new Point((int)DNA2[i].x, (int)DNA2[i].y));
                    }
                    for (int i = DNA2.Count-1; i >= edgeMatch.t21; i--)
                    {
                        pointToDraw2.Add(new Point((int)DNA2[i].x, (int)DNA2[i].y));
                    }
                    
                }
                else
                {

                    for (int i = edgeMatch.t22; i >= edgeMatch.t21; i--)
                    {
                        pointToDraw2.Add(new Point((int)DNA2[i].x, (int)DNA2[i].y));

                    }
                }*/

                if (edgeMatch.t21 > edgeMatch.t22)
                {
                    /*for (int i = edgeMatch.t21; i< DNA2.Count; i++)
                    {
                        pointToDraw2.Add(new Point((int)DNA2[i].x, (int)DNA2[i].y));
                    }
                    for (int i = 0; i <= edgeMatch.t22; i++)
                    {
                        pointToDraw2.Add(new Point((int)DNA2[i].x, (int)DNA2[i].y));
                    }*/
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

                pic1.DrawPolyline(pointToDraw1.ToArray(), false, new Bgr(0, 255, 0), 2);
                pic2.DrawPolyline(pointToDraw2.ToArray(), false, new Bgr(0, 255, 0), 2);
                pictureBox1.Image = pic1.Resize(pictureBox1.Width, pictureBox1.Height, INTER.CV_INTER_LINEAR).ToBitmap();
                pictureBox2.Image = pic2.Resize(pictureBox2.Width, pictureBox2.Height, INTER.CV_INTER_LINEAR).ToBitmap();
            }
        }

        private void TwoPieceMatchAnalysis_Activated(object sender, EventArgs e)
        {
            pic1 = Form1.sourceImages[map1.imageIndex].Clone();
            pic2 = Form1.sourceImages[map2.imageIndex].Clone();
            map1.DrawTo(pic1);
            map2.DrawTo(pic2);
            pictureBox1.Image = pic1.Resize(pictureBox1.Width,pictureBox1.Height,INTER.CV_INTER_CUBIC).ToBitmap();
            pictureBox2.Image = pic2.Resize(pictureBox2.Width, pictureBox2.Height, INTER.CV_INTER_CUBIC).ToBitmap();
            DNA1 = map1.extractDNA();
            DNA2 = map2.extractDNA();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                pic1 = Form1.sourceImages[map1.imageIndex].Clone();
                pic2 = Form1.sourceImages[map2.imageIndex].Clone();
                map1.DrawTo(pic1);
                map2.DrawTo(pic2);
                pictureBox1.Image = pic1.Resize(pictureBox1.Width, pictureBox1.Height, INTER.CV_INTER_LINEAR).ToBitmap();
                pictureBox2.Image = pic2.Resize(pictureBox2.Width, pictureBox2.Height, INTER.CV_INTER_LINEAR).ToBitmap();
            }
            else
            {
                pic1 = Form1.sourceImages[map1.imageIndex].Clone();
                pic2 = Form1.sourceImages[map2.imageIndex].Clone();
                map1.DrawColorTo(pic1);
                map2.DrawColorTo(pic2);
                pictureBox1.Image = pic1.Resize(pictureBox1.Width, pictureBox1.Height, INTER.CV_INTER_LINEAR).ToBitmap();
                pictureBox2.Image = pic2.Resize(pictureBox2.Width, pictureBox2.Height, INTER.CV_INTER_LINEAR).ToBitmap();
            }
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            Point p = pointTransform(e.Location,map1);
            CursorXText.Text = p.X.ToString();
            CursorYText.Text = p.Y.ToString();
            


        }

        private Point pointTransform(Point p, ColorfulContourMap map)
        {
            Point result = new Point();
            double requiredWidth = Form1.sourceImages[map.imageIndex].Width;
            double requiredHeight = Form1.sourceImages[map.imageIndex].Height;
            result.X = (int)((p.X + 0.0) / pictureBox1.Width * requiredWidth);
            result.Y = (int)((p.Y + 0.0) / pictureBox1.Height * requiredHeight);
            return result;

        }

        private void pictureBox2_MouseMove(object sender, MouseEventArgs e)
        {
            Point p = pointTransform(e.Location, map2);
            CursorXText.Text = p.X.ToString();
            CursorYText.Text = p.Y.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            pic1 = Form1.sourceImages[map1.imageIndex].Clone();
            pic2 = Form1.sourceImages[map2.imageIndex].Clone();
            Transformation.transformation(DNA1, DNA2, ref edgeMatch, ref centroid1, ref centroid2, ref angle);
            angle = angle * 180 / Math.PI;
            angle = -angle;
            Console.WriteLine(centroid1.ToString());
            Console.WriteLine(centroid2.ToString());
            Console.WriteLine(angle);
            mask1 = pic1.Clone();
            mask2 = pic2.Clone();
            ReturnColorImg result = Transformation.transformColor(pic1, mask1, pic2, mask2, joined, joined_mask, centroid1, centroid2, -angle,p1Tweak,p2Tweak);
            joined = result.img;
            pictureBox3.Image = result.img./*Resize(pictureBox1.Width, pictureBox1.Height, INTER.CV_INTER_LINEAR).*/ToBitmap();
            confidence = edgeMatch.confidence;
            overlap = result.overlap;
            ConfidenceView.Text = confidence.ToString();
            OverlapView.Text = overlap.ToString();
            AddMatchHistory();
            

        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form1.sourceImages.Add(joined);
            map1.matched = true;
            map2.matched = true;

            List<ColorfulContourMap> cmap = ColorfulContourMap.getAllContourMap(joined, Form1.sourceImages.Count-1);
            Form1.contourMaps.AddRange(cmap);
            
            


            Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            pic1 = Form1.sourceImages[map1.imageIndex].Clone();
            pic2 = Form1.sourceImages[map2.imageIndex].Clone();
            Transformation.transformation(DNA1, DNA2, ref edgeMatch, ref centroid1, ref centroid2, ref angle);
            angle = angle * 180 / Math.PI;
            angle = -angle;
            Console.WriteLine(centroid1.ToString());
            Console.WriteLine(centroid2.ToString());
            Console.WriteLine(angle);
            mask1 = pic1.Clone();
            mask2 = pic2.Clone();
            ReturnColorImg result = Transformation.transformColor(pic1, mask1, pic2, mask2, joined, joined_mask, centroid1, centroid2, -angle+180,p1Tweak,p2Tweak);
            joined = result.img;
            pictureBox3.Image = result.img./*Resize(pictureBox1.Width, pictureBox1.Height, INTER.CV_INTER_LINEAR).*/ToBitmap();
            confidence = edgeMatch.confidence;
            overlap = result.overlap;
            ConfidenceView.Text = confidence.ToString();
            OverlapView.Text = overlap.ToString();
            AddMatchHistory();
        }

        private void AddMatchHistory()
        {
            Form1.matchHistory.Add(new MatchHistoryData { img1 = pic1, img2 = pic2, confident = confidence, overlap = overlap });
        }

        private void button5_Click(object sender, EventArgs e)
        {
            p1Tweak.Y--;
            label7.Text = p1Tweak.ToString();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            p1Tweak.X++;
            label7.Text = p1Tweak.ToString();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            p1Tweak.Y++;
            label7.Text = p1Tweak.ToString();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            p1Tweak.X--;
            label7.Text = p1Tweak.ToString();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            p2Tweak.Y--;
            label8.Text = p2Tweak.ToString();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            p2Tweak.Y++;
            label8.Text = p2Tweak.ToString();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            p2Tweak.X--;
            label8.Text = p2Tweak.ToString();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            p2Tweak.X++;
            label8.Text = p2Tweak.ToString();
        }
    }
}
