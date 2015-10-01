using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace TextRecognition
{
    public class Alphabet
    {
        static Alphabet instance;
        static string appPath = AppDomain.CurrentDomain.BaseDirectory + "Images\\";
        Dictionary<Letters, byte[]> lettersImages = new Dictionary<Letters, byte[]>();
        Dictionary<Letters, int> strides = new Dictionary<Letters, int>();

        static string[] lettersPaths = new string[]
        {
            appPath+"a.png",
            appPath+"b.png",
            appPath+"c.png",
            appPath+"d.png",
            appPath+"e.png",
            appPath+"f.png",
            appPath+"g.png",
            appPath+"h.png",
            appPath+"i.png",
            appPath+"j.png",
            appPath+"k.png",
            appPath+"l.png",
            appPath+"m.png",
            appPath+"n.png",
            appPath+"o.png",
            appPath+"p.png",
            appPath+"q.png",
            appPath+"r.png",
            appPath+"s.png",
            appPath+"t.png",
            appPath+"u.png",
            appPath+"v.png",
            appPath+"w.png",
            appPath+"x.png",
            appPath+"y.png",
            appPath+"z.png",
            appPath+"space.png",
        };

        public static Alphabet Instance()
        {
            if (instance == null)
            {
                instance = new Alphabet();
            }
            return instance;
        }

        public byte[] GetLetterImage(Letters letter)
        {
            return lettersImages[letter];
        }

        public int GetWidthOfLetter(Letters leter)
        {
            return strides[leter];
        }

        private Alphabet()
        {
            for (int letterIndex = 0; letterIndex < Alphabet.GetAlphabetLength(); letterIndex++)
            {
                BitmapImage image = new BitmapImage(new Uri(GetLetterPath((Letters)letterIndex)));
                //new Bitmap(GetLetterPath((Letters)letterIndex));
                int stride = 0;
                int pxl = image.Format.BitsPerPixel;
                lettersImages.Add((Letters)letterIndex, BitmapSourceToArray(image, ref stride));
                strides.Add((Letters)letterIndex, stride);
            }
        }

        public static int GetAlphabetLength()
        {
            return  lettersPaths.Length;
        }

        public static string GetLetterPath(Letters letter)
        {
            return lettersPaths[(int)letter];
        }

        public static float GetProbability(Letters a, Letters b)
        {
            return 1.0f;
        }

        public static char GetLetter(Letters letter)
        {
            if ((int)letter == lettersPaths.Length - 1)
            {
                return ' ';
            }
            else
            {
                return (char)(97 + ((int)letter));
            }
        }

        public byte[] BitmapSourceToArray(BitmapSource bitmapSource, ref int stride)
        {
            // Stride = (width) x (bytes per pixel)
            stride = (int)bitmapSource.PixelWidth * (bitmapSource.Format.BitsPerPixel + 7 / 8);
            byte[] pixels = new byte[(int)bitmapSource.PixelHeight * stride];

            bitmapSource.CopyPixels(pixels, stride, 0);

            return pixels;
        }
    }
}
