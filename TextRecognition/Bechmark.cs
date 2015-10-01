using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRecognition
{
    static class Bechmark
    {
        static Stopwatch start = new Stopwatch();
        public static void Start()
        {
            start.Reset();
            start.Start();
        } 
        public static void Finish()
        {
            start.Stop();
        }

        public static long GetTime()
        {
            return start.ElapsedMilliseconds;
        }
    }
}
