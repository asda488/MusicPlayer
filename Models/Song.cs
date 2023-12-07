//class for Song
using System.Collections.Generic;
using System.Drawing;

namespace MusicPlayer.Models {
    public class Song {
        public int SongID { get; set;}
        public string Title { get; set; }
        public int Length { get; set; }
        public List<string> Artists { get; set;}
        public List<string> Albums { get; set;}
        public Image SongImage { get; set;}

        public Song(int songID, string title, int length, List<string> artists, Image songImage, List<string> albums) {
            SongID = songID;
            Title = title;
            Length = length;
            Artists = artists;
            SongImage = songImage; //can be null
            Albums = albums ?? new List<string>();
        }

    }
}