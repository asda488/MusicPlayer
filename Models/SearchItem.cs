using System.Linq;

namespace MusicPlayer.Models {
    public class SearchItem {
        public string Title { get; }
        public string Artist { get; }
        public int Length { get; }
        public string AlbumName { get; }
        public int NumberOfSongs { get; }
        public string ThirdFieldConverter { 
            get {
                if (NumberOfSongs == -1)
                    return AlbumName;
                else {return $"{NumberOfSongs} songs";}
            }
        }

        public int SongID { get; }
        public SearchItem(Playlist p){
            Title = p.Title;
            Artist = "";
            Length = p.Songs.Sum(s => s.Length);
            AlbumName = "";
            NumberOfSongs = p.Songs.Count;
            SongID = 0;

        }
        public SearchItem(Song s){
            Title = s.Title;
            Artist = s.Artist;
            Length = s.Length;
            AlbumName = s.FirstAlbum;
            NumberOfSongs = -1;
            SongID = s.SongID;
        }
    }
}