using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using FitnessStudio.Models;
using System.Windows;

namespace FitnessStudio.ViewModels
{
    public partial class KalenderWindowViewModel : ObservableObject
    {
        // Properties
        [ObservableProperty]
        private long kalendID;

        [ObservableProperty]
        private DateTime kalendDatum;

        [ObservableProperty]
        private long kalendTrainID;

        [ObservableProperty]
        private string kalendAktivName;

        [ObservableProperty]
        private List<string> activitaetNamen = new List<string> { "Yoga", "Step-Aerobic", "Tanz-Aerobic", "Pilates", "Sprungfitness", "Stretching", "Fitball", "Körper-Skulptur", "Crossfit" };

        [ObservableProperty] 
        private List<Trainer> kalendTrainerList;

        [ObservableProperty]
        private Kalender kalendContext;

        [ObservableProperty]
        private DateTime kalendDateStart = new DateTime(DateTime.Today.Date.Year, DateTime.Today.Date.Month, 1);

        [ObservableProperty]
        private DateTime kalendDateEnd = new DateTime(DateTime.Today.Date.Year + 1, DateTime.Today.Date.Month, DateTime.Today.Date.Day);

        [ObservableProperty]
        private List<KundenSelection> kundSelectionList;

        [ObservableProperty]
        private List<Kalender> kalendList;

        [ObservableProperty]
        private List<SpecifKalend> delKalendList;

        [ObservableProperty]
        private Trainer kalendTrain;

        [ObservableProperty]
        private DateTime kalendDateToSelect = new DateTime(DateTime.Today.Date.Year, DateTime.Today.Date.Month, 1);

        [ObservableProperty]
        private string kalendAktivNameToSelect;

        [ObservableProperty]
        private TimeSpan kalendZeit;

        [ObservableProperty]
        private TimeSpan kalendZeitToSelect;

        [ObservableProperty]
        private List<SpecifKalend> specifKalendList;

