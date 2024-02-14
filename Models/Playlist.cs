using System.Collections.Generic;
using Avalonia.Media.Imaging;

namespace MusicPlayer.Models {
    public class Playlist(int PlaylistID, string Title, string? Description, Bitmap PlaylistImage, List<Song>? Songs) //corresponds to Playlists table in DB, fulfills F2
    //we do not inherit from List<T> as that is bad practice, and composition over inheritance is preferred
    {
        public int PlaylistID { get; set; } = PlaylistID;
        public string Title { get; set; } = Title;
        public string Description { get; set; } = Description ?? string.Empty; //catch null
        public Bitmap PlaylistImage { get; set; } = PlaylistImage;
        public List<Song> Songs { get; set; } = Songs ?? [];
    }
}