using FitnessStudio.Models;
using FitnessStudio.Styles;
using FitnessStudio.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace FitnessStudio.Views
{
    /// <summary>
    /// Interaction logic for RechnungenWindow.xaml
    /// </summary>
    public partial class RechnungenWindow : Window
    {
        private CultureInfo culture = new CultureInfo("en-US");
        public RechnungenWindow(Kunden? kund = null)
        {
            InitializeComponent();

            try
            {
                culture.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
                Thread.CurrentThread.CurrentCulture = culture;

                var data = this.DataContext as RechnungWindowViewModel;
                if (data != null && data.RechContext != null)
                {
                    // Fokus setzen

                    var gr = rechnungGrid as DataGrid;
                    if (gr != null)
                    {
                        gr.SelectedIndex = 0;
                        gr.Focus();
                    }

                    // Festlegen des Kartenobjekts
                    var cb = karteCombo as ComboBox;
                    if (cb != null)
                    {
                        if (data.RechContext.RechKund != null)
                        {
                            cb.SelectedItem = data.RechKartenList.FirstOrDefault(k => k.KarteID == data.RechContext.RechKund.KundKartNum);
                        }
                        else
                        {
                            cb.SelectedItem = data.RechKartenList.FirstOrDefault();
                        }
                    }

                    if (data.RechContext.RechSumme != null && data.RechContext.RechSumme > 0)
                    {
                        summeTb.Text = data.RechContext.RechSumme.ToString("N2");
                    }

                    // Überprüfung des Kundezustands
                    if (kund != null && kund.KundID > 0)
                    {
                        data.RechnungList = data.RechnungList.Where(r => r.RechKundID == kund.KundID).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "", MessageBoxButton.OK);
            }
        }

        private void RechnnungGrid_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Aktualisierung Daten
            var grid = sender as DataGrid;
            UpdateFields(grid);
        }

        private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var items = e.AddedItems;
                var data = this.DataContext as RechnungWindowViewModel;

                if (data != null)
                {
                    foreach (var item in items)
                    {
                        // Aktualisierung RechSumme

                        var karte = item as Karte;

                        if (karte != null)
                        {
                            var tb = summeTb as TextBox;

                            if (tb != null)
                            {
                                summeTb.Text = karte.KartePrice.ToString("N2");
                            }
                          
                            data.RechContext.RechSumme = (double)karte.KartePrice;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "", MessageBoxButton.OK);
            }
        }

        private void Kund_OnKeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.F3)
                {
                    var listKund = new List<Kunden>();
                    var data = this.DataContext as RechnungWindowViewModel;

                    var id = 0;
                    var name = "";
                    var nachName = "";

                    var idTb = kundIdTb as TextBox;
                    if (idTb != null)
                    {
                        idTb.RaiseEvent(new RoutedEventArgs(LostFocusEvent, idTb));
                        id = int.Parse(idTb.Text);
                    }

                    var nameTb = kundNameTb as TextBox;
                    if (nameTb != null)
                    {
                        nameTb.RaiseEvent(new RoutedEventArgs(LostFocusEvent, nameTb));
                        name = nameTb.Text.Trim().ToUpper();
                    }

                    var nachNameTb = kundNachNameTb as TextBox;
                    if (nachNameTb != null)
                    {
                        nachNameTb.RaiseEvent(new RoutedEventArgs(LostFocusEvent, nachNameTb));
                        nachName = nachNameTb.Text.Trim().ToUpper();
                    }

                    if (data != null && data.RechKundList != null)
                    {
                        // Filtern der Liste nach Kunde

                        listKund = data.RechKundList;

                        if (data.RechContext != null)
                        {
                            if (id > 0) listKund = listKund.Where(k => k.KundID == id).ToList();
                            if (name.Length > 0) listKund = listKund.Where(k => k.KundVorname.Trim().ToUpper().StartsWith(name)).ToList();
                            if (nachName.Length > 0) listKund = listKund.Where(k => k.KundNachname.Trim().ToUpper().StartsWith(nachName)).ToList();
                        }
                    }

                    // Kunden suchen

                    KundenSucheWindow kundSuchWindow = new KundenSucheWindow(listKund);
                    kundSuchWindow.ShowDialog();

                    if (data != null && data.RechContext != null && kundSuchWindow.KundResult != null && kundSuchWindow.KundResult.KundID > 0)
                    {
                        data.RechContext.RechKund = kundSuchWindow.KundResult;

                        if (data.RechContext.RechKund != null && data.RechContext.RechKund.KundID > 0)
                        {
                            // Gibt den ausgewählten Wert zurück
                            kundIdTb.Text = data.RechContext.RechKund.KundID.ToString();
                            kundNameTb.Text = data.RechContext.RechKund.KundVorname;
                            kundNachNameTb.Text = data.RechContext.RechKund.KundNachname;

                            var cb = karteCombo as ComboBox;
                            if (cb != null)
                            {
                                cb.SelectedItem = data.RechContext.RechKund.KundKartNumNavigation;

                                if (data.RechContext.RechKund.KundKartNumNavigation != null)
                                {
                                    data.RechContext.RechSumme = data.RechContext.RechKund.KundKartNumNavigation.KartePrice;
                                }
                            }

                            data.RechContext.RechKundID = kundSuchWindow.KundResult.KundID;
                        }

                        this.DataContext = data;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "", MessageBoxButton.OK);
            }
        }

        private void RechBezahlt_OnChecked(object sender, RoutedEventArgs e)
        {
            try
            {
                var cB = sender as CheckBox;
                var group = groupGrid as Grid;

                if (cB != null && group != null)
                {
                    // Änderung des Zahlungsstatus

                    foreach (Control c in groupGrid.Children)
                    {
                        if (c.GetType() == typeof(CustomizedTextBox))
                        {
                            CustomizedTextBox txt = (CustomizedTextBox)c;
                            txt.IsEnabled = false;
                        }

                        if (c.GetType() == typeof(CustomizedComboBox))
                        {
                            CustomizedComboBox combo = (CustomizedComboBox)c;
                            combo.IsEnabled = false;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "", MessageBoxButton.OK);
            }
        }

        private void RechBezahlt_OnUnchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                // Änderung des Zahlungsstatus

                var cB = sender as CheckBox;
                var group = groupGrid as Grid;

                if (cB != null && group != null)
                {
                    foreach (Control c in groupGrid.Children)
                    {
                        if (c.GetType() == typeof(CustomizedTextBox))
                        {
                            CustomizedTextBox txt = (CustomizedTextBox)c;
                            txt.IsEnabled = true;
                        }

                        if (c.GetType() == typeof(CustomizedComboBox))
                        {
                            CustomizedComboBox combo = (CustomizedComboBox)c;
                            combo.IsEnabled = true;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "", MessageBoxButton.OK);
            }
        }

        private void RechnungGrid_OnGotFocus(object sender, RoutedEventArgs e)
        {
            // Aktualisierung Tabelle

            var grid = sender as DataGrid;
            UpdateFields(grid);
        }

        private void UpdateFields(DataGrid grid)
        {
            try
            {
                var listForDel = new List<Rechnung>();

                if (grid != null)
                {
                    // Aktualisierung der Aufzeichnungen ausgewählter Rechnung

                    var listDel = grid.SelectedItems;
                    var curItem = grid.SelectedItem as Rechnung;
                    var data = grid.DataContext as RechnungWindowViewModel;

                    if (data != null)
                    {
                        if (listDel != null)
                        {
                            foreach (var item in listDel)
                            {
                                var rech = item as Rechnung;
                                if (rech != null)
                                {
                                    listForDel.Add(rech);
                                }
                            }

                            if (listForDel != null)
                            {
                                data.DelRechList = listForDel;
                            }
                        }

                        if (curItem != null)
                        {
                            data.RechContext = curItem;
                        }

                        var cb = karteCombo as ComboBox;
                        if (cb != null)
                        {
                            var curKarte =
                                data.RechKartenList.FirstOrDefault(k => k.KarteID == data.RechContext.RechKund?.KundKartNum);

                            if (curKarte != null && curKarte.KarteID > 0)
                            {
                                cb.SelectedItem = data.RechKartenList.FirstOrDefault(k => k.KarteID == data.RechContext.RechKund?.KundKartNum);
                            }
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