        [RelayCommand]
        private void Del(object parameter)
        {
            try
            {
                // Überprüfung der Liste
                if (DelKalendList != null && DelKalendList.Count > 0)
                {
                    using (FitnessStudioDBContext context = new())
                    {
                        //Laden Sie die Tabelle mit Schlüsseln
                        var extList = context.KalendKund.ToList();

                        foreach (var kalend in DelKalendList)
                        {
                            var listForDel = extList.Where(d =>
                                    d.ExtKalendID == kalend.SpecifKalendID && d.ExtKundID == kalend.SpecifKalendKundID)
                                .ToList();

                            foreach (var delObj in listForDel)
                            {
                                //Laden Sie Kalender objekt
                                var item = context.Kalender.Find(delObj.ExtKalendID);

                                if (item != null)
                                {
                                    if (item.KalendDatum >= DateTime.Now.Date && delObj != null)
                                    {
                                        //Trainingsaufzeichnungen löschen
                                        context.KalendKund.Remove(delObj);
                                        context.SaveChanges();
                                    }
                                }
                            }
                        }

                        var kalendIdList = DelKalendList.Select(k => k.SpecifKalendID).ToList().Distinct().ToList();

                        foreach (var kalendItem in kalendIdList)
                        {
                            // Überprüfen Sie, ob für diesen Termin noch Trainingseinheiten verfügbar sind
                            var checkList = context.KalendKund.Where(k => k.ExtKalendID == kalendItem).ToList();

                            if (checkList?.Count <= 0)
                            {
                                // Für dieses Datum gibt es keine weiteren Trainingseinheiten. Löschen Sie das Kalenderobjekt
                                var item = context.Kalender.Find(kalendItem);

                                if (item != null)
                                {
                                    context.Kalender.Remove(item);
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
                    // Überprüfen Sie das Datum, um ein neues Training zu erstellen. Es sollte nicht in der Vergangenheit liegen
                    if (KalendDateToSelect >= DateTime.Now.Date || KalendZeitToSelect >= new TimeSpan(DateTime.Now.Hour +1, 0, 0))
                    {
                        //Laden Sie Kalender objekt
                        Kalender? kalendObj = KalendList.FirstOrDefault(k =>
                            k.KalendDatum == KalendDateToSelect && k.KalendZeit == KalendZeitToSelect &&
                            k.KalendTrainID == KalendContext.KalendTrainID);

                        if (kalendObj == null)
                        {
                            //Aktualisieren der Felder mit aktuellen Daten
                            KalendContext.KalendID = 0;
                            KalendContext.KalendDatum = KalendDateToSelect;
                            KalendContext.KalendZeit = KalendZeitToSelect;
                            KalendContext.KalendTrainID = KalendContext.KalendTrainID;
                            KalendContext.KalendAktivName = KalendAktivNameToSelect;
                            KalendContext.KalendTrain = null;
                            KalendContext.KalendKund = null;

                            // Speichern Sie das neue Objekt
                            context.Kalender.Add(KalendContext);
                            context.SaveChanges();
                        }

                        Kalender? kalendSavedObj = context.Kalender.Find(KalendContext.KalendID);
                        if (kalendSavedObj != null)
                        {
                            foreach (var kund in KundSelectionList)
                            {
                                if (kund != null && kund.Selected)
                                {
                                    //Laden Sie Kunden objekt
                                    Kunden kundObj = context.Kunden.Find(kund.KundID);

                                    if (kundObj != null && kundObj.KundKartGuthaben > 0)
                                    {
                                        KalendKund kalendKund = new KalendKund() { ExtKalendID = kalendSavedObj.KalendID, ExtKundID = kund.KundID };

                                        // Speichern Sie das Training für jeden Kunden
                                        context.KalendKund.Add(kalendKund);
                                        context.SaveChanges();

                                        //Aktualisieren KundKartGuthaben für Kunde
                                        kundObj.KundKartGuthaben--;
                                        context.Kunden.Update(kundObj);
                                        context.SaveChanges();
                                    }
                                    else
                                    {
                                        MessageBox.Show($"Die Karte für {kundObj?.KundVorname} {kundObj?.KundNachname} ist nicht gültig.");
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

        public KalenderWindowViewModel()
        {
            try
            {
                // Objekte initialisieren
                KalendList = new List<Kalender>();
                KundSelectionList = new List<KundenSelection>();
                KalendTrainerList = new List<Trainer>();
                KalendTrain = new Trainer();
                ActivitaetNamen = ActivitaetNamen.OrderBy(n => n).ToList();
                KalendDateToSelect = DateTime.Today.Date;
                KalendAktivNameToSelect = ActivitaetNamen.First();
                KalendZeitToSelect = new TimeSpan(DateTime.Now.Hour + 1, 0, 0);
                SpecifKalendList = new List<SpecifKalend>();

                using (FitnessStudioDBContext context = new FitnessStudioDBContext())
                {
                    //Laden Sie Kunden List
                    var kundelist = context.Kunden.OrderBy(k => k.KundID).ToList();
                    foreach (var kund in kundelist)
                    {
                        //Erstellen Sie ein Objekt einer neuen Klasse, um es in der Kunden Tabelle anzuzeigen
                        var k = new KundenSelection()
                        {
                            KundID = kund.KundID,
                            KundGebDatum = kund.KundGebDatum,
                            KundNachname = kund.KundNachname,
                            KundVorname = kund.KundVorname,
                            Selected = false
                        };

                        if (k.KundID > 0)
                        {
                            KundSelectionList.Add(k);
                        }
                    }

                    //Laden Sie Traineren List
                    var trainerList = context.Trainer.OrderBy(k => k.TrainID);
                    foreach (var trainer in trainerList)
                    {
                        KalendTrainerList.Add(trainer);
                    }

                    //Laden Sie Kalender List
                    var list = context.Kalender.OrderBy(k => k.KalendID);

                    //Laden Sie der List mit Zusammenhängen
                    var extList = context.KalendKund.ToList();

                    foreach (var kalender in list)
                    {
                        foreach (var item in extList)
                        {
                            if (kalender.KalendID == item.ExtKalendID)
                            {
                                //Erstellen Sie ein Objekt einer neuen Klasse, um es in der Kalender Tabelle anzuzeigen
                                var specif = new SpecifKalend()
                                {
                                    SpecifKalendID = kalender.KalendID,
                                    SpecifKalendKundID = item.ExtKundID ?? 0,
                                    SpecifKalendDatum = kalender.KalendDatum,
                                    SpecifKalendZeit = kalender.KalendZeit,
                                    SpecifKalendKunden = kundelist?.FirstOrDefault(k => k.KundID == item?.ExtKundID) ?? new Kunden(),
                                    SpecifKalendTrainer = KalendTrainerList?.FirstOrDefault(k => k.TrainID == kalender?.KalendTrainID) ?? new Trainer(),
                                    SpecifKalendAktivitat = kalender.KalendAktivName
                                };

                                SpecifKalendList.Add(specif);
                                KalendList.Add(kalender);
                            }
                        }
                    }
                }

                // Datensätze sortieren
                SpecifKalendList = SpecifKalendList.OrderBy(k => k.SpecifKalendDatum).ThenBy(z => z.SpecifKalendZeit).ToList();
                KalendList = KalendList.OrderBy(k => k.KalendDatum).ThenBy(z => z.KalendZeit).ToList();

                // Legen die Werte fest
                _ = KalendList?.Count > 0 ? KalendContext = KalendList.First() : KalendContext = new Kalender();

                KalendContext.KalendAktivName = ActivitaetNamen.First();

                if (KalendTrainerList != null && KalendTrainerList.Count > 0)
                {
                    KalendContext.KalendTrain = KalendTrainerList.First();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "", MessageBoxButton.OK);
            }
        }
    }
}
