using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace QR
{
    class Program
    {

        static void Main(string[] args)
        {
            string search_dir = Directory.GetCurrentDirectory();

            try
            {
                foreach (var pathes in args)
                {
                    foreach (var path in Directory.GetFiles(search_dir, pathes))
                    {
                        Console.WriteLine(path);
                        var result = QRReader.Read(path);
                        ResultParser red = new ResultParser(result);
                        File.WriteAllBytes(Path.ChangeExtension(path, "dat"), red.GetByte());
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }
    }
}
