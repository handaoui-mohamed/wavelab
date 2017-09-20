using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Security.Cryptography;

namespace Wave_Lab
{
	public class ComparingImages
	{
		public int Compare(Bitmap first, Bitmap second)
		{
            int DiferentPixels = 0;
            Bitmap container = new Bitmap(first.Width, first.Height);
            for (int i = 0; i < first.Width; i++)
            {
                for (int j = 0; j < first.Height; j++)
                {
                    int r1 = second.GetPixel(i, j).R;
                    int g1 = second.GetPixel(i, j).G;
                    int b1 = second.GetPixel(i, j).B;

                    int r2 = first.GetPixel(i, j).R;
                    int g2 = first.GetPixel(i, j).G;
                    int b2 = first.GetPixel(i, j).B;

                    if (r1 != r2 && g1 != g2 && b1 != b2)
                    {
                        DiferentPixels++;
                        container.SetPixel(i, j, Color.Red);
                    }
                    else
                        container.SetPixel(i, j, first.GetPixel(i, j));
                }
            }
            int TotalPixels = first.Width * first.Height;
            float dierence = (float)((float)DiferentPixels / (float)TotalPixels);
            float percentage = dierence * 100;
            percentage = 100 - percentage;
            return Convert.ToInt32(percentage);
		}
	}
}
