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
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FitnessStudio.Views
{
    /// <summary>
    /// Interaction logic for TrainerenWindow.xaml
    /// </summary>
    public partial class TrainerenWindow : Window
    {
        public TrainerenWindow()
        {
            InitializeComponent();

            try
            {
                CultureInfo culture = new CultureInfo("en-US");
                culture.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
                Thread.CurrentThread.CurrentCulture = culture;

                var data = this.DataContext as TrainerWindowViewModel;
                if (data != null && data.TrainerContext != null)
                {
                    // Fokus setzen

                    var gr = trainerenGrid as DataGrid;
                    if (gr != null)
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

                    var data = this.DataContext as TrainerWindowViewModel;
                    if (data != null && data.TrainerContext != null && plzWindow.Post != null && plzWindow.Post.PostID > 0)
                    {
                        // Gibt den ausgewählten Wert zurück
                        data.TrainerContext.TrainPLZNavigation = new Postleitzahlen();
                        data.TrainerContext.TrainPLZNavigation = plzWindow.Post;
                        data.TrainerContext.TrainPLZ = plzWindow.Post.PostID;

                        this.DataContext = data;

                        plzTb.Text = data.TrainerContext.TrainPLZNavigation.PostID.ToString();
                        ortTb.Text = data.TrainerContext.TrainPLZNavigation.PostOrt;
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

        private void TrainerenGrid_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var grid = sender as DataGrid;
                var listForDel = new List<Trainer>();

                if (grid != null)
                {
                    // Aktualisierung der Aufzeichnungen ausgewählter Kunden 

                    var listDel = grid.SelectedItems;
                    var curItem = grid.SelectedItem as Trainer;
                    var data = grid.DataContext as TrainerWindowViewModel;

                    if (data != null)
                    {
                        if (listDel != null)
                        {
                            foreach (var item in listDel)
                            {
                                var trainer = item as Trainer;
                                if (trainer != null)
                                {
                                    listForDel.Add(trainer);
                                }
                            }

                            if (listForDel != null)
                            {
                                data.DelTrainList = listForDel;
                            }
                        }

                        if (curItem != null)
                        {
                            data.TrainerContext = curItem;
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
