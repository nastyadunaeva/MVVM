using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;
using System.Collections;
using System.Linq;
using System.ComponentModel;
using System.Collections.Specialized;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

namespace Model
{
    [Serializable]
    public class V3MainCollection : IEnumerable<V3Data>, INotifyCollectionChanged, INotifyPropertyChanged
    {
        private List<V3Data> v3Datas { get; set; }

        [field: NonSerialized]
        public event NotifyCollectionChangedEventHandler CollectionChanged;
        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;
        private bool is_changed;
        public bool Is_changed
        {
            get
            {
                return is_changed;
            }
            set
            {
                is_changed = value;
            }
        }
        private float max_dist;
        public float Max_dist
        {
            get
            {
                return max_dist;
            }
            set
            {
                max_dist = value;
                OnPropertyChanged("max_dist");
            }
        }
        public void change_bool(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.Is_changed = true;
            this.Max_dist = max_from_origin();
        }
        protected virtual void OnCollectionChanged(object source, NotifyCollectionChangedAction action)
        {
            CollectionChanged?.Invoke(source, new NotifyCollectionChangedEventArgs(action));
        }
        protected virtual void OnPropertyChanged(string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
        public V3Data this[int index]
        {
            get
            {
                return v3Datas[index];
            }
            set
            {
                string tmp = "before: " + v3Datas.Count.ToString();
                v3Datas[index] = value;
                tmp += ", after: " + v3Datas.Count.ToString();

                OnDataChanged(this, new DataChangedEventArgs(ChangeInfo.Replace, tmp));
                OnCollectionChanged(this, NotifyCollectionChangedAction.Reset);
            }
        }

        [field: NonSerialized]
        public event DataChangedEventHandler DataChanged;
        public void OnDataChanged(object source, DataChangedEventArgs args)
        {
            DataChanged?.Invoke(source, args);
        }
        public void property_change(object sender, PropertyChangedEventArgs args)
        {
            string tmp = "before: " + v3Datas.Count.ToString() + ", after: " + v3Datas.Count.ToString();
            OnDataChanged(this, new DataChangedEventArgs(ChangeInfo.ItemChanged, tmp));
        }
        public V3MainCollection()
        {
            v3Datas = new List<V3Data>();
            this.CollectionChanged += change_bool;
        }
        public int Count => v3Datas.Count;
        public IEnumerator<V3Data> GetEnumerator()
        {
            return v3Datas.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return v3Datas.GetEnumerator();
        }

        public void Add(V3Data item)
        {
            item.PropertyChanged += property_change;
            string tmp = "before: " + v3Datas.Count.ToString();
            v3Datas.Add(item);

            tmp += ", after: " + v3Datas.Count.ToString();
            OnDataChanged(this, new DataChangedEventArgs(ChangeInfo.Add, tmp));
            OnCollectionChanged(this, NotifyCollectionChangedAction.Reset);

        }
        public void AddRandomDataCollection()
        {
            V3DataCollection datacollection = new V3DataCollection();
            datacollection.InitRandom(3, (float)0.5, (float)0.5, 0.1, 1.0);
            this.Add(datacollection);
        }
        public void AddRandomDataGrid()
        {
            Grid1D x = new Grid1D((float)0.3, 3);
            Grid1D y = new Grid1D((float)0.3, 3);
            V3DataOnGrid dataongrid = new V3DataOnGrid("", DateTime.Now, x, y);
            dataongrid.InitRandom(0.1, 1.0);
            this.Add(dataongrid);
        }
        public bool Remove(string id, DateTime dt)
        {
            string tmp = "before: " + v3Datas.Count.ToString();
            int amount = v3Datas.RemoveAll(x => ((x.information == id) && (x.time == dt)));
            tmp += ", after: " + v3Datas.Count.ToString();
            if (amount == 0)
            {
                OnDataChanged(this, new DataChangedEventArgs(ChangeInfo.Remove, tmp));
                return false;
            }
            else
            {
                OnDataChanged(this, new DataChangedEventArgs(ChangeInfo.Remove, tmp));
                OnCollectionChanged(this, NotifyCollectionChangedAction.Reset);
                return true;
            }
        }
        public void RemoveAt(int index)
        {
            v3Datas.RemoveAt(index);
            OnCollectionChanged(this, NotifyCollectionChangedAction.Reset);
        }
        public void RemoveAll()
        {
            v3Datas.Clear();
            OnCollectionChanged(this, NotifyCollectionChangedAction.Reset);
        }
        public void AddDefaults()
        {
            Grid1D x = new Grid1D((float)0.2, 2);
            Grid1D y = new Grid1D((float)0.2, 2);
            V3DataOnGrid data1 = new V3DataOnGrid("", DateTime.Now, x, y);
            data1.InitRandom(0.25, 0.5);

            Grid1D x1 = new Grid1D((float)0.0, 0);
            Grid1D y1 = new Grid1D((float)0.0, 0);
            V3DataOnGrid data2 = new V3DataOnGrid("", DateTime.Now, x1, y1);

            V3DataCollection data3 = new V3DataCollection();
            data3.InitRandom(4, (float)0.4, (float)0.4, 1.25, 0.2);

            V3DataCollection data4 = new V3DataCollection();
            this.Add(data1);
            this.Add(data2);
            this.Add(data3);
            this.Add(data4);
        }
        public override string ToString()
        {
            string tmp = "";
            foreach (V3Data v3 in v3Datas)
            {
                tmp = tmp + v3.ToString() + " ";
            }
            return tmp;
        }
        public string ToLongString(string format)
        {
            string tmp = "";
            foreach (V3Data v3 in v3Datas)
            {
                tmp = tmp + v3.ToLongString(format) + " ";
                Console.WriteLine();
            }
            return tmp;
        }
        public float RMin(Vector2 v)
        {
            var query = from data in v3Datas
                        from item in data
                        select Vector2.Distance(v, item.Vec);

            return query.Min();

        }
        public float max_from_origin()
        {
            Vector2 v0 = new Vector2(0, 0);
            var query = from data in v3Datas
                        from item in data
                        select Vector2.Distance(v0, item.Vec);
            if (query.Count() != 0)
            {
                return query.Max();
            }
            else
            {
                return 0;
            }
        }
        public DataItem RMinDataItem(Vector2 v)
        {
            var query = from data in v3Datas
                        from item in data
                        where Vector2.Distance(v, item.Vec) == RMin(v)
                        select item;
            return query.First();
        }
        public IEnumerable<Vector2> IEnumerableVectors
        {
            get
            {
                IEnumerable<Vector2> vector_set1 = from data in v3Datas
                                                   from item in data
                                                   where data.GetType().ToString() == "Lab2.V3DataCollection"
                                                   select item.Vec;
                IEnumerable<Vector2> vector_set2 = from data in v3Datas
                                                   from item in data
                                                   where data.GetType().ToString() == "Lab2.V3DataOnGrid"
                                                   select item.Vec;
                var query = from item in vector_set1.Except(vector_set2)
                            select item;
                return query;
            }
        }

        public void Save(string filename)
        {
            FileStream fileStream = null;
            try
            {
                fileStream = File.Create(filename);
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(fileStream, this);
                is_changed = false;
            }
            catch (Exception e)
            {
                Console.WriteLine("Save\n" + e.Message);
            }
            finally
            {
                if (fileStream != null)
                {
                    fileStream.Close();
                }
            }
        }
        public void Load(string filename)
        {
            FileStream fileStream = null;
            V3MainCollection res = null;
            try
            {
                fileStream = File.OpenRead(filename);
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                res = binaryFormatter.Deserialize(fileStream) as V3MainCollection;
                this.v3Datas = res.v3Datas;
                OnCollectionChanged(this, NotifyCollectionChangedAction.Reset);
            }
            finally
            {
                if (fileStream != null)
                {
                    fileStream.Close();
                }
            }

        }
        public V3MainCollection SelectDataCollection()
        {
            V3MainCollection Collection = new V3MainCollection();
            foreach (V3Data item in v3Datas)
            {
                if (item.GetType().ToString() == "Model.V3DataCollection") Collection.Add(item);
            }
            return Collection;
        }
        public V3MainCollection SelectDataOnGrid()
        {
            V3MainCollection Collection = new V3MainCollection();
            foreach (V3Data item in v3Datas)
            {
                if (item.GetType().ToString() == "Model.V3DataOnGrid") Collection.Add(item);
            }
            return Collection;
        }
    }
}
