using Emgu.CV;
using Emgu.CV.OCR;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TornRepair2
{
    public static class OCR
    {
        
        // read the text from an image
        public static String ReadText(Image<Bgr,byte> img)
        {
            Tesseract _ocr=new Tesseract("", "eng", Tesseract.OcrEngineMode.OEM_TESSERACT_CUBE_COMBINED);

            Image<Gray, byte> gray = img.Convert<Gray, Byte>();
            {
                _ocr.Recognize(gray);
                Tesseract.Charactor[] charactors = _ocr.GetCharactors();
                /*foreach (Tesseract.Charactor c in charactors)
                {
                    img.Draw(c.Region, new Bgr(0,255,0), 1);
                }*/

                //imageBox1.Image = image;

                //String text = String.Concat( Array.ConvertAll(charactors, delegate(Tesseract.Charactor t) { return t.Text; }) );
                //String text = _ocr.GetText();
                
            }
            return _ocr.GetText();
        }
    }
}
