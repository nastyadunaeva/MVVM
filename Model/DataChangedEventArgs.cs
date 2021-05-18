using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public enum ChangeInfo
    {
        ItemChanged,
        Add,
        Remove,
        Replace
    }
    public delegate void DataChangedEventHandler(object source, DataChangedEventArgs args);
    public class DataChangedEventArgs
    {
        public ChangeInfo change_info { get; set; }
        public string info { get; set; }
        public DataChangedEventArgs(ChangeInfo changeInfo, string constr_info)
        {
            change_info = changeInfo;
            info = constr_info;
        }
        public override string ToString() => change_info.ToString() + "\n" + info;
    }
}
