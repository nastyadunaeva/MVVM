﻿using System;
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
        MainViewModel model = new MainViewModel();
        public static RoutedCommand AddCommand = new RoutedCommand("Add", typeof(View.MainWindow));
        public MainWindow()
        {
            InitializeComponent();
            DataContext = model;
        }
        public void SaveChanges()
        {
            if (model.HasUnsavedChanges())
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
                            model.Save(filename);
                        }
                        break;
                    case MessageBoxResult.No:
                        break;
                }
            }
        }
        private void Button_New_Click(object sender, RoutedEventArgs e)
        {
            SaveChanges();
            model.RemoveAll();
        }
        private void Window_Closed(object sender, EventArgs e)
        {
            SaveChanges();
        }
        private void Button_AddDefaults_Click(object sender, RoutedEventArgs e)
        {
            model.AddDefaults();
        }
        private void Button_AddDefaultCollection_Click(object sender, RoutedEventArgs e)
        {
            model.AddDefaultCollection();
        }
        private void Button_AddDefaultGrid_Click(object sender, RoutedEventArgs e)
        {
            model.AddDefaultGrid();
        }
        private void Button_AddFromFile_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.Filter = "Text documents (.txt)|*.txt";
            try
            {
                if (dlg.ShowDialog() == true)
                {
                    string filename = dlg.FileName;
                    model.AddGridFromFile(filename);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void lisBox_DataCollection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            gridAddDataItem.DataContext = model.GetSelectedItem((sender as ListBox).SelectedItem);
        }
        private void lisBox_DataOnGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            model.UpdateInfoOnGrid((sender as ListBox).SelectedItem);
        }
        private void CommandBinding_Executed_Open(object sender, ExecutedRoutedEventArgs e)
        {
            SaveChanges();
            try
            {
                Microsoft.Win32.OpenFileDialog dlg1 = new Microsoft.Win32.OpenFileDialog();
                dlg1.Filter = "Text documents (.txt)|*.txt";
                if (dlg1.ShowDialog() == true)
                {
                    string filename = dlg1.FileName;
                    model.Load(filename);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Некорректный файл");
            }
        }
        private void CommandBinding_CanExecute_Save(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = model.CanExecute();
        }
        private void CommandBinding_Executed_Save(object sender, ExecutedRoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.Filter = "Text documents (.txt)|*.txt";
            dlg.CreatePrompt = true;
            dlg.OverwritePrompt = true;
            if (dlg.ShowDialog() == true)
            {
                string filename = dlg.FileName;
                model.Save(filename);
            }
        }
        private void CommandBinding_CanExecute_Delete(object sender, CanExecuteRoutedEventArgs e)
        {
            if ((lisBox_Main != null) && (lisBox_Main.Items.Contains(lisBox_Main.SelectedItem)))
                e.CanExecute = true;
            else
                e.CanExecute = false;
        }
        private void CommandBinding_Executed_Delete(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                model.RemoveAt(lisBox_Main.SelectedIndex);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Сначала выберите элемент");
            }
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
        private void CommandBinding_Executed_Add(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                model.AddItem();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

    }
}