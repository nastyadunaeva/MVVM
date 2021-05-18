using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Numerics;

namespace Model
{
    [Serializable]
    public class DataItem : ISerializable
    {
        public double field { get; set; }
        public System.Numerics.Vector2 Vec { get; set; }
        public DataItem(System.Numerics.Vector2 vector2, double f)
        {
            Vec = vector2;
            field = f;
        }
        public DataItem(SerializationInfo info, StreamingContext context)
        {
            float x = info.GetSingle("Vec_X");
            float y = info.GetSingle("Vec_Y");
            Vec = new System.Numerics.Vector2(x, y);
            field = (double)info.GetValue("field", typeof(System.Double));
        }
        public override string ToString()
        {
            string tmp = field.ToString() + " " + Vec.ToString();
            return tmp;
        }
        public string ToString(string format)
        {
            string tmp = "";
            tmp = tmp + Vec.ToString(format) + " " + field.ToString(format);
            return tmp;
        }
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Vec_X", Vec.X);
            info.AddValue("Vec_Y", Vec.Y);
            info.AddValue("field", field);
        }

    }
}
