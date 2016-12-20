using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Drawing;

/* @author: Tammie Thong (tt24)
 * @date: 4/11/2016
 * 
 * Fern.cs
 * This program creates a fern and randomizes berries, position of branches, and the "season."
 * The three different "shapes" are a flower pot, line, ellipse(berry)
 * */



namespace FernNamespace
{
    /* The fern class creates a fern and randomizes berries, position of branches, and the "season."
     * Bugs: WPF and shape objects are the wrong tool for the task 
     */
    class Fern
    {
        private static int BERRYMIN = 7;
        private static int TENDRILS = 10;
        private static int TENDRILMIN = 4;
        private static double DELTATHETA = 0.01;
        private static double SEGLENGTH = 3.0;
        private bool season = true;


        /* 
         * Fern constructor erases screen and draws a fern
         * 
         * Size: number of 3-pixel segments of tendrils
         * Redux: how much smaller children clusters are compared to parents
         * Turnbias: how likely to turn right vs. left (0=always left, 0.5 = 50/50, 1.0 = always right)
         * canvas: the canvas that the fern will be drawn on
         * 
         * The cluster size is Based in the position of the slider. 
         * A color is randomly generated and changes the "season" boolean value

         */
        public Fern(double size, double redux, double turnbias, Canvas canvas)
        {
            canvas.Children.Clear();                                // delete old canvas contents
            ImageBrush ib = new ImageBrush(); // Creates background image
            ib.ImageSource = new BitmapImage(new Uri(@"../../table.jpg", UriKind.Relative));
            canvas.Background = ib;
            Random random = new Random();
            int color = random.Next(0, 4); //randomly generate a number, which determines the "season" which is a bool value
            if (color < 2)
            {
                season = false;
            }
            if (size < 15)
            {
                cluster((int)(canvas.Width / 2) - 85, (int)(canvas.Height * .25) + 25, 15, redux, turnbias, canvas); //creates upper "bush"
                cluster((int)(canvas.Width / 2) - 95, (int)(canvas.Height * .25) + 25, 10, redux, turnbias, canvas);
                cluster((int)(canvas.Width / 2) - 85, (int)(canvas.Height * .5) - 18, 23, redux, turnbias, canvas); //creates lower "bush"
                cluster((int)(canvas.Width / 2) - 95, (int)(canvas.Height * .5) - 18, 15, redux, turnbias, canvas);
            }
            else if (size < 10)
            {
                cluster((int)(canvas.Width / 2) - 85, (int)(canvas.Height * .25) + 25, 20, redux, turnbias, canvas);//creates upper "bush"
                cluster((int)(canvas.Width / 2) - 95, (int)(canvas.Height * .25) + 25, 13, redux, turnbias, canvas);
                cluster((int)(canvas.Width / 2) - 85, (int)(canvas.Height * .5) - 18, 23, redux, turnbias, canvas);//creates lower "bush"
                cluster((int)(canvas.Width / 2) - 95, (int)(canvas.Height * .5) - 18, 15, redux, turnbias, canvas);
            }
            else if (size < 25)
            {
                cluster((int)(canvas.Width / 2) - 85, (int)(canvas.Height * .25) + 25, 28, redux, turnbias, canvas);//creates upper "bush"
                cluster((int)(canvas.Width / 2) - 95, (int)(canvas.Height * .25) + 25, 21, redux, turnbias, canvas);
                cluster((int)(canvas.Width / 2) - 85, (int)(canvas.Height * .5) - 18, 30, redux, turnbias, canvas);//creates lower "bush"
                cluster((int)(canvas.Width / 2) - 95, (int)(canvas.Height * .5) - 18, 23, redux, turnbias, canvas);
            }
            else
            {
                cluster((int)(canvas.Width / 2) - 85, (int)(canvas.Height * .25) + 25, 35, redux, turnbias, canvas);//creates upper "bush"
                cluster((int)(canvas.Width / 2) - 95, (int)(canvas.Height * .25) + 25, 30, redux, turnbias, canvas);
                cluster((int)(canvas.Width / 2) - 85, (int)(canvas.Height * .5) - 18, 35, redux, turnbias, canvas);//creates lower "bush"
                cluster((int)(canvas.Width / 2) - 95, (int)(canvas.Height * .5) - 18, 30, redux, turnbias, canvas);
            }


        }

        /*
         * cluster draws a cluster at the given location and then draws a bunch of tendrils out in 
         * regularly-spaced directions out of the cluster.
         * @param: int x, int y, double size, double redux, double turnbias, Canvas canvas
         */
        private void cluster(int x, int y, double size, double redux, double turnbias, Canvas canvas)
        {
            for (int i = 0; i < TENDRILS; i++)
            {
                // compute the angle of the outgoing tendril
                double theta = (i * Math.PI / TENDRILS) + Math.PI / 2;
                tendril(x, y, (size * .8), redux, turnbias, theta, canvas);

                if ((size > BERRYMIN) && season == false)
                    berry(x, y, 7, canvas);
            }
        }

