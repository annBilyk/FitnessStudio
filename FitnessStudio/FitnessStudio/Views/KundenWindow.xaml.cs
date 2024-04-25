using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using FitnessStudio.Models;
using FitnessStudio.ViewModels;

namespace FitnessStudio.Views
{
    /// <summary>
    /// Interaction logic for KundenWindow.xaml
    /// </summary>
    public partial class KundenWindow : Window
    {
        public KundenWindow()
        {
            InitializeComponent();

            try
            {
                CultureInfo culture = new CultureInfo("en-US");
                culture.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
                Thread.CurrentThread.CurrentCulture = culture;

                var data = this.DataContext as KundenWindowViewModel;
                if (data != null)
                {
                    //if (data.KundenContext != null)
                    //{
                    //    // Festlegen des Kartenobjekts
                    //    var guth = data.KundenContext.KundKartGuthaben;
                    //    var karteWert = 0;

                    //    if (data.KundenContext.KundKartNumNavigation != null)
                    //    {
                    //        karteWert = data.KundenContext.KundKartNumNavigation.KarteWert;
                    //    }

                    //    if (guth > 0 && karteWert > 0 && guth != karteWert)
                    //    {
                    //        karteCombo.IsEnabled = false;
                    //    }
                    //}

                    //var cb = karteCombo as ComboBox;
                    //if (cb != null)
                    //{
                    //    cb.SelectedItem = data.KundKartenList.FirstOrDefault();
                    //}

                    // Fokus setzen
                    var gr = kundenGrid as DataGrid;
                    if (gr != null && data.KundList != null && data.KundList.Count > 0)
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

        private void Plz_OnKeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.F3)
                {
                    // Postleitzahlen suchen

                    PostleitzahlenWindow plzWindow = new PostleitzahlenWindow();
                    plzWindow.ShowDialog();

                    var data = this.DataContext as KundenWindowViewModel;
                    if (data != null && data.KundenContext != null && plzWindow.Post != null && plzWindow.Post.PostID > 0)
                    {
                        // Gibt den ausgewählten Wert zurück
                        data.KundenContext.KundPLZNavigation = new Postleitzahlen();
                        data.KundenContext.KundPLZNavigation = plzWindow.Post;
                        data.KundenContext.KundPLZ = plzWindow.Post.PostID;
                        
                        this.DataContext = data;

                        plzTb.Text = data.KundenContext.KundPLZNavigation.PostID.ToString();
                        ortTb.Text  = data.KundenContext.KundPLZNavigation.PostOrt;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "", MessageBoxButton.OK);
            }
        }

        private void Termin_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                // Aufruf des KalenderFenster
                KalenderWindow kalendWindow = new KalenderWindow();
                kalendWindow.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "", MessageBoxButton.OK);
            }
        }

        private void Rechnungen_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                // Aufruf des RechnungenFenster
                var data = this.DataContext as KundenWindowViewModel;
                var kund = data?.KundenContext;

                RechnungenWindow rechnungenWindow = new RechnungenWindow(kund);
                rechnungenWindow.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "", MessageBoxButton.OK);
            }
        }

        private void KundenGrid_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var grid = sender as DataGrid;
                var listForDel = new List<Kunden>();

                if (grid != null)
                {
                    // Aktualisierung der Aufzeichnungen ausgewählter Kunden

                    var listDel = grid.SelectedItems;
                    var curItem = grid.SelectedItem as Kunden;
                    var data = grid.DataContext as KundenWindowViewModel;

                    if (data != null)
                    {
                        if (listDel != null)
                        {
                            foreach (var item in listDel)
                            {
                                var kunde = item as Kunden;
                                if (kunde != null)
                                {
                                    listForDel.Add(kunde);
                                }
                            }

                            if (listForDel != null)
                            {
                                data.DelKundList = listForDel;
                            }
                        }

                        if (curItem != null)
                        {
                            data.KundenContext = curItem;
                        }

                        //var cb = karteCombo as ComboBox;
                        //if (cb != null)
                        //{
                        //    var curKarte =
                        //        data.KundKartenList.FirstOrDefault(k => k.KarteID == data.KundenContext.KundKartNum);

                        //    if (curKarte != null && curKarte.KarteID > 0)
                        //    {
                        //        cb.SelectedItem = data.KundKartenList.FirstOrDefault(k => k.KarteID == data.KundenContext.KundKartNum);
                        //    }
                        //}
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
