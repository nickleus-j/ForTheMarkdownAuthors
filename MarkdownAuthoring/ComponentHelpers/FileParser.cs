using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkdownAuthoring.ComponentHelpers
{
    public class FileParser
    {
        public static string? ReturnTextContentOfFile(string fileName = "output.txt")
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                using (System.IO.StreamReader file = new System.IO.StreamReader(fileName))
                {
                    sb.Append(file.ReadToEnd());
                }
            }
            catch (System.IO.IOException e) { return null; }
            
            return sb.ToString();
        }
        public static string WriteTextToFile(string fileName, string text) {
            try
            {
                System.IO.File.WriteAllText(fileName, text);
            }
            catch { }
            return String.Empty;
        }
    }
}
