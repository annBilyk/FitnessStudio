using FitnessStudio.Models;
using FitnessStudio.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace FitnessStudio.Views
{
    /// <summary>
    /// Interaction logic for KarteWindow.xaml
    /// </summary>
    public partial class KarteWindow : Window
    {
        public KarteWindow()
        {
            InitializeComponent();

            try
            {
                var data = this.DataContext as KarteWindowViewModel;
                if (data != null)
                {
                    // Fokus setzen
                    var gr = kartenGrid as DataGrid;
                    if (gr != null && data.KartenList != null && data.KartenList.Count > 0)
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

        private void KartenGrid_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var grid = sender as DataGrid;
                var listForDel = new List<Karte>();

                if (grid != null)
                {
                    // Aktualisierung der Aufzeichnungen ausgewählter Karten

                    var listDel = grid.SelectedItems;
                    var curItem = grid.SelectedItem as Karte;
                    var data = grid.DataContext as KarteWindowViewModel;

                    if (data != null)
                    {
                        if (listDel != null)
                        {
                            foreach (var item in listDel)
                            {
                                var karte = item as Karte;
                                if (karte != null)
                                {
                                    listForDel.Add(karte);
                                }
                            }

                            if (listForDel != null)
                            {
                                data.DelKartList = listForDel;
                            }
                        }

                        if (curItem != null)
                        {
                            data.KarteContext = curItem;
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
