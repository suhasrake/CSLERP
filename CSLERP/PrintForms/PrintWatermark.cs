using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CSLERP.PrintForms
{
    class PrintWaterMark
    {
        public static void PdfStampWithNewFile(string watermarkImagePath, string sourceFilePath)
        {
            try
            {
                System.IO.File.Delete(sourceFilePath + "w");
                System.IO.File.Copy(sourceFilePath, sourceFilePath + "w");
                System.IO.File.Delete(sourceFilePath);
                var pdfReader = new PdfReader(sourceFilePath + "w");
                var pdfStamper = new PdfStamper(pdfReader, new FileStream(sourceFilePath, FileMode.Create));
                var image = iTextSharp.text.Image.GetInstance(watermarkImagePath);
                ////image.SetAbsolutePosition(15, 200);
                image.SetAbsolutePosition(15, 400);
                for (var i = 0; i < pdfReader.NumberOfPages; i++)
                {
                    var content = pdfStamper.GetUnderContent(i + 1);
                    content.AddImage(image);
                }
                pdfStamper.Close();
                pdfReader.Close();
                System.IO.File.Delete(sourceFilePath + "w");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while Watermarking pdf");
            }
        }

    }
}
