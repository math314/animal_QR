using com.google.zxing;
using com.google.zxing.common;
using com.google.zxing.qrcode;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace QR
{
    static class QRReader
    {
        static public Result Read(string path)
        {
            using (Bitmap bmp = new Bitmap(path))
            {
                Reader reader = new QRCodeReader();
                LuminanceSource source = new RGBLuminanceSource(bmp, bmp.Width, bmp.Height);
                BinaryBitmap image = new BinaryBitmap(new HybridBinarizer(source));

                //QRコードを高速で探さずに精査するオプション
                Hashtable hints = new Hashtable();
                hints[DecodeHintType.TRY_HARDER] = true;

                return reader.decode(image, hints);
            }
        }
    }
}
