using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;

namespace DDV
{
    /**  All the methods and glossary variables that can be made static should be made static so that DDVLayoutManager and Form1.cs 
     * can access them in the same way.
     * 
     * Utils mainly contains glossary alias constants, color palettes, and image writing utils.
     */ 
    static class utils
    {
        public static char T = '0';
        public static char A = '1';
        public static char G = '2';
        public static char C = '3';

        public static char R = 'R';
        public static char Y = 'Y';
        public static char S = 'S';
        public static char W = 'W';
        public static char K = 'K';
        public static char M = 'M';
        public static char B = 'B';
        public static char D = 'D';
        public static char H = 'H';
        public static char V = 'V';
        public static char N = 'N';
        
        public static Dictionary<char, byte> colorIndex = new Dictionary<char, byte>{ //since this table has redundant lower case values, there's no need to .ToUpper() your strings
             {'A', 1},  {'a', 1},
             {'G', 2},  {'g', 2},
             {'T', 3},  {'t', 3}, {'U', 3},  {'u', 3},
             {'C', 4},  {'c', 4},
             {'N', 5},  {'n', 5},//* N.................any base
             {'R', 6},  {'r', 6}, //* R.................A or G
             {'Y', 7},  {'y', 7}, //* Y.................C or T
             {'S', 8},  {'s', 8}, //* S.................G or C
             {'W', 9},  {'w', 9}, //* W.................A or T
             {'K', 10}, {'k', 10},//* K.................G or T
             {'M', 11}, {'m', 11},//* M.................A or C
             {'B', 12}, {'b', 12},//* B.................C or G or T
             {'D', 13}, {'d', 13},//* D.................A or G or T
             {'H', 14}, {'h', 14},//* H.................A or C or T
             {'V', 15}, {'v', 15} //* V.................A or C or G
        };

        public static string ConvertToDigits(string strTGACN)
        {
            strTGACN = strTGACN.Replace('T', T);
            strTGACN = strTGACN.Replace('A', A);
            strTGACN = strTGACN.Replace('G', G);
            strTGACN = strTGACN.Replace('C', C);

            return strTGACN;
        }

        public static string ConvertToTGACN(string strDigits)
        {

            //strDigits.Replace("0", "N");

            strDigits = strDigits.Replace(T, 'T');
            strDigits = strDigits.Replace(A, 'A');
            strDigits = strDigits.Replace(G, 'G');
            strDigits = strDigits.Replace(C, 'C');
            return strDigits;
        }

        public static string CleanInputFile(string strFile)
        {
            strFile = strFile.Replace("0", "");
            strFile = strFile.Replace("1", "");
            strFile = strFile.Replace("2", "");
            strFile = strFile.Replace("3", "");
            strFile = strFile.Replace("4", "");
            strFile = strFile.Replace("5", "");
            strFile = strFile.Replace("6", "");
            strFile = strFile.Replace("7", "");
            strFile = strFile.Replace("8", "");
            strFile = strFile.Replace("9", "");
            strFile = strFile.Replace(" ", "");
            return strFile.ToUpper();
        }



        public static void SetMyPalette(ref Bitmap b)
        {
            ColorPalette pal = b.Palette;
            for (int i = 0; i < 16; i++)
            {
                pal.Entries[i] = Color.FromArgb(255, 240, 240, 240);
            }

            //Original DDV Colors
            pal.Entries[1] = Color.FromArgb(255, 255, 0, 0); //A
            pal.Entries[2] = Color.FromArgb(255, 0, 255, 0); //G
            pal.Entries[3] = Color.FromArgb(255, 250, 240, 114);//T
            pal.Entries[4] = Color.FromArgb(255, 0, 0, 255);//C

            //Skittle Colors
            //pal.Entries[1] = Color.FromArgb(255, 0, 0, 0); //A
            //pal.Entries[2] = Color.FromArgb(255, 0, 255, 0); //G
            //pal.Entries[3] = Color.FromArgb(255, 0, 0, 255);//T
            //pal.Entries[4] = Color.FromArgb(255, 255, 0, 0);//C

            //Red vs. Blue Colors
            //pal.Entries[3] = Color.FromArgb(255, 253, 174, 97);//T
            //pal.Entries[1] = Color.FromArgb(255, 215, 25, 28); //A
            //pal.Entries[4] = Color.FromArgb(255, 171, 217, 233);//G
            //pal.Entries[2] = Color.FromArgb(255, 44, 123, 182);//C

            pal.Entries[5] = Color.FromArgb(255, 30, 30, 30);//N
            pal.Entries[6] = Color.FromArgb(255, 60, 60, 60);//R
            pal.Entries[7] = Color.FromArgb(255, 70, 70, 70);//Y
            pal.Entries[8] = Color.FromArgb(255, 80, 80, 80);//S
            pal.Entries[9] = Color.FromArgb(255, 90, 90, 90);//W
            pal.Entries[10] = Color.FromArgb(255, 100, 100, 100);//K
            pal.Entries[11] = Color.FromArgb(255, 110, 110, 110);//M
            pal.Entries[12] = Color.FromArgb(255, 120, 120, 120);//B
            pal.Entries[13] = Color.FromArgb(255, 130, 130, 130);//D
            pal.Entries[14] = Color.FromArgb(255, 140, 140, 140);//H
            pal.Entries[15] = Color.FromArgb(255, 150, 150, 150);//V

            pal.Entries[16] = Color.FromArgb(255, 0, 0, 0);//unknown - error

            b.Palette = pal;
        }


        public static unsafe void UnsafeSetPixel(int x, int y, byte c, ref BitmapData bmd)
        {
            // BitmapData bmd = b.LockBits(new Rectangle(0, 0, b.Width, b.Height),
            //                ImageLockMode.ReadWrite, b.PixelFormat);
            byte* p = (byte*)bmd.Scan0.ToPointer();
            int offset = y * bmd.Stride + x;
            p[offset] = c;
        }


        public static void Write1BaseToBMP(char nucleotide, ref Bitmap Tex, int x, int y, ref BitmapData bmd)
        {
            byte bytePaletteIndex = 16;
            bytePaletteIndex = colorIndex[nucleotide];

            UnsafeSetPixel(x, y, bytePaletteIndex, ref bmd);
        }

    }
}
