using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitnessStudio.Models;
using System.Windows;
using System.Reflection.Metadata;

namespace FitnessStudio.ViewModels
{
    public partial class RechnungWindowViewModel : ObservableObject
    {
        // Properties
        [ObservableProperty]
        private int rechID;

        [ObservableProperty]
        private DateTime rechDatum = DateTime.Today.Date;

        [ObservableProperty]
        private long rechKundID;

        [ObservableProperty]
        private double rechSumme;

        [ObservableProperty]
        private bool rechIstBezahlt;

        [ObservableProperty]
        private Rechnung rechContext;

        [ObservableProperty]
        private Kunden rechKund;

        [ObservableProperty]
        private List<Rechnung> rechnungList;

        [ObservableProperty]
        private List<Rechnung> delRechList;

        [ObservableProperty]
        private List<Karte> rechKartenList;

        [ObservableProperty]
        private List<Kunden> rechKundList;

        [ObservableProperty]
        private Karte rechKarte;

        [RelayCommand]
        private void New()
        {
            try
            {
                // Legen die Werte fest für neue Objekt
                RechContext = new Rechnung();
                RechContext.RechDatum = DateTime.Today.Date;
                RechContext.RechKund = new Kunden();
                RechKarte = RechKartenList.First();
                RechContext.RechSumme = RechKarte.KartePrice;
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
                if (DelRechList != null && DelRechList.Count > 0)
                {
                    using (FitnessStudioDBContext context = new())
                    {
                        foreach (var rech in DelRechList)
                        {
                            if (rech.RechID > 0)
                            {
                                // Ein Objekt in Datenbank finden
                                Rechnung? rechObj = context.Rechnung.Find(rech.RechID);
                                if (rechObj != null)
                                {
                                    // Ein Objekt löschen
                                    context.Rechnung.Remove(rechObj);
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
                // Überprüfung das Objekt
                if (RechContext != null)
                {
                    using (FitnessStudioDBContext context = new())
                    {
                        // Ein Objekt in Datenbank finden
                        Rechnung? rechObj = context.Rechnung.Find(RechContext.RechID);

                        if (rechObj != null)
                        {
                            //Aktualisieren der Felder mit aktuellen Daten
                            rechObj.RechID = RechContext.RechID;
                            rechObj.RechDatum = RechContext.RechDatum;
                            rechObj.RechKundID = RechContext.RechKundID;
                            rechObj.RechSumme = RechContext.RechSumme;
                            rechObj.RechIstBezahlt = RechContext.RechIstBezahlt;

                            if (rechObj.RechKundID != null && rechObj.RechKundID > 0)
                            {
                                // Ein Kunden Objekt in Datenbank finden um Karte information zu ändern
                                Kunden kund = context.Kunden.Find(rechObj.RechKundID);

                                if (kund != null && RechKarte != null)
                                {
                                    kund.KundKartNum = RechKarte.KarteID;
                                    kund.KundKartGuthaben = RechKarte.KarteWert;

                                    // Speichern das geänderte Kunde Objekt
                                    context.Kunden.Update(kund);
                                    context.SaveChanges();
                                }
                            }
                            
                            RechContext.RechKund = null;

                            // Speichern das geänderte Rechnung Objekt
                            context.Rechnung.Update(rechObj);
                            context.SaveChanges();
                        }
                        else
                        {
                            if (RechContext.RechKundID != null && RechContext.RechKundID > 0)
                            {
                                // Ein Kunden Objekt in Datenbank finden um Karte information zu ändern
                                Kunden kund = context.Kunden.Find(RechContext.RechKundID);

                                if (kund != null && RechKarte != null)
                                {
                                    if (kund.KundKartGuthaben == null || kund.KundKartGuthaben <= 0)
                                    {
                                        kund.KundKartNum = RechKarte.KarteID;
                                        kund.KundKartGuthaben = RechKarte.KarteWert;

                                        // Speichern das geänderte Kunde Objekt
                                        context.Kunden.Update(kund);
                                        context.SaveChanges();

                                        RechContext.RechKund = null;
                                        // Speichern das neues Rechnung Objekt
                                        context.Rechnung.Add(RechContext);
                                        context.SaveChanges();
                                    }
                                    else
                                    {
                                        MessageBox.Show("Ein neues Rechnung kann nicht erstellt werden. Die bestehende Karte ist weiterhin gültig.");
                                    }
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

        private void Close(object parameter) => (parameter as Window)?.Close();

        public RechnungWindowViewModel()
        {
            try
            {
                // Objekte initialisieren
                RechnungList = new List<Rechnung>();
                RechKartenList = new List<Karte>();
                RechKundList = new List<Kunden>();
                RechKund = new Kunden();
                RechKarte = new Karte();

                using (FitnessStudioDBContext context = new FitnessStudioDBContext())
                {
                    //Laden die Rechnung List
                    var list = context.Rechnung.OrderBy(k => k.RechID);
                    foreach (var rech in list)
                    {
                        RechnungList.Add(rech);
                    }

                    //Laden die Karten List
                    var kartelist = context.Karte.OrderBy(k => k.KarteID);
                    foreach (var karte in kartelist)
                    {
                        RechKartenList.Add(karte);
                    }

                    //Laden die Kunden List
                    var kundelist = context.Kunden.OrderBy(k => k.KundID);
                    foreach (var kund in kundelist)
                    {
                        RechKundList.Add(kund);
                    }
                }

                // Legen die Werte fest
                if (RechnungList.Count > 0)
                {
                    RechContext = RechnungList.First();
                    RechContext.RechKund = RechKundList.First(k => k.KundID == RechContext.RechKundID);
                    RechContext.RechKund.KundKartNumNavigation = RechKartenList.First(k => k.KarteID == RechContext.RechKund.KundKartNum);
                    RechKarte = RechContext.RechKund?.KundKartNumNavigation;
                }
                else
                {
                    RechContext = new Rechnung() { RechDatum = DateTime.Today.Date, RechKund = new Kunden() };
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "", MessageBoxButton.OK);
            }
        }

    }
}
