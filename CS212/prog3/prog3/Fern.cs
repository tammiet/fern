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

//namespace FernNamespace
//{
    //    /*
    //     * this class draws a fractal fern when the constructor is called.
    //     * Written as sample C# code for a CS 212 assignment -- October 2011.
    //     * 
    //     * Bugs: WPF and shape objects are the wrong tool for the task 
    //     */
    //    class Fern
    //    {

    //        double[,] stem = { { 0, 0 }, { 0, 0.16 } };
    //        double[,] stem_trans =  { { 0, 0 }, { 0, 0 } };
    //        double[,] smallerLeaves = { { 0.85, 0.04 }, { 0.04, 0.85 } };
    //        double[,] smallerLeaves_trans = { { 0, 0 }, { 1.6, 1.6 } };
    //        double[,] largeLeftLeaf = { { 0.2, -0.26 }, { 0.23, 0.22 } };
    //        double[,] largeLeftLeaf_trans = { { 0, 0 }, { 1.6, 1.6 } };
    //        double[,] largeRightLeaf = { { -0.15, 0.28 }, { 0.26, 0.24 } };
    //        double[,] largeRightLeaf_trans = { { 0, 0 }, { 0.44, 0.44 } };
    //        double p_stem = 0.01;
    //        double p_smallerLeaves = 0.85;
    //        double p_largeLeftLeaf = 0.07;
    //        double p_largeRightLeaf = 0.07;
    //        private static int BERRYMIN = 10;
    //        private static int TENDRILS = 7;
    //        private static int TENDRILMIN = 10;
    //        private static double DELTATHETA = 0.1;
    //        private static double SEGLENGTH = 3.0;

    //        private static int iterations = 100000;


    //        public Fern(double size, double redux, double turnbias, Canvas canvas) {
    //            canvas.Children.Clear();
    //            int x = (int)canvas.Width / 2;
    //            int y = (int)canvas.Height / 2;
    //            cluster(x, y, size, redux, turnbias, canvas);
    //        }
    //        private void cluster(int x, int y, double size, double redux, double turnbias, Canvas canvas)
    //        {
    //            double[,] vec = { { x, x}, {y, y} };

    //            for (int i = 2; i < 2; i++)
    //            {
    //                Random k = new Random();
    //                double rand = k.NextDouble() * (1);
    //                if (rand < p_stem)
    //                {


    //                    vec = multiplyMatrix(stem, vec);
    //                    vec = addMatrix(vec, stem_trans);
    //                }
    //                else if (rand < (p_stem + p_smallerLeaves))
    //                {
    //                    vec = multiplyMatrix(smallerLeaves, vec);
    //                    vec = addMatrix(vec, smallerLeaves_trans);
    //                }
    //                else if (rand < (p_stem + p_smallerLeaves + p_largeLeftLeaf))
    //                {
    //                    vec = multiplyMatrix(vec, largeLeftLeaf);
    //                    vec = addMatrix(vec, largeLeftLeaf_trans);
    //                }
    //                else
    //                {
    //                    vec = multiplyMatrix(vec, largeRightLeaf);
    //                    vec = addMatrix(vec, largeRightLeaf_trans);
    //                }
    //                 line(vec, size, canvas);
    //                Console.Write("size " + size);
    //            } 

    //        }

    //        private double[,] multiplyMatrix(double[,] matrix, double[,] vec)
    //        {
    //            double[,] vec1 = new double[2, 2];
    //            for (int i = 0; i < 2; i++)
    //            {
    //                for (int j = 0; j < 2; j++)
    //                {
    //                    vec1[i, j] = 0;
    //                    for (int a = 0; a < 2; a++)
    //                    {
    //                        vec1[i, j] = vec[i, a] * matrix[a, j];
    //                    }
    //                }
    //            }
    //            return vec1;
    //        }
    //        private double[,] addMatrix(double[,] matrix, double[,] vec)
    //        {
    //            double[,] vec1 = new double[2, 2];
    //            for (int i = 0; i < 2; i++)
    //            {
    //                for (int j = 0; j < 2; j++)
    //                {
    //                    vec1[i, j] = 0;
    //                    for (int a = 0; a < 2; a++)
    //                    {
    //                        vec1[i, j] = vec[i, a] + matrix[a, j];
    //                    }
    //                }
    //            }
    //            return vec1;
    //        }

    //        private void line(double[,] matrix, double size, Canvas canvas)
    //        {
    //            Line myLine = new Line();
    //            SolidColorBrush mySolidColorBrush = new SolidColorBrush();
    //            byte r = (byte)(100 + size / 2);
    //            byte g = (byte)(220 - size / 3);
    //            byte b = (byte)(1 + size / 80);
    //            mySolidColorBrush.Color = Color.FromArgb(255, r, g, b);
    //            myLine.X1 = matrix[0, 0];
    //            myLine.Y1 = matrix[1, 0];
    //            myLine.X2 = matrix[0, 1];
    //            myLine.Y2 = matrix[1, 1];
    //            myLine.Stroke = mySolidColorBrush;
    //            myLine.VerticalAlignment = VerticalAlignment.Center;
    //            myLine.HorizontalAlignment = HorizontalAlignment.Left;
    //            myLine.StrokeThickness = 1;
    //            canvas.Children.Add(myLine);
    //        }

    //    }
    //}



    namespace FernNamespace
    {
        /*
         * this class draws a fractal fern when the constructor is called.
         * Written as sample C# code for a CS 212 assignment -- October 2011.
         * 
         * Bugs: WPF and shape objects are the wrong tool for the task 
         */
        class Fern
        {
            private static int BERRYMIN = 10;
            private static int TENDRILS = 10;
            private static int TENDRILMIN = 8;
            private static double DELTATHETA = 0.01;
            private static double SEGLENGTH = 3.0;

            /* 
             * Fern constructor erases screen and draws a fern
             * 
             * Size: number of 3-pixel segments of tendrils
             * Redux: how much smaller children clusters are compared to parents
             * Turnbias: how likely to turn right vs. left (0=always left, 0.5 = 50/50, 1.0 = always right)
             * canvas: the canvas that the fern will be drawn on
             */
            public Fern(double size, double redux, double turnbias, Canvas canvas)
            {
                canvas.Children.Clear();                                // delete old canvas contents
                                                                        // draw a new fern at the center of the canvas with given parameters
                cluster((int)(canvas.Width / 2), (int)(canvas.Height * .75), 50, redux, turnbias, canvas);
                line((int)(canvas.Width / 2), (int)(canvas.Height * .75), (int)(canvas.Width / 2) + 10, 10, 34, 139,34, 2.0, canvas);
        }

            /*
             * cluster draws a cluster at the given location and then draws a bunch of tendrils out in 
             * regularly-spaced directions out of the cluster.
             */
            private void cluster(int x, int y, double size, double redux, double turnbias, Canvas canvas)
            {
                for (int i = 0; i < TENDRILS; i++)
                {
                    // compute the angle of the outgoing tendril
                    double theta = i * 2* Math.PI / TENDRILS;
                    tendril(x, y, (size*.8), redux, turnbias, theta, canvas);
                    //if (size > BERRYMIN)
                    //   berry(x, y, 5, canvas);
                }
            }

            /*
             * tendril draws a tendril (a randomly-wavy line) in the given direction, for the given length, 
             * and draws a cluster at the other end if the line is big enough.
             */
            private void tendril(int x1, int y1, double size, double redux, double turnbias, double direction, Canvas canvas)
            {
                int x2 = x1, y2 = y1;
                Random random = new Random();
                //direction +=  DELTATHETA;
                direction += (random.NextDouble() > turnbias) ? -1 * DELTATHETA : DELTATHETA;
                x1 = x2; y1 = y2;
                x2 = x1 + (int)(SEGLENGTH * Math.Sin(direction));
                y2 = y1 + (int)(SEGLENGTH * -direction);
               
                line(x1, y1, x2, y2, 34, 139, 34, 1 + (size / 80), canvas);
               
                for (int i = 0; i < size-1; i++)
                {
                    direction += (random.NextDouble() > turnbias) ? -1 * DELTATHETA : DELTATHETA;
                    //direction += (1 > turnbias) ? -1 * DELTATHETA : DELTATHETA;
                    x1 = x2; y1 = y2;
                    x2 = x1 + (int)(SEGLENGTH * Math.Sin(direction));
                    y2 = y1 + (int)(SEGLENGTH * -direction);
                    byte red = (byte)(100 + size / 2);
                    byte green = (byte)(220 - size / 3);
                    //if (size>120) red = 138; green = 108;
                    //if (y2 > (int)(canvas.Height * .75))
                    //if (y2 > y1)
                    //{
                    // y2 = (y2 - y1)+ 140;
                    //}
                    line(x1, y1, x2, y2, red, green, 0, 1 + (size / 80), canvas);
                    line(-x1, y1, -x2, y2, red, green, 0, 1 + (size / 80), canvas); //constructs tendrils on te opposite side
                    }
                    if (size > TENDRILMIN)
                        cluster(x2, y2, size / redux, redux, turnbias, canvas);
                 }

            /*
             * draw a red circle centered at (x,y), radius radius, with a black edge, onto canvas
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
                myEllipse.Width = 2 * radius;
                myEllipse.Height = 2 * radius;
                myEllipse.SetCenter(x, y);
                canvas.Children.Add(myEllipse);
            }

            /*
             * draw a line segment (x1,y1) to (x2,y2) with given color, thickness on canvas
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
     */
    public static class EllipseX
    {
        public static void SetCenter(this Ellipse ellipse, double X, double Y)
        {
            Canvas.SetTop(ellipse, Y - ellipse.Height / 2);
            Canvas.SetLeft(ellipse, X - ellipse.Width / 2);
        }
    }

