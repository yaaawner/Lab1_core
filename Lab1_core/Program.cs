using System;
using System.Numerics;
using System.Collections.Generic;
using System.Collections;

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
            
            public override string ToString()
            {
                return "Info: " + Info + " Frequency: " + Freq.ToString();
            }

        }

        class V2DataOnGrid : V2Data
        {

            //public Grid1D Ox { get; set; }
            //public Grid1D Oy { get; set; }

            public Grid1D[] Grids { get; set; } /* { get; AppDomainSetup; } */

            /*
            public Grid1D Ox 
            { 
                get { return grids[0];  }
                set { grids[0] = value; } 
            }

            public Grid1D Oy
            {
                get { return grids[1]; }
                set { grids[1] = value; }
            }
            */

            public Complex[,] Node { get; set; }

            public V2DataOnGrid (string info, double freq, Grid1D ox, Grid1D oy) : base(info, freq)
            {
                Info = info;
                Freq = freq;
                Grids = new Grid1D[2] { ox, oy };
            }

            public void initRandom(double minValue, double maxValue)
            {
                Complex[,] Node = new Complex[Grids[0].Num, Grids[1].Num];
                Random rnd = new Random();
                
                for (int i = 0; i < Grids[0].Num; i++)
                {
                    for (int j = 0; j < Grids[1].Num; j++)
                    {
                        Node[i, j] = new Complex(rnd.NextDouble() * (maxValue - minValue), rnd.NextDouble() * (maxValue - minValue));
                    }
                }
            } 

            /*
            public V2DataCollection()
            {

            }
            */

            public override Complex[] NearAverage(float eps)
            {
                int N = Grids[0].Num * Grids[1].Num;
                double sum = 0;
                //Complex[] ret = new Complex[]();
                for (int i = 0; i < Grids[0].Num; i++)
                {
                    for (int j = 0; j < Grids[1].Num; j++)
                    {
                        sum += Node[i, j].Real;
                    }
                }

                double average = sum / N;
                int count = 0;
                for (int i = 0; i < Grids[0].Num; i++)
                {
                    for (int j = 0; j < Grids[1].Num; j++)
                    {
                        if (Math.Abs(Node[i,j].Real - average) < eps) {
                            count++;
                        }
                    }
                }

                Complex[] ret = new Complex[count];
                count = 0;
                for (int i = 0; i < Grids[0].Num; i++)
                {
                    for (int j = 0; j < Grids[1].Num; j++)
                    {
                        if (Math.Abs(Node[i, j].Real - average) < eps)
                        {
                            ret[count++] = Node[i, j];
                        }
                    }
                }

                return ret;
            }

            public override string ToString()
            {
                return "lalal";
            }

            public override string ToLongString()
            {
                return "alalalla";
            }
        }

        class V2DataCollection : V2Data
        {
            public List<DataItem> dataItems { get; set; }

            public V2DataCollection(string info, double freq) : base(info, freq) {
                Info = info;
                Freq = freq;
            }

            public void initRandom(int nItems, float xmax, float ymax, double minValue, double maxValue)
            {
                //DataItem item = new DataItem();
                //Vector2 bufVec;
                //Complex bufCompl;

                dataItems = new List<DataItem>();
                Random rnd = new Random();
                for (int i = 0; i < nItems; i++)
                {
                    //bufVec.X = (float)rnd.NextDouble() * xmax;
                    //bufVec.Y = (float)rnd.NextDouble() * ymax;
                    //item.Vector = new Vector2((float)rnd.NextDouble() * xmax, (float)rnd.NextDouble() * ymax);
                    //item.Complex

                    dataItems.Add(new DataItem()
                    {
                        Vector = new Vector2((float)rnd.NextDouble() * xmax, (float)rnd.NextDouble() * ymax),
                        Complex = new Complex(rnd.NextDouble() * (maxValue - minValue), rnd.NextDouble() * (maxValue - minValue))
                    });
                }
            }

            public override Complex[] NearAverage(float eps)
            {
                //throw new NotImplementedException();
                int count = 0;
                double sum = 0;
                foreach(DataItem item in dataItems)
                {
                    sum += item.Complex.Real;
                }

                double average = sum / dataItems.Count;

                foreach (DataItem item in dataItems)
                {
                    if (Math.Abs(item.Complex.Real - average) < eps)
                    {
                        count++;
                    }
                }

                Complex[] ret = new Complex[count];
                count = 0;
                foreach (DataItem item in dataItems)
                {
                    if (Math.Abs(item.Complex.Real - average) < eps)
                    {
                        ret[count++] = item.Complex;
                    }
                }

                return ret;
            }

            public override string ToString()
            {
                return "lalalal";
            }

            public override string ToLongString()
            {
                return "lalalala";
            }
        }

        class V2MainCollection /* : IEnumerable<V2Data> */ {
            private List<V2Data> v2Datas;

            public int Count 
            {
                get { return v2Datas.Count; }
            }

            public void Add(V2Data item)
            {
                v2Datas.Add(item);
            }

            public bool Remove (string id, double w)
            {
                bool flag = false;
                foreach(V2Data data in v2Datas)
                {
                    if (data.Freq == w && data.Info == id)
                    {
                        v2Datas.Remove(data);
                        flag = true;
                    }
                }

                return flag;
            }

            public void AddDefaults()
            {
                Grid1D Ox = new Grid1D(10, 10);
                Grid1D Oy = new Grid1D(10, 10);
                v2Datas = new List<V2Data>();
                V2DataOnGrid[] grid = new V2DataOnGrid[3];
                V2DataCollection[] collections = new V2DataCollection[3];

                for (int i = 0; i < 3; i++)
                {
                    grid[i] = new V2DataOnGrid("data info" + i.ToString(), i, Ox, Oy);
                    collections[i] = new V2DataCollection("collection info" + i.ToString(), i);

                }

                for (int i = 0; i < 3; i++)
                {
                    grid[i].initRandom(0, 100);
                    collections[i].initRandom(4, 100, 100, 0, 100);
                    v2Datas.Add(grid[i]);
                    v2Datas.Add(collections[i]);
                }
            }

            public override string ToString()
            {
                string ret = "";
                foreach(V2Data data in v2Datas)
                {
                    ret += (data.ToString() + '\n');
                }
                return ret;
            }
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            //V2DataCollection dataCollection = new V2DataCollection("info", 1.23);
            //dataCollection.

            /* 2 */
            V2MainCollection mainCollection = new V2MainCollection();
            mainCollection.AddDefaults();
            Console.WriteLine(mainCollection.ToString());
        }
    }
}
