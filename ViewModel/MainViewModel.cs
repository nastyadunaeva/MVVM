using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Model;

namespace ViewModel
{
    public interface IUIServices
    {
        string SelectFileSave();
        string SelectFile();
    }
    public class MainViewModel : ViewModelBase
    {
        private V3MainCollection coll = new V3MainCollection();
        Item item = null;
        public MainViewModel(IUIServices svc)
        {
            NewCommand = new RelayCommand(
                _ => {
                    if (this.HasUnsavedChanges())
                    {
                        string filename = svc.SelectFileSave();
                        if (filename != "No filename")
                        {
                            this.Save(filename);
                        }
                    }
                    this.RemoveAll();
                }
                );
            AddDefaultsCommand = new RelayCommand(
                _ =>
                {
                    this.AddDefaults();
                }
                );
            AddCollectionCommand = new RelayCommand(
                _ =>
                {
                    this.AddDefaultCollection();
                }
                );
            AddGridCommand = new RelayCommand(
                _ =>
                {
                    this.AddDefaultGrid();
                }
                );
            AddFromFileCommand = new RelayCommand(
                _ =>
                {
                    string filename = svc.SelectFile();
                    if (filename != "Error")
                    {
                        this.AddGridFromFile(filename);
                    }
                }
                );
            OpenCommand = new RelayCommand(
                _ =>
                {
                    string filename1 = svc.SelectFileSave();
                    if (filename1 != "No filename")
                    {
                        this.Save(filename1);
                    }
                    string filename2 = svc.SelectFile();
                    if (filename2 != "Error")
                    {
                        this.Load(filename2);
                    }
                }
                );
            SaveCommand = new RelayCommand(
                _ =>
                {
                    string filename = svc.SelectFile();
                    if (filename != "Error")
                    {
                        this.Save(filename);
                    }
                },
                _ => this.CanExecute()
                );
        }
        public ICommand NewCommand { get; private set; }
        public ICommand AddDefaultsCommand { get; private set; }
        public ICommand AddCollectionCommand { get; private set; }
        public ICommand AddGridCommand { get; private set; }
        public ICommand AddFromFileCommand { get; private set; }
        public ICommand OpenCommand { get; private set; }
        public ICommand SaveCommand { get; private set; }
        public V3MainCollection Coll
        {
            get => coll;
            set
            {
                coll = value;
                RaisePropertyChanged("Coll");
            }
        }
        private V3MainCollection dataCollection = new V3MainCollection();
        public V3MainCollection DataCollection
        {
            get => Coll.SelectDataCollection();
            set
            {
                dataCollection = Coll.SelectDataCollection();
                RaisePropertyChanged("DataCollection");
            }
        }
        private V3MainCollection dataOnGrid = new V3MainCollection();
        public V3MainCollection DataOnGrid
        {
            get => Coll.SelectDataOnGrid();
            set
            {
                dataCollection = Coll.SelectDataOnGrid();
                RaisePropertyChanged("DataOnGrid");
            }
        }
        private string infoOnGrid;
        public string InfoOnGrid
        {
            get => infoOnGrid;
            set
            {
                infoOnGrid = value;
                RaisePropertyChanged("InfoOnGrid");
            }
        }
        private string max_dist;
        public float Max_dist
        {
            get => Coll.Max_dist;
            
        }

        override public string ToString()
        {
            return Coll.ToString();
        }
        public void PropChanged()
        {
            RaisePropertyChanged("Coll");
            RaisePropertyChanged("DataCollection");
            RaisePropertyChanged("DataOnGrid");
            RaisePropertyChanged("InfoOnGrid");
            RaisePropertyChanged("Max_dist");
        }
        public bool HasUnsavedChanges()
        {
            if ((Coll != null) && (Coll.Is_changed == true))
            {
                return true;
            } else
            {
                return false;
            }
        }
        public void Save(string filename)
        {
            if (Coll != null)
            {
                Coll.Save(filename);
            }
        }
        public void RemoveAll()
        {
            if (Coll != null)
            {
                Coll.RemoveAll();
                PropChanged();
            }
        }
        public void AddDefaults()
        {
            if (Coll != null)
            {
                Coll.AddDefaults();
                PropChanged();
            }
        }
        public void AddDefaultCollection()
        {
            if (Coll != null)
            {
                Coll.AddRandomDataCollection();
                PropChanged();
            }
        }
        public void AddDefaultGrid()
        {
            if (coll != null)
            {
                coll.AddRandomDataGrid();
                PropChanged();
            }
        }
        public void AddGridFromFile(string filename)
        {
            V3DataOnGrid dataongrid = new V3DataOnGrid(filename);
            if (Coll != null)
            {
                Coll.Add(dataongrid);
                PropChanged();
            }
        }
        public Item GetSelectedItem(Object obj)
        {
            V3DataCollection si = (obj as V3DataCollection);
            item = new Item(ref si);
            return item;
        }
        public void Load(string filename)
        {
            if (Coll != null)
            {
                Coll.Load(filename);
                Coll.Is_changed = false;
                PropChanged();
            }
        }
        public bool CanExecute()
        {
            if (Coll != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public void RemoveAt(int index)
        {
            if (Coll != null)
            {
                Coll.RemoveAt(index);
                PropChanged();
            }
        }
        public bool ItemIsNull()
        {
            if (item == null) return true;
            else return false;
        }
        public void AddItem()
        {
            item.AddDataItem();
            Coll.Is_changed = true;
            PropChanged();
        }
        public void UpdateInfoOnGrid(Object obj)
        {
            V3DataOnGrid element = obj as V3DataOnGrid;
            if (element != null)
            {
                InfoOnGrid = element.ToShortString();
            }
            
        }
    }
    
}
