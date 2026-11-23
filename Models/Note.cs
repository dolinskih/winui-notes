using System;
using System.Collections.Generic;
using System.Text;
using Windows.Storage;

namespace WinUINotes.Models
{
    public class Note
    {
        private readonly StorageFolder storageFolder = Microsoft.Windows.Storage.ApplicationData.GetDefault().LocalFolder;
        public string Filename { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
        public DateTime Date { get; set; } = DateTime.Now;

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
        }

        public async Task DeleteAsync()
        {
            // Delete the note from the file system.
            StorageFile? noteFile = await storageFolder.TryGetItemAsync(Filename) as StorageFile;
            if (noteFile is not null)
            {
                await noteFile.DeleteAsync();
            }
        }
    }
}
