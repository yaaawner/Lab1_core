using System;

namespace Lab1_core
{
    class Program
    {
        struct DataItem
        {
            //public int x;
            //public int y;
            System.Numerics.Vector2 Vec { get; set; }
        }

        struct Grid1D { }

        class V2Data { }

        class V2DataOnGrid : V2Data { }

        class V2DataCollection : V2Data { }

        class V2MainCollection { }
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }
}
