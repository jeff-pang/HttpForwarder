using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HttpForwarder
{
    public static class StreamExtensions
    {
        public static string AsString(this Stream stream)
        {
            if (stream != null)
            {
                StreamReader reader = new StreamReader(stream);
                string text = reader.ReadToEnd();
                return text;
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
