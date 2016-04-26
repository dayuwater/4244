using Emgu.CV;
using Emgu.CV.Util;
using Emgu.CV.CvEnum;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV.Structure;

namespace TornRepair3
{
    public class ColorfulContourMap
    {
        public List<ColorfulPoint> _points;
        public List<ColorfulPoint> _polyPoints;
        public int Length { get; internal set; }
        public int Width { get; internal set; }
        public int Height { get; internal set; }
        public Point Center { get; internal set; }
        public int imageIndex { get; internal set; }
        public bool matched = false;

        // get all of the valid contour maps, valid means circumfence > 200 px
        // this was not in their code, I added this feature, but I used their logic
        public static List<ColorfulContourMap> getAllContourMap(Mat input, int index, int mode = 0)
        {
            // use for all members
            List<ColorfulContourMap> result = new List<ColorfulContourMap>();
            MatImage m1 = new MatImage(input);
            m1.Convert();
            Mat gray = m1.Out();
            // use for black background
            if (mode == 0)
            {
                MatImage m2 = new MatImage(gray);
                m2.SmoothGaussian(3);
                m2.ThresholdBinaryInv(245, 255);
                gray = m2.Out();
            }
            // use for white background
            else
            {
                MatImage m2 = new MatImage(gray);
                m2.SmoothGaussian(3);
                m2.ThresholdBinaryInv(100, 255);
                gray = m2.Out();
            }


            // one time use
            List<Point> pointList = new List<Point>();
            List<Point> polyPointList = new List<Point>();
            List<ColorfulPoint> cps = new List<ColorfulPoint>();
            List<ColorfulPoint> pcps = new List<ColorfulPoint>();

            // fetch all the contours using Emgu CV
            // fetch all the polys using Emgu CV
            // extract the points and colors

            Mat temp = gray.Clone();
            VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint();
            CvInvoke.FindContours(gray, contours, new Mat(), RetrType.List, ChainApproxMethod.ChainApproxNone);

            double area = Math.Abs(CvInvoke.ContourArea(contours[0]));
            VectorOfPoint maxArea = contours[0]; // maxArea is used as the current contour
                                                 //contour = contour.HNext;
                                                 // use this to loop
            for (int i = 0; i < contours.Size; i++)
            {


                double nextArea = Math.Abs(CvInvoke.ContourArea(contours[i], false));  //  Find the area of contour
                area = nextArea;
                if (area >= Constants.MIN_AREA)
                {
                    maxArea = contours[i];
                    VectorOfPoint poly = new VectorOfPoint();
                    CvInvoke.ApproxPolyDP(maxArea, poly, 1.0, true);
                    pointList = maxArea.ToArray().ToList();
                    polyPointList = poly.ToArray().ToList();
                    foreach (Point p in pointList)
                    {
                        ColorfulPoint cp = new ColorfulPoint { X = p.X, Y = p.Y, color = extractPointColor(p, input) };
                        cps.Add(cp);
                    }
                    foreach (Point p in polyPointList)
                    {
                        ColorfulPoint cp = new ColorfulPoint { X = p.X, Y = p.Y, color = extractPointColor(p, input) };
                        pcps.Add(cp);
                    }
                    result.Add(new ColorfulContourMap(cps, pcps, index));

                }

               
            }
            // clear temporal lists
            pointList = new List<Point>();
            polyPointList = new List<Point>();
            cps = new List<ColorfulPoint>();
            pcps = new List<ColorfulPoint>();







            return result;
        }

        // no use at all, just to bypass the IDE error checking
        public ColorfulContourMap()
        {

        }

        public ColorfulContourMap(List<ColorfulPoint> p, List<ColorfulPoint> poly, int index)
        {
            _points = p;
            _polyPoints = poly;
            int minX = 99999;
            int maxX = -99999;
            int minY = 99999;
            int maxY = -99999;
            foreach (ColorfulPoint pp in p)
            {
                if (pp.X > maxX)
                {
                    maxX = pp.X;
                }
                if (pp.Y > maxY)
                {
                    maxY = pp.Y;
                }
                if (pp.X < minX)
                {
                    minX = pp.X;
                }
                if (pp.Y < minY)
                {
                    minY = pp.X;
                }
            }
            Height = maxY - minY;
            Width = maxX - minX;
            Center = new Point((maxX + minX) / 2, (maxY + minY) / 2);
            Length = p.Count;
            imageIndex = index;


        }

        // forced static
        private static Bgr extractPointColor(Point p, Mat input)
        {
            HashSet<Bgr> nearbyColors = new HashSet<Bgr>();

            for (int i = p.X - Constants.AREA_SHIFT; i <= p.X + Constants.AREA_SHIFT; i++)
            {
                for (int j = p.Y - Constants.AREA_SHIFT; j <= p.Y + Constants.AREA_SHIFT; j++)
                {
                    if (i >= 0 && i < input.Width)
                    {
                        if (j >= 0 && j < input.Height)
                        {

                            nearbyColors.Add(new Bgr(input.GetData(j, i)[0], input.GetData(j, i)[1], input.GetData(j, i)[2]));
                        }
                    }
                }
            }

            // check the whiteness of nearby pixels, use the less whiteness pixel as edge color
            double maxWhiteness = 0; // check max
            int index = 0;
            int maxIndex = 0;
            foreach (Bgr c in nearbyColors)
            {
                double whiteness = Metrics.Whiteness(c);
                if (whiteness > maxWhiteness)
                {
                    maxWhiteness = whiteness;
                    maxIndex = index;
                }
                index++;
            }




            //Color ccolor = Color.FromArgb((int)color.Red, (int)color.Green, (int)color.Blue);



            return nearbyColors.ElementAt(maxIndex);

        }

        public void DrawColorTo(Mat output)
        {
            foreach (ColorfulPoint p in _points)
            {
                CvInvoke.Circle(output, new Point(p.X,p.Y), 1, p.color.MCvScalar);
                
            }
            foreach (ColorfulPoint p in _polyPoints)
            {
                CvInvoke.Circle(output, new Point(p.X, p.Y), 1, new Bgr(255,0,0).MCvScalar);

            }
        }


    }
}
