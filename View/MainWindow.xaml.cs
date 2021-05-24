using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ViewModel;

namespace View
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MainViewModel model = new MainViewModel(new WPFUIServices());
        public static RoutedCommand AddCommand = new RoutedCommand("Add", typeof(View.MainWindow));
        public MainWindow()
        {
            InitializeComponent();
            DataContext = model;
        }
        
        private void lisBox_DataCollection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            gridAddDataItem.DataContext = model.GetSelectedItem((sender as ListBox).SelectedItem);
        }
        private void lisBox_DataOnGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            model.UpdateInfoOnGrid((sender as ListBox).SelectedItem);
        }
        private void CommandBinding_CanExecute_Add(object sender, CanExecuteRoutedEventArgs e)
        {
            if (model.ItemIsNull())
            {
                e.CanExecute = false;
                return;
            }
            if (gridAddDataItem != null)
            {
                foreach (FrameworkElement child in gridAddDataItem.Children)
                {
                    if (Validation.GetHasError(child) == true)
                    {
                        e.CanExecute = false;
                        return;
                    }
                    e.CanExecute = true;
                }
            }
            else e.CanExecute = false;
        }
        private void CommandBinding_Executed_Add(object sender, ExecutedRoutedEventArgs e) => model.AddItem();
    }
    public class WPFUIServices : IUIServices
    {
        public event EventHandler RequerySuggested
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
        public string SelectFileSave()
        {
            MessageBoxResult result = MessageBox.Show("Изменения будут потеряны. Сохранить изменения?", "", MessageBoxButton.YesNo);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
                    dlg.Filter = "Text documents (.txt)|*.txt";
                    dlg.CreatePrompt = true;
                    dlg.OverwritePrompt = true;
                    if (dlg.ShowDialog() == true)
                    {
                        string filename = dlg.FileName;
                        return filename;
                    }
                    return "No filename";
                case MessageBoxResult.No:
                    return "No filename";
                default:
                    return "No filename";
            }
        }
        public string SelectFile()
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.Filter = "Text documents (.txt)|*.txt";
            try
            {
                if (dlg.ShowDialog() == true)
                {
                    string filename = dlg.FileName;
                    return filename;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return "Error";
            }
            return "Error";
        }
    }
}
