using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;
using System.Collections;
using System.IO;
using System.Threading.Tasks;
using System.Globalization;

namespace Model
{
    [Serializable]
    public class V3DataOnGrid : V3Data, IEnumerable<DataItem>
    {
        public Grid1D x { get; set; }
        public Grid1D y { get; set; }
        public double[,] value { get; set; }
        public Grid1D[] comp
        {
            get
            {
                return new Grid1D[] { x, y };
            }
            set
            {
                comp = value;
            }
        }
        public V3DataOnGrid() { }
        public V3DataOnGrid(string filename) // в файле все далее перечисленное на отдельной строке: сначала информация (типа string, для базового класса), время (типа string в формате "MM/dd/yyyy hh:mm", для базового класса), шаг по сетке для оси х (тип float), количество узлов по оси х (тип int), шаг по сетке для оси у (тип float), количество узлов на сетке (тип int), далее на каждой строке значения поля (тип float) 
        {
            try
            {
                using (StreamReader sr = new StreamReader(filename))
                {
                    string constr_information, constr_time_str, x0_step_str, y0_step_str, x0_node_str, y0_node_str, field;
                    constr_information = sr.ReadLine();
                    constr_time_str = sr.ReadLine();
                    x0_step_str = sr.ReadLine();
                    x0_node_str = sr.ReadLine();
                    y0_step_str = sr.ReadLine();
                    y0_node_str = sr.ReadLine();
                    CultureInfo provider = new CultureInfo("en-US"); // сделала явными региональные настройки
                    //CultureInfo provider = CultureInfo.InvariantCulture;
                    DateTime constr_time = DateTime.ParseExact(constr_time_str, "MM/dd/yyyy hh:mm", provider);
                    float x0_step = float.Parse(x0_step_str, provider);
                    int x0_node = int.Parse(x0_node_str, provider);
                    float y0_step = float.Parse(y0_step_str, provider);
                    int y0_node = int.Parse(y0_node_str, provider);
                    Grid1D x0 = new Grid1D(x0_step, x0_node);
                    Grid1D y0 = new Grid1D(y0_step, y0_node);
                    base.information = constr_information;
                    base.time = constr_time;
                    x = x0;
                    y = y0;
                    value = new double[x.node, y.node];
                    for (int i = 0; i < x.node; i++)
                    {
                        for (int j = 0; j < y.node; j++)
                        {
                            field = sr.ReadLine();
                            value[i, j] = float.Parse(field, provider);

                        }
                    }
                }

            }
            catch (ArgumentException)
            {
                Console.WriteLine("Filename is empty string");
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("File is not found");
            }
            catch (DirectoryNotFoundException)
            {
                Console.WriteLine("Directory is not found");
            }
            catch (IOException)
            {
                Console.WriteLine("Unacceptable filename");
            }
            catch (FormatException)
            {
                Console.WriteLine("String could not be parsed");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        public V3DataOnGrid(string constr_information, DateTime constr_time, Grid1D x0, Grid1D y0) : base(constr_information, constr_time)
        {
            x = x0;
            y = y0;
            value = new double[x.node, y.node];
        }
        public void InitRandom(double minValue, double maxValue)
        {
            Random rnd = new Random(1);
            for (int i = 0; i < x.node; i++)
            {
                for (int j = 0; j < y.node; j++)
                {
                    if ((i == x.node - 1) && (j == y.node - 1))
                    {
                        Vector2 v22 = new Vector2((float)0.2, (float)0.2);
                        value[i, j] = 0.55;
                    }
                    else
                    {
                        value[i, j] = minValue + rnd.NextDouble() * (maxValue - minValue);
                        Vector2 v2 = new Vector2(i * x.step, j * y.step);
                        double fl = value[i, j];
                    }

                }
            }

        }
        public static explicit operator V3DataCollection(V3DataOnGrid v3DtOnGrid)
        {
            V3DataCollection ret = new V3DataCollection(v3DtOnGrid.information, v3DtOnGrid.time);
            List<DataItem> ins = new List<DataItem>();
            for (int i = 0; i < v3DtOnGrid.x.node; i++)
            {
                for (int j = 0; j < v3DtOnGrid.y.node; j++)
                {
                    Vector2 v2 = new Vector2(i * v3DtOnGrid.x.step, j * v3DtOnGrid.y.step);
                    double field = v3DtOnGrid.value[i, j];
                    DataItem di = new DataItem(v2, field);
                    ins.Add(di);
                }
            }
            ret.dataItems = ins;
            return ret;
        }
        public override Vector2[] Nearest(Vector2 dot)
        {
            List<Vector2> ans = new List<Vector2>();
            double minDist = 0.0;
            for (int i = 0; i < x.node; i++)
            {
                for (int j = 0; j < y.node; j++)
                {
                    Vector2 cur = new Vector2(i * x.step, j * y.step);
                    double cur_dist = Vector2.Distance(cur, dot);
                    if ((i == 0) && (j == 0))
                    {
                        minDist = cur_dist;
                        ans.Add(cur);
                    }
                    else
                    {
                        if (cur_dist < minDist)
                        {
                            ans.Clear();
                            minDist = cur_dist;
                            ans.Add(cur);
                        }
                        else if (cur_dist == minDist)
                        {
                            ans.Add(cur);
                        }
                    }
                }
            }
            return ans.ToArray();
        }
        public override string ToString()
        {
            return "V3DataOnGrid " + base.ToString() + " x axis " + x.ToString() + ", y axis " + y.ToString() + "\n";
        }
        public override string ToLongString()
        {
            string tmp = "";
            tmp = tmp + this.ToString() + '\n';
            for (int i = 0; i < x.node; i++)
            {
                for (int j = 0; j < y.node; j++)
                {
                    tmp = tmp + "x = " + (i * x.step).ToString() + " y = " + (j * y.step).ToString() + " value = " + value[i, j].ToString() + "\n";
                }
            }
            return tmp;
        }
        public override string ToLongString(string format)
        {
            string tmp = "";
            tmp = tmp + "V3DataOnGrid " + base.ToString() + " x axis " + x.ToString(format) + ", y axis " + y.ToString(format) + '\n';
            for (int i = 0; i < x.node; i++)
            {
                for (int j = 0; j < y.node; j++)
                {
                    tmp = tmp + "x = " + (i * x.step).ToString(format) + " y = " + (j * y.step).ToString(format) + " value = " + value[i, j].ToString(format) + "\n";
                }
            }
            return tmp;
        }
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
        public string ToShortString()
        {
            return "x axis: " + x.node + ", y axis: " + y.node;
        }
    }
    
}
