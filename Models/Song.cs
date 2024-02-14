//class for Song
using System.Collections.Generic;
using Avalonia.Media.Imaging;

namespace MusicPlayer.Models {
    public class Song(int SongID, string Title, int Length, string Filename, Bitmap Image, List<string>? Artists, List<string>? Albums) //corresponds to Songs table in DB, fulfills F2
    {
        public int SongID { get; set; } = SongID;
        public string Title { get; set; } = Title;
        public int Length { get; set; } = Length;
        public string Filename { get; set; } = Filename;
        public List<string> Artists { get; set; } = Artists ?? [];
        public string Artist { get => string.Join(", ", Artists); }
        public List<string> Albums { get; set; } = Albums ?? [];
        public string FirstAlbum { //gets the first album name as a simple string
            get {
                if (Albums.Count > 0) {return Albums[0];}
                else {return "";}
            }
        }
        public Bitmap Image { get; set; } = Image;
    }
}