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
    static class Utils
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

        public static string ConvertToDigits(string strTGACN)
        {

            /* AGCT
             *  T="0"
		        A="1"
		        G="2"
		        C="3"
             * 
             * ----------------------------
             * 
             * T (or U)..........Thymine (or Uracil) 
             * 
             * R.................A or G 
             * Y.................C or T 
             * S.................G or C 
             * W.................A or T 
             * K.................G or T 
             * M.................A or C 
             * B.................C or G or T 
             * D.................A or G or T 
             * H.................A or C or T 
             * V.................A or C or G 
             * N.................any base 
             * . or -............gap   * 
             */

            //now convert all to digits:

            strTGACN = strTGACN.Replace('T', T);
            strTGACN = strTGACN.Replace('A', A);
            strTGACN = strTGACN.Replace('G', G);
            strTGACN = strTGACN.Replace('C', C);
            strTGACN = strTGACN.Replace('N', N);
            strTGACN = strTGACN.Replace('V', V);
            strTGACN = strTGACN.Replace('H', H);
            strTGACN = strTGACN.Replace('D', D);
            strTGACN = strTGACN.Replace('B', B);
            strTGACN = strTGACN.Replace('M', M);
            strTGACN = strTGACN.Replace('K', K);
            strTGACN = strTGACN.Replace('W', W);
            strTGACN = strTGACN.Replace('S', S);
            strTGACN = strTGACN.Replace('Y', Y);
            strTGACN = strTGACN.Replace('R', R);
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


        public static void Write1BaseToBMPUncompressed4X(char nucleotide, ref Bitmap Tex, int x, int y, ref BitmapData bmd)
        {
            /*
             * R.................A or G 
             * Y.................C or T 
             * S.................G or C 
             * W.................A or T 
             * K.................G or T 
             * M.................A or C 
             * B.................C or G or T 
             * D.................A or G or T 
             * H.................A or C or T 
             * V.................A or C or G 
             * N.................any base 
             */

            byte bytePaletteIndex = 0;

            if (nucleotide == A){
                bytePaletteIndex = 1;
            }
            else if (nucleotide == G){
                bytePaletteIndex = 2;
            }
            else if (nucleotide == T){
                bytePaletteIndex = 3;
            }
            else if (nucleotide == C){
                bytePaletteIndex = 4;
            }
            //logical FASTA probabilities
            else if (nucleotide == R){
                bytePaletteIndex = 6;
            }
            else if (nucleotide == Y){
                bytePaletteIndex = 7;
            }
            else if (nucleotide == S){
                bytePaletteIndex = 8;
            }
            else if (nucleotide == W){
                bytePaletteIndex = 9;
            }
            else if (nucleotide == K){
                bytePaletteIndex = 10;
            }
            else if (nucleotide == M){
                bytePaletteIndex = 11;
            }
            else if (nucleotide == B){
                bytePaletteIndex = 12;
            }
            else if (nucleotide == D){
                bytePaletteIndex = 13;
            }
            else if (nucleotide == H){
                bytePaletteIndex = 14;
            }
            else if (nucleotide == V){
                bytePaletteIndex = 15;
            }
            else if (nucleotide == N){
                bytePaletteIndex = 5;
            }
            else
            {
                bytePaletteIndex = 16;
            }

            UnsafeSetPixel(x, y, bytePaletteIndex, ref bmd);
        }

    }
}
