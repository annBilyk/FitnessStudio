using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FitnessStudio.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FitnessStudio.ViewModels
{
    public partial class TrainerWindowViewModel : ObservableObject
    {
        // Properties
        [ObservableProperty]
        private int trainID;

        [ObservableProperty]
        private DateTime trainGebDatum;

        [ObservableProperty]
        private string trainVorname;

        [ObservableProperty]
        private string trainNachname;

        [ObservableProperty]
        private string trainAdresse;

        [ObservableProperty]
        private int trainPLZ;

        [ObservableProperty]
        private List<Trainer> trainList;

        [ObservableProperty]
        private Trainer trainerContext;

        [ObservableProperty]
        private List<Postleitzahlen> trainPlzList;

        [ObservableProperty]
        private List<Trainer> delTrainList;

        [ObservableProperty]
        private bool? yoga = false;

        [ObservableProperty]
        private bool? stepAerobic = false;

        [ObservableProperty]
        private bool? tanzAerobic = false;

        [ObservableProperty]
        private bool? pilates = false;

        [ObservableProperty]
        private bool? sprungfitness = false;

        [ObservableProperty]
        private bool? stretching = false;

        [ObservableProperty]
        private bool? fitball = false;

        [ObservableProperty]
        private bool? koerperSkulptur = false;

        [ObservableProperty]
        private bool? crossfit = false;

        [RelayCommand]
        private void New()
        {
            try
            {
                // Legen die Werte fest für neue Objekt
                TrainerContext = new Trainer();
                TrainerContext.TrainGebDatum = DateTime.Today.Date;
                TrainerContext.TrainPLZNavigation = new Postleitzahlen();
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
                if (DelTrainList != null && DelTrainList.Count > 0)
                {
                    using (FitnessStudioDBContext context = new())
                    {
                        foreach (var trainer in DelTrainList)
                        {
                            if (trainer.TrainID > 0)
                            {
                                // Ein Objekt in Datenbank finden
                                Trainer? trainObj = context.Trainer.Find(trainer.TrainID);
                                if (trainObj != null)
                                {
                                    // Ein Objekt löschen
                                    context.Trainer.Remove(trainObj);
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
                { // Ein Objekt in Datenbank finden
                    Trainer trainer = TrainerContext != null ? context.Trainer.Find(TrainerContext.TrainID) : null;

                    // Überprüfung das Objekt
                    if (trainer != null && TrainerContext != null)
                    {
                        //Aktualisieren der Felder mit aktuellen Daten
                        trainer.TrainID = TrainerContext.TrainID;
                        trainer.TrainGebDatum = TrainerContext.TrainGebDatum;
                        trainer.TrainVorname = TrainerContext.TrainVorname;
                        trainer.TrainNachname = TrainerContext.TrainNachname;
                        trainer.TrainAdresse = TrainerContext.TrainAdresse;
                        trainer.Yoga = TrainerContext.Yoga;
                        trainer.StepAerobic = TrainerContext.StepAerobic;
                        trainer.TanzAerobic = TrainerContext.TanzAerobic;
                        trainer.Pilates = TrainerContext.Pilates;
                        trainer.Sprungfitness = TrainerContext.Sprungfitness;
                        trainer.Stretching = TrainerContext.Stretching;
                        trainer.Fitball = TrainerContext.Fitball;
                        trainer.KoerperSkulptur = TrainerContext.KoerperSkulptur;
                        trainer.Crossfit = TrainerContext.Crossfit;

                        if (TrainerContext.TrainPLZNavigation != null)
                        {
                            trainer.TrainPLZ = TrainerContext.TrainPLZNavigation.PostID;
                        }

                        // Speichern das geänderte Objekt
                        context.Trainer.Update(trainer);
                        context.SaveChanges();
                    }
                    else
                    {
                        if (TrainerContext != null)
                        {
                            if (TrainerContext.TrainPLZNavigation != null)
                            {
                                TrainerContext.TrainPLZ = TrainerContext.TrainPLZNavigation.PostID;
                            }

                            TrainerContext.TrainPLZNavigation = null;

                            // Speichern das neues Objekt
                            context.Trainer.Add(TrainerContext);
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

        public TrainerWindowViewModel()
        {
            try
            {
                // Objekte initialisieren
                TrainList = new List<Trainer>();
                var activ = "";
                using (FitnessStudioDBContext context = new FitnessStudioDBContext())
                {
                    //Laden die Traineren List
                    var trainerlist = context.Trainer?.Include(p => p.TrainPLZNavigation)?.OrderBy(k => k.TrainID);

                    if (trainerlist.Count() > 0)
                    {
                        foreach (var trainer in trainerlist)
                        {
                            TrainList.Add(trainer);
                        }
                    }
                }

                // Legen die Werte fest
                _ = TrainList?.Count > 0 ? TrainerContext = TrainList.First() : TrainerContext = new Trainer() { TrainGebDatum = DateTime.Today.Date };
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "", MessageBoxButton.OK);
            }
        }
    }
}
