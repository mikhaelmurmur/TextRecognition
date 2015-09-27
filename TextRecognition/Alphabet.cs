using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRecognition
{
    static class Alphabet
    {
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

        static string appPath = AppDomain.CurrentDomain.BaseDirectory+"Images/";

        public static int GetAlphabetLength()
        {
            return lettersPaths.Length;
        }

        public static string GetLetterPath(Letters letter)
        {
            return lettersPaths[(int)letter];
        }
    }
}
