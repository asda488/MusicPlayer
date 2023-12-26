//class for Song
using System.Collections.Generic;

namespace MusicPlayer.Models {
    public class Song {
        public int songID { get; set;}
        public string title { get; set; }
        public int length { get; set; }
        public string filename { get; set; }
        public List<string> artists { get; set;}
        public List<string> albums { get; set;}
        public byte[]? songImageBlob { get; set;}

        public Song(int SongID, string Title, int Length, string Filename, byte[]? SongImageBlob, List<string>? Artists, List<string>? Albums) {
            songID = SongID;
            title = Title;
            length = Length;
            filename = Filename;
            songImageBlob = SongImageBlob; //nullable
            artists = Artists ?? new List<string>();
            albums = Albums ?? new List<string>();
        }

    }
}