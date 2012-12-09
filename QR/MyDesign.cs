using com.google.zxing.common;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace QR
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct MyDesign_core
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string DesignName;

        public short hoge1;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 10)]
        public string UserName;

        public short hoge2;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 10)]
        public string DS_Name;

        public short Version;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public byte[] Parret;

        public int Version2;

        //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 512)]
        //public byte[] bmp_data;
    }

    public class MyDesign
    {
        public string DesignName { get; set; }

        public short hoge1;
        public string UserName { get; set; }

        public short hoge2;
        public string DS_Name { get; set; }

        public short Version { get; set; }

        public byte[] Parret { get; set; }

        public int Version2 { get; set; }

        public byte[] bmp { get; set; }

        public MyDesign(string path)
        {
            var result = QRReader.Read(path);
            ResultParser red = new ResultParser(result);
            init(red.GetByte());
        }

        public MyDesign(byte[] b)
        {
            init(b);
        }

        private void init(byte[] b)
        {
            var design_data = NativeStructHelper.ReadStruct<MyDesign_core>(b);

            //構造体のサイズを取得
            int size = Marshal.SizeOf(typeof(MyDesign_core));

            bmp = new byte[b.Length - size];

            Array.Copy(b, size, bmp, 0, bmp.Length);

            hoge1 = design_data.hoge1;
            hoge2 = design_data.hoge2;
            Version = design_data.Version;
            Version2 = design_data.Version2;

            DesignName = design_data.DesignName;
            UserName = design_data.UserName;
            DS_Name = design_data.DS_Name;
            Parret = design_data.Parret;
        }

        public Image CreateImage()
        {
            //まず32*32
            BitSource bits = new BitSource(bmp);
            Bitmap bitmap = new Bitmap(32, 32);

            for (int i = 0; i < 32; i++)
            {
                for (int j = 0; j < 32; j++)
                {
                    int clr = bits.readBits(4);
                    bitmap.SetPixel(j, i, ColorTable.GetColor(Parret[clr]));
                }
            }

            return bitmap;
        }

    }
}
