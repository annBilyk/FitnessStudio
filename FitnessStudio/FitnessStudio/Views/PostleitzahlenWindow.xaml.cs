using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using FitnessStudio.Models;
using FitnessStudio.ViewModels;

namespace FitnessStudio.Views
{
    /// <summary>
    /// Interaction logic for PostleitzahlenWindow.xaml
    /// </summary>
    public partial class PostleitzahlenWindow : Window
    {
        public Postleitzahlen Post { get; private set; } = new Postleitzahlen();
        public PostleitzahlenWindow()
        {
            InitializeComponent();

            try
            {
                var data = this.DataContext as PostleitzahlenWindowViewModel;
                if (data != null)
                {
                    // Fokus setzen

                    var gr = postGrid as DataGrid;
                    if (gr != null && data.PostList != null && data.PostList.Count > 0)
                    {
                        gr.SelectedIndex = 0;
                        gr.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "", MessageBoxButton.OK);
            }
        }

        private void PostGrid_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                var grid = sender as DataGrid;

                if (grid != null)
                {
                    // Festlegen des Rückgabewerts

                    var curItem = grid.SelectedItem as Postleitzahlen;

                    if (curItem != null)
                    {
                        Post = curItem;
                    }
                }

                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "", MessageBoxButton.OK);
            }
        }

        private void PostGrid_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var grid = sender as DataGrid;
                var listForDel = new List<Postleitzahlen>();

                if (grid != null)
                {
                    var listDel = grid.SelectedItems;
                    var curItem = grid.SelectedItem as Postleitzahlen;
                    var data = grid.DataContext as PostleitzahlenWindowViewModel;

                    if (data != null)
                    {
                        if (listDel != null)
                        {
                            // Aktualisierung der Aufzeichnungen ausgewählter Postleitzahlen

                            foreach (var item in listDel)
                            {
                                var plz = item as Postleitzahlen;
                                if (plz != null)
                                {
                                    listForDel.Add(plz);
                                }
                            }

                            if (listForDel != null)
                            {
                                data.DelPostList = listForDel;
                            }
                        }

                        if (curItem != null)
                        {
                            data.PostContext = curItem;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "", MessageBoxButton.OK);
            }
        }
    }
}
