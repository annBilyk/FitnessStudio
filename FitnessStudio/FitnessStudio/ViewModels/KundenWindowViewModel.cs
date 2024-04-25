using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FitnessStudio.Models;
using Microsoft.EntityFrameworkCore;

namespace FitnessStudio.ViewModels
{
    public partial class KundenWindowViewModel : ObservableObject
    {
        // Properties
        [ObservableProperty]
        private int kundID;

        [ObservableProperty]
        private DateTime kundGebDatum = DateTime.Today.Date;

        [ObservableProperty]
        private string kundVorname;

        [ObservableProperty]
        private string kundNachname;

        [ObservableProperty]
        private string kundAdresse;

        [ObservableProperty]
        private int kundPLZ;

        [ObservableProperty]
        private int? kundKartNum;

        [ObservableProperty]
        private bool kundConsent;

        [ObservableProperty]
        private List<Kunden> kundList;

        [ObservableProperty]
        private List<Postleitzahlen> kundPlzList;

        [ObservableProperty]
        private List<Karte> kundKartenList;

        [ObservableProperty]
        private Kunden kundenContext;

        [ObservableProperty]
        private List<Kunden> delKundList;

        [RelayCommand]
        private void New()
        {
            try
            {
                // Legen die Werte fest für neue Objekt

                //var guth = KundenContext?.KundKartGuthaben;
                //var karte = KundenContext?.KundKartNumNavigation;

                KundenContext = new Kunden();
                KundenContext.KundGebDatum = DateTime.Today.Date;
                KundenContext.KundPLZNavigation = new Postleitzahlen();
                KundenContext.KundKartNumNavigation = null;
                KundenContext.KundKartGuthaben = 0;
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
                if (DelKundList != null && DelKundList.Count > 0)
                {
                    using (FitnessStudioDBContext context = new())
                    {
                        foreach (var kunde in DelKundList)
                        {
                            if (kunde.KundID > 0)
                            {
                                // Ein Objekt in Datenbank finden
                                Kunden? kundeObj = context.Kunden.Find(kunde.KundID);
                                if (kundeObj != null)
                                {
                                    // Ein Objekt löschen
                                    context.Kunden.Remove(kundeObj);
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
            {
                using (FitnessStudioDBContext context = new())
                {
                    // Ein Objekt in Datenbank finden
                    Kunden kund = KundenContext != null ? context.Kunden.Find(KundenContext.KundID) : null;
                    if (kund != null && kund.KundConsent && KundenContext != null)
                    {
                        //Aktualisieren der Felder mit aktuellen Daten
                        kund.KundID = KundenContext.KundID;
                        kund.KundGebDatum = KundenContext.KundGebDatum;
                        kund.KundVorname = KundenContext.KundVorname;
                        kund.KundNachname = KundenContext.KundNachname;
                        kund.KundAdresse = KundenContext.KundAdresse;
                        kund.KundConsent = KundenContext.KundConsent;

                        if (KundenContext.KundPLZNavigation != null)
                        {
                            kund.KundPLZ = KundenContext.KundPLZNavigation.PostID;
                        }

                        if (KundenContext.KundKartNumNavigation != null)
                        {
                            kund.KundKartNum = KundenContext.KundKartNumNavigation.KarteID;
                        }

                        // Speichern Sie das geänderte Objekt
                        context.Kunden.Update(kund);
                        context.SaveChanges();
                    }
                    else
                    {
                        if (KundenContext != null && KundenContext.KundConsent)
                        {
                            if (KundenContext.KundPLZNavigation != null)
                            {
                                KundenContext.KundPLZ = KundenContext.KundPLZNavigation.PostID;
                            }

                            if (KundenContext.KundKartNumNavigation != null)
                            {
                                KundenContext.KundKartNum = KundenContext.KundKartNumNavigation.KarteID;
                            }

                            KundenContext.KundPLZNavigation = null;
                            KundenContext.KundKartNumNavigation = null;

                            // Speichern Sie das neues Objekt
                            context.Kunden.Add(KundenContext);
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

        public KundenWindowViewModel()
        {
            try
            {
                // Objekte initialisieren
                KundList = new List<Kunden>();
                KundKartenList = new List<Karte>();

                using (FitnessStudioDBContext context = new FitnessStudioDBContext())
                {
                    //Laden die Kunden List
                    var kundlist = context.Kunden.Include(p => p.KundPLZNavigation).Include(k => k.KundKartNumNavigation).OrderBy(k => k.KundID);

                    if (kundlist.Count() > 0)
                    {
                        foreach (var kund in kundlist)
                        {
                            KundList.Add(kund);
                        }
                    }

                    //Laden die Karten List
                    var kartelist = context.Karte.OrderBy(k => k.KarteID);
                    foreach (var karte in kartelist)
                    {
                        KundKartenList.Add(karte);
                    }
                }

                // Legen die Werte fest
                _ = KundList?.Count > 0 ? KundenContext = KundList.First() : KundenContext = new Kunden() { KundGebDatum = DateTime.Today.Date };
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "", MessageBoxButton.OK);
            }
        }
    }
}
