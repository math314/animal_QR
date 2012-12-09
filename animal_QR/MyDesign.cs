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

        public MyDesign(string[] pathes){
            if (pathes.Length != 4)
                throw new ArgumentException(string.Format("{0}分割のファイルは読み込めません、4分割ファイルのみ読み込み可能です。",pathes.Length));

            byte[] use;
            using (MemoryStream ms = new MemoryStream(2500))
            {
                for (int i = 0; i < 4; i++)
                {
                    var result = QRReader.Read(pathes[i]);
                    ResultParser red = new ResultParser(result);
                    byte[] b = red.GetByte();
                    ms.Write(b, 0, b.Length);
                }

                ms.Seek(0, SeekOrigin.Begin);
                use = new byte[ms.Length];
                ms.Read(use, 0, use.Length);
            }

            init(use);
        }

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

        private BitSource CreateBitSource(byte[] b){
            sbyte[] s = new sbyte[b.Length];
            Buffer.BlockCopy(b,0,s,0,b.Length);
            return new BitSource(s);
        }

        public Image CreateImage()
        {
            BitSource bits = CreateBitSource(bmp);
            int h,w;
            if(bmp.Length == 512){
                h = w = 32;
            } else {
                h = 128;
                w = 32;
            }
            //h * w
            Bitmap bitmap = new Bitmap(w, h);

            for (int i = 0; i < h; i++)
            {
                for (int j = 0; j < w; j++)
                {
                    int clr = bits.readBits(4);
                    bitmap.SetPixel(j, i, ColorTable.GetColor(Parret[clr]));
                }
            }

            return bitmap;
        }

    }
}
