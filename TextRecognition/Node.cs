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
        s, t, u, v, w, x, y, z, space
    }

    class Node
    {
        public StructureItem owner { get; private set; }
        public Node fromWhereCame
        {
            get; set;
        }


        public Node whereToGo
        {
            get; set;
        }
        public float weightToCome = float.MaxValue;
        public Letters currentLetter { get; private set; }
        BitmapImage letterImage;
        public Node(Letters letter, string pathToImage, StructureItem owner)
        {
            currentLetter = letter;
            this.owner = owner;
            letterImage = new BitmapImage(new Uri(pathToImage));

        }

        public BitmapImage GetImage()
        {
            return letterImage;
        }
    }
}
