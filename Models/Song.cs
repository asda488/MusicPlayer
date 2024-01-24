//class for Song
using System.Collections.Generic;
using Avalonia.Media.Imaging;

namespace MusicPlayer.Models {
    public class Song {
        public int SongID { get; set;}
        public string Title { get; set; }
        public int Length { get; set; }
        public string Filename { get; set; }
        public List<string> Artists { get; set;}
        public string Artist { get => string.Join(", ", Artists); }
        public List<string> Albums { get; set;}
        public string FirstAlbum { get => Albums[0]; }
        public Bitmap Image { get; set;}

        public Song(int SongID, string Title, int Length, string Filename, Bitmap Image, List<string>? Artists, List<string>? Albums) {
            this.SongID = SongID;
            this.Title = Title;
            this.Length = Length;
            this.Filename = Filename;
            this.Image = Image;
            this.Artists = Artists ?? new List<string>();
            this.Albums = Albums ?? new List<string>();
        }

    }
}