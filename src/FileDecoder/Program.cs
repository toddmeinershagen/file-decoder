using System;
using System.IO;
using System.Text;

namespace FileDecoder
{
    class Program
    {
        static void Main(string[] args)
        {
            var inputFile = args.Length > 0 ? args[0] : "fifthird.fld";
            var directoryPath = @"C:\git\toddmeinershagen\file-decoder\files";
            var inputPath = Path.Combine(directoryPath, inputFile);
            var outputPath = Path.Combine(directoryPath, $"{Path.GetFileNameWithoutExtension(inputFile)}-output.txt");

            string[] all = File.ReadAllLines(inputPath);
            StringBuilder stringBuilder = new StringBuilder();        
            for (int i = 1; i < all.Length - 2; i++)
            {
                if (all[i].Length < 61)
                    all[i] = all[i].PadRight(61, ' ');
                var k = uuDecode(all[i]);
                stringBuilder.Append(System.Text.Encoding.UTF8.GetString(k));
            }
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(outputPath))
            {
                file.WriteLine(stringBuilder.ToString()); 
            }        

            Console.WriteLine("done.");
        }

        public static byte[] uuDecode(string buffer)
        {
          
            byte[] outBuffer = new byte[(buffer.Length - 1) / 4 * 3];
            int outIdx = 0;

           
            byte[] asciiBytes = Encoding.ASCII.GetBytes(buffer);

            for (int i = 0; i < asciiBytes.Length; i++)
            {
                asciiBytes[i] = (byte)((asciiBytes[i] - 0x20) & 0x3f);
            }
           
            for (int i = 1; i <= (asciiBytes.Length - 1); i += 4)
            {
                outBuffer[outIdx++] = (byte)(asciiBytes[i] << 2 | asciiBytes[i + 1] >> 4);
                outBuffer[outIdx++] = (byte)(asciiBytes[i + 1] << 4 | asciiBytes[i + 2] >> 2);
                outBuffer[outIdx++] = (byte)(asciiBytes[i + 2] << 6 | asciiBytes[i + 3]);
            }

            return outBuffer;
        }
    }
}

