using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;
using System.Data;
using System.Drawing.Drawing2D;

namespace Starzack
{
    [Serializable]
    class QuadTree
    {
        public static int Val { get; set; }
        //public Color TheColor { get; set; }
        private int[] myVar;

        public Color TheColor
        {
            get { return Color.FromArgb(myVar[0], myVar[1], myVar[2]); }
            set { myVar = new int[3] { value.R, value.G, value.B }; }
        }

        private bool Monochrome = true;
        public QuadTree NE { get; set; }
        public QuadTree NO { get; set; }
        public QuadTree SE { get; set; }
        public QuadTree SO { get; set; }

        public QuadTree(Bitmap theBitmap, Point Origin, Size Taille)
        {
            TheColor = theBitmap.GetPixel(Origin.X, Origin.Y);
            int x = Origin.X;
            //int R = TheColor.R, G = TheColor.G, B = TheColor.B;
            //  int i = 1;
            while (Monochrome && x < Origin.X + Taille.Width)
            {
                int y = Origin.Y;
                while (Monochrome && y < Origin.Y + Taille.Height)
                {
                    if (!TCO(TheColor, theBitmap.GetPixel(x, y))) Monochrome = false;
                    //Color.FromArgb(R / i,G / i, B / i)
                    //R += theBitmap.GetPixel(x, y).R;
                    //G += theBitmap.GetPixel(x, y).G;
                    //B += theBitmap.GetPixel(x, y).B;
                    y++;
                    //i++;
                 }
                x++;
            }
 
            if(Monochrome)
            {
                //TheColor = Color.FromArgb(R / i, G / i, B / i);
                return;
            }
            else
            {
                
 
                
                    Size TailleMoit = new Size(Taille.Width / 2, Taille.Height / 2);
                    NO = new QuadTree(theBitmap, Origin,TailleMoit);
                    NE = new QuadTree(theBitmap, new Point(Origin.X + TailleMoit.Width, Origin.Y), TailleMoit);
                    SO = new QuadTree(theBitmap, new Point(Origin.X , Origin.Y+TailleMoit.Height), TailleMoit);
                    SE = new QuadTree(theBitmap, new Point(Origin.X + TailleMoit.Width, Origin.Y + TailleMoit.Height), TailleMoit);

                
            }
       
        }

        public QuadTree(Color a)
        {
            TheColor = a;
             Monochrome= true;

        }
        public static bool TCO(Color a, Color b)
        {
           
            return (b.R <= a.R + Val && b.R >= a.R - Val && b.B <= a.B + Val && b.B >= a.B - Val && b.G <= a.G + Val && b.G >= a.G - Val);

        }
        public Color GetC(int x , int y, Point Origin,Size Taille) 
        {
            if (Monochrome)
                return TheColor;
            else
            {
                Size TailleMoit = new Size(Taille.Width / 2, Taille.Height / 2);
                if (x < Origin.X + TailleMoit.Width)
                    if (y < Origin.Y + TailleMoit.Height)
                        return NO.GetC(x, y, Origin, TailleMoit);
                    else
                        return SO.GetC(x, y, new Point(Origin.X, Origin.Y + TailleMoit.Height), TailleMoit);
                else
                {
                    if (y < Origin.Y + TailleMoit.Height)
                        return NE.GetC(x, y, new Point(Origin.X + TailleMoit.Width, Origin.Y ), TailleMoit);
                    else
                        return SE.GetC(x, y, new Point(Origin.X + TailleMoit.Width, Origin.Y + TailleMoit.Height), TailleMoit);

                }
                

            }
        }
        public void ConvertToBitmap(Bitmap Beatmap,Point Origin, Size Taille)
        {
            if(!Monochrome)
            {
                Size TailleMoit = new Size(Taille.Width / 2, Taille.Height / 2);
                NO.ConvertToBitmap(Beatmap, Origin, TailleMoit);
                NE.ConvertToBitmap(Beatmap, new Point(Origin.X + TailleMoit.Width, Origin.Y), TailleMoit);
                SO.ConvertToBitmap(Beatmap, new Point(Origin.X, Origin.Y + TailleMoit.Height), TailleMoit);
                SE.ConvertToBitmap(Beatmap, new Point(Origin.X + TailleMoit.Width, Origin.Y + TailleMoit.Height), TailleMoit);


            }
            else
                for (int i = Origin.X; i < Origin.X+Taille.Width; i++)
                    for (int j = Origin.Y; j < Origin.Y + Taille.Height; j++)
                    {
                        
                        Beatmap.SetPixel(i, j, TheColor);
                    }

        }
        public void Decoupage(Bitmap Beatmap, Point Origin, Size Taille)
        {

            if (Monochrome)
            {
                for(int x = Origin.X; x < Origin.X + Taille.Width;x++)
                {
                    Beatmap.SetPixel(x, Origin.Y, Color.Black);
                    Beatmap.SetPixel(x, Origin.Y + Taille.Height - 1, Color.Black);
                }
                for (int y = Origin.Y; y < Origin.Y + Taille.Height; y++)
                {
                    Beatmap.SetPixel(Origin.X, y, Color.Black);
                    Beatmap.SetPixel(Origin.X + Taille.Width - 1, y, Color.Black);
                }
            }
            else
            {
                Size TailleMoit = new Size(Taille.Width / 2, Taille.Height / 2);
                NO.Decoupage(Beatmap,Origin, TailleMoit);
                SO.Decoupage(Beatmap, new Point(Origin.X, Origin.Y + TailleMoit.Height), TailleMoit);
                NE.Decoupage(Beatmap, new Point(Origin.X + TailleMoit.Width, Origin.Y), TailleMoit);
                SE.Decoupage(Beatmap, new Point(Origin.X + TailleMoit.Width, Origin.Y + TailleMoit.Height), TailleMoit);

                


            }
        }
    }
}
