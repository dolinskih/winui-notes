using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using WinUINotes.Models;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace WinUINotes.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AllNotesPage : Page
    {
        private readonly AllNotes notesModel = new AllNotes();

        public AllNotesPage()
        {
            InitializeComponent();
            Loaded += AllNotesPage_Loaded;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is Note note)
            {
                if (note.State == NoteState.Deleted)
                {
                    notesModel.RemoveNote(note);
                }
                else if (!notesModel.Notes.Contains(note))
                {
                    notesModel.AddNote(note);
                }
                // This navigation should be treated like a back navigation, so clear the backstack.
                Frame.BackStack.Clear();
            }
        }

        private async void AllNotesPage_Loaded(object sender, RoutedEventArgs e)
        {
            Loaded -= AllNotesPage_Loaded;
            await notesModel.LoadNotesAsync();
        }

        private void NewNoteButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(NotePage));
        }

        private void ItemsView_ItemInvoked(ItemsView sender, ItemsViewItemInvokedEventArgs args)
        {
            Frame.Navigate(typeof(NotePage), args.InvokedItem);
        }
    }
}
