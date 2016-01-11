using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TornRepair2
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

        // static factory methods
        public static ColorfulContourMap getMaxContourMap(Image<Bgr, byte> input,int index)
        {
            ColorfulContourMap result = new ColorfulContourMap();
            Image<Gray, byte> gray = input.Convert<Gray, byte>();
            gray = gray.SmoothGaussian(3).ThresholdBinaryInv(new Gray(245), new Gray(255)).MorphologyEx(null, CV_MORPH_OP.CV_MOP_CLOSE, 2);
            List<Point> pointList=new List<Point>();
            List<Point> polyPointList = new List<Point>();
            List<ColorfulPoint> cps = new List<ColorfulPoint>();
            List<ColorfulPoint> pcps = new List<ColorfulPoint>();
            using (MemStorage storage1 = new MemStorage())
            {
                Image<Gray, Byte> temp = gray.Clone();
                Contour<Point> contour = temp.FindContours(CHAIN_APPROX_METHOD.CV_CHAIN_APPROX_NONE, RETR_TYPE.CV_RETR_EXTERNAL);
                double area = Math.Abs(contour.Area);
                Contour<Point> maxArea = contour;
                contour = contour.HNext;
                for (; contour != null; contour = contour.HNext)
                {
                    double nextArea = Math.Abs(contour.Area);
                    if (area < nextArea)
                    {
                        area = nextArea;
                        maxArea = contour;
                    }

                }
                Contour<Point> poly = maxArea.ApproxPoly(1.0, storage1);
                pointList = maxArea.ToList();
                polyPointList = poly.ToList();
                foreach(Point p in pointList)
                {
                    ColorfulPoint cp = new ColorfulPoint { X = p.X, Y = p.Y, color = extractPointColor(p, input) };
                    cps.Add(cp);
                }
                foreach (Point p in polyPointList)
                {
                    ColorfulPoint cp = new ColorfulPoint { X = p.X, Y = p.Y, color = extractPointColor(p, input) };
                    pcps.Add(cp);
                }
                result = new ColorfulContourMap(cps, pcps,index);
                result.matched = false;
                
            }
            return result;
        }

        
        // get all of the valid contour maps, valid means circumfence > 10 px
        public static List<ColorfulContourMap> getAllContourMap(Image<Bgr, byte> input, int index)
        {
            // use for all members
            List<ColorfulContourMap> result = new List<ColorfulContourMap>();
            Image<Gray, byte> gray = input.Convert<Gray, byte>();
            gray = gray.SmoothGaussian(3).ThresholdBinaryInv(new Gray(245), new Gray(255)).MorphologyEx(null, CV_MORPH_OP.CV_MOP_CLOSE, 2);
            // one time use
            List<Point> pointList = new List<Point>();
            List<Point> polyPointList = new List<Point>();
            List<ColorfulPoint> cps = new List<ColorfulPoint>();
            List<ColorfulPoint> pcps = new List<ColorfulPoint>();

            // fetch all the contours using Emgu CV
            // fetch all the polys using Emgu CV
            // extract the points and colors
            using (MemStorage storage1 = new MemStorage())
            {
                Image<Gray, Byte> temp = gray.Clone();
                Contour<Point> contour = temp.FindContours(CHAIN_APPROX_METHOD.CV_CHAIN_APPROX_NONE, RETR_TYPE.CV_RETR_EXTERNAL);
                double area = Math.Abs(contour.Area);
                Contour<Point> maxArea = contour; // maxArea is used as the current contour
                //contour = contour.HNext;
                // use this to loop
                for (; contour != null; contour = contour.HNext)
                {
                    double nextArea = Math.Abs(contour.Area);
                    area = nextArea;
                    if (area >= Constants.MIN_AREA)
                    {
                        
                        maxArea = contour;
                        Contour<Point> poly = maxArea.ApproxPoly(1.0, storage1);
                        pointList = maxArea.ToList();
                        polyPointList = poly.ToList();
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
                        result.Add(new ColorfulContourMap(cps, pcps,index));

                    }
                    // clear temporal lists
                    pointList = new List<Point>();
                    polyPointList = new List<Point>();
                    cps = new List<ColorfulPoint>();
                    pcps = new List<ColorfulPoint>();

                }
                
               
            }
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

        public void DrawTo(Image<Bgr, byte> output)
        {
            foreach (ColorfulPoint p in _points)
            {
                output.Draw(new CircleF(new PointF(p.X, p.Y), 1), new Bgr(255,0,0), 2);
            }
            foreach (ColorfulPoint p in _polyPoints)
            {
                output.Draw(new CircleF(new PointF(p.X, p.Y), 1), new Bgr(0,0,255), 2);

            }
        }
        
        public void DrawColorTo(Image<Bgr, byte> output)
        {
            foreach (ColorfulPoint p in _points)
            {
                output.Draw(new CircleF(new PointF(p.X, p.Y), 1), p.color, 2);
            }
            foreach (ColorfulPoint p in _polyPoints)
            {
                output.Draw(new CircleF(new PointF(p.X, p.Y), 1), p.color, 2);

            }
        }

        // forced static
        private static Bgr extractPointColor(Point p,Image<Bgr,byte> input)
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
                            nearbyColors.Add(new Bgr(input.Data[j, i, 0], input.Data[j, i, 1], input.Data[j, i, 2]));
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

        private int IndexOfPolyPoint(ColorfulPoint p)
        {
            for (int i = 0; i < _points.Count; i++)
            {
                if (p.X == _points[i].X && p.Y == _points[i].Y)
                {
                    return i;
                }
            }
            return -1;
        }

        // transformations, plan to inherit in base class but have some technical difficulties for now
        // translate
        public void Translate(int x, int y)
        {
            for (int i = 0; i < _points.Count; i++)
            {
                ColorfulPoint p = _points[i];
                p.X += x;
                p.Y += y;
                _points[i] = p;

            }
            for (int i = 0; i < _polyPoints.Count; i++)
            {
                ColorfulPoint p = _polyPoints[i];
                p.X += x;
                p.Y += y;
                _polyPoints[i] = p;

            }

        }

        // rotate, angle is in degree
        public void Rotate(double angle)
        {
            for (int i = 0; i < _points.Count; i++)
            {
                ColorfulPoint p = _points[i];
                p.X -= Center.X;
                p.Y -= Center.Y;
                int _px, _py;
                _px = (int)(p.X * Math.Cos(angle / (180 / Math.PI)) - p.Y * Math.Sin(angle / (180 / Math.PI)));//rotate by theta
                _py = (int)(p.X * Math.Sin(angle / (180 / Math.PI)) + p.Y * Math.Cos(angle / (180 / Math.PI)));
                p.X = _px + Center.X;
                p.Y = _py + Center.Y;
                _points[i] = p;

            }
            for (int i = 0; i < _polyPoints.Count; i++)
            {
                ColorfulPoint p = _polyPoints[i];
                p.X -= Center.X;
                p.Y -= Center.Y;
                int _px, _py;
                _px = (int)(p.X * Math.Cos(angle / (180 / Math.PI)) - p.Y * Math.Sin(angle / (180 / Math.PI)));//rotate by theta
                _py = (int)(p.X * Math.Sin(angle / (180 / Math.PI)) + p.Y * Math.Cos(angle / (180 / Math.PI)));
                p.X = _px + Center.X;
                p.Y = _py + Center.Y;
                _polyPoints[i] = p;

            }

        }

        public void RotateAboutImage(double angle, Image<Bgr, byte> input)
        {
            for (int i = 0; i < _points.Count; i++)
            {
                ColorfulPoint p = _points[i];
                p.X -= input.Width / 2;
                p.Y -= input.Height / 2;
                int _px, _py;
                _px = (int)(p.X * Math.Cos(angle / (180 / Math.PI)) - p.Y * Math.Sin(angle / (180 / Math.PI)));//rotate by theta
                _py = (int)(p.X * Math.Sin(angle / (180 / Math.PI)) + p.Y * Math.Cos(angle / (180 / Math.PI)));
                p.X = _px + input.Width / 2;
                p.Y = _py + input.Height / 2;
                _points[i] = p;

            }
            for (int i = 0; i < _polyPoints.Count; i++)
            {
                ColorfulPoint p = _polyPoints[i];
                p.X -= input.Width / 2;
                p.Y -= input.Height / 2;
                int _px, _py;
                _px = (int)(p.X * Math.Cos(angle / (180 / Math.PI)) - p.Y * Math.Sin(angle / (180 / Math.PI)));//rotate by theta
                _py = (int)(p.X * Math.Sin(angle / (180 / Math.PI)) + p.Y * Math.Cos(angle / (180 / Math.PI)));
                p.X = _px + input.Width / 2;
                p.Y = _py + input.Height / 2;
                _polyPoints[i] = p;

            }

        }

        // scale, although this will compromise the accuracy of the contour map
        public void Scale(double x, double y = -1)
        {
            if (y < 0)
            {
                y = x;
            }
            for (int i = 0; i < _points.Count; i++)
            {
                ColorfulPoint p = _points[i];
                p.X = (int)(p.X * x);
                p.Y = (int)(p.Y * y);
                _points[i] = p;

            }
            for (int i = 0; i < _polyPoints.Count; i++)
            {
                ColorfulPoint p = _polyPoints[i];
                p.X = (int)(p.X * x);
                p.Y = (int)(p.Y * y);
                _polyPoints[i] = p;

            }

        }
        // scale to a size
        public void ScaleTo(int x, int y)
        {


        }

        // extract this contour map into DNA, the feature map
        public List<Phi> extractDNA()
        {
            List<Phi> DNA = new List<Phi>(); // DNA for poly 
            List<Phi> DNAseq = new List<Phi>(); // DNA for all
            List<Phi> verticies = new List<Phi>();
            // start of extraction
            int i = 0;
            foreach (ColorfulPoint point in _points)
            {
                Phi tempPhi = new Phi();
                tempPhi.x = point.X;
                tempPhi.y = point.Y;
                tempPhi.theta = 0;
                tempPhi.l = i;
                tempPhi.color = point.color;
                DNAseq.Add(tempPhi);
                i++;
            }




            i = 0;
            foreach (ColorfulPoint point in _polyPoints)
            {
                Phi tempPhi = new Phi();
                tempPhi.x = point.X;
                tempPhi.y = point.Y;
                tempPhi.theta = 0;
                tempPhi.l = 0;
                tempPhi.color = point.color;
                verticies.Add(tempPhi);
                i++;
            }
            // interpolate the arc length
            for (int j = 0, t = 0; j < verticies.Count; ++j)
            {
                while (!(verticies[j].x == DNAseq[t].x && verticies[j].y == DNAseq[t].y))
                {
                    t++;
                }

                Phi vert = verticies[j];
                vert.l = t;
                verticies[j] = vert;


                t = 0;

            }
            // End of Functional codes

            // Start of Experimental

            double angle = 0;
            for (i = 0; i < verticies.Count; ++i)
            {
                int next = i + 1;
                if (next == verticies.Count)
                {
                    next = 0;
                }//Bounds check

                //Turning angle computation
                //If this is starting vertex theta = 0
                if (i == 0)
                {
                    angle = 0.0;
                }
                else
                { //Compute turning angle
                    double turn_angle = Geometry.calcAngle(verticies[next].x, verticies[next].y, verticies[i].x, verticies[i].y, verticies[i - 1].x, verticies[i - 1].y);
                    double vector1_x = verticies[i].x - verticies[i - 1].x;
                    double vector1_y = verticies[i].y - verticies[i - 1].y;
                    double vector2_x = verticies[next].x - verticies[i].x;
                    double vector2_y = verticies[next].y - verticies[i].y;
                    int direction = Geometry.sign((vector1_y * vector2_x) - (vector1_x * vector2_y));
                    // Cumulate the turning angles
                    angle += (180 - turn_angle) * direction;
                }

                //Store this turning function value
                Phi vert = verticies[i];
                vert.theta = Math.Round(angle);
                verticies[i] = vert;
                //For all points between vertex[i] and vertex[next] theta will be the same
                // Theta is the total angle turned from start point
                if (verticies[next].l > verticies[i].l)
                {
                    for (int j = verticies[i].l; j <= verticies[next].l; ++j)
                    {
                        Phi v = DNAseq[j];
                        v.theta = Math.Round(angle);
                        DNAseq[j] = v;
                    }
                }
                else
                {
                    for (int j = verticies[i].l; j <= verticies[next].l + Length; ++j)
                    {
                        int index = j;
                        if (index >= Length)
                        {
                            index -= Length;
                        }
                        Phi vv = DNAseq[index];
                        vv.theta = Math.Round(angle);
                        DNAseq[index] = vv;
                    }
                }
            }

            for (i = 0; i < DNAseq.Count; i++)
            {
                int index = i + verticies[0].l + 1;
                if (index >= DNAseq.Count)
                {
                    index -= DNAseq.Count;

                }
                DNA.Add(DNAseq[index]);
                Phi tem = DNA[i];
                tem.l = i;
                DNA[i] = tem;
            }
            return DNA;
        }

        public ColorfulContourMap Clone()
        {
            ColorfulContourMap cmap = new ColorfulContourMap();
            cmap._points = _points.ToList();
            cmap._polyPoints = _polyPoints.ToList();
            cmap.Width = Width;
            cmap.Height = Height;
            cmap.Center = Center;
            return cmap;
        }
    }
}
