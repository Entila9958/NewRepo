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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Configuration;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

using Path = System.IO.Path;
using Color = System.Drawing.Color;

namespace Starzack
{
    /// <summary>
    /// Dosser d'analyse Designe D'alexis Zamouth, 2020-2021?
    /// </summary>
    public partial class MainWindow : Window
    {
        Bitmap ImageDeDep;
        Bitmap ImageFinal;
        public MainWindow()
        {
            InitializeComponent();
            QuadTree.Val = 10;
        }

        private void ButOpenFile_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog();
            {
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "Images (*.bmp,*.jpeg/jpg,*.png,*.gif)|*.bmp*;*.jpeg;*.jpg;*.png;*.gif|Images QuadTree(.Quad)|*.Quad|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {

                    if (Path.GetExtension(openFileDialog.FileName).Equals(".Quad"))
                    {
                        IFormatter formatter = new BinaryFormatter();
                        Stream stream = new FileStream(openFileDialog.FileName, FileMode.Open, FileAccess.Read);
                        QuadTreeBase Tree =(QuadTreeBase) formatter.Deserialize(stream);
                        stream.Close();
                        ImageDeDep = Tree.ToBitmap();
                    }
                    else
                    ImageDeDep = new Bitmap(openFileDialog.FileName);
                    UpdateImageDep();




                }
            }
        }



        /*
         * Code Copié de la : 
         * https://stackoverflow.com/questions/37890121/fast-conversion-of-bitmap-to-imagesource 
         * 
         * 
         * 
         */

        public BitmapImage ConvertBitmap(System.Drawing.Bitmap bitmap)
        {
            MemoryStream ms = new MemoryStream();
            bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            ms.Seek(0, SeekOrigin.Begin);
            image.StreamSource = ms;
            image.EndInit();

            return image;
        }

        private void ButChangTaille_Click(object sender, RoutedEventArgs e)
        {
            if (ImageDeDep == null)
            {
                System.Windows.Forms.MessageBox.Show("Pas d'image de départ", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                int j = Convert.ToInt32(TextH.Text);
                int i = Convert.ToInt32(TextL.Text);
                ImageFinal = new Bitmap(i, j);
                if (RadBil.IsChecked == true)
                {
                    float Val1, Val2;
                    Val1 = ((float)ImageDeDep.Width) / i;
                    Val2 = ((float)ImageDeDep.Height) / j;
                    int x1, x2, y1, y2;
                    for (int x = 0; x < j; x++)
                    {

                        x1 = (int)(Val2 * x);
                        if (x1 >= ImageDeDep.Height - 2)
                        {

                            x2 = ImageDeDep.Height - 1;
                            x1 = ImageDeDep.Height - 2;

                        }
                        else
                        {
                            x2 = x1 + 1;
                        }

                        for (int y = 0; y < i; y++)
                        /**if (y >= ImageDeDep.Height || x >= ImageDeDep.Width)
                            ImageFinal.SetPixel(x, y, System.Drawing.Color.Black); // Dossier 1
                        else
                            ImageFinal.SetPixel(x, y, ImageDeDep.GetPixel(x, y));
                        **/
                        {

                            y1 = (int)(Val1 * y);
                            if (y1 >= ImageDeDep.Width - 2)
                            {
                                y2 = ImageDeDep.Width - 1;
                                y1 = ImageDeDep.Width - 2;
                            }
                            else
                            {
                                y2 = y1 + 1;
                            }

                            System.Drawing.Color tempox1 = ImageDeDep.GetPixel(y1, x1);
                            System.Drawing.Color tempox2 = ImageDeDep.GetPixel(y1, x2);
                            System.Drawing.Color tempox3 = ImageDeDep.GetPixel(y2, x1);
                            System.Drawing.Color tempox4 = ImageDeDep.GetPixel(y2, x2);
                            ImageFinal.SetPixel(y, x, System.Drawing.Color.FromArgb(InterpolBil(tempox1.R, tempox2.R, tempox3.R, tempox4.R, (Val2 * x), Val1 * y), InterpolBil(tempox1.G, tempox2.G, tempox3.G, tempox4.G, (Val2 * x), Val1 * y), InterpolBil(tempox1.B, tempox2.B, tempox3.B, tempox4.B, Val2 * x, Val1 * y)));


                        }
                    }
                }
                else
                {
                    float Val1, Val2;
                    Val1 = ((float)ImageDeDep.Width) / i;
                    Val2 = ((float)ImageDeDep.Height) / j;
                    int x1, x2, x3, y1, y2, y3;
                    for (int x = 0; x < j; x++)
                    {

                        x1 = (int)(Val2 * x);
                        if (x1 > ImageDeDep.Height - 3)
                        {
                            x3 = ImageDeDep.Height - 1;
                            x2 = ImageDeDep.Height - 2;
                            x1 = ImageDeDep.Height - 3;

                        }
                        else
                        {

                            x2 = x1 + 1;
                            x3 = x2 + 1;
                        }

                        for (int y = 0; y < i; y++)
                        /**if (y >= ImageDeDep.Height || x >= ImageDeDep.Width)
                            ImageFinal.SetPixel(x, y, System.Drawing.Color.Black); // Dossier 1
                        else
                            ImageFinal.SetPixel(x, y, ImageDeDep.GetPixel(x, y));
                        **/
                        {

                            y1 = (int)(Val1 * y);
                            if (y1 >= ImageDeDep.Width - 2)
                            {
                                y3 = ImageDeDep.Width - 1;
                                y2 = ImageDeDep.Width - 2;
                                y1 = ImageDeDep.Width - 3;
                            }
                            else
                            {
                                y2 = y1 + 1;
                                y3 = y2 + 1;
                            }

                            System.Drawing.Color tempox1 = ImageDeDep.GetPixel(y1, x1);
                            System.Drawing.Color tempox2 = ImageDeDep.GetPixel(y1, x2);
                            System.Drawing.Color tempox3 = ImageDeDep.GetPixel(y1, x3);
                            System.Drawing.Color tempox4 = ImageDeDep.GetPixel(y2, x1);
                            System.Drawing.Color tempox5 = ImageDeDep.GetPixel(y2, x2);
                            System.Drawing.Color tempox6 = ImageDeDep.GetPixel(y2, x3);
                            System.Drawing.Color tempox7 = ImageDeDep.GetPixel(y3, x1);
                            System.Drawing.Color tempox8 = ImageDeDep.GetPixel(y3, x2);
                            System.Drawing.Color tempox9 = ImageDeDep.GetPixel(y3, x3);
                            ImageFinal.SetPixel(y, x, System.Drawing.Color.FromArgb(InterpolQuad(tempox1.R, tempox2.R, tempox3.R, tempox4.R, tempox5.R, tempox6.R, tempox7.R, tempox8.R, tempox9.R, ((Val2) * x) - x2 * Val2, (1 / Val2) * y - y2),
                                                                                    InterpolQuad(tempox1.G, tempox2.G, tempox3.G, tempox4.G, tempox5.G, tempox6.G, tempox7.G, tempox9.G, tempox9.G, ((1 / Val1) * x) - x2, (1 / Val2) * y - y2),
                                                                                    InterpolQuad(tempox1.B, tempox2.B, tempox3.B, tempox4.B, tempox5.B, tempox6.B, tempox7.B, tempox9.B, tempox9.B, ((1 / Val1) * x) - x2, (1 / Val2) * y - y2)));


                        }
                    }


                }


                TextH_P1.Text = 0.ToString();
                TextH_P2.Text = 0.ToString();
                TextL_P1.Text = 0.ToString();
                TextL_P2.Text = 0.ToString();
                UpdateImageFinal();
            }
            catch (FormatException)
            {
                System.Windows.Forms.MessageBox.Show("L'un des nombre n'est pas un nombre correcte", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            catch (OverflowException)
            {
                System.Windows.Forms.MessageBox.Show("L'un des nombres est trop grand (int32)", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }



        }

        private void ButBalckAndWhite_Click(object sender, RoutedEventArgs e)
        {

            if (ImageDeDep == null)
            {
                System.Windows.Forms.MessageBox.Show("Pas d'image de départ", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            int j = ImageDeDep.Height;
            int i = ImageDeDep.Width;
            ImageFinal = new Bitmap(i, j);
            for (int x = 0; x < i; x++)
                for (int y = 0; y < j; y++)
                {
                    System.Drawing.Color tempo = ImageDeDep.GetPixel(x, y);
                    int v = (tempo.R + tempo.G + tempo.B) / 3;

                    tempo = System.Drawing.Color.FromArgb(v, v, v);

                    ImageFinal.SetPixel(x, y, tempo);

                }
            UpdateImageFinal();

        }

        private void ButFinalToDep_Click(object sender, RoutedEventArgs e)
        {
            if (ImageFinal == null)
            {
                System.Windows.Forms.MessageBox.Show("Pas d'image final", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            int j = ImageFinal.Height;
            int i = ImageFinal.Width;
            //ImageDeDep = new Bitmap(i, j);
            /**  for (int x = 0; x < i; x++)               // Dossier 1 : lent car travaille inutile.
                  for (int y = 0; y < j; y++)
                  {

                      ImageDeDep.SetPixel(x, y, ImageFinal.GetPixel(x, y));

                  }

              **/
            ImageDeDep = ImageFinal;
            UpdateImageDep();
            // ConvertBitmap(ImageDeDep);

            TextH_P1.Text = 0.ToString();
            TextH_P2.Text = 0.ToString();
            TextL_P1.Text = 0.ToString();
            TextL_P2.Text = 0.ToString();
        }

        private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if ((e.Key == Key.LWin) || (e.Key == Key.RWin))
            {
                ButFinalToDep_Click(this, null);

            }
        }

        private void ImaDep_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            double i = e.GetPosition(ImaDep).X;
            double j = (ImageDeDep.Width / ImaDep.Width);
            i = j * i;
            TextL_P1.Text = ((int)i).ToString();
            int x = (int)i;
            i = e.GetPosition(ImaDep).Y;
            j = (ImageDeDep.Width / ImaDep.Width);
            i = j * i;
            int y = (int)i;
            TextH_P1.Text = ((int)i).ToString();

            
        }

        private void ImaDep_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            double i = e.GetPosition(ImaDep).X;
            double j = (ImageDeDep.Width / ImaDep.Width);
            i = j * i;
            TextL_P2.Text = ((int)i).ToString();

            i = e.GetPosition(ImaDep).Y;
            j = (ImageDeDep.Width / ImaDep.Width);
            i = j * i;
            TextH_P2.Text = ((int)i).ToString();
        }

        private void ButROI_Click(object sender, RoutedEventArgs e)
        {
            if (ImageDeDep == null)
            {
                System.Windows.Forms.MessageBox.Show("Pas d'image de départ", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                int x1 = Convert.ToInt32(TextL_P1.Text);
                int x2 = Convert.ToInt32(TextL_P2.Text);
                if (x1 > x2)
                {
                    int xtempo = x1;
                    x1 = x2;
                    x2 = xtempo;

                }

                int y1 = Convert.ToInt32(TextH_P1.Text);
                int y2 = Convert.ToInt32(TextH_P2.Text);

                if (y1 > y2)
                {
                    int ytempo = y1;
                    y1 = y2;
                    y2 = ytempo;

                } 
                int j = x2 - x1;
                int i = y2 - y1;

                ImageFinal = new Bitmap(j, i);
                for (int x = 0; x < j; x++)
                    for (int y = 0; y < i; y++)
                        ImageFinal.SetPixel(x, y, ImageDeDep.GetPixel(x + x1, y + y1));

                TextH_P1.Text = 0.ToString();
                TextH_P2.Text = 0.ToString();
                TextL_P1.Text = 0.ToString();
                TextL_P2.Text = 0.ToString();
                UpdateImageFinal();
            }
            catch (FormatException)
            {
                System.Windows.Forms.MessageBox.Show("L'un des nombre n'est pas un nombre correcte", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            catch (OverflowException)
            {
                System.Windows.Forms.MessageBox.Show("L'un des nombres est trop grand (int32)", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            catch (System.ArgumentException)
            {
                System.Windows.Forms.MessageBox.Show("Valeur des ROI Incorrecte", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void But8Bit_Click(object sender, RoutedEventArgs e)
        {
            if (ImageDeDep == null)
            {
                System.Windows.Forms.MessageBox.Show("Pas d'image de départ", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            int j = ImageDeDep.Height;
            int i = ImageDeDep.Width;
            ImageFinal = new Bitmap(i, j);
            try
            {
                int v = Convert.ToInt32(TextX.Text);
                v = 256 / v;
                if (v < 0 || v > 256)
                {

                    System.Windows.Forms.MessageBox.Show("Valeur de X trop petite ou trop grande", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                for (int x = 0; x < i; x++)
                    for (int y = 0; y < j; y++)
                    {
                        System.Drawing.Color tempo = ImageDeDep.GetPixel(x, y);
                        int g = (((tempo.G / v))) * v;
                        int r = (((tempo.R / v)) * v);
                        int b = ((((tempo.B / v))) * v);
                        tempo = System.Drawing.Color.FromArgb(r, g, b);

                        ImageFinal.SetPixel(x, y, tempo);

                    }
                UpdateImageFinal();
            }
            catch (FormatException)
            {
                System.Windows.Forms.MessageBox.Show("X n'est pas un nombre correcte", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            catch (OverflowException)
            {
                System.Windows.Forms.MessageBox.Show("x est trop grand (int32)", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }
        private void UpdateImageDep()
        {

            ImaDep.Height = ImageDeDep.Height;
            ImaDep.Width = ImageDeDep.Width;
            ImaDep.Source = ConvertBitmap(ImageDeDep);

            Bitmap RedHist = new Bitmap(256, 101);
            Bitmap BluHist = new Bitmap(256, 101);
            Bitmap GreHist = new Bitmap(256, 101);

            int[] R = new int[256];
            int[] G = new int[256];
            int[] B = new int[256];

            for (int x = 0; x < 256; x++) // just in case;
            {
                R[x] = 0;
                G[x] = 0;
                B[x] = 0;

            }



            for (int x = 0; x < ImageDeDep.Height; x++)
            {
                for (int y = 0; y < ImageDeDep.Width; y++)
                {
                    R[ImageDeDep.GetPixel(y, x).R]++;
                    G[ImageDeDep.GetPixel(y, x).G]++;
                    B[ImageDeDep.GetPixel(y, x).B]++;
                }
            }




            int MaxRed = R.Max();
            int MaxGre = G.Max();
            int MaxBlu = B.Max();
            MaxRedDep.Text = MaxRed.ToString();
            MaxGreDep.Text = MaxGre.ToString();
            MaxBluDep.Text = MaxBlu.ToString();
            for (int x = 0; x < 256; x++) // just in case;
            {
                R[x] = (int)((float)R[x] / MaxRed * 100);
                G[x] = (int)((float)G[x] / MaxGre * 100);
                B[x] = (int)((float)B[x] / MaxBlu * 100);

            }
            for (int x = 0; x < 256; x++)
            {

                for (int y = 0; y < R[x]; y++)
                {
                    RedHist.SetPixel(x, 100 - y, System.Drawing.Color.White);
                }
                for (int y = 0; y < G[x]; y++)
                {
                    GreHist.SetPixel(x, 100 - y, System.Drawing.Color.White);
                }
                for (int y = 0; y < B[x]; y++)
                {
                    BluHist.SetPixel(x, 100 - y, System.Drawing.Color.White);
                }

            }

            RedHistoDep.Source = ConvertBitmap(RedHist);
            GreHistoDep.Source = ConvertBitmap(GreHist);
            BluHistoDep.Source = ConvertBitmap(BluHist);
        }
        private void UpdateImageFinal()
        {

            ImaFinal.Height = ImageFinal.Height;
            ImaFinal.Width = ImageFinal.Width;
            ImaFinal.Source = ConvertBitmap(ImageFinal);



            Bitmap RedHist = new Bitmap(256, 101);
            Bitmap BluHist = new Bitmap(256, 101);
            Bitmap GreHist = new Bitmap(256, 101);

            int[] R = new int[256];
            int[] G = new int[256];
            int[] B = new int[256];

            for (int x = 0; x < 256; x++) // just in case;
            {
                R[x] = 0;
                G[x] = 0;
                B[x] = 0;

            }



            for (int x = 0; x < ImageFinal.Height; x++)
            {
                for (int y = 0; y < ImageFinal.Width; y++)
                {
                    R[ImageFinal.GetPixel(y, x).R]++;
                    G[ImageFinal.GetPixel(y, x).G]++;
                    B[ImageFinal.GetPixel(y, x).B]++;
                }
            }




            int MaxRed = R.Max();
            int MaxGre = G.Max();
            int MaxBlu = B.Max();
            MaxRedFin.Text = MaxRed.ToString();
            MaxGreFin.Text = MaxGre.ToString();
            MaxBluFin.Text = MaxBlu.ToString();
            for (int x = 0; x < 256; x++) // just in case;
            {
                R[x] = (int)((float)R[x] / MaxRed * 100);
                G[x] = (int)((float)G[x] / MaxGre * 100);
                B[x] = (int)((float)B[x] / MaxBlu * 100);

            }
            for (int x = 0; x < 256; x++)
            {

                for (int y = 0; y < R[x]; y++)
                {
                    RedHist.SetPixel(x, 100 - y, System.Drawing.Color.White);
                }
                for (int y = 0; y < G[x]; y++)
                {
                    GreHist.SetPixel(x, 100 - y, System.Drawing.Color.White);
                }
                for (int y = 0; y < B[x]; y++)
                {
                    BluHist.SetPixel(x, 100 - y, System.Drawing.Color.White);
                }

            }

            RedHistoFin.Source = ConvertBitmap(RedHist);
            GreHistoFin.Source = ConvertBitmap(GreHist);
            BluHistoFin.Source = ConvertBitmap(BluHist);

        }

        private void ButSeuilUnique_Click(object sender, RoutedEventArgs e)
        {
            if (ImageDeDep == null)
            {
                System.Windows.Forms.MessageBox.Show("Pas d'image de départ", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            int j = ImageDeDep.Height;
            int i = ImageDeDep.Width;
            ImageFinal = new Bitmap(i, j);
            try
            {
                int r = Convert.ToInt32(RedSeuilUnique.Text);
                int g = Convert.ToInt32(GreSeuilUnique.Text);
                int b = Convert.ToInt32(BluSeuilUnique.Text);

                if ((r < 0 || r > 256) || g < 0 || g > 256 || b < 0 || b > 256)
                {

                    System.Windows.Forms.MessageBox.Show("Valeur de RGB trop petite ou trop grande", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                for (int x = 0; x < i; x++)
                    for (int y = 0; y < j; y++)
                    {
                        System.Drawing.Color tempo = ImageDeDep.GetPixel(x, y);
                        if (tempo.R < r || tempo.G < g || tempo.B < b)
                            ImageFinal.SetPixel(x, y, System.Drawing.Color.Black);
                        else
                            ImageFinal.SetPixel(x, y, System.Drawing.Color.White);

                    }
                UpdateImageFinal();
            }
            catch (FormatException)
            {
                System.Windows.Forms.MessageBox.Show("X n'est pas un nombre correcte", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            catch (OverflowException)
            {
                System.Windows.Forms.MessageBox.Show("x est trop grand (int32)", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void ButEgalisation_Click(object sender, RoutedEventArgs e)
        {
            float[] R = new float[256];
            float[] G = new float[256];
            float[] B = new float[256];
            for (int x = 0; x < 256; x++) // just in case ;
            {
                R[x] = 0;
                G[x] = 0;
                B[x] = 0;

            }
            for (int x = 0; x < ImageDeDep.Height; x++)
            {
                for (int y = 0; y < ImageDeDep.Width; y++)
                {
                    R[ImageDeDep.GetPixel(y, x).R]++;
                    G[ImageDeDep.GetPixel(y, x).G]++;
                    B[ImageDeDep.GetPixel(y, x).B]++;
                }
            }
            float g = (ImageDeDep.Height * ImageDeDep.Width);

            for (int x = 0; x < 256; x++)
            {
                R[x] = (R[x] / g);
                G[x] = (G[x] / g);
                B[x] = (B[x] / g);
            }
            for (int x = 1; x < 256; x++)
            {
                R[x] += R[x - 1];
                G[x] += G[x - 1];
                B[x] += B[x - 1];
            }
            ImageFinal = new Bitmap(ImageDeDep.Width, ImageDeDep.Height);
            for (int x = 0; x < ImageDeDep.Height; x++)
            {
                for (int y = 0; y < ImageDeDep.Width; y++)
                {
                    ImageFinal.SetPixel(y, x, System.Drawing.Color.FromArgb((int)(255 * R[ImageDeDep.GetPixel(y, x).R]), (int)(255 * G[ImageDeDep.GetPixel(y, x).G]), (int)(255 * B[ImageDeDep.GetPixel(y, x).B])));

                }
            }
            UpdateImageFinal();
        }

        private void ButSeulMultiple_Click(object sender, RoutedEventArgs e)
        {
            if (ImageDeDep == null)
            {
                System.Windows.Forms.MessageBox.Show("Pas d'image de départ", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            int j = ImageDeDep.Height;
            int i = ImageDeDep.Width;
            ImageFinal = new Bitmap(i, j);
            try
            {
                int rs1 = Convert.ToInt32(RedSeuil1.Text);
                int gs1 = Convert.ToInt32(GreSeuil1.Text);
                int bs1 = Convert.ToInt32(BluSeuil1.Text);
                int rs2 = Convert.ToInt32(RedSeuil2.Text);
                int gs2 = Convert.ToInt32(GreSeuil2.Text);
                int bs2 = Convert.ToInt32(BluSeuil2.Text);
                int rs3 = Convert.ToInt32(RedSeuil3.Text);
                int gs3 = Convert.ToInt32(GreSeuil3.Text);
                int bs3 = Convert.ToInt32(BluSeuil3.Text);

                int rx0 = Convert.ToInt32(RedSeuilx0.Text);
                int gx0 = Convert.ToInt32(GreSeuilx0.Text);
                int bx0 = Convert.ToInt32(BluSeuilx0.Text);
                int rx1 = Convert.ToInt32(RedSeuilx1.Text);
                int gx1 = Convert.ToInt32(GreSeuilx1.Text);
                int bx1 = Convert.ToInt32(BluSeuilx1.Text);
                int rx2 = Convert.ToInt32(RedSeuilx2.Text);
                int gx2 = Convert.ToInt32(GreSeuilx2.Text);
                int bx2 = Convert.ToInt32(BluSeuilx2.Text);
                int rx3 = Convert.ToInt32(RedSeuilx3.Text);
                int gx3 = Convert.ToInt32(GreSeuilx3.Text);
                int bx3 = Convert.ToInt32(BluSeuilx3.Text);

                int r, g, b;
                for (int x = 0; x < i; x++)
                    for (int y = 0; y < j; y++)
                    {
                        System.Drawing.Color tempo = ImageDeDep.GetPixel(x, y);
                        if (tempo.R < rs1)
                            r = rx0;
                        else if (tempo.R < rs2)
                            r = rx1;
                        else if (tempo.R < rs3)
                            r = rx2;
                        else
                            r = rx3;

                        if (tempo.G < gs1)
                            g = gx0;
                        else if (tempo.G < gs2)
                            g = gx1;
                        else if (tempo.G < gs3)
                            g = gx2;
                        else
                            g = gx3;

                        if (tempo.B < bs1)
                            b = bx0;
                        else if (tempo.B < bs2)
                            b = bx1;
                        else if (tempo.B < bs3)
                            b = bx2;
                        else
                            b = bx3;




                        ImageFinal.SetPixel(x, y, System.Drawing.Color.FromArgb(r, g, b));

                    }
                UpdateImageFinal();

            }
            catch (FormatException)
            {
                System.Windows.Forms.MessageBox.Show("X n'est pas un nombre correcte", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            catch (OverflowException)
            {
                System.Windows.Forms.MessageBox.Show("x est trop grand (int32)", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

        }

        private void ButSaveAs_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "JPeg Image|*.jpg|Bitmap Image|*.bmp|Gif Image|*.gif|Quad Image|*.Quad";
            dialog.Title = "Save an Image File";
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                int width = Convert.ToInt32(ImageFinal.Width);
                int height = Convert.ToInt32(ImageFinal.Height);

                switch (dialog.FilterIndex)
                {
                    case 1:
                        ImageFinal.Save(dialog.FileName, System.Drawing.Imaging.ImageFormat.Jpeg);
                        break;

                    case 2:
                        ImageFinal.Save(dialog.FileName, System.Drawing.Imaging.ImageFormat.Bmp);
                        break;

                    case 3:
                        ImageFinal.Save(dialog.FileName, System.Drawing.Imaging.ImageFormat.Gif);
                        break;
                    case 4:

                        BinaryFormatter dab = new BinaryFormatter();
                        
                        Stream stream = new FileStream(dialog.FileName, FileMode.Create, FileAccess.Write, FileShare.None);
                        dab.Serialize(stream, new QuadTreeBase(ImageFinal));
                        stream.Close();
                        break;

                }

            }
        }
        private int InterpolBil(int x1, int x2, int x3, int x4, float x, float y)
        {
            x %= 1;
            y %= 1;
            float g1 = x1 + ((x2 - x1) / 2) * x;
            float g2 = x3 + ((x4 - x3) / 2) * x;
            return (int)(g1 + ((g2 - g1) / 2) * y);
        }
        int xd = 0;

        private int InterpolQuad(int x1, int x2, int x3, int x4, int x5, int x6, int x7, int x8, int x9, float x, float y)
        {

            float v1 = x2 + ((x3 - x1) / 2) * x + (x3 - 2 * x2 + x1) * x * x / 2;
            float v2 = x5 + ((x6 - x4) / 2) * x + (x6 - 2 * x5 + x4) * x * x / 2;
            float v3 = x8 + ((x9 - x7) / 2) * x + (x9 - 2 * x8 + x7) * x * x / 2;

            xd++;
            if (xd > 250 * 125)
                xd--;
            int i = (int)(v2 + ((v3 - v1) / 2) * y + (v3 - 2 * v2 + v1) * y * y / 2);
            if (i < 0)
                return 0;
            else if (i > 255)
                return 255;
            return (int)(v2 + ((v3 - v1) / 2) * y + (v3 - 2 * v2 + v1) * y * y / 2);
        }



        private void ButAppliFiltre_Click(object sender, RoutedEventArgs e)
        {
            if (ImageDeDep == null)
            {
                System.Windows.Forms.MessageBox.Show("Pas d'image de départ", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            int[,] num;

            switch (ComboFiltre.SelectedIndex)
            {
                case 0: //Filtre moyen
                    num = new int[3, 3] { { 1, 1, 1 }, { 1, 1, 1 }, { 1, 1, 1 } };
                    ImageFinal = new TabInt(ImageDeDep, 9, num, 3).ToBitmap();
                    break;
                case 1: // Filtre moyen pondéré
                    num = new int[3, 3] { { 1, 2, 1 }, { 2, 4, 2 }, { 1, 2, 1 } };
                    ImageFinal = new TabInt(ImageDeDep, 14, num, 3).ToBitmap();
                    break;

                case 2: // filtre Gaussien 3x3 a= 0,391
                    num = new int[3, 3] { { 1, 4, 1 }, { 4, 12, 4 }, { 1, 4, 1 } };
                    ImageFinal = new TabInt(ImageDeDep, 32, num, 3).ToBitmap();
                    break;

                case 3:// filtre Gaussien 5x5 a= 0,691
                    num = new int[5, 5] { { 1, 2, 3, 2, 1 }, { 2, 7, 11, 7, 2 }, { 3, 11, 17, 11, 3 }, { 2, 7, 11, 7, 2 }, { 1, 2, 3, 2, 1 } };
                    ImageFinal = new TabInt(ImageDeDep, 121, num, 5).ToBitmap();
                    break;
                case 4: //filtre médian
                    ImageFinal = new Bitmap(ImageDeDep.Width, ImageDeDep.Height);

                    for (int i = 0; i < ImageDeDep.Width; i++)
                        for (int j = 0; j < ImageDeDep.Height; j++)
                        {


                            int[] Ra = new int[9] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                            int[] Ga = new int[9] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                            int[] Ba = new int[9] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                            int number = 0;
                            int h = 0;
                            for (int u = 0; u < 3; u++)
                                for (int o = 0; o < 3; o++)
                                {

                                    if ((i + u - 1 >= 0) && (i + u - 1 < ImageDeDep.Width) && (j + o - 1 >= 0) && (j + o - 1 < ImageDeDep.Height))
                                    {
                                        number++;
                                        System.Drawing.Color tempo = ImageDeDep.GetPixel(i + u - 1, j + o - 1);

                                        Ra[h] = tempo.R;
                                        Ga[h] = tempo.G;
                                        Ba[h] = tempo.B;
                                        h++;
                                    }
                                }

                            for (int g = 0; g < number; g++)
                            {

                                int exch = 0;
                                for (int v = 0; v < number - 1; v++)
                                {

                                    if (Ra[v + 1] < Ra[v])
                                    {
                                        exch = Ra[v];
                                        Ra[v] = Ra[v + 1];
                                        Ra[v + 1] = exch;
                                    }
                                    if (Ga[v + 1] < Ga[v])
                                    {
                                        exch = Ga[v];
                                        Ga[v] = Ga[v + 1];
                                        Ga[v + 1] = exch;
                                    }
                                    if (Ba[v + 1] < Ba[v])
                                    {
                                        exch = Ba[v];
                                        Ba[v] = Ba[v + 1];
                                        Ba[v + 1] = exch;
                                    }
                                }


                            }


                            ImageFinal.SetPixel(i, j, System.Drawing.Color.FromArgb(Ra[number / 2 + 1], Ga[number / 2 + 1], Ba[number / 2 + 1]));

                        }
                    break;
                case 5:// Filtre Laplacien
                    num = new int[3, 3] { { 0, 1, 0 }, { 1, -4, 1 }, { 0, 1, 0 } };
                    ImageFinal = new TabInt(ImageDeDep, 1, num, 3).ToBitmap();
                    break;

                case 6: // Laplacien bis 
                    num = new int[3, 3] { { 1, 1, 1 }, { 1, -8, 1 }, { 1, 1, 1 } };
                    ImageFinal = new TabInt(ImageDeDep, 1, num, 3).ToBitmap();
                    break;
                case 7: // KIRCH 

                    TabInt[] Tab = new TabInt[8];
                    Tab[0] = new TabInt(ImageDeDep, 1, new int[3, 3] { {  5,  5,  5 },
                                                                       { -3,  0, -3 },
                                                                       { -3, -3, -3 } }, 3);

                    Tab[1] = new TabInt(ImageDeDep, 1, new int[3, 3] { { -3,  5,  5 },
                                                                       { -3,  0,  5 },
                                                                       { -3, -3, -3 } }, 3);

                    Tab[2] = new TabInt(ImageDeDep, 1, new int[3, 3] { { -3, -3,  5 },
                                                                       { -3 , 0,  5 },
                                                                       { -3, -3,  5 } }, 3);

                    Tab[3] = new TabInt(ImageDeDep, 1, new int[3, 3] { { -3, -3, -3 },
                                                                       { -3,  0,  5 },
                                                                       { -3,  5,  5 } }, 3);

                    Tab[4] = new TabInt(ImageDeDep, 1, new int[3, 3] { { -3, -3, -3 },
                                                                       { -3,  0, -3 },
                                                                       {  5,  5,  5 } }, 3);

                    Tab[5] = new TabInt(ImageDeDep, 1, new int[3, 3] { { -3, -3, -3 },
                                                                       {  5,   0, -3 },
                                                                       {  5,   5, -3 } }, 3);

                    Tab[6] = new TabInt(ImageDeDep, 1, new int[3, 3] { {  5,  -3, -3 },
                                                                       {  5,   0, -3 },
                                                                       {  5,  -3, -3 } }, 3);

                    Tab[7] = new TabInt(ImageDeDep, 1, new int[3, 3] { {  5,   5, -3 },
                                                                       {  5,   0, -3 },
                                                                       { -3,  -3, -3 } }, 3);

                    TabInt Dab = new TabInt(ImageDeDep.Width, ImageDeDep.Height);

                    foreach (TabInt de in Tab)
                    {
                        for (int i = 0; i < ImageDeDep.Width; i++)
                            for (int j = 0; j < ImageDeDep.Height; j++)
                            {
                                de.TheTab[0, i, j] /= 15;
                                de.TheTab[1, i, j] /= 15;
                                de.TheTab[2, i, j] /= 15;



                                if (de.TheTab[0, i, j] > Dab.TheTab[0, i, j])
                                    Dab.TheTab[0, i, j] = de.TheTab[0, i, j];
                                if (de.TheTab[1, i, j] > Dab.TheTab[1, i, j])
                                    Dab.TheTab[1, i, j] = de.TheTab[1, i, j];
                                if (de.TheTab[2, i, j] > Dab.TheTab[2, i, j])
                                    Dab.TheTab[2, i, j] = de.TheTab[2, i, j];

                            }

                    }
                    ImageFinal = Dab.ToBitmap();



                    break;
                case 8: // kirsh bis 
                    TabInt[] Tab1 = new TabInt[4];
                    Tab1[0] = new TabInt(ImageDeDep, 1, new int[3, 3] { {  -1,  0,  1 },
                                                                       { -1,  0, 1 },
                                                                       { -1, -0, 1 } }, 3);

                    Tab1[1] = new TabInt(ImageDeDep, 1, new int[3, 3] { { -1,  -1, -1 },
                                                                       { 0,  0,  0 },
                                                                       { 1, 1, 1 } }, 3);

                    Tab1[2] = new TabInt(ImageDeDep, 1, new int[3, 3] { { 0, 1,  1},
                                                                       { -1 , 0,  1 },
                                                                       { -1, -1,  0 } }, 3);

                    Tab1[3] = new TabInt(ImageDeDep, 1, new int[3, 3] { { -1, -1, 0 },
                                                                       { -1,  0,  1 },
                                                                       { 0,  1,  1 } }, 3);

                    

                    TabInt Dab1 = new TabInt(ImageDeDep.Width, ImageDeDep.Height);

                    foreach (TabInt de in Tab1)
                    {
                        for (int i = 0; i < ImageDeDep.Width; i++)
                            for (int j = 0; j < ImageDeDep.Height; j++)
                            {
                                de.TheTab[0, i, j] /= 3;
                                de.TheTab[1, i, j] /= 3;
                                de.TheTab[2, i, j] /= 3;



                                if (de.TheTab[0, i, j] > Dab1.TheTab[0, i, j])
                                    Dab1.TheTab[0, i, j] = de.TheTab[0, i, j];
                                if (de.TheTab[1, i, j] > Dab1.TheTab[1, i, j])
                                    Dab1.TheTab[1, i, j] = de.TheTab[1, i, j];
                                if (de.TheTab[2, i, j] > Dab1.TheTab[2, i, j])
                                    Dab1.TheTab[2, i, j] = de.TheTab[2, i, j];

                            }

                    }
                    ImageFinal = Dab1.ToBitmap();


                    break;
                case 9: // Sobel 1 ( horizontal ) 
                    num = new int[3, 3] { { -1, 0, 1 }, { -2, 0, 2 }, { -1, 0, 1 } };
                    ImageFinal = new TabInt(ImageDeDep, 4, num, 3).ToBitmap();
                    break;
                case 10: // Sobel 2 ( Verticale ) 
                    num = new int[3, 3] { { -1, -2, -1 }, { 0, 0, 0 }, { 1, 2, 1 } };
                    ImageFinal = new TabInt(ImageDeDep, 4, num, 3).ToBitmap();
                    break;
                case 11: //NormeGradient
                    TabInt Grad1 = new TabInt(ImageDeDep, 4, new int[3, 3] { { -1, -2, -1 }, { 0, 0, 0 }, { 1, 2, 1 } }, 3);
                    TabInt Grad2 = new TabInt(ImageDeDep, 4, new int[3, 3] { { -1, 0, 1 }, { -2, 0, 2 }, { -1, 0, 1 } }, 3);
                    ImageFinal = TabInt.NormGrad(Grad1, Grad2).ToBitmap();
                    break;

                case 12: // Prewitt 1 ( horizontal ) 
                    num = new int[3, 3] { { -1, 0, 1 }, { -1, 0, 1 }, { -1, 0, 1 } };
                    ImageFinal = new TabInt(ImageDeDep, 3, num, 3).ToBitmap();
                    break;
                case 13: // Prewitt 2 ( Verticale ) 
                    num = new int[3, 3] { { -1, -1, -1 }, { 0, 0, 0 }, { 1, 1, 1 } };
                    ImageFinal = new TabInt(ImageDeDep, 4, num, 3).ToBitmap();
                    break;
                case 14: //NormeGradient
                    ImageFinal = TabInt.NormGrad(new TabInt(ImageDeDep, 3, new int[3, 3] { { -1, -1, -1 }, { 0, 0, 0 }, { 1, 1, 1 } }, 3), new TabInt(ImageDeDep, 3, new int[3, 3] { { -1, 0, 1 }, { -1, 0, 1 }, { -1, 0, 1 } }, 3)).ToBitmap();
                    break;
                case 15:// Robert
                    ImageFinal = TabInt.NormGrad(new TabInt(ImageDeDep, 1, new int[3, 3] { { 0, 0, 0 }, { 0, 1, 0 }, { 0, 0, -1 } }, 3), new TabInt(ImageDeDep, 1, new int[3, 3] { { 0, 0, 0 }, { 0, 0, 1 }, { 0, -1, 0 } }, 3)).ToBitmap();


                    break;
                case 16: // érosion

                    ImageFinal = new TabInt(ImageDeDep).Erode().ToBitmap();
                    break;
                case 17: // dilatation
                    ImageFinal = new TabInt(ImageDeDep).Dilate().ToBitmap();
                    break;

                case 18: // Overture 
                    ImageFinal = new TabInt(ImageDeDep).Erode().Dilate().ToBitmap();
                    break;
                case 19: //fermeture
                    ImageFinal = new TabInt(ImageDeDep).Dilate().Erode().ToBitmap();
                    break;
            }

            UpdateImageFinal();
        }

        private void ButQuadSave(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "Quad Image|*.Quad";
            dialog.Title = "Save an Image File";
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                IFormatter formatter = new BinaryFormatter();
                Stream stream = new FileStream(dialog.FileName, FileMode.Create, FileAccess.Write, FileShare.None);
                formatter.Serialize(stream, new QuadTreeBase(ImageDeDep));
                stream.Close();

            }
        }

        private void ButTry(object sender, RoutedEventArgs e)
        {
            if (ImageDeDep == null)
            {
                System.Windows.Forms.MessageBox.Show("Pas d'image original", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            ImageFinal = new QuadTreeBase(ImageDeDep).ToBitmap();
            UpdateImageFinal();
        }

        private void ButDeptoFinal_Click(object sender, RoutedEventArgs e)
        {
            if (ImageDeDep == null)
            {
                System.Windows.Forms.MessageBox.Show("Pas d'image original", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            int j = ImageFinal.Height;
            int i = ImageFinal.Width;
            Bitmap dab = ImageDeDep;
            ImageDeDep = ImageFinal;
            ImageFinal = dab;
            UpdateImageFinal();
            UpdateImageDep();
            // ConvertBitmap(ImageDeDep);

            TextH_P1.Text = 0.ToString();
            TextH_P2.Text = 0.ToString();
            TextL_P1.Text = 0.ToString();
            TextL_P2.Text = 0.ToString();
        }

        private void Decoupe1_Click(object sender, RoutedEventArgs e)
        {
            if (ImageDeDep == null)
            {
                System.Windows.Forms.MessageBox.Show("Pas d'image de départ", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (ImageDeDep.Width != ImageDeDep.Height || Math.Pow((int)Math.Sqrt(ImageDeDep.Width), 2) == ImageDeDep.Width)
                if (System.Windows.Forms.MessageBox.Show("Pas d'image de départ", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error) == System.Windows.Forms.DialogResult.No)
                    return;
            ImageFinal = new QuadTreeBase(ImageDeDep).Decoupage();
             UpdateImageFinal();
            
        }

        private void Decoupe2_Click(object sender, RoutedEventArgs e)
        {
            Bitmap Tempo = new QuadTreeBase(ImageDeDep).ToBitmap();
            ImageFinal = new Bitmap(ImageDeDep.Width, ImageDeDep.Height);
            if (BoolTCO.IsChecked == false)
            {


                for (int x = 1; x < ImageDeDep.Width - 1; x++)
                    for (int y = 1; y < ImageDeDep.Height - 1; y++)
                        if (Tempo.GetPixel(x, y) == Tempo.GetPixel(x + 1, y) && Tempo.GetPixel(x, y) == Tempo.GetPixel(x, y + 1) && Tempo.GetPixel(x, y) == Tempo.GetPixel(x - 1, y) && Tempo.GetPixel(x, y) == Tempo.GetPixel(x, y - 1))
                            ImageFinal.SetPixel(x, y, Tempo.GetPixel(x, y));
                        else
                        {
                            ImageFinal.SetPixel(x, y, Color.Black);

                        }
            }
            else
            { 
                for (int x = 1; x < ImageDeDep.Width - 1; x++)
                    for (int y = 1; y < ImageDeDep.Height - 1; y++)
                        if (QuadTree.TCO(Tempo.GetPixel(x, y), Tempo.GetPixel(x + 1, y))&& QuadTree.TCO(Tempo.GetPixel(x, y),Tempo.GetPixel(x, y + 1)) && QuadTree.TCO(Tempo.GetPixel(x, y),  Tempo.GetPixel(x - 1, y)) && QuadTree.TCO(Tempo.GetPixel(x, y) , Tempo.GetPixel(x, y - 1)))
                            ImageFinal.SetPixel(x, y, Tempo.GetPixel(x, y));
                        else
                        {
                            ImageFinal.SetPixel(x, y, Color.Black);

                        }
            }
            UpdateImageFinal();
        }

        private void ValTCO_TextChanged(object sender, RoutedEventArgs e)
        {
            try
            {
                int v = Convert.ToInt32(ValTCO.Text);
                
                if (v < 0 || v > 256)
                {

                    System.Windows.Forms.MessageBox.Show("Valeur de X trop petite ou trop grande", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else QuadTree.Val = v;
                
            }
            catch (FormatException)
            {
                System.Windows.Forms.MessageBox.Show("X n'est pas un nombre correcte", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            catch (OverflowException)
            {
                System.Windows.Forms.MessageBox.Show("x est trop grand (int32)", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        
        
    }

}


