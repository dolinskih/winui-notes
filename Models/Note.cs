using System;
using System.Collections.Generic;
using System.Text;
using Windows.Storage;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WinUINotes.Models
{
    public class Note : INotifyPropertyChanged
    {
        private readonly StorageFolder storageFolder = Microsoft.Windows.Storage.ApplicationData.GetDefault().LocalFolder;
        public string Filename { get; set; } = string.Empty;
        private string _text = string.Empty;
        public NoteState State { get; set; } = NoteState.Unset;
        public string Text
        {
            get => _text;
            set
            {
                if (_text != value)
                {
                    _text = value;
                    State = NoteState.Unsaved;
                    OnPropertyChanged();
                }
            }
        }
        public DateTime Date { get; set; } = DateTime.Now;

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public Note()
        {
            Filename = "notes" + DateTime.Now.ToBinary().ToString() + ".txt";
        }

        public async Task SaveAsync()
        {
            // Save the note to a file.
            StorageFile? noteFile = await storageFolder.TryGetItemAsync(Filename) as StorageFile;
            if (noteFile is null)
            {
                noteFile = await storageFolder.CreateFileAsync(Filename, CreationCollisionOption.ReplaceExisting);
            }
            await FileIO.WriteTextAsync(noteFile, Text);
            State = NoteState.Saved;
        }

        public async Task DeleteAsync()
        {
            // Delete the note from the file system.
            StorageFile? noteFile = await storageFolder.TryGetItemAsync(Filename) as StorageFile;
            if (noteFile is not null)
            {
                await noteFile.DeleteAsync();
            }
            Filename = string.Empty;
            State = NoteState.Deleted;
        }
    }

    public enum NoteState
    {
        Unset = 0, Unsaved, Saved, Deleted
    }
}
