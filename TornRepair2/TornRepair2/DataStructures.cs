using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TornRepair2
{
    public struct ColorfulPoint
    {
        public int X;
        public int Y;
        public Bgr color; // Color of the edge


    }

    public struct Phi
    {
        public double x; // x position
        public double y; // y position
        public double theta; // turning angle
        public int l; // arc length
        public Bgr color; // edge color
    }

    public struct Match
    {
        //Match parameters


        public int t11, t12;//starting and ending arc length parameter for piece1
        public int t21, t22;//starting and ending arc length parameter for piece2
        public int x11, y11, x12, y12; //starting and ending coordinates for piece1
        public int x21, y21, x22, y22; //starting and ending coordinates for piece2
        public double confidence;

    }

    public struct ReturnImg
    {
        public Image<Gray, byte> img;
        public Image<Gray, byte> img_mask;
        public Image<Gray, Byte> source1;
        public Image<Gray, Byte> source2;
        public Point center1;
        public Point center2old;
        public Point center2new;
        public LineSegment2D centerLinee;
        public Image<Gray, byte> rimg;
        public bool returnbool;
        public Point translate1; // t1
        public Point translate2; // t2
        public double overlap;
    }

    public struct ReturnColorImg
    {
        public Image<Bgr, byte> img;
        public Image<Bgr, byte> img_mask;
        public Image<Bgr, Byte> source1;
        public Image<Bgr, Byte> source2;
        public Point center1;
        public Point center2old;
        public Point center2new;
        public LineSegment2D centerLinee;
        public Image<Bgr, byte> rimg;
        public bool returnbool;
        public Point translate1; // t1
        public Point translate2; // t2
        public double overlap;
    }

    public struct MatchHistoryData
    {
        public Image<Bgr, byte> img1;
        public Image<Bgr, byte> img2;
        public double confident;
        public double overlap;
    }
}
