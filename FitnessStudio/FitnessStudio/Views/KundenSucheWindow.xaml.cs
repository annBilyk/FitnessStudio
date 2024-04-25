using FitnessStudio.Models;
using FitnessStudio.ViewModels;
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
using System.Windows.Shapes;

namespace FitnessStudio.Views
{
    /// <summary>
    /// Interaction logic for KundenSucheWindow.xaml
    /// </summary>
    public partial class KundenSucheWindow : Window
    {
        public Kunden KundResult { get; private set; } = new Kunden();
        public List<Kunden> KundenList = new List<Kunden>();

        public KundenSucheWindow(List<Kunden> kundList)
        {
            InitializeComponent();

            KundenList = kundList;

            try
            {
                var gr = kundSucheGrid as DataGrid;

                if (gr != null)
                {
                    // Fokus setzen

                    gr.DataContext = this;
                    gr.ItemsSource = KundenList;

                    if (KundenList != null && KundenList.Count > 0)
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

        private void KundSucheGrid_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                var grid = sender as DataGrid;

                if (grid != null)
                {
                    // Gibt den ausgewählten Wert zurück
                    var curItem = grid.SelectedItem as Kunden;

                    if (curItem != null)
                    {
                        KundResult = curItem;
                    }
                }

                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "", MessageBoxButton.OK);
            }
        }
    }
}
