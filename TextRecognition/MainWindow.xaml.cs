using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media.Imaging;
using System.IO;
using System.Drawing;
using XnaFan.ImageComparison;
using System.Windows.Interop;

namespace TextRecognition
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<StructureItem> items = new List<StructureItem>();
        private static int numberOfSentece = 0;
        private int totalWidth = 0;
        byte[] pixels;
        int stride = 0;
        private System.Drawing.Image[] letters = new System.Drawing.Image[]
        {
            System.Drawing.Image.FromFile("Images/a.png"),
            System.Drawing.Image.FromFile("Images/b.png"),
            System.Drawing.Image.FromFile("Images/c.png"),
            System.Drawing.Image.FromFile("Images/d.png"),
            System.Drawing.Image.FromFile("Images/e.png"),
            System.Drawing.Image.FromFile("Images/f.png"),
            System.Drawing.Image.FromFile("Images/g.png"),
            System.Drawing.Image.FromFile("Images/h.png"),
            System.Drawing.Image.FromFile("Images/i.png"),
            System.Drawing.Image.FromFile("Images/j.png"),
            System.Drawing.Image.FromFile("Images/k.png"),
            System.Drawing.Image.FromFile("Images/l.png"),
            System.Drawing.Image.FromFile("Images/m.png"),
            System.Drawing.Image.FromFile("Images/n.png"),
            System.Drawing.Image.FromFile("Images/o.png"),
            System.Drawing.Image.FromFile("Images/p.png"),
            System.Drawing.Image.FromFile("Images/q.png"),
            System.Drawing.Image.FromFile("Images/r.png"),
            System.Drawing.Image.FromFile("Images/s.png"),
            System.Drawing.Image.FromFile("Images/t.png"),
            System.Drawing.Image.FromFile("Images/u.png"),
            System.Drawing.Image.FromFile("Images/v.png"),
            System.Drawing.Image.FromFile("Images/w.png"),
            System.Drawing.Image.FromFile("Images/x.png"),
            System.Drawing.Image.FromFile("Images/y.png"),
            System.Drawing.Image.FromFile("Images/z.png"),
            System.Drawing.Image.FromFile("Images/space.png")
        };

        public MainWindow()
        {
            InitializeComponent();
        }

        private void GenerateText(object sender, RoutedEventArgs e)
        {
            Random random = new Random();
            int length = random.Next(1, 30);
            length = 100;
            List<System.Drawing.Image> sentence = new List<System.Drawing.Image>();
            Alphabet.Instance();
            for (int letterIndex = 0; letterIndex < length; letterIndex++)
            {
                int letterValue = random.Next(0, Alphabet.GetAlphabetLength() - 1);
                sentence.Add(letters[letterValue]);
                totalWidth += letters[letterValue].Width;
            }
            int totalHeight = 26;
            Bitmap result = new Bitmap(totalWidth, totalHeight);
            result.SetResolution(300, 300);
            Graphics g = Graphics.FromImage(result);

            int xPosition = 0;
            for (int letterIndex = 0; letterIndex < length; letterIndex++)
            {
                xPosition += letterIndex == 0 ? 0 : sentence[letterIndex - 1].Width;
                g.DrawImage(sentence[letterIndex], new System.Drawing.Point(xPosition, 0));
            }
            g.Dispose();
            result.Save(numberOfSentece.ToString() + "text.gif", System.Drawing.Imaging.ImageFormat.Gif);

            string currentPath = AppDomain.CurrentDomain.BaseDirectory;
            itextGeneration.Source = new BitmapImage(new Uri(currentPath + numberOfSentece.ToString() + "text.gif"));
            numberOfSentece++;
            BitmapImage textImage = (BitmapImage)itextGeneration.Source;
            int pxl = textImage.Format.BitsPerPixel;
            pixels = Alphabet.Instance().BitmapSourceToArray(textImage, ref stride);
            
        }

        private void crop(object sender, RoutedEventArgs e)
        {
            FillStructure();
            tbResult.Text = GetTextFromStructure(items);
        }

        private string GetTextFromStructure(List<StructureItem> items)
        {
            string textRecongized = string.Empty;
            items[items.Count - 1].GetMinimum();
            textRecongized = items[items.Count - 1].GetMinimumWord();
            return textRecongized;
        }


        void BenchmarkAddTime(string time)
        {
            string text = tbBenchmark.Text;
            text = text + time + "\n";
            tbBenchmark.Text = text;
        }

        private void FillStructure()
        {
            var source = itextGeneration.Source;
            BitmapImage image = (BitmapImage)source;
            var toRecognize = BitmapImage2Bitmap(image);
            for (var structureIndex = 0; structureIndex < totalWidth; structureIndex++)
            {
                StructureItem item;
                if (structureIndex == 0)
                {
                    item = new StructureItem(null);
                }
                else
                {
                    item = new StructureItem(items[structureIndex - 1]);
                }
                items.Add(item);
            }
            //weak place hear
            for (int structureIndex = 0; structureIndex < totalWidth; structureIndex++)
            {
                for (int letterIndex = 0; letterIndex < Alphabet.GetAlphabetLength(); letterIndex++)
                {
                    byte[] originalLetter = Alphabet.Instance().GetLetterImage(items[structureIndex].nodes[letterIndex].currentLetter);
                    int letterStride = Alphabet.Instance().GetWidthOfLetter(items[structureIndex].nodes[letterIndex].currentLetter);
                    if (letterStride/8 <= stride/8 - structureIndex)
                    {
                        float difference = GetDifference(originalLetter, letterStride, structureIndex);
                        items[structureIndex + letterStride/8 - 1].nodes[letterIndex].weightToCome = difference;
                        items[structureIndex + letterStride/8 - 1].nodes[letterIndex].fromWhereCame = items[structureIndex].nodes[letterIndex];
                    }
                }
            }
        }

        private float GetDifference(byte[] original, int width, int horizontalShift)
        {
            float difference = .0f;
            for (int y = 0; y < original.Length / width; y++)
            {
                for (int x = 0; x < width/8; x++)
                {
                    difference += (float)(Math.Abs(original[y * (width) + x] - pixels[y * stride +  (x + horizontalShift)]) )/ 255;
                    //difference += (float)(Math.Abs(original[y * (width) + x + 1] - pixels[y * stride + 4 * (x + horizontalShift) + 1]) / 255);
                    //difference += (float)(Math.Abs(original[y * (width) + x + 2] - pixels[y * stride + 4 * (x + horizontalShift) + 2]) / 255);
                }
            }

            return 100 * difference / (original.Length/8);
        }


        private Bitmap BitmapImage2Bitmap(BitmapSource bitmapImage)
        {
            using (var outStream = new MemoryStream())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(bitmapImage));
                enc.Save(outStream);
                Bitmap bitmap = new System.Drawing.Bitmap(outStream);

                return new Bitmap(bitmap);
            }
        }
    }
}
