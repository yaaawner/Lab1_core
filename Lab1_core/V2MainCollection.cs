using System.Collections.Generic;
using System.Collections;

namespace Lab1_core
{
    class V2MainCollection : IEnumerable<V2Data>
    {
        private List<V2Data> v2Datas;

        public int Count
        {
            get { return v2Datas.Count; }
        }

        public void Add(V2Data item)
        {
            v2Datas.Add(item);
        }

        public bool Remove(string id, double w)
        {
            bool flag = false;
            foreach (V2Data data in v2Datas)
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
            Grid1D Ox = new Grid1D(10, 3);
            Grid1D Oy = new Grid1D(10, 3);
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
            foreach (V2Data data in v2Datas)
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
}
