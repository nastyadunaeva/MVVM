using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    [Serializable]
    public class Grid1D
    {
        public float step { get; set; }
        public int node { get; set; }
        public Grid1D(float first_step, int first_node)
        {
            step = first_step;
            node = first_node;
        }
        public override string ToString()
        {
            string tmp = "step: " + step.ToString() + " nodes: " + node.ToString();
            return tmp;
        }
        public string ToString(string format)
        {
            string tmp = "";
            tmp = tmp + "step: " + step.ToString(format) + " nodes: " + node.ToString("D");
            return tmp;
        }
    }
}
