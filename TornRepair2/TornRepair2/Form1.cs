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
using Emgu.CV;

namespace TornRepair2

{
   
    public partial class Form1 : Form
    {

        // all the data storages, use public static is fine
        public static int status = 0; // 0 for torn photo, 1 for torn document
        public static List<Image<Bgr, byte>> sourceImages=new List<Image<Bgr, byte>>(); // the source image fragments
        public static List<ColorfulContourMap> contourMaps = new List<ColorfulContourMap>(); // the contour maps
       
        public static List<Image<Bgr, byte>> finalImages = new List<Image<Bgr, byte>>();
        public static List<List<Image<Bgr, byte>>> candidateImages = new List<List<Image<Bgr, byte>>>();
        public static Image<Bgr, byte> coverImage=new Image<Bgr, byte>(640,480);
        public static List<MatchHistoryData> matchHistory = new List<MatchHistoryData>();

        public static bool BKG_WHITE = true; // if the background is white, this is true, if the background is black, this is false
        public static MemStorage mem = new MemStorage();




        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            status = 0;
            TornPieceInput tp1 = new TornPieceInput();
            tp1.Show();
            
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            status = 1;
            TornPieceInput tp1 = new TornPieceInput();
            tp1.Show();
           
            this.Hide();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
