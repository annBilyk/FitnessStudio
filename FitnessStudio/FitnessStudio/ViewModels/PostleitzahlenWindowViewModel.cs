using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FitnessStudio.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace FitnessStudio.ViewModels
{
    public partial class PostleitzahlenWindowViewModel : ObservableObject
    {
        // Properties
        [ObservableProperty]
        private int postID;

        [ObservableProperty]
        private string postOrt;

        [ObservableProperty]
        private Postleitzahlen postContext;

        [ObservableProperty]
        private List<Postleitzahlen> postList;

        [ObservableProperty]
        private List<Postleitzahlen> delPostList;

        [RelayCommand]
        private void New()
        {
            // Legen die Werte fest für neue Objekt
            var newPost = new Postleitzahlen();
            PostList.Add(newPost);
            PostContext = newPost;
        }

        [RelayCommand]
        private void Del(object parameter)
        {
            try
            {
                // Überprüfung der Liste
                if (DelPostList != null && DelPostList.Count > 0)
                {
                    using (FitnessStudioDBContext context = new())
                    {
                        foreach (var post in DelPostList)
                        {
                            if (post.PostID > 0)
                            {
                                // Ein Objekt in Datenbank finden
                                Postleitzahlen? postObj = context.Postleitzahlen.Find(post.PostID);
                                if (postObj != null)
                                {
                                    // Ein Objekt löschen
                                    context.Postleitzahlen.Remove(postObj);
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
                if (PostContext != null)
                {
                    using (FitnessStudioDBContext context = new())
                    {
                        // Ein Objekt in Datenbank finden
                        Postleitzahlen? plzObj = context.Postleitzahlen.Find(PostContext.PostID);
                        if (plzObj != null)
                        {
                            //Aktualisieren der Felder mit aktuellen Daten
                            plzObj.PostID = PostContext.PostID;
                            plzObj.PostOrt = PostContext.PostOrt;

                            // Speichern das geänderte Objekt
                            context.Postleitzahlen.Update(plzObj);
                            context.SaveChanges();
                        }
                        else
                        {
                            // Speichern das neues Objekt
                            context.Postleitzahlen.Add(PostContext);
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

        public PostleitzahlenWindowViewModel()
        {
            try
            {
                // Objekte initialisieren
                PostList = new List<Postleitzahlen>();

                using (FitnessStudioDBContext context = new FitnessStudioDBContext())
                {
                    //Laden die Postleitzahlen List
                    var list = context.Postleitzahlen.OrderBy(k => k.PostID);
                    foreach (var post in list)
                    {
                        PostList.Add(post);
                    }
                }

                // Legen die Werte fest
                _ = PostList?.Count > 0 ? PostContext = PostList.First() : PostContext = new Postleitzahlen();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "", MessageBoxButton.OK);
            }
        }
    }
}
