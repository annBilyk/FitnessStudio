using FitnessStudio.Views;
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

namespace FitnessStudio
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void KundenButton_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                // Aufruf des KundenFenster
                KundenWindow kundenWindow = new KundenWindow();
                kundenWindow.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "", MessageBoxButton.OK);
            }
        }

        private void TrainerenButton_OnClick(object sender, RoutedEventArgs e)
        {
            try
            { 
                // Aufruf des TrainerenFenster
                TrainerenWindow trainerenWindow = new TrainerenWindow();
                trainerenWindow.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "", MessageBoxButton.OK);
            }
        }

        private void KalenderButton_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                // Aufruf des KalenderFenster
                KalenderWindow kalenderWindow = new KalenderWindow();
                kalenderWindow.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "", MessageBoxButton.OK);
            }
        }

        private void RechnungenButton_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                // Aufruf des RechnungenFenster
                RechnungenWindow rechnungenWindow = new RechnungenWindow();
                rechnungenWindow.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "", MessageBoxButton.OK);
            }
        }

        private void KundenkartenButton_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                // Aufruf des KarteFenster
                KarteWindow karteWindow = new KarteWindow();
                karteWindow.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "", MessageBoxButton.OK);
            }
        }
    }
}
