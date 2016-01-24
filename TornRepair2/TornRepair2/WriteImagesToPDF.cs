using Emgu.CV;
using Emgu.CV.Structure;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TornRepair2
{
    public class WriteImagesToPDF
    {
        private string filePath = "";
        private List<Image<Bgr, byte>> images=new List<Image<Bgr, byte>>();
        private List<string> description = new List<string>(); // assume the images and description are 1-1

        public WriteImagesToPDF(string filePath)
        {
            this.filePath = filePath;
        }

        // 1-1 add
        public void AddImage(Image<Bgr,byte> img, string description="" )
        {
            images.Add(img);
            this.description.Add(description);
        }

        // output the PDF file
        public void print()
        {
            string tempDir = "";
            int fileCount = 0;
            // save the image file, get the directory
            tempDir = temporaryImageOutput(filePath, ref fileCount);
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

            _pdfDocument.Add(new Paragraph(description[0]));
            iTextSharp.text.Image imm = iTextSharp.text.Image.GetInstance(tempDir + "\\" + 1 + ".png");
            imm.ScaleToFit(PageSize.A5);
            _pdfDocument.Add(imm);
            for (int i = 2; i <= fileCount; i++)
            {
                _pdfDocument.NewPage();
                _pdfDocument.Add(new Paragraph(description[i - 1]));
                iTextSharp.text.Image ima = iTextSharp.text.Image.GetInstance(tempDir + "\\" + i + ".png");
                ima.ScaleToFit(PageSize.A5);
                _pdfDocument.Add(ima);
            }
            _pdfDocument.Close();

        }
        // since PDF and HTML output require an actual link to the image outputs, use this method to save the image file first
        // return the directory (folder) name
        // WARNING: this method uses the Windows file directory format, might not work correctly in UNIX systems
        private String temporaryImageOutput(string dir, ref int fileCount)
        {

            string directoryName = dir.Substring(0, dir.LastIndexOf('\\')) + dir.Substring(dir.LastIndexOf("\\"), dir.LastIndexOf(".") - dir.LastIndexOf("\\"));
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
           
            string fName = "";
            int index = 1;
            foreach (Image<Bgr, byte> img in images)
            {
                fileToSave.Add(img.ToBitmap());
            }
            foreach (Bitmap bmp in fileToSave)
            {
                fName = index.ToString() + ".png";
                bmp.Save(directoryName + "\\" + fName, ImageFormat.Png);
                index++;
            }

            // return the folder name
            fileCount = index - 1;
            return directoryName;
        }





    }
}
