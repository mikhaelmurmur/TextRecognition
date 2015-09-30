using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Media;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Drawing;
using System.Diagnostics;
using XnaFan.ImageComparison;

namespace TextRecognition
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<StructureItem> items = new List<StructureItem>();
        static int numberOfSentece = 0;
        int totalWidth = 0;
        #region images
        System.Drawing.Image[] letters = new System.Drawing.Image[]
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
        #endregion
        public MainWindow()
        {
            InitializeComponent();
        }

        private void GenerateText(object sender, RoutedEventArgs e)
        {
            Random random = new Random();
            int length = random.Next(1, 20);
            List<System.Drawing.Image> sentence = new List<System.Drawing.Image>();
            Alphabet.Instance();
            for (int letterIndex = 0; letterIndex < length; letterIndex++)
            {
                int letterValue = random.Next(0, 3);
                sentence.Add(letters[letterValue]);
                totalWidth += letters[letterValue].Width;
            }
            int totalHeight = 26;
            Bitmap result = new Bitmap(totalWidth, totalHeight);
            result.SetResolution(300, 300);
            Graphics g = Graphics.FromImage(result);
            // g.Clear(System.Drawing.Color.White);
            int xPosition = 0;
            for (int letterIndex = 0; letterIndex < length; letterIndex++)
            {
                xPosition += letterIndex == 0 ? 0 : sentence[letterIndex - 1].Width;
                g.DrawImage(sentence[letterIndex], new System.Drawing.Point(xPosition, 0));
            }
            g.Dispose();
            result.Save(numberOfSentece.ToString() + "text.png", System.Drawing.Imaging.ImageFormat.Png);
            result.Dispose();
            string currentPath = AppDomain.CurrentDomain.BaseDirectory;
            itextGeneration.Source = new BitmapImage(new Uri(string.Format("{0}{1}text.png", currentPath, numberOfSentece)));
            numberOfSentece++;
        }

        private void crop(object sender, RoutedEventArgs e)
        {
            FillStructure();
            tbResult.Text = GetTextFromStructure(items);
        }

        private string GetTextFromStructure(List<StructureItem> items)
        {
            string textRecongized = "";
            float minim = items[items.Count - 1].GetMinimum();
            textRecongized = items[items.Count - 1].GetMinimumWord();
            return textRecongized;
        }

        private void FillStructure()
        {
            ImageSource source = itextGeneration.Source;
            BitmapImage image = (BitmapImage)source;
            Bitmap toRecognize = BitmapImage2Bitmap(image);
            for (int structureIndex = 0; structureIndex < totalWidth; structureIndex++)
            {
                StructureItem item;
                if (structureIndex == 0)
                    item = new StructureItem(null);
                else
                    item = new StructureItem(items[structureIndex-1]);
                items.Add(item);
            }
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
            float difference = .0f;

            for (int y = 0; y < original.Height; y++)
            {
                for (int x = 0; x < original.Width; x++)
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
            // BitmapImage bitmapImage = new BitmapImage(new Uri("../Images/test.png", UriKind.Relative));

            using (MemoryStream outStream = new MemoryStream())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(bitmapImage));
                enc.Save(outStream);
                System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(outStream);

                return new Bitmap(bitmap);
            }
        }

    }
}
