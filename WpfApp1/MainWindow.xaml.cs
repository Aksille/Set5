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
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Windows.Interop;
using System.IO;
using System.Drawing.Imaging;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public int[] PoleAll;
        public int pocetKaret = 40;
        public int[] PoleChosed;
        public int numberOfChosed;
        public Bitmap[] PoleBitmap;
        public System.Windows.Controls.Image[] poleImage;
        public Button[] poleButtonDelete;
        public System.Drawing.Color[] poleColors;
        public DateTime dtTotal;
        public DateTime dtLast;
        public int NumberOfVictorious;
        public MainWindow()
        {
            dtTotal = DateTime.Now;
            dtLast = DateTime.Now;
            NumberOfVictorious = 0;
            InitializeComponent();
            Main();
        }
        public bool Main()
        {
            generateBitmaps();
            generateCards();
            generateFirstRound();
            generateButtons();
            return true;
        }

        public void generateBitmaps()
        {
            poleColors = new System.Drawing.Color[5];

            Bitmap b = new Bitmap(@"obr\01.png");
            for (int i = 0; i < 5; i++)
                poleColors[i]=b.GetPixel(i, 0);

            PoleBitmap = new Bitmap[25];
            Uri uri = new Uri(@"obr\11.png", UriKind.Relative);
            for (int i = 0; i < 5; i++)
                for (int j = 0; j < 5; j++)
                    //PoleBitmap[i * 5 + j] = new Bitmap(@"C:\Users\ivanovsky\Desktop\ted\obr\" + ((i+1).ToString()) + ((j+1).ToString()) + ".png");
                    PoleBitmap[i * 5 + j] = new Bitmap(@"obr\" + ((i + 1).ToString()) + ((j + 1).ToString()) + ".png");
            poleImage = new System.Windows.Controls.Image[5];
            for (int i = 0; i < 5; i++)
                poleImage[i] = new System.Windows.Controls.Image();
        }
        public void generateCards()
        {
            numberOfChosed = 0;
            PoleChosed = new int[5];
            PoleAll = new int[625];
            for (int i = 0; i < 625; i++)
                PoleAll[i]=-1;
        }
        public void generateFirstRound()
        {
            for (int i = 0; i < pocetKaret; i++)
            {
                bool b = true;
                int r;
                Random rn=new Random();
                
                while (b)
                {
                    b = false;
                    r = rn.Next() % 625;
                    PoleAll[i] = r;
                    for (int j = 0; j < i; j++)
                        if (PoleAll[j] == r) b = true;
                }                
            }
        }
        public void generateFourMore()
        {
            pocetKaret += 4;
            if (pocetKaret>623)
            {
                throw new Exception("Konec, dosly karty, pokud se ti to nezda, tak mi napis - pac a pusu J");
            }
            for (int i = (pocetKaret-4); i < pocetKaret; i++)
            {
                bool b = true;
                int r;
                Random rn = new Random();

                while (b)
                {
                    b = false;
                    r = rn.Next() % 625;
                    PoleAll[i] = r;
                    for (int j = 0; j < i; j++)
                        if (PoleAll[j] == r) b = true;
                }
            }
        }
        public void chooseColor(System.Windows.Controls.Button b,int x)
        {
            if (x==0)
                b.Background = System.Windows.Media.Brushes.Blue;
            else if (x == 1)
                b.Background = System.Windows.Media.Brushes.Red;
            else if(x == 2)
                b.Background = System.Windows.Media.Brushes.Yellow;
            else if (x == 3)
                b.Background = System.Windows.Media.Brushes.Green;
            else b.Background = System.Windows.Media.Brushes.Pink;
        }

        public static Bitmap ReplaceColor(Bitmap source, System.Drawing.Color toReplace, System.Drawing.Color replacement)
        {
            var target = new Bitmap(source.Width, source.Height);

            for (int x = 0; x < source.Width; ++x)
            {
                for (int y = 0; y < source.Height; ++y)
                {
                    var color = source.GetPixel(x, y);
                    target.SetPixel(x, y, (color.ToArgb() == toReplace.ToArgb()) ? replacement : color);
                }
            }

            return target;
        }
        public static Bitmap ReplaceColor2(Bitmap source, System.Drawing.Color toReplace, System.Drawing.Color replacement)
        {
            var target = new Bitmap(source.Width, source.Height);

            for (int x = 0; x < source.Width; ++x)
            {
                for (int y = 0; y < source.Height; ++y)
                {
                    var color = source.GetPixel(x, y);
                    target.SetPixel(x, y, (color.ToArgb() != toReplace.ToArgb()) ? replacement : color);
                }
            }

            return target;
        }

        public BitmapImage ToBitmapImage(Bitmap bitmap)
        {
            using (var memory = new MemoryStream())
            {
                bitmap.Save(memory, ImageFormat.Png);
                memory.Position = 0;

                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                bitmapImage.Freeze();

                return bitmapImage;
            }
        }

        public System.Drawing.Color returnColor(int x)
        {
            if (x == 0)
                return poleColors[0];
            else if (x == 1)
                return poleColors[1];
            else if (x == 2)
                return poleColors[2];
            else if (x == 3)
                return poleColors[3];
            else
                return poleColors[4];
        }
        public void chooseEverything(System.Windows.Controls.Button b, int x, int y, int z, int a)
        {
            Bitmap bm = PoleBitmap[x * 5 + y];
            System.Drawing.Color c = returnColor(z);
            bm = ReplaceColor2(bm, System.Drawing.Color.White, c);
           
            var bimage = new BitmapImage();
            bimage = ToBitmapImage(bm);
            StackPanel stackPnl = new StackPanel();
            stackPnl.Orientation = Orientation.Horizontal;
            stackPnl.Margin = new Thickness(10);
            for (int i=0;i<(a+1);i++)
            {
                poleImage[i] = new System.Windows.Controls.Image();
                poleImage[i].Source = bimage.Clone();
                poleImage[i].VerticalAlignment = VerticalAlignment.Center;
                stackPnl.Children.Add(poleImage[i]);
            }
            b.Content = stackPnl;
            b.Background = System.Windows.Media.Brushes.White;

        }
        public void chooseSymbolsAndNumberOfThem(System.Windows.Controls.Button b, int x, int y)
        {

            System.Windows.Controls.Image img = new System.Windows.Controls.Image();

            // img = new BitmapImage(new Uri("jan.png", UriKind.Relative));
            Bitmap bm = new Bitmap(@"C:\Users\ivanovsky\Desktop\ted\obr\11.png");
            bm=ReplaceColor2(bm, System.Drawing.Color.White, System.Drawing.Color.Pink);
            var bimage = new BitmapImage();//new Uri(@"C:\Users\ivanovsky\Desktop\ted\WpfApp1\WpfApp1\bin\Debug\jan.png"));//(new Uri(@"ian.png", UriKind.Relative));
            bimage = ToBitmapImage(bm);
            img.Source = bimage.Clone();
            img.VerticalAlignment = VerticalAlignment.Center;
            StackPanel stackPnl = new StackPanel();
            stackPnl.Orientation = Orientation.Horizontal;
            stackPnl.Margin = new Thickness(10);
            stackPnl.Children.Add(img);
            var img2 = new System.Windows.Controls.Image();
            img2.Source = bimage.Clone();
            stackPnl.Children.Add(img2);
            stackPnl.VerticalAlignment = VerticalAlignment.Center;
            //bimage


            b.FontSize = 15;
            if (x == 0)
                for (int i = 0; i < (y + 1); i++)
                {
                    b.Content = stackPnl;
                    //b.Content += "♥";
                }
            else if (x == 1)
                for (int i = 0; i < (y + 1); i++)
                    b.Content += "♦";
            else if (x == 2)
                for (int i = 0; i < (y + 1); i++)
                    b.Content += "♣";
            else if (x == 3)
                for (int i = 0; i < (y + 1); i++)
                    b.Content += "♠";
            else for (int i = 0; i < (y + 1); i++)
                    b.Content += "•";
        }

        public void chooseFilling(System.Windows.Controls.Button b, int x)
        {
           
            if (x == 0)
                b.Foreground = System.Windows.Media.Brushes.LightBlue;
            else if (x == 1)
                b.Foreground = System.Windows.Media.Brushes.IndianRed;
            else if (x == 2)
                b.Foreground = System.Windows.Media.Brushes.LightYellow;
            else if (x == 3)
                b.Foreground = System.Windows.Media.Brushes.LightGreen;
            else b.Foreground = System.Windows.Media.Brushes.LightPink;
        }


        public bool areSame(int a,int b,int c,int d, int e)
        {
            return ((a == b)&&(a == c) &&(a == d)&&(a == e));
        }
        public bool areEvery(int a, int b, int c, int d, int e)
        {
            if ((a + 1) * (b + 1) * (c + 1) * (d + 1) * (e + 1) == 120)
                if ((a + b + c + d + e) == 10)
                    return true;
            return false;
        }
        public bool isVictorious()
        {
            if (   (areSame(PoleChosed[0] / 125 % 5, PoleChosed[1] / 125 % 5, PoleChosed[2] / 125 % 5, PoleChosed[3] / 125 % 5, PoleChosed[4] / 125 % 5) | areEvery(PoleChosed[0] / 125 % 5, PoleChosed[1] / 125 % 5, PoleChosed[2] / 125 % 5, PoleChosed[3] / 125 % 5, PoleChosed[4] / 125 % 5))
                && (areSame(PoleChosed[0] / 25 % 5, PoleChosed[1] / 25 % 5, PoleChosed[2] / 25 % 5, PoleChosed[3] / 25 % 5, PoleChosed[4] / 25 % 5) | areEvery(PoleChosed[0] / 25 % 5, PoleChosed[1] / 25 % 5, PoleChosed[2] / 25 % 5, PoleChosed[3] / 25 % 5, PoleChosed[4] / 25 % 5))
                && (areSame(PoleChosed[0] / 5 % 5, PoleChosed[1] / 5 % 5, PoleChosed[2] / 5 % 5, PoleChosed[3] / 5 % 5, PoleChosed[4] / 5 % 5) | areEvery(PoleChosed[0] / 5 % 5, PoleChosed[1] / 5 % 5, PoleChosed[2] / 5 % 5, PoleChosed[3] / 5 % 5, PoleChosed[4] / 5 % 5))
                && (areSame(PoleChosed[0] % 5, PoleChosed[1] % 5, PoleChosed[2] % 5, PoleChosed[3] % 5, PoleChosed[4] % 5) | areEvery(PoleChosed[0] % 5, PoleChosed[1] % 5, PoleChosed[2] % 5, PoleChosed[3] % 5, PoleChosed[4] % 5))
                )
            {
                return true;
            }
            return false;
        }

        public void searchAndKill()
        {

            for (int i = 0; i < 5; i++)
            {
                var gh = sp1.Children.OfType<Button>();
                var counter = PoleChosed[i];
                var child = sp1.Children.OfType<Button>().Where(x => x.Name == "Button" + counter)?.FirstOrDefault();
                if (child != null)
                    sp1.Children.Remove(child);
                else
                {
                    child = sp2.Children.OfType<Button>().Where(x => x.Name == "Button" + counter)?.FirstOrDefault();
                    if (child != null)
                        sp2.Children.Remove(child);
                    else
                    {
                        child = sp3.Children.OfType<Button>().Where(x => x.Name == "Button" + counter)?.FirstOrDefault();
                        if (child != null)
                            sp3.Children.Remove(child);
                        else
                        {
                            child = sp4.Children.OfType<Button>().Where(x => x.Name == "Button" + counter)?.FirstOrDefault();
                            if (child != null)
                                sp4.Children.Remove(child);
                            else
                            {
                                throw new Exception("Karta nebyla nalezena, prosim kontaktujte administratora Ja");
                            }
                                
                        }
                    }
                }
                child = null;
            }
        }
        public void searchAndDestroy()
        {
            for (int i = 0; i < 5; i++)
            {
                var gh = sp1.Children.OfType<Button>();
                var counter = PoleChosed[i];
                var child = sp1.Children.OfType<Button>().Where(x => x.Name == "Button" + counter)?.FirstOrDefault();
                if (child != null)
                    child.Background = System.Windows.Media.Brushes.White;
                else
                {
                    child = sp2.Children.OfType<Button>().Where(x => x.Name == "Button" + counter)?.FirstOrDefault();
                    if (child != null)
                        child.Background = System.Windows.Media.Brushes.White;
                    else
                    {
                        child = sp3.Children.OfType<Button>().Where(x => x.Name == "Button" + counter)?.FirstOrDefault();
                        if (child != null)
                            child.Background = System.Windows.Media.Brushes.White;
                        else
                        {
                            child = sp4.Children.OfType<Button>().Where(x => x.Name == "Button" + counter)?.FirstOrDefault();
                            if (child != null)
                                child.Background = System.Windows.Media.Brushes.White;
                            else
                            {
                                throw new Exception("Karta nebyla nalezena, prosim kontaktujte administratora Ja");
                            }

                        }
                    }
                }
                //child = null;
            }
        }


        private void newBtn_Click(object sender, RoutedEventArgs e)
        {
            if (((Button)sender).Background == System.Windows.Media.Brushes.LightYellow)
            {
                ((Button)sender).Background = System.Windows.Media.Brushes.White;
                bool a = false;
                for (int i = 0; i < numberOfChosed; i++)
                {
                    if (PoleChosed[i] == Int32.Parse(((Button)sender).Name.Remove(0, 6)))
                        a = true;
                    if (a)
                    {
                        if ((i + 1) < numberOfChosed) PoleChosed[i] = PoleChosed[i + 1];
                        else PoleChosed[i] = 0;
                    }
                }
                numberOfChosed--;
                if (numberOfChosed < 0) numberOfChosed = 0;


            }
            else
            {
                PoleChosed[numberOfChosed++] = Int32.Parse(((Button)sender).Name.Remove(0, 6));
                ((Button)sender).Background = System.Windows.Media.Brushes.LightYellow;
                if (numberOfChosed == 5)
                {
                    Window popup = new Window();
                    popup.Height = 100;
                    popup.Width = 300;
                    numberOfChosed = 0;
                    if (isVictorious())
                    {
                        popup.Title = "Mia vyhrala! Last: " + (DateTime.Now - dtLast) + " Total: " + (DateTime.Now - dtTotal);
                        dtLast = DateTime.Now;
                        NumberOfVictorious++;
                        searchAndKill();
                        StackPanel stackPnl = new StackPanel();
                        stackPnl.Orientation = Orientation.Horizontal;
                        stackPnl.Margin = new Thickness(10);

                        Bitmap bm = PoleBitmap[1]; //srdicko
                        System.Drawing.Color c = System.Drawing.Color.Pink;
                        bm = ReplaceColor2(bm, System.Drawing.Color.White, c);

                        var bimage = new BitmapImage();
                        bimage = ToBitmapImage(bm);

                        var i1 = new System.Windows.Controls.Image();
                        i1.Source = bimage.Clone();
                        i1.VerticalAlignment = VerticalAlignment.Center;
                        stackPnl.Children.Add(i1);
                        popup.Content = stackPnl;
                        numberOfChosed = 0;
                        popup.ShowDialog();
                    }
                    else
                    {
                        popup.Title = "zvoleno pet a reset pole. Last: " + (DateTime.Now - dtLast) + " Total: " + (DateTime.Now - dtTotal);
                        searchAndDestroy();
                        numberOfChosed = 0;
                    }


                    //popup.ShowDialog();
                }
            }
        }

        public void generateButtons()
        {

            for (int i = 0; i < pocetKaret; i++)
            {
                System.Windows.Controls.Button newBtn = new Button();

                //newBtn.Content = (PoleAll[i]/125%5).ToString()+ (PoleAll[i] / 25 % 5).ToString()+ (PoleAll[i] / 5 % 5).ToString()+ (PoleAll[i] % 5).ToString();
                newBtn.Name = "Button" + PoleAll[i].ToString();
                newBtn.VerticalAlignment = VerticalAlignment.Top;

                //chooseColor(newBtn, (PoleAll[i] / 125 % 5));
                //chooseSymbolsAndNumberOfThem(newBtn, (PoleAll[i] / 25 % 5), (PoleAll[i] / 5 % 5));
                //chooseFilling(newBtn, (PoleAll[i] % 5));
                chooseEverything(newBtn, (PoleAll[i] / 125 % 5), (PoleAll[i] / 25 % 5), (PoleAll[i] / 5 % 5), (PoleAll[i] % 5));
                //chooseFillingAndColor(newBtn, (PoleAll[i] / 125 % 5), (PoleAll[i] % 5));
                newBtn.Click += new RoutedEventHandler(newBtn_Click);
                if (i*4/pocetKaret==0)
                    sp1.Children.Add(newBtn);                    
                else if (i*4/pocetKaret == 1)
                    sp2.Children.Add(newBtn);
                else if (i*4/pocetKaret == 2)
                    sp3.Children.Add(newBtn);
                else
                    sp4.Children.Add(newBtn);
            }
        }
        public void generateFourMoreButtons()
        {

            for (int i = pocetKaret-4; i < pocetKaret; i++)
            {
                System.Windows.Controls.Button newBtn = new Button();

                //newBtn.Content = (PoleAll[i]/125%5).ToString()+ (PoleAll[i] / 25 % 5).ToString()+ (PoleAll[i] / 5 % 5).ToString()+ (PoleAll[i] % 5).ToString();
                newBtn.Name = "Button" + PoleAll[i].ToString();
                newBtn.VerticalAlignment = VerticalAlignment.Top;
                chooseEverything(newBtn, (PoleAll[i] / 125 % 5), (PoleAll[i] / 25 % 5), (PoleAll[i] / 5 % 5), (PoleAll[i] % 5));
                newBtn.Click += new RoutedEventHandler(newBtn_Click);
                //newBtn.AddToEventRoute(newBtn_Click);
                if (i == pocetKaret - 4)
                    sp1.Children.Add(newBtn);
                else if (i == pocetKaret - 3)
                    sp2.Children.Add(newBtn);
                else if (i == pocetKaret - 2)
                    sp3.Children.Add(newBtn);
                else
                    sp4.Children.Add(newBtn);
            }
        }

        private void bx_Click(object sender, RoutedEventArgs e)
        {
            //PoleChosed[numberOfChosed++] = Int32.Parse(e.Source.ToString());
            generateFourMore();
            generateFourMoreButtons();
        }
        public void RestartEverything()
        {
            //poleButtonDelete = new Button[pocetKaret];
            //int i = 0;
            //foreach (var ch in sp1.Children.OfType<Button>()) //odstrani tlacitka
            //    poleButtonDelete[i++] = ch;
            //for (int j = 0; j < i; j++)
            //    sp1.Children.Remove(poleButtonDelete[j]);
            pocetKaret = 40;
            sp1.Children.Clear();
            sp2.Children.Clear();
            sp3.Children.Clear();
            sp4.Children.Clear();
            Main(); //vygeneruje vse jak na zacatku
        }

        private void br_Click(object sender, RoutedEventArgs e)
        {
            RestartEverything();
        }
    }
}
