using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace TextRecognition
{
    public enum Letters
    {
        a, b, c, d, e, f, g, h, i,
        j, k, l, m, n, o, p, q, r,
        s, t, u, v, w, x, y, z
    }

    class Node
    {
        public Node fromWhereCame
        {
            get; set;
        }

        public Node whereToGo
        {
            get; set;
        }
        public int? weightToCome = null;
        Letters currentLetter;
        BitmapImage letterImage;
        public Node(Letters letter, string pathToImage)
        {
            currentLetter = letter;
            try
            {
                letterImage = new BitmapImage(new Uri(pathToImage));
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
            finally
            {
                letterImage = null;
            }
        }


    }
}
