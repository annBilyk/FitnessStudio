using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FitnessStudio.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace FitnessStudio.ViewModels
{
    public partial class KarteWindowViewModel : ObservableObject
    {
        // Properties
        [ObservableProperty]
        private int karteID;

        [ObservableProperty]
        private string karteName;

        [ObservableProperty]
        private double kartePrice;

        [ObservableProperty]
        private int karteWert;

        [ObservableProperty]
        private Karte karteContext;

        [ObservableProperty]
        private List<Karte> kartenList;

        [ObservableProperty]
        private List<Karte> delKartList;

        [RelayCommand]
        private void New()
        {
            try
            {
                // Legen die Werte fest für neue Objekt
                KarteContext = new Karte();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "", MessageBoxButton.OK);
            }
        }

        [RelayCommand]
        private void Del(object parameter)
        {
            try
            {
                // Überprüfung der Liste
                if (DelKartList != null && DelKartList.Count > 0)
                {
                    using (FitnessStudioDBContext context = new())
                    {
                        foreach (var karte in DelKartList)
                        {
                            if (karte.KarteID > 0)
                            {
                                // Ein Objekt in Datenbank finden
                                Karte? karteObj = context.Karte.Find(karte.KarteID);
                                if (karteObj != null)
                                {
                                    // Ein Objekt löschen
                                    context.Karte.Remove(karteObj);
                                    context.SaveChanges();
                                }
                            }
                        }
                    }
                }

                Close(parameter);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "", MessageBoxButton.OK);
            }
        }

        [RelayCommand]
        private void Save(object parameter)
        {
            try
            {  // Überprüfung das Objekt
                if (KarteContext != null)
                {
                    using (FitnessStudioDBContext context = new())
                    {
                        // Ein Objekt in Datenbank finden
                        Karte? karteObj = context.Karte.Find(KarteContext.KarteID);
                        if (karteObj != null)
                        {
                            //Aktualisieren der Felder mit aktuellen Daten
                            karteObj.KarteName = KarteContext.KarteName;
                            karteObj.KartePrice = KarteContext.KartePrice;
                            karteObj.KarteWert = KarteContext.KarteWert;

                            // Speichern das geänderte Objekt
                            context.Karte.Update(karteObj);
                            context.SaveChanges();
                        }
                        else
                        {
                            // Speichern das neues Objekt
                            context.Karte.Add(KarteContext);
                            context.SaveChanges();
                        }
                    }
                }

                Close(parameter);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "", MessageBoxButton.OK);
            }
        }

        private void Close(object parameter) => (parameter as Window)?.Close();

        public KarteWindowViewModel()
        {
            try
            {
                // Objekte initialisieren
                KartenList = new List<Karte>();

                using (FitnessStudioDBContext context = new FitnessStudioDBContext())
                {
                    //Laden die Karten List
                    var list = context.Karte.OrderBy(k => k.KarteWert);
                    foreach (var karte in list)
                    {
                        KartenList.Add(karte);
                    }
                }

                // Legen die Werte fest
                _ = KartenList?.Count > 0 ? KarteContext = KartenList.First() : KarteContext = new Karte();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "", MessageBoxButton.OK);
            }
        }
    }
}
