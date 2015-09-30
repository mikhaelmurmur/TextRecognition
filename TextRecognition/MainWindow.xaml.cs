using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;
using System.IO;
using System.Drawing;

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
            var random = new Random();
            var length = random.Next(1, 100);
            var sentence = new List<System.Drawing.Image>();
            Alphabet.Instance();
            for (var letterIndex = 0; letterIndex < length; letterIndex++)
            {
                var letterValue = random.Next(0, Alphabet.GetAlphabetLength()-1);
                sentence.Add(letters[letterValue]);
                totalWidth += letters[letterValue].Width;
            }
            var totalHeight = 26;
            var result = new Bitmap(totalWidth, totalHeight);
            result.SetResolution(300, 300);
            var g = Graphics.FromImage(result);

            var xPosition = 0;
            for (var letterIndex = 0; letterIndex < length; letterIndex++)
            {
                xPosition += letterIndex == 0 ? 0 : sentence[letterIndex - 1].Width;
                g.DrawImage(sentence[letterIndex], new System.Drawing.Point(xPosition, 0));
            }
            g.Dispose();
            result.Save(numberOfSentece.ToString() + "text.png", System.Drawing.Imaging.ImageFormat.Png);
            result.Dispose();
            var currentPath = AppDomain.CurrentDomain.BaseDirectory;
            itextGeneration.Source = new BitmapImage(new Uri(string.Format("{0}{1}text.png", currentPath, numberOfSentece)));
            numberOfSentece++;
            FillStructure();
        }

        private void crop(object sender, RoutedEventArgs e)
        {
           // FillStructure();
            tbResult.Text = GetTextFromStructure(items);
        }

        private string GetTextFromStructure(List<StructureItem> items)
        {
            var textRecongized = string.Empty;
            items[items.Count - 1].GetMinimum();
            textRecongized = items[items.Count - 1].GetMinimumWord();
            return textRecongized;
        }

        private void FillStructure()
        {
            var source = itextGeneration.Source;
            var image = (BitmapImage)source;
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
                    Bitmap originalLetter = Alphabet.Instance().GetLetterImage(items[structureIndex].nodes[letterIndex].currentLetter);

                    if (toRecognize.Width - structureIndex >= originalLetter.Width)
                    {
                        float difference = GetDifference(originalLetter, toRecognize, structureIndex);
                        items[structureIndex].nodes[letterIndex].whereToGo = items[structureIndex + originalLetter.Width - 1].nodes[letterIndex];
                        items[structureIndex + originalLetter.Width - 1].nodes[letterIndex].weightToCome = difference;
                        items[structureIndex + originalLetter.Width - 1].nodes[letterIndex].fromWhereCame = items[structureIndex].nodes[letterIndex];
                    }
                }
            }
        }

        private float GetDifference(Bitmap original, Bitmap compared, int horizontalShift)
        {
            var difference = .0f;

            for (var y = 0; y < original.Height; y++)
            {
                for (var x = 0; x < original.Width; x++)
                {
                    difference += (float)Math.Abs(original.GetPixel(x, y).R - compared.GetPixel(x + horizontalShift, y).R) / 255;
                    difference += (float)Math.Abs(original.GetPixel(x, y).G - compared.GetPixel(x + horizontalShift, y).G) / 255;
                    difference += (float)Math.Abs(original.GetPixel(x, y).B - compared.GetPixel(x + horizontalShift, y).B) / 255;
                }
            }

            return 100 * difference / (original.Width * original.Height * 3);
        }

        private Bitmap BitmapImage2Bitmap(BitmapSource bitmapImage)
        {
            using (var outStream = new MemoryStream())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(bitmapImage));
                enc.Save(outStream);
                var bitmap = new System.Drawing.Bitmap(outStream);

                return new Bitmap(bitmap);
            }
        }
    }
}
