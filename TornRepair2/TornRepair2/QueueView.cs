using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using System;
using System.Collections;
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

        private void button1_Click(object sender, EventArgs e)
        {
            refresh();
        }

        private void refresh()
        {
            dataGridView1.Rows.Clear();
            for (int i = 0; i < Form1.sourceImages.Count; i++)
            {
                if (/*Form1.matched[i] == false*/true)
                {
                    using (Image<Bgr, Byte> thumbnail = Form1.sourceImages[i].Resize(150, 150, INTER.CV_INTER_CUBIC, true))
                    {
                        DataGridViewRow row = dataGridView1.Rows[dataGridView1.Rows.Add()];
                        row.Cells["SourceImage"].Value = thumbnail.ToBitmap();
                        row.Height = 150;
                    }
                }
            }
            ConfidenceView.Text = confidence.ToString();
            OverlapView.Text = overlap.ToString();
        }

        private void dataGridView1_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            listBox1.Items.Clear();
            num = e.RowIndex;
           
            int index = 1;
            foreach(ColorfulContourMap cmap in Form1.contourMaps)
            {
                if (cmap.imageIndex == num)
                {
                    
                    listBox1.Items.Add("Contour " + index+cmap.matched.ToString() );
                    index++;
                }
            }
            listBox1.SelectedIndex = 0;
           


        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int firstAppear = 0;
            foreach(ColorfulContourMap cmap in Form1.contourMaps)
            {
                if (cmap.imageIndex == num)
                {
                    break;
                }
                firstAppear++;
            }

            Image<Bgr, byte> img1 = Form1.sourceImages[num].Clone();
            Image<Bgr, byte> img2 = Form1.sourceImages[num].CopyBlank();
            Form1.contourMaps[firstAppear + listBox1.SelectedIndex].DrawTo(img1);
            Form1.contourMaps[firstAppear + listBox1.SelectedIndex].DrawColorTo(img2);
            pictureBox1.Image = img1.Resize(pictureBox1.Width, pictureBox1.Height, INTER.CV_INTER_LINEAR).ToBitmap();
            pictureBox2.Image = img2.Resize(pictureBox2.Width, pictureBox2.Height, INTER.CV_INTER_LINEAR).ToBitmap();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (Form1.contourMaps.Count != 1)
            {
                if (ind1 < 0 || ind2 < 0)
                {
                    TwoPieceMatchAnalysis tpma = new TwoPieceMatchAnalysis { map1 = Form1.contourMaps[0], map2 = Form1.contourMaps[1] };
                    tpma.Show();
                }
                else
                {
                    TwoPieceMatchAnalysis tpma = new TwoPieceMatchAnalysis { map1 = Form1.contourMaps[ind1], map2 = Form1.contourMaps[ind2] };
                    tpma.Show();
                }
            }
           
        }

        private void button2_Enter(object sender, EventArgs e)
        {
            refresh();
        }

        private void QueueView_Activated(object sender, EventArgs e)
        {
            refresh();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // determine the index of the first contour map for a image
            int firstAppear = 0;
            foreach (ColorfulContourMap cmap in Form1.contourMaps)
            {
                if (cmap.imageIndex == num)
                {
                    break;
                }
                firstAppear++;
            }
            ind1 = firstAppear + listBox1.SelectedIndex;
            Image<Bgr, byte> img1 = Form1.sourceImages[num].Clone();
           
            Form1.contourMaps[ind1].DrawTo(img1);
            pictureBox3.Image = img1.Resize(pictureBox1.Width, pictureBox1.Height, INTER.CV_INTER_LINEAR).ToBitmap();



        }

        private void button4_Click(object sender, EventArgs e)
        {
            // determine the index of the first contour map for a image
            int firstAppear = 0;
            foreach (ColorfulContourMap cmap in Form1.contourMaps)
            {
                if (cmap.imageIndex == num)
                {
                    break;
                }
                firstAppear++;
            }
            ind2 = firstAppear + listBox1.SelectedIndex;
            Image<Bgr, byte> img1 = Form1.sourceImages[num].Clone();

            Form1.contourMaps[ind2].DrawTo(img1);
            pictureBox4.Image = img1.Resize(pictureBox1.Width, pictureBox1.Height, INTER.CV_INTER_LINEAR).ToBitmap();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Form1.finalImages.Add(Form1.sourceImages[num]);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                Form1.candidateImages[listBox2.SelectedIndex].Add(Form1.sourceImages[num]);
            }
            catch
            {
                MessageBox.Show("Please select a candidate");
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Form1.candidateImages.Add(new List<Image<Bgr, byte>>());
            listBox2.Items.Add("Candidate " + Form1.candidateImages.Count);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            MatchHistory history = new MatchHistory();
            history.Show();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        // algorithm plot:
        // 1. Try edge match
        //      Find max confidence
        //      Try put them together
        // 2. Try color match
        //      Find max confidence
        //      Try put them together
        // 3. If still fail, that is from other page
        // 4. If success, find the best tweak
        // 5. Join the images according to the final parameters

        // the return value is how many pages should be in the original images
        private int bestMatchTwo()
        {
            int pageCount = 0;
            double maxConfidence = 0.0;
            ColorfulContourMap map1 = new ColorfulContourMap();
            ColorfulContourMap map2 = new ColorfulContourMap();
            double overlap = 0.0;
            bool matched = false;
            // Double-thresholding algorithm (Worked for 5-piece precisely torned paper):
            // search for all unmatched pairs, calculate the confidence level ( fixed the problem with different results caused by different ordering )
            // reject all matches that has confidence less than 80 ( eliminate some of the false positives)
            // calculate the intersection for each matching pair
            // the pair with the lowest intersection is the best
            // if the intersection of the lowest pair is still greater than 5000, there are no match for the given fragments

            List<MatchMetricData> matchMetricData = new List<MatchMetricData>();

            // search for all unmatched pairs, calculate the confidence level 
            // if the max confidence level is 0, that means the remaining pieces cannot be matched by turning angle
            // for this case, use the color matching algorithm
            

            // get the potential matching edges
            foreach (ColorfulContourMap cmap in Form1.contourMaps)
            {
                if (cmap.matched == false)
                {
                    foreach (ColorfulContourMap cmap2 in Form1.contourMaps)
                    {
                        if (cmap2.matched == false && cmap != cmap2)
                        {
                            Match match = DNAUtil.partialMatch(cmap.extractDNA(), cmap2.extractDNA());
                            double confidence = match.confidence;
                            matchMetricData.Add(new MatchMetricData { map1 = cmap, map2 = cmap2, confident = confidence, dna1 = cmap.extractDNA(), dna2 = cmap2.extractDNA(), match = match });
                            if (confidence > maxConfidence)
                            {
                                maxConfidence = confidence;
                                map1 = cmap;
                                map2 = cmap2;
                            }
                        }
                    }
                }
            }
            // if there are no pieces eligible for edge matching, the metric data should have all elements with confidence level 0
            // at this time, calculate the metrics for color matching
            // Double-thresholding algorithm works for color matching too
            if (matchMetricData.Count==0||matchMetricData.OrderBy(o=>o.confident).Last().confident==0)
            {
                foreach (ColorfulContourMap cmap in Form1.contourMaps)
                {
                    if (cmap.matched == false)
                    {
                        foreach (ColorfulContourMap cmap2 in Form1.contourMaps)
                        {
                            if (cmap2.matched == false && cmap != cmap2)
                            {
                                Match match = DNAUtil.partialColorMatch(cmap.extractDNA(), cmap2.extractDNA());
                                double confidence = match.confidence;
                                matchMetricData.Add(new MatchMetricData { map1 = cmap, map2 = cmap2, confident = confidence, dna1 = cmap.extractDNA(), dna2 = cmap2.extractDNA(), match = match });
                                if (confidence > maxConfidence)
                                {
                                    maxConfidence = confidence;
                                    map1 = cmap;
                                    map2 = cmap2;
                                }
                            }
                        }
                    }
                }

            }

            // if there are still no matches, the process is done
            // It will consider the rest of the pieces from another page

            // 1st funnel: select the most potential matching edges
            matchMetricData = matchMetricData.Where(o => o.confident > 60).OrderBy(o => o.confident).Reverse().ToList();
            Console.WriteLine(maxConfidence);
            Console.WriteLine(map1.imageIndex);
            Console.WriteLine(map2.imageIndex);

            // calculate the intersection for each matching pair
            List<MatchMetricData> data2 = new List<MatchMetricData>();
            foreach (MatchMetricData m in matchMetricData)
            {
                Image<Bgr, byte> pic1 = Form1.sourceImages[m.map1.imageIndex].Clone();
                Image<Bgr, byte> pic2 = Form1.sourceImages[m.map2.imageIndex].Clone();
                Point centroid1 = new Point();
                Point centroid2 = new Point();
                double angle = 0.0;
                Match edgeMatch = m.match;
                Transformation.transformation(m.dna1, m.dna2, ref edgeMatch, ref centroid1, ref centroid2, ref angle);
                angle = angle * 180 / Math.PI;
                angle = -angle;
                Console.WriteLine(centroid1.ToString());
                Console.WriteLine(centroid2.ToString());
                Console.WriteLine(angle);
                Image<Bgr, byte> mask1 = pic1.Clone();
                Image<Bgr, byte> mask2 = pic2.Clone();
                Image<Bgr, byte> joined = pic1.Clone();
                Image<Bgr, byte> joined_mask = joined.Clone();
                ReturnColorImg result = Transformation.transformColor(pic1, mask1, pic2, mask2, joined, joined_mask, centroid1, centroid2, -angle + 180, new Point(0, 0), new Point(0, 0));
                data2.Add(new MatchMetricData { map1 = m.map1, map2 = m.map2, overlap = result.overlap, dna1 = m.dna1, dna2 = m.dna2, match = m.match });


            }
            if (data2.Count == 0)
            {
                MessageBox.Show("No match found");
                return 1; // temporary code, just to let the program run
            }
            MatchMetricData MinOverlap = data2.OrderBy(o => o.overlap).First();
            // the pair with highest confidence with a valid intersection is the best
            // 2nd funnel: select only the matches that can actually match the picture together
            if (MinOverlap.overlap < Constants.THRESHOLD)
            {
                Console.WriteLine("Map1 " + MinOverlap.map1.imageIndex);
                Console.WriteLine("Map2 " + MinOverlap.map2.imageIndex);
                Console.WriteLine("Overlap " + MinOverlap.overlap); // correct until this point
                // add the resulting image into the queue
                Image<Bgr, byte> pic1 = Form1.sourceImages[MinOverlap.map1.imageIndex].Clone();
                Image<Bgr, byte> pic2 = Form1.sourceImages[MinOverlap.map2.imageIndex].Clone();
                Point centroid1 = new Point();
                Point centroid2 = new Point();
                double angle = 0.0;
                Match edgeMatch = MinOverlap.match;
                Transformation.transformation(MinOverlap.dna1, MinOverlap.dna2, ref edgeMatch, ref centroid1, ref centroid2, ref angle);
                // correct until this point
                angle = angle * 180 / Math.PI;
                angle = -angle;

                Image<Bgr, byte> mask1 = pic1.Clone();
                Image<Bgr, byte> mask2 = pic2.Clone();
                Image<Bgr, byte> joined = pic1.Clone();
                Image<Bgr, byte> joined_mask = joined.Clone();


                // tweak
                ReturnColorImg bestResult = new ReturnColorImg();
                double minOverlap = 999999;
                Console.WriteLine(centroid1.ToString());
                Console.WriteLine(centroid2.ToString()); // correct until this point

                // The tweaking is messing up with the result, I will just ignore this for a moment
                // the tweaking now works for 1-button matching
                // but the algorithm runs extremely slow ( still way faster than gluing the pieces :)
                for (int i = -2; i < 3; i++)
                {
                    for (int j = -2; j < 3; j++)
                    {
                        
                        ReturnColorImg result = Transformation.transformColor(pic1, mask1, pic2, mask2, joined, joined_mask, centroid1, centroid2, -angle + 180, new Point(0, 0), new Point(i, j));
                        if (result.overlap < minOverlap&&result.success) // if the overlap is 0, that means the transformation is failed
                        {
                            bestResult = result;
                            minOverlap = result.overlap;

                        }
                        //DisplayImage dip = new DisplayImage(result.img, new Point(0,0), new Point(i, j), (int)result.overlap);
                        //dip.Show();

                    }
                }
                Form1.sourceImages.Add(bestResult.img);
                MinOverlap.map1.matched = true; // correct
                MinOverlap.map2.matched = true; // correct
                List<ColorfulContourMap> cmap = ColorfulContourMap.getAllContourMap(bestResult.img, Form1.sourceImages.Count - 1);
                Form1.contourMaps.AddRange(cmap);
                this.overlap += bestResult.overlap; // correct

                refresh();
                MessageBox.Show("Best Match Found.");
                return 0;

            }
            else
            {
                Console.WriteLine("Failed");
                return 1;
            }
           

        }

        private void button9_Click(object sender, EventArgs e)
        {
            bestMatchTwo();


        }

        private void button10_Click(object sender, EventArgs e)
        {
            // because only n-1 best matches for two pieces is needed for auto matching, the auto matching process is really simple

            // get the image count
            int count = Form1.sourceImages.Count;
            int pageCount = 1;
            // best match for n-1 times
            for(int i=1; i< count; i++)
            {
                pageCount+=bestMatchTwo();
            }
            MessageBox.Show(pageCount.ToString());
            

            // try to put multiple results into the output manager if the source image parts is from multiple pages
            Form1.finalImages.AddRange(Form1.sourceImages.GetRange(Form1.sourceImages.Count-pageCount,pageCount));

        }
    }
}
