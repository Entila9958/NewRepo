using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Starzack
{
    class TabInt
    {
        #region Variables
        public int[,,] TheTab { get; }
        protected int Height { get; set; }
        protected int Width { get; set; }
        #endregion

        #region Constructeurs
        public TabInt(int Wid, int hei)
        {
            Width = Wid;
            Height = hei;
            TheTab = new int[3, Width, Height];
            for (int i = 0; i < Width; i++)
                for (int j = 0; j < Height; j++)
                {
                    TheTab[0, i, j] = 0;
                    TheTab[1, i, j] = 0;
                    TheTab[2, i, j] = 0;
                }

        }
        public TabInt(TabInt first)
        {
            Height = first.Height;
            Width = first.Width;

            TheTab = new int[3, Width, Height];
            for (int i = 0; i < first.Width; i++)
                for (int j = 0; j < first.Height; j++)
                {
                    TheTab[0, i, j] = first.TheTab[0, i, j];
                    TheTab[1, i, j] = first.TheTab[1, i, j];
                    TheTab[2, i, j] = first.TheTab[2, i, j];




                }

        }

        public TabInt(Bitmap first)
        {
            Height = first.Height;
            Width = first.Width;

            TheTab = new int[3, Width, Height];
            for (int i = 0; i < first.Width; i++)
                for (int j = 0; j < first.Height; j++)
                {
                    Color tempo = first.GetPixel(i, j);
                    TheTab[0, i, j] = tempo.R;
                    TheTab[1, i, j] = tempo.G;
                    TheTab[2, i, j] = tempo.B;




                }

        }
        public TabInt(Bitmap dab, int div, int[,] num,int taille)
        {
            Width = dab.Width;
            Height = dab.Height;
            int moit = taille / 2;
            TheTab = new int[3, Width, Height];
            for (int i = 0; i < Width; i++)
                for (int j = 0; j < Height; j++)
                {
                    TheTab[1, i, j] = 0;                  
                    TheTab[2, i, j] = 0;
                    TheTab[2, i, j] = 0;

                    for (int u = 0; u < taille; u++)
                        for (int o = 0; o < taille; o++)
                        {

                            if ((i + u - moit >= 0) && (i + u - moit < Width) && (j + o - moit >= 0) && (j + o - moit < Height))
                            {

                                Color tempo = dab.GetPixel(i + u - moit, j + o - moit);
                                TheTab[0, i, j] += tempo.R * num[u, o] ;
                                TheTab[1, i, j] += tempo.G * num[u, o] ;
                                TheTab[2, i, j] += tempo.B * num[u, o] ;
                            }
                        }
                    TheTab[0, i, j] /= div;
                    TheTab[1, i, j] /= div;
                    TheTab[2, i, j] /= div;

                }
        }
        #endregion

        #region Méthodes
        public Bitmap ToBitmap()
        {
            Bitmap Tempo = new Bitmap(Width, Height);

            for (int i = 0; i < Width; i++)
                for (int j = 0; j < Height; j++)
                {
                    int R = TheTab[0, i, j];
                    int G = TheTab[1, i, j];
                    int B = TheTab[2, i, j];
                    if (B > 255)
                        B = 255;
                    else if (B < 0)
                        B = 0;

                    if (G > 255)
                        G = 255;
                    else if (G < 0)
                        G = 0;

                    if (R > 255)
                        R = 255;
                    else if (R < 0)
                        R = 0;
                    Tempo.SetPixel(i, j, Color.FromArgb(R,G,B));


                }

            return Tempo;

        }

       
        public static TabInt NormGrad(TabInt first, TabInt Other)
        {
            
                TabInt Thetab = new TabInt(first.Width, first.Height);

                for (int i = 0; i < first.Width; i++)
                    for (int j = 0; j < first.Height; j++)
                    {
                        Thetab.TheTab[0, i, j] = first.TheTab[0, i, j] * first.TheTab[0, i, j] + Other.TheTab[0, i, j] * Other.TheTab[0, i, j];
                        Thetab.TheTab[1, i, j] = first.TheTab[1, i, j] * first.TheTab[1, i, j] + Other.TheTab[1, i, j] * Other.TheTab[1, i, j];
                        Thetab.TheTab[2, i, j] = first.TheTab[2, i, j] * first.TheTab[2, i, j] + Other.TheTab[2, i, j] * Other.TheTab[2, i, j];

                        Thetab.TheTab[0, i, j] = (int)Math.Sqrt((double)Thetab.TheTab[0, i, j]);
                        Thetab.TheTab[1, i, j] = (int)Math.Sqrt((double)Thetab.TheTab[1, i, j]);
                        Thetab.TheTab[2, i, j] = (int)Math.Sqrt((double)Thetab.TheTab[2, i, j]);


                    }
                return Thetab;
           
        }

        public static TabInt operator +(TabInt first, TabInt Other)
        {
            TabInt Thetab = new TabInt(first.Width, first.Height);

            for (int i = 0; i < first.Width; i++)
                for (int j = 0; j < first.Height; j++)
                {
                    Thetab.TheTab[0, i, j] = first.TheTab[0, i, j] + Other.TheTab[0, i, j];
                    Thetab.TheTab[1, i, j] = first.TheTab[1, i, j] + Other.TheTab[1, i, j];
                    Thetab.TheTab[2, i, j] = first.TheTab[2, i, j] + Other.TheTab[2, i, j];



                    
                }
            return Thetab;
        }

        public TabInt Erode()
        {
            TabInt thetab = new TabInt(Width, Height);
            int R = 0;
            int G = 0;
            int B = 0;
            for (int i = 0; i < Width; i++)
                for (int j = 0; j < Height; j++)
                {

                    
                    R = 255;
                    G = 255;
                    B = 255;
                    for (int u = 0; u < 3; u++)
                        for (int o = 0; o < 3; o++)
                        {

                            if ((i + u - 1 >= 0) && (i + u - 1 < Width) && (j + o - 1 >= 0) && (j + o - 1 < Height))
                            {

                                if (R > TheTab[0, i + u - 1, j + o -1])
                                    R = TheTab[0, i + u - 1, j + o -1];
                                if (G > TheTab[1, i + u - 1, j + o -1])
                                    G = TheTab[1, i + u - 1, j + o -1];
                                if (B > TheTab[2, i + u - 1, j + o -1])
                                    B = TheTab[2, i + u - 1, j + o -1];
                            }
                        }


                    thetab.TheTab[0, i, j] = R;
                    thetab.TheTab[1, i, j] = G;
                    thetab.TheTab[2, i, j] = B;

                }
            
            return thetab;
                

    }

         public TabInt Dilate()
        {
            TabInt thetab = new TabInt(Width, Height);
            int R = 0;
            int G = 0;
            int B = 0;
            for (int i = 0; i < Width; i++)
                for (int j = 0; j < Height; j++)
                {


                    R = 0;
                    G = 0;
                    B = 0;
                    for (int u = 0; u < 3; u++)
                        for (int o = 0; o < 3; o++)
                        {

                            if ((i + u - 1 >= 0) && (i + u - 1 < Width) && (j + o - 1 >= 0) && (j + o - 1 < Height))
                            {
                               
                                if (R < TheTab[0, i + u - 1, j + o -1])
                                    R = TheTab[0, i + u - 1, j + o -1];
                                if (G < TheTab[1, i + u - 1, j + o -1])
                                    G = TheTab[1, i + u - 1, j + o -1];
                                if (B < TheTab[2, i + u - 1, j + o -1])
                                    B = TheTab[2, i + u - 1, j + o -1];
                            }
                        }


                    thetab.TheTab[0, i, j] = R;
                    thetab.TheTab[1, i, j] = G;
                    thetab.TheTab[2, i, j] = B;

                }

            return thetab;
        }
        #endregion
    }
}
