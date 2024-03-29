﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace QR
{
    public class ColorTable
    {
        static int[] table_r = {
              255, 255, 239, 255, 255, 189, 206, 156, 82, 
              255, 255, 222, 255, 255, 206, 189, 189, 140, 
              222, 255, 222, 255, 255, 189, 222, 189, 99, 
              255, 255, 255, 255, 255, 222, 189, 156, 140, 
              255, 239, 206, 189, 206, 156, 140, 82, 49, 
              255, 255, 222, 255, 255, 140, 189, 140, 82, 
              222, 206, 115, 173, 156, 115, 82, 49, 33, 
              255, 255, 222, 255, 255, 206, 156, 140, 82, 
              222, 189, 99, 156, 99, 82, 66, 33, 33, 
              189, 140, 49, 49, 0, 49, 0, 16, 0, 
              156, 99, 33, 66, 0, 82, 33, 16, 0, 
              222, 206, 140, 173, 140, 173, 99, 82, 49, 
              189, 115, 49, 99, 16, 66, 33, 0, 0, 
              173, 82, 0, 82, 0, 66, 0, 0, 0, 
              206, 173, 49, 82, 0, 115, 0, 0, 0, 
              173, 115, 99, 0, 33, 82, 0, 0, 33, 
              255, 239, 222, 206, 189, 
              173, 156, 140, 115, 99, 
              82, 66, 49, 33, 0
            };

        static int[] table_g = {
          239, 154, 85, 101, 0, 69, 0, 0, 32, 
          186, 117, 48, 85, 0, 101, 69, 0, 32, 
          207, 207, 101, 170, 101, 138, 69, 69, 48, 
          239, 223, 207, 186, 170, 138, 101, 85, 69, 
          207, 138, 101, 138, 0, 101, 0, 0, 0, 
          186, 154, 32, 85, 0, 85, 0, 0, 0, 
          186, 170, 69, 117, 48, 48, 32, 16, 16, 
          255, 255, 223, 255, 223, 170, 154, 117, 85, 
          186, 154, 48, 85, 0, 69, 0, 0, 16, 
          186, 154, 48, 85, 0, 48, 0, 16, 0, 
          239, 207, 101, 170, 138, 117, 85, 48, 32, 
          255, 255, 170, 223, 255, 186, 186, 154, 101, 
          223, 207, 85, 154, 117, 117, 69, 32, 16, 
          255, 255, 138, 186, 207, 154, 101, 69, 32, 
          255, 239, 207, 239, 255, 170, 170, 138, 69, 
          255, 255, 223, 255, 223, 186, 186, 138, 69, 
          255, 239, 223, 207, 186, 
          170, 154, 138, 117, 101, 
          85, 69, 48, 32, 0
        };
        static int[] table_b = {
          255, 173, 156, 173, 99, 115, 82, 49, 49, 
          206, 115, 16, 66, 0, 99, 66, 0, 33, 
          189, 99, 33, 33, 0, 82, 0, 0, 16, 
          222, 206, 173, 140, 140, 99, 66, 49, 33, 
          255, 255, 222, 206, 255, 156, 173, 115, 66, 
          255, 255, 189, 239, 206, 115, 156, 99, 66, 
          156, 115, 49, 66, 0, 33, 0, 0, 0, 
          206, 115, 33, 0, 0, 0, 0, 0, 0, 
          255, 239, 206, 255, 255, 140, 156, 99, 49, 
          255, 255, 173, 239, 255, 140, 173, 99, 33, 
          189, 115, 16, 49, 49, 82, 0, 33, 16, 
          189, 140, 82, 140, 0, 156, 0, 0, 0, 
          255, 255, 156, 255, 255, 173, 115, 115, 66, 
          255, 255, 189, 206, 255, 173, 140, 82, 49, 
          239, 222, 173, 189, 206, 173, 156, 115, 49, 
          173, 115, 66, 0, 33, 82, 0, 0, 33, 
          255, 239, 222, 206, 189, 
          173, 156, 140, 115, 99, 
          82, 66, 49, 33, 0
        };

        static int ConvertIndexFromColorTableToPallet(int color_table)
        {
            return 0;
        }

        static int ConvertIndexFromPalletToColorTable(int pallet)
        {
            int u = pallet >> 4;
            int d = pallet & 0x0F;

            if (d == 0x0F)
            {
                //グレースケール
                return 4 * 4 * 9 + u;
            }
            else
            {
                return 9 * u + d;
            }
        }

        static public Color GetColor(int index)
        {
            int parret = ConvertIndexFromPalletToColorTable(index);

            return Color.FromArgb(table_r[parret], table_g[parret], table_b[parret]);
        }
    }
}
