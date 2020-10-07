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
            public Grid1D[] Grids { get; set; }

            public Complex[,] Node { get; set; }

            public V2DataOnGrid (string info, double freq, Grid1D ox, Grid1D oy) : base(info, freq)
            {
                Info = info;
                Freq = freq;
                Grids = new Grid1D[2] { ox, oy };
            }

            public void initRandom(double minValue, double maxValue)
            {
                Node = new Complex[Grids[0].Num, Grids[1].Num];
                Random rnd = new Random();
                
                for (int i = 0; i < Grids[0].Num; i++)
                {
                    for (int j = 0; j < Grids[1].Num; j++)
                    {
                        Node[i, j] = new Complex(rnd.NextDouble() * (maxValue - minValue), rnd.NextDouble() * (maxValue - minValue));
                    }
                }
            } 

            public static explicit operator V2DataCollection(V2DataOnGrid val)
            {
                V2DataCollection ret = new V2DataCollection(val.Info, val.Freq);

                for (int i = 0; i < val.Grids[0].Num; i++)
                {
                    for (int j = 0; j < val.Grids[1].Num; j++)
                    {
                        ret.dataItems.Add(new DataItem(new Vector2((i + 1) * val.Grids[0].Step, (j + 1) * val.Grids[1].Step), val.Node[i, j]));
                    }
                }

                return ret;
            }

            public override Complex[] NearAverage(float eps)
            {
                int N = Grids[0].Num * Grids[1].Num;
                double sum = 0;

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
                return "Type: 2DataOnGrid Base: Info: " + Info.ToString() + " Freq: " + Freq.ToString() /*ToString()*/
                    + " Ox: " + Grids[0].ToString() + " Oy: " + Grids[1].ToString();
            }

            public override string ToLongString()
            {
                string ret = "";

                for (int i = 0; i < Grids[0].Num; i++)
                {
                    for (int j = 0; j < Grids[1].Num; j++)
                    {
                        ret = ret + ("Coord: ( " + (Grids[0].Step * (i + 1)).ToString() + ", " + (Grids[1].Step * (j + 1)).ToString()
                            + ") Value: " + Node[i, j].ToString());
                    }
                    ret = ret + "\n";
                }

                return "Type: 2DataOnGrid Base: Info: " + Info + " Freq: " + Freq.ToString() /*ToString()*/
                    + " Ox: " + Grids[0].ToString() + " Oy: " + Grids[1].ToString() + "\n" + ret;
            }
        }

        class V2DataCollection : V2Data
        {
            public List<DataItem> dataItems { get; set; }

            public V2DataCollection(string info, double freq) : base(info, freq) {
                dataItems = new List<DataItem>();
                Info = info;
                Freq = freq;
            }

            public void initRandom(int nItems, float xmax, float ymax, double minValue, double maxValue)
            { 
                dataItems = new List<DataItem>();
                Random rnd = new Random();
                for (int i = 0; i < nItems; i++)
                { 
                    dataItems.Add(new DataItem()
                    {
                        Vector = new Vector2((float)rnd.NextDouble() * xmax, (float)rnd.NextDouble() * ymax),
                        Complex = new Complex(rnd.NextDouble() * (maxValue - minValue), rnd.NextDouble() * (maxValue - minValue))
                    });
                }
            }

            public override Complex[] NearAverage(float eps)
            {
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
                return "Type: V2DataCollection Base: info " + Info + " freq " + Freq.ToString()
                        + " Count: " + dataItems.Count.ToString();
            }

            public override string ToLongString()
            {
                string ret = "";

                foreach (DataItem item in dataItems)
                {
                    ret += (item.ToString() + " ");
                }

                return "Type: V2DataCollection Base: info " + Info + " freq " + Freq.ToString()
                        + " Count: " + dataItems.Count.ToString() + "\n" + ret;
            }
        }

        class V2MainCollection : IEnumerable<V2Data> {
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

            public IEnumerator<V2Data> GetEnumerator()
            {
                return ((IEnumerable<V2Data>)v2Datas).GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return ((IEnumerable)v2Datas).GetEnumerator();
            }
        }

        static void Main(string[] args)
        {
            /* 1 */
            Grid1D x = new Grid1D(10, 10);
            Grid1D y = new Grid1D(5, 10);
            V2DataOnGrid test = new V2DataOnGrid("test", 100, x, y);
            test.initRandom(0, 100);
            Console.WriteLine(test.ToLongString());

            Console.WriteLine("\n\n ========== OPERATOR ========== \n\n");

            V2DataCollection collection = (V2DataCollection)test;
            Console.WriteLine(collection.ToLongString());

            /* 2 */
            V2MainCollection mainCollection = new V2MainCollection();
            mainCollection.AddDefaults();
            Console.WriteLine(mainCollection.ToString());

            /* 3 */
            Complex[] c;
            foreach (V2Data item in mainCollection)
            {
                c = item.NearAverage(10);
                for (int i = 0; i < c.Length; i++)
                {
                    Console.WriteLine(c[i].ToString());
                }
            } 
        }
    }
}
