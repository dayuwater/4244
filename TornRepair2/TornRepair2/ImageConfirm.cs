using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html;
using System.IO;

namespace TornRepair2
{
    public partial class ImageConfirm : Form
    {
        public ImageConfirm()
        {
            InitializeComponent();
            for (int i = 0; i < Form1.finalImages.Count; i++)
            {

                using (Image<Bgr, Byte> thumbnail = Form1.finalImages[i].Resize(150, 150, INTER.CV_INTER_CUBIC, true))
                {
                    DataGridViewRow row = dataGridView1.Rows[dataGridView1.Rows.Add()];
                    row.Cells["Image"].Value = thumbnail.ToBitmap();
                    row.Height = 150;
                }
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            AddFile af = new AddFile { status = 1 };
            
            af.Show();
        }

        private void ImageConfirm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Form1 fm1 = new Form1();
            fm1.Show();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void ImageConfirm_Activated(object sender, EventArgs e)
        {
            pictureBox1.Image = Form1.coverImage.Resize(pictureBox1.Width, pictureBox1.Height,INTER.CV_INTER_CUBIC).ToBitmap();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
            SaveFileDialog sfd = new SaveFileDialog();
            
            sfd.Filter = "BMP|*.bmp|EMF|*.emf|PNG|*.png|JPG|*.jpeg;*.jpg|GIF|*.gif|TIFF|*.tiff";
            sfd.AddExtension = false;
            
            List<Bitmap> fileToSave = new List<Bitmap>();
            fileToSave.Add(Form1.coverImage.ToBitmap());
            foreach(Image<Bgr,byte> img in Form1.finalImages)
            {
                fileToSave.Add(img.ToBitmap());
            }
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                int index = 1;
                foreach (Bitmap bmp in fileToSave)
                {
                    switch (sfd.FilterIndex)
                    {
                        case 1:
                            bmp.Save(sfd.FileName + "_" + index.ToString() + "." + ImageFormat.Bmp.ToString().ToLower(), ImageFormat.Bmp);
                            Console.WriteLine(sfd.FileName);
                            break;
                        case 2:
                            bmp.Save(sfd.FileName + "_" + index.ToString() + "." + ImageFormat.Emf.ToString().ToLower(), ImageFormat.Emf);
                            break;
                        case 3:
                            bmp.Save(sfd.FileName + "_" + index.ToString() + "." + ImageFormat.Png.ToString().ToLower(), ImageFormat.Png);
                            break;
                        case 4:
                            bmp.Save(sfd.FileName + "_" + index.ToString() + "." + ImageFormat.Jpeg.ToString().ToLower(), ImageFormat.Jpeg);
                            break;
                        case 5:
                            bmp.Save(sfd.FileName + "_" + index.ToString() + "." + ImageFormat.Gif.ToString().ToLower(), ImageFormat.Gif);
                            break;
                        case 6:
                            bmp.Save(sfd.FileName + "_" + index.ToString() + "." + ImageFormat.Tiff.ToString().ToLower(), ImageFormat.Tiff);
                            break;
                      

                    }
                   
                    index++;
                }
                MessageBox.Show("File saved");
            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();

            sfd.Filter = "PDF|*.pdf";
            string filePath="";
            string tempDir = "";
            int fileCount = 0;
            if (sfd.ShowDialog()== DialogResult.OK)
            {
                filePath = sfd.FileName;

                // save the image file, get the directory
                tempDir=temporaryImageOutput(filePath,ref fileCount);
                // link the file to PDF
                if (tempDir == "Error")
                {
                    MessageBox.Show("Duplicated file name");
                    return;
                }
                Document _pdfDocument = new Document(PageSize.A4, 10, 10, 25, 25);

                PdfWriter.GetInstance(_pdfDocument, new FileStream(filePath, FileMode.Create));
                _pdfDocument.Open();

                // output the PDF
               

                _pdfDocument.Add(iTextSharp.text.Image.GetInstance(tempDir + "\\" + 1 + ".png"));
                for(int i=2; i <= fileCount; i++)
                {
                    _pdfDocument.NewPage();
                    _pdfDocument.Add(iTextSharp.text.Image.GetInstance(tempDir + "\\" + i + ".png"));
                }
                _pdfDocument.Close();
            }
           
            
        }

        // since PDF and HTML output require an actual link to the image outputs, use this method to save the image file first
        // return the directory (folder) name
        // WARNING: this method uses the Windows file directory format, might not work correctly in UNIX systems
        private String temporaryImageOutput(string dir, ref int fileCount)
        {
           
            string directoryName=dir.Substring(0, dir.LastIndexOf('\\'))+dir.Substring(dir.LastIndexOf("\\"),dir.LastIndexOf(".")-dir.LastIndexOf("\\"));
            // create a folder at the directory

            if (!Directory.Exists(directoryName))

            {

                Directory.CreateDirectory(directoryName);

            }
            else
            {
                return "Error";
            }
            // export the image into the folder
            List<Bitmap> fileToSave = new List<Bitmap>();
            fileToSave.Add(Form1.coverImage.ToBitmap());
            string fName = "";
            int index = 1;
            foreach (Image<Bgr, byte> img in Form1.finalImages)
            {
                fileToSave.Add(img.ToBitmap());
            }
            foreach(Bitmap bmp in fileToSave)
            {
                fName = index.ToString() + ".png";              
                bmp.Save(directoryName+"\\"+fName,ImageFormat.Png);
                index++;
            }

            // return the folder name
            fileCount = index-1;
            return directoryName;
        }


        //http://www.cnblogs.com/liuxinls/p/3365276.html
        // convert the information in a bitmap image to bytes
        public static byte[] Bitmap2Byte(Bitmap bitmap)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                bitmap.Save(stream, ImageFormat.Jpeg);
                byte[] data = new byte[stream.Length];
                stream.Seek(0, SeekOrigin.Begin);
                stream.Read(data, 0, Convert.ToInt32(stream.Length));
                return data;
            }
        }
    }
}
