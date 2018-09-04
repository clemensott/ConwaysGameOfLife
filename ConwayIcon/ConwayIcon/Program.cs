using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConwayIcon
{
    class Program
    {
        const int margintel = 10;

        static void Main(string[] args)
        {
            int[] squareSizes = new int[] { 89, 107, 71, 142, 284, 188, 225, 150,
                300, 600, 388, 465, 310, 620, 1240, 55, 66, 44, 88, 176, 16,
                24, 48, 256, 50, 63, 75, 100, 200, 96, 48, 36, 30, 24 };

            int[,] rechtekSizes = new int[,] { { 388, 188 }, { 465, 225 }, { 310, 150 },
                { 620, 300 }, { 1240, 600 }, { 775, 375 }, { 930, 450 }, { 2480, 1200 } };

            Bitmap bmp = Create();

            foreach (int size in squareSizes)
            {
                //   SaveSquare(bmp,size);
            }

            for (int i = 0; i < rechtekSizes.GetLength(0); i++)
            {
                SaveRecheteck(bmp, rechtekSizes[i, 0], rechtekSizes[i, 1]);
            }
        }

        private static void SaveSquare(Bitmap bmp, int b)
        {
            Bitmap bmpOut = new Bitmap(bmp, b, b);
            bmpOut.Save(string.Format("Icon{0}.png", b), ImageFormat.Png);

            bmpOut.Dispose();
        }

        private static void SaveRecheteck(Bitmap bmp, int a, int b)
        {
            Bitmap bmp2 = new Bitmap(bmp.Width * a / b, bmp.Height);

            using (Graphics g = Graphics.FromImage(bmp2))
            {
                g.DrawImage(bmp, (bmp2.Width-bmp.Width) / 2, 0);
            }

            Bitmap bmpOut = new Bitmap(bmp2, a, b);
            bmpOut.Save(string.Format("Icon{0}Width.png", b), ImageFormat.Png);

            bmp2.Dispose();
            bmpOut.Dispose();
        }

        private static Bitmap Create()
        {
            int a = 10, f = 200;

            Color color = Color.White;
            Bitmap bmp = new Bitmap(a * f, a * f);

            SetPixel(bmp, 2, 2, f, color);
            SetPixel(bmp, 2, 3, f, color);
            SetPixel(bmp, 2, 4, f, color);
            SetPixel(bmp, 2, 5, f, color);
            SetPixel(bmp, 2, 6, f, color);
            SetPixel(bmp, 2, 7, f, color);

            SetPixel(bmp, 3, 1, f, color);
            SetPixel(bmp, 3, 2, f, color);
            SetPixel(bmp, 3, 3, f, color);
            SetPixel(bmp, 3, 4, f, color);
            SetPixel(bmp, 3, 5, f, color);
            SetPixel(bmp, 3, 6, f, color);
            SetPixel(bmp, 3, 7, f, color);
            SetPixel(bmp, 3, 8, f, color);

            SetPixel(bmp, 4, 1, f, color);
            SetPixel(bmp, 4, 8, f, color);

            SetPixel(bmp, 5, 1, f, color);
            SetPixel(bmp, 5, 8, f, color);

            SetPixel(bmp, 6, 1, f, color);
            SetPixel(bmp, 6, 2, f, color);
            SetPixel(bmp, 6, 7, f, color);
            SetPixel(bmp, 6, 8, f, color);

            SetPixel(bmp, 7, 2, f, color);
            SetPixel(bmp, 7, 7, f, color);

            return bmp;
        }

        private static void SetPixel(Bitmap bmp, int x, int y, int width, Color color)
        {
            for (int i = width / margintel; i + width / margintel < width; i++)
            {
                for (int j = width / margintel; j + width / margintel < width; j++)
                {
                    bmp.SetPixel(x * width + i, y * width + j, color);
                }
            }
        }
    }
}
