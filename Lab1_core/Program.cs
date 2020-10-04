using System;
using System.Numerics;

namespace Lab1_core
{
    class Program
    {
        struct DataItem
        {
            public Vector2 Vector { get; set; }
            public Complex Complex { get; set; }

            public DataItem (Vector2 vector, Complex complex)
            {
                Vector = vector;
                Complex = complex;
            }

            public override string ToString()
            {
                return "Vector: " + Vector.X.ToString() + " " + Vector.Y.ToString() + "\n" +
                       "Complex: " + Complex.Real.ToString() + " + " + Complex.Imaginary.ToString() + "i\n";
            }
        }

        struct Grid1D
        {
            public float Step { get; set; }
            public int Num { get; set; }

            public Grid1D(float step, int num)
            {
                Step = step;
                Num = num;
            }

            public override string ToString()
            {
                return "Step: " + Step.ToString() + "; Num: " + Num.ToString();
            }
        }

        abstract class V2Data
        {
            public string Info { get; set; }
            public double Freq { get; set; }

            public V2Data (string info, double freq)
            {
                Info = info;
                Freq = freq;
            }

            public abstract Complex[] NearAverage(float eps);
            public abstract string ToLongString();
            
            //ToString

        }

        class V2DataOnGrid : V2Data
        {
            public Grid1D[] Ox { get; set; }
            public Grid1D[] Oy { get; set; }
            public Complex[,] Node { get; set; }

            public V2DataOnGrid (Grid1D[] ox, Grid1D[] oy, string info, double freq) : base(info, freq)
            {
                Info = info;
                Freq = freq;
                Ox = new Grid1D[ox.Length];
                Ox = (Grid1D[])ox.Clone();              //!!!

                Oy = new Grid1D[oy.Length];
                Oy = (Grid1D[])oy.Clone();              //!!!
            }

            public void initRandom(double minValue, double maxValue)
            {
                Node = new Complex[Ox.Length, Oy.Length];
                Random rnd = new Random();
                
                for (int i = 0; i < Ox.Length; i++)
                {
                    for (int j = 0; j < Oy.Length; j++)
                    {
                        Node[i, j] = rnd.NextDouble() * (maxValue - minValue);
                    }
                }
            } 

            //V2DataCollection
        }

        class V2DataCollection : V2Data { }

        class V2MainCollection { }
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }
}
