using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkdownAuthoring.ComponentHelpers
{
    public class FileParser
    {
        public static string ReturnTextContent(string fileName = "output.txt")
        {
            StringBuilder sb = new StringBuilder();
            using (System.IO.StreamReader file = new System.IO.StreamReader(fileName))
            {
                sb.Append(file.ReadToEnd());
            }
            return sb.ToString();
        }
        public static void WriteTextToFile(string fileName, string text) {
            try
            {
                System.IO.File.WriteAllText(fileName, text);
            }
            catch { }
        }
    }
}
