using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;
using System.Collections;
using System.Collections.Specialized;

namespace Model
{
    [Serializable]
    public class V3DataCollection : V3Data, IEnumerable<DataItem>, INotifyCollectionChanged
    {
        public event NotifyCollectionChangedEventHandler CollectionChanged;
        public List<DataItem> dataItems { get; set; }
        public V3DataCollection()
        {
            dataItems = new List<DataItem>();
        }
        public V3DataCollection(string constr_information, DateTime constr_time) : base(constr_information, constr_time)
        {
            dataItems = new List<DataItem>();
        }
        public void Add(DataItem di)
        {
            dataItems.Add(di);
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }
        public void InitRandom(int nitems, float xmax, float ymax, double minValue, double maxValue)
        {
            for (int i = 0; i < nitems - 1; i++)
            {
                Random rnd = new Random(i);
                Vector2 vec2 = new Vector2((float)rnd.NextDouble() * xmax, (float)rnd.NextDouble() * ymax);
                dataItems.Add(new DataItem(vec2, minValue + rnd.NextDouble() * (maxValue - minValue)));
            }
            Vector2 v22 = new Vector2((float)0.2, (float)0.2);
            dataItems.Add(new DataItem(v22, 0.55));
        }
        public override Vector2[] Nearest(Vector2 v)
        {
            List<Vector2> ans = new List<Vector2>();
            double minDist = float.MaxValue;
            foreach (DataItem item in dataItems)
            {
                double cur_dist = Vector2.Distance(item.Vec, v);
                if (cur_dist < minDist)
                {
                    ans.Clear();
                    minDist = cur_dist;
                    ans.Add(item.Vec);
                }
                else if (cur_dist == minDist)
                {
                    ans.Add(item.Vec);
                }
            }
            return ans.ToArray();
        }
        public override string ToString()
        {
            return "V3DataCollection" + " " + base.ToString() + " number of items = " + dataItems.Count.ToString() + "\n";
        }
        public override string ToLongString()
        {
            string tmp = "";
            tmp = this.ToString() + "\n";
            foreach (DataItem el in dataItems)
            {
                tmp += el.ToString() + "\n";
            }
            return tmp;
        }
        public override string ToLongString(string format)
        {
            string tmp = "";
            tmp = "V3DataCollection" + " " + base.ToString() + " number of items = " + dataItems.Count.ToString() + "\n";
            foreach (DataItem el in dataItems)
            {
                tmp += el.ToString(format) + "\n";
            }
            return tmp;
        }
        public IEnumerator<DataItem> GetEnumerator()
        {
            return dataItems.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return dataItems.GetEnumerator();
        }
    }
}
