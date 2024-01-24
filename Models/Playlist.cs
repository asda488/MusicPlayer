//class for Playlist
using System.Collections.Generic;
using Avalonia.Media.Imaging;

namespace MusicPlayer.Models {
    public class Playlist {
        public int PlaylistID { get; set;}
        public string Title { get; set; }
        public string Description { get; set; } 
        public Bitmap PlaylistImage { get; set; }
        public List<Song> Songs { get; set;}
        public Playlist(int PlaylistID, string Title, string? Description, Bitmap PlaylistImage, List<Song>? Songs) {
            this.PlaylistID = PlaylistID;
            this.Title = Title;
            this.Description = Description ?? string.Empty; //catch null
            this.PlaylistImage = PlaylistImage; //nullable
            this.Songs = Songs ?? new List<Song>();
        }

    }
}