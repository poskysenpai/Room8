using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Room8.Infrastructure.Helpers
{
    public static class IFormFileExtensions
    {
        public static IFormFile ToIFormFile(string base64String, string fileName)
        {
            try
            {
                var base64Array = Convert.FromBase64String(base64String);
                var stream = new MemoryStream(base64Array);
                var file = new FormFile(stream, 0, base64Array.Length, fileName, fileName)
                {
                    Headers = new HeaderDictionary(),
                    ContentType = "image/jpeg"
                };
                return file;
            }
            catch(FormatException ex) 
            {
                throw new Exception("Invalid base64 string", ex);

            }
        }
    }
}
