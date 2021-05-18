using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;
using System.Collections;
using System.ComponentModel;

namespace Model
{
    [Serializable]
    public abstract class V3Data : IEnumerable<DataItem>, INotifyPropertyChanged
    {
        public string information;
        public DateTime time { get; set; }

        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(object source, string propertyName)
        {
            PropertyChanged?.Invoke(source, new PropertyChangedEventArgs(propertyName));
        }

        public string Info
        {
            get => information;
            set
            {
                information = value;
                OnPropertyChanged(this, "base");
            }
        }

        public DateTime Time
        {
            get => time;
            set
            {
                time = value;
                OnPropertyChanged(this, "base");
            }
        }

        public V3Data() { }
        public V3Data(string constr_information, DateTime constr_time)
        {
            information = constr_information;
            time = constr_time;
        }
        public abstract Vector2[] Nearest(Vector2 v);
        public abstract string ToLongString();
        public override string ToString()
        {
            return (information + " " + time.ToString());
        }
        public abstract string ToLongString(string format);
        public IEnumerator<DataItem> GetEnumerator()
        {
            V3DataCollection tmp = new V3DataCollection();
            tmp = (V3DataCollection)this;
            return tmp.dataItems.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            V3DataCollection tmp = new V3DataCollection();
            tmp = (V3DataCollection)this;
            return tmp.GetEnumerator();
        }

    }
}
