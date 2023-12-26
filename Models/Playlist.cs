//class for Playlist
using System.Collections.Generic;
using System.Drawing;

namespace MusicPlayer.Models {
    public class Playlist {
        public int playlistID { get; set;}
        public string title { get; set; }
        public string description { get; set; } 
        public byte[]? playlistImage { get; set; }
        public List<Song> playlistSongs { get; set;}

        public Playlist(int PlaylistID, string Title, string? Description, byte[]? PlaylistImage, List<Song>? PlaylistSongs) {
            playlistID = PlaylistID;
            title = Title;
            description = Description ?? string.Empty; //catch null
            playlistImage = PlaylistImage; //nullable
            playlistSongs = PlaylistSongs ?? new List<Song>();
        }

    }
}