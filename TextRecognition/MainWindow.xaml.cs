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

namespace TextRecognition
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static int numberOfSentece = 0;
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

        public MainWindow()
        {
            InitializeComponent();
        }



        private void GenerateText(object sender, RoutedEventArgs e)
        {
            Random random = new Random();
            int length = random.Next(0, 100);
            List<System.Drawing.Image> sentence = new List<System.Drawing.Image>();
            int totalWidth = 0;
            for(int letterIndex = 0; letterIndex<length;letterIndex++)
            {
                int letterValue = random.Next(0, 26);
                sentence.Add(letters[letterValue]);
                totalWidth += letters[letterValue].Width;
            }
            int totalHeight = 30;
            Bitmap result = new Bitmap(totalWidth, totalHeight);
            result.SetResolution(300, 300);
            Graphics g = Graphics.FromImage(result);
            g.Clear(System.Drawing.Color.White);
            int xPosition = 0;
            for (int letterIndex = 0; letterIndex < length; letterIndex++)
            {
                xPosition += letterIndex == 0 ? 0 : sentence[letterIndex - 1].Width;
                g.DrawImage(sentence[letterIndex], new System.Drawing.Point(xPosition, 0));
            }
            g.Dispose();
            result.Save(numberOfSentece.ToString()+ "text.png", System.Drawing.Imaging.ImageFormat.Png);
            result.Dispose();
            string currentPath = AppDomain.CurrentDomain.BaseDirectory;
            itextGeneration.Source = new BitmapImage(new Uri(currentPath + numberOfSentece.ToString()+  "text.png"));
            numberOfSentece++;
        }
    }
}
