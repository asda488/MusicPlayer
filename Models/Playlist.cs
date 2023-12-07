//class for Playlist
using System;
using System.Collections.Generic;
using System.Drawing;

namespace MusicPlayer.Models {
    public class Playlist {
        public int PlaylistID { get; set;}
        public string Title { get; set; }
        public string Description { get; set; } 
        public Image PlaylistImage { get; set; }
        public List<Song> PlaylistSongs { get; set;}

        public Playlist(int playlistID, string title, string description, Image playlistImage, List<Song> playlistSongs) {
            PlaylistID = playlistID;
            Title = title;
            Description = description ?? String.Empty;
            PlaylistImage = playlistImage; //can be null
            PlaylistSongs = playlistSongs;
        }

    }
}