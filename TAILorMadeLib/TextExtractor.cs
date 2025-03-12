using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TAILorMadeLib
{
    public class TextExtractor
    {
        public static string ExtractTextFromPdf([FromForm] IFormFile resumePdf)
        {
            throw new NotImplementedException();
        }
    }
}
