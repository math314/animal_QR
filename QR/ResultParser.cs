using com.google.zxing;
using com.google.zxing.common;
using com.google.zxing.qrcode.decoder;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace QR
{

    class ResultParser
    {
        Result _qr;

        public ResultParser(Result result)
        {
            _qr = result;
        }

        public byte[] GetByte()
        {

            BitSource bits = new BitSource(_qr.RawBytes);
            using (MemoryStream ms = new MemoryStream(1024))
            {
                //System.Text.StringBuilder result = new System.Text.StringBuilder(50);
                //CharacterSetECI currentCharacterSetECI = null;
                //bool fc1InEffect = false;
                //System.Collections.ArrayList byteSegments = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList(1));
                Mode mode;
                do
                {
                    // While still another segment to read...
                    if (bits.available() < 4)
                    {
                        // OK, assume we're done. Really, a TERMINATOR mode should have been recorded here
                        mode = Mode.TERMINATOR;
                    }
                    else
                    {
                        try
                        {
                            mode = Mode.forBits(bits.readBits(4)); // mode is encoded by 4 bits
                        }
                        catch (System.ArgumentException)
                        {
                            throw ReaderException.Instance;
                        }
                    }
                    if (!mode.Equals(Mode.TERMINATOR))
                    {
                        if (mode.Equals(Mode.FNC1_FIRST_POSITION) || mode.Equals(Mode.FNC1_SECOND_POSITION))
                        {
                            // We do little with FNC1 except alter the parsed result a bit according to the spec
                            //fc1InEffect = true;
                            throw new Exception("QRコード : Modeを判別できませんでした");
                        }
                        else if (mode.Equals(Mode.STRUCTURED_APPEND))
                        {
                            // not really supported; all we do is ignore it
                            // Read next 8 bits (symbol sequence #) and 8 bits (parity data), then continue
                            bits.readBits(16);
                        }
                        else if (mode.Equals(Mode.ECI))
                        {
                            // Count doesn't apply to ECI
                            //int value_Renamed = parseECIValue(bits);
                            //currentCharacterSetECI = CharacterSetECI.getCharacterSetECIByValue(value_Renamed);
                            //if (currentCharacterSetECI == null)
                            //{
                            //    throw ReaderException.Instance;
                            //}
                            throw new Exception("QRコード : Modeを判別できませんでした");
                        }
                        else
                        {
                            // How many characters will follow, encoded in this mode?
                            int count = bits.readBits(16);
                            if (!mode.Equals(Mode.BYTE))
                                throw new Exception("QRコード : Modeを判別できませんでした");

                            byte[] b = new byte[count];
                            for (int i = 0; i < count; i++)
                                b[i] = (byte)bits.readBits(8);

                            ms.Write(b, 0, count);
                        }
                    }
                }
                while (!mode.Equals(Mode.TERMINATOR));

                ms.Seek(0,SeekOrigin.Begin);
                byte[] ret = new byte[ms.Length];
                ms.Read(ret, 0, ret.Length);

                return ret;
            }

        }

    }
}
