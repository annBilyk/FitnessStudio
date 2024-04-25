using FitnessStudio.Models;
using FitnessStudio.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FitnessStudio.Views
{
    /// <summary>
    /// Interaction logic for KalenderWindow.xaml
    /// </summary>
    public partial class KalenderWindow : Window
    {
        public KalenderWindow()
        {
            InitializeComponent();

            try
            {
                CultureInfo culture = new CultureInfo("en-US");
                culture.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
                Thread.CurrentThread.CurrentCulture = culture;

                var data = this.DataContext as KalenderWindowViewModel;
                if (data != null)
                {
                    // Fokus setzen
                    var grKund = kalenderkundGrid as DataGrid;
                    if (grKund != null && data.KundSelectionList != null && data.KundSelectionList.Count > 0)
                    {
                        grKund.SelectedIndex = 0;
                        grKund.Focus();
                    }

                    var grKalend = kalenderGrid as DataGrid;
                    if (grKalend != null && data.KalendList != null && data.KalendList.Count > 0)
                    {
                        grKalend.SelectedIndex = 0;
                        grKalend.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "", MessageBoxButton.OK);
            }
        }

        private void Cmb_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var items = e.AddedItems;
                var data = this.DataContext as KalenderWindowViewModel;

                if (data != null)
                {
                    foreach (var item in items)
                    {
                        // Aktualisieren des Trainerobjekts
                        var trainer = item as Trainer;

                        if (trainer != null && data.KalendContext != null)
                        {
                            data.KalendContext.KalendTrain = trainer;
                            data.KalendContext.KalendTrainID = trainer.TrainID;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "", MessageBoxButton.OK);
            }
        }

        private void KalenderGrid_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var grid = sender as DataGrid;
                var listForDel = new List<SpecifKalend>();

                if (grid != null)
                {
                    // Aktualisierung der Aufzeichnungen ausgewählter Trainings

                    var listDel = grid.SelectedItems;
                    var curItem = grid.SelectedItem as SpecifKalend;
                    var data = grid.DataContext as KalenderWindowViewModel;

                    if (data != null)
                    {
                        if (listDel != null)
                        {
                            foreach (var item in listDel)
                            {
                                var kalend = item as SpecifKalend;
                                if (kalend != null)
                                {
                                    listForDel.Add(kalend);
                                }
                            }

                            if (listForDel != null)
                            {
                                data.DelKalendList = listForDel;
                            }
                        }

                        if (curItem != null)
                        {
                            data.KalendContext = data.KalendList.FirstOrDefault(k => k.KalendID == curItem.SpecifKalendID) ?? new Kalender();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "", MessageBoxButton.OK);
            }
        }

        private void OnPreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            // Handhabung des Fokus im Kalender
            base.OnPreviewMouseUp(e);
            if (Mouse.Captured is CalendarItem)
            {
                Mouse.Capture(null);
            }
        }

        private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var items = e.AddedItems;
                var data = this.DataContext as KalenderWindowViewModel;

                if (data != null)
                {
                    // Aktualisieren die Aktivität
                    foreach (var item in items)
                    {
                        var kalend = item as Kalender;

                        if (kalend != null)
                        {
                            var actCmb = activCmb as ComboBox;
                            if (actCmb != null)
                            {
                                actCmb.SelectedItem = data.KalendAktivNameToSelect;
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
