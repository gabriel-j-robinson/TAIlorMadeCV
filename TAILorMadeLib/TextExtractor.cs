using DocumentFormat.OpenXml.Packaging;
using System.IO;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UglyToad.PdfPig;
using System;

namespace TAILorMadeLib
{
    public class TextExtractor
    {
        public static string ExtractTextFromPdf([FromForm] IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                throw new BadFileException("Error: the file is invalid (either empty or null)");
            }

            string extractedText = string.Empty;
            string fileExtension = Path.GetExtension(file.FileName).ToLower();

            using (var stream = file.OpenReadStream())
            {
                if (fileExtension == ".pdf")
                {
                    extractedText = ExtractTextFromPdf(stream);
                }
                else if (fileExtension == ".docx")
                {
                    extractedText = ExtractTextFromDocx(stream);
                }
                else
                {
                    throw new UnsupportedFileException($"Error: files with the extension {fileExtension} are unsupported. Only .pdf and docx is currently supported.");
                }
            }

            return extractedText;
        }

        private static string ExtractTextFromPdf(Stream stream)
        {
            StringBuilder text = new StringBuilder();
            using (PdfDocument document = PdfDocument.Open(stream))
            {
                foreach (var page in document.GetPages())
                {
                    text.AppendLine(page.Text);
                }
            }
            return text.ToString();
        }

        private static string ExtractTextFromDocx(Stream stream)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                stream.CopyTo(memoryStream);
                using (WordprocessingDocument doc = WordprocessingDocument.Open(memoryStream, false))
                {
                    return doc.MainDocumentPart.Document.Body.InnerText;
                }
            }
        }
    }
}
