using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

using System.Text;

using System.Windows.Forms;

namespace Starzack
{
    [Serializable]
    class QuadTreeBase
    {
        private int Width,Height;
        private QuadTree Racine;

        public QuadTreeBase(Bitmap Thebitmap)
        {
            Width = Thebitmap.Width;
            Height = Thebitmap.Height;
            Racine = new QuadTree(Thebitmap, new Point(0,0), new Size(Width,Height));
        }

        public Color GetC(int x, int y)
        {
            return Racine.GetC(x, y, new Point(0, 0),new Size (Width,Height)) ;


        }

       
        public Bitmap ToBitmap()
        {
           
            Bitmap dab = new Bitmap(Width,Height);
            Racine.ConvertToBitmap(dab, new Point (0,0),new Size( dab.Width, dab.Height));
            return dab;
        }

        public Bitmap Decoupage()
        {

            Bitmap dab = ToBitmap();
            Racine.Decoupage(dab, new Point(0, 0), new Size(dab.Width, dab.Height));
            return dab;
        }

    }


   
}