        /*
         * tendril draws a tendril (a randomly-wavy line) in the given direction, for the given length, 
         * and draws a cluster at the other end if the line is big enough.
         * @param: int x1, int y1, double size, double redux, double turnbias, double direction, Canvas canvas
         */
        private void tendril(int x1, int y1, double size, double redux, double turnbias, double direction, Canvas canvas)
        {
            int x2 = x1, y2 = y1;
            Random random = new Random();
            x1 = x2; y1 = y2;
            x2 = x1 + (int)(SEGLENGTH * Math.Sin(direction));
            y2 = y1 + (int)(SEGLENGTH * -direction);
            if (season == false) //checks to see if it is "augumn" or "summer" and colors leaves accordingly 1 set of colors
            {
                line(x1, y1, x2, y2, 51, 102, 0, 1 + (size / 80), canvas);
            }
            else
            {
                line(x1, y1, x2, y2, 240, 144, 72, 1 + (size / 80), canvas);
            }

            double localTurnbias = turnbias; //sets a local variable to determine value of DELTATHETA
            double originalDir = direction;
            for (int i = 0; i < size; i++)
            {
                direction += (random.NextDouble() > localTurnbias) ? -1 * DELTATHETA : DELTATHETA;
                x1 = x2; y1 = y2;
                x2 = x1 + (int)(SEGLENGTH * Math.Sin(direction));
                y2 = y1 + (int)(SEGLENGTH * Math.Cos(direction));
                byte red = (byte)(100 + size / 2);
                byte green = (byte)(220 - size / 3);
                if (size > 120) red = 138; green = 108;
                if (season == false) //checks to see if it is "augumn" or "summer" and colors leaves accordingly for 2nd set of colors
                {
                    line(x1, y1, x2, y2, red, green, 0, 1 + (size / 80), canvas);
                }
                else
                {
                    line(x1, y1, x2, y2, 216, 72, 48, 1 + (size / 80), canvas);
                }

            }

            if (size > TENDRILMIN)
                cluster(x2, y2, size / redux, redux, turnbias, canvas);
        }

        /*
         * draw a red circle centered at (x,y), radius radius, with a black edge, onto canvas
         * @param:int x, int y, double radius, Canvas canvas
         */
        private void berry(int x, int y, double radius, Canvas canvas)
        {
            Ellipse myEllipse = new Ellipse();
            SolidColorBrush mySolidColorBrush = new SolidColorBrush();
            mySolidColorBrush.Color = Color.FromArgb(255, 255, 0, 0);
            myEllipse.Fill = mySolidColorBrush;
            myEllipse.StrokeThickness = 1;
            myEllipse.Stroke = Brushes.Black;
            myEllipse.HorizontalAlignment = HorizontalAlignment.Center;
            myEllipse.VerticalAlignment = VerticalAlignment.Center;
            myEllipse.Width = radius;
            myEllipse.Height = radius;
            myEllipse.SetCenter(x, y);
            canvas.Children.Add(myEllipse);
        }

        /*
         * draw a line segment (x1,y1) to (x2,y2) with given color, thickness on canvas
         * @param: int x1, int y1, int x2, int y2, byte r, byte g, byte b, double thickness, Canvas canvas
         * @author: Harry Plantinga
         */
        private void line(int x1, int y1, int x2, int y2, byte r, byte g, byte b, double thickness, Canvas canvas)
        {
            Line myLine = new Line();
            SolidColorBrush mySolidColorBrush = new SolidColorBrush();
            mySolidColorBrush.Color = Color.FromArgb(255, r, g, b);
            myLine.X1 = x1;
            myLine.Y1 = y1;
            myLine.X2 = x2;
            myLine.Y2 = y2;
            myLine.Stroke = mySolidColorBrush;
            myLine.VerticalAlignment = VerticalAlignment.Center;
            myLine.HorizontalAlignment = HorizontalAlignment.Left;
            myLine.StrokeThickness = thickness;
            canvas.Children.Add(myLine);
        }
    }
}

/*
 * this class is needed to enable us to set the center for an ellipse (not built in?!)
 * @author: Harry Plantinga
 */
public static class EllipseX
{
    public static void SetCenter(this Ellipse ellipse, double X, double Y)
    {
        Canvas.SetTop(ellipse, Y - ellipse.Height / 2);
        Canvas.SetLeft(ellipse, X - ellipse.Width / 2);
    }
}

