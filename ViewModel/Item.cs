using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Model;
using System.ComponentModel;
using System.Collections.Specialized;

namespace ViewModel
{
    public class Item : IDataErrorInfo, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private float x;
        public float X
        {
            get
            {
                return x;
            }
            set
            {
                x = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("X"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Y"));
            }
        }
        private float y;
        public float Y
        {
            get
            {
                return y;
            }
            set
            {
                y = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Y"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("X"));
            }
        }
        private double field;
        public double Field
        {
            get
            {
                return field;
            }
            set
            {
                field = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Field"));
            }
        }
        public V3DataCollection collection;
        public Item()
        {
            collection = new V3DataCollection();
        }
        public Item(ref V3DataCollection collection)
        {
            this.collection = collection;
        }
        public Item(float x, float y, double field)
        {
            this.X = x;
            this.Y = y;
            this.Field = field;
        }

        public void AddDataItem()
        {
            System.Numerics.Vector2 v2 = new System.Numerics.Vector2((float)X, (float)Y);
            DataItem di = new DataItem(v2, Field);
            collection.Add(di);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("X"));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Y"));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Field"));
        }
        public string Error { get { return "Error data"; } }
        public string this[string property]
        {
            get
            {
                string msg = null;
                switch (property)
                {
                    case "X":
                        if (collection != null)
                        {
                            foreach (DataItem it in collection.dataItems)
                            {
                                if ((it.Vec.X == X) && (it.Vec.Y == Y)) msg = "Point exists";
                            }
                        }

                        break;
                    case "Y":
                        if (collection != null)
                        {
                            foreach (DataItem it in collection.dataItems)
                            {
                                if ((it.Vec.X == X) && (it.Vec.Y == Y)) msg = "Point exists";
                            }
                        }

                        break;
                    case "Field":
                        if (Field <= 0) msg = "Field must be positive";
                        break;
                    default:
                        break;
                }
                return msg;
            }
        }
    }
}
