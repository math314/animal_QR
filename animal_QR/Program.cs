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
            Read4();
            return;

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

        static void Read4()
        {
            string dir = @"C:\tools\etc\Dropbox\かいせきどうぶつの森\サンプル\マイデザイン\4分割";
            string path = "*.png";

            var pathes = Directory.GetFiles(dir, path);
            MyDesign design = new MyDesign(pathes);

            design.CreateImage().Save("test.bmp");
        }
    }
}
