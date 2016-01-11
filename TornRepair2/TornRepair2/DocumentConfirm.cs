using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NetOffice;
using Word = NetOffice.WordApi;
using NetOffice.WordApi.Enums;
using NetOffice.WordApi.Tools.Utils;
using NetOffice.WordApi.Tools;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;

namespace TornRepair2
{
    public partial class DocumentConfirm : Form
    {
        private int pageNum = 1;
        private int totalPageNum = 0;
        public List<String> content=new List<string>();
        public DocumentConfirm()
        {
            InitializeComponent();
        }

        private void DocumentConfirm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Form1 fm1 = new Form1();
            fm1.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (pageNum >= totalPageNum)
            {
                MessageBox.Show("This is the last page.");
            }
            else
            {
                pageNum++;
                PageNumDisplay.Text = pageNum.ToString();
                richTextBox1.Text = content[pageNum - 1];
            }
        }

        private void DocumentConfirm_Activated(object sender, EventArgs e)
        {
            totalPageNum = content.Count;
           
            PageNumDisplay.Text = pageNum.ToString();
            PageTotalNumDisplay.Text = totalPageNum.ToString();
            richTextBox1.Text = content[pageNum-1];
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (pageNum < 2)
            {
                MessageBox.Show("This is the first page.");
            }
            else
            {
                pageNum--;
                PageNumDisplay.Text = pageNum.ToString();
                richTextBox1.Text = content[pageNum - 1];
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            
            SaveFileDialog sfd = new SaveFileDialog();

            sfd.Filter = "PDF|*.pdf";
            string filePath = "";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                filePath = sfd.FileName;
                iTextSharp.text.Document _pdfDocument = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, 10, 10, 25, 25);

                PdfWriter.GetInstance(_pdfDocument, new FileStream(filePath, FileMode.Create));
                _pdfDocument.Open();
                // First page
                _pdfDocument.Add(new iTextSharp.text.Paragraph(content[0]));
                // for each other page
                // add a page
                for(int i=1; i < content.Count; i++)
                {
                    _pdfDocument.NewPage();
                    _pdfDocument.Add(new iTextSharp.text.Paragraph(content[i]));
                }

                // _pdfDocument.NewPage();
                _pdfDocument.Close();
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();

            sfd.Filter = "HTML|*.htm;*.html";
            string filePath = "";
           
        }

        private void button7_Click(object sender, EventArgs e)
        {
            content[pageNum - 1] = richTextBox1.Text;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();

            sfd.Filter = "MS Word 2007 and after|*.docx";
            string filePath = "";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                filePath = sfd.FileName;
                
            }
            else
            {
                MessageBox.Show("Failed to save");
                return;
            }
            /*
            // ①：创建WordprocessingDocument实例doc，对应于TEST.docx文件
            using (WordprocessingDocument doc = WordprocessingDocument.Create("1.docx", WordprocessingDocumentType.Document))
            {
                
                // ②：为doc添加MainDocumentPart部分
                MainDocumentPart mainPart = doc.AddMainDocumentPart();

                // ③：为mainPart添加Document，对应于Word里的文档内容部分
                mainPart.Document = new DocumentFormat.OpenXml.Wordprocessing.Document();

                // ④：为Document添加Body，之后所有于内容相关的均在此body中
                Body body = mainPart.Document.AppendChild(new Body());

                // ⑤：添加段落P，P中包含一个文本“TEST”
                DocumentFormat.OpenXml.Drawing.Paragraph p = mainPart.Document.Body.AppendChild(new DocumentFormat.OpenXml.Drawing.Paragraph());
                p.AppendChild(new Run(new Text("TEST")));
            }
            */



        }
    }
}
