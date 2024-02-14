using System.Linq;
using Avalonia.Media.Imaging;

namespace MusicPlayer.Models {
    public class SearchItem { //wrapper object for searching
        public Bitmap ItemImage { get; }
        public string Title { get; }
        public string Artist { get; }
        public int Length { get; }
        public string AlbumName { get; }
        public int NumberOfSongs { get; }
        //an item will have one of SongID/PlaylistID, the other will be 0
        public int SongID { get; }
        public int PlaylistID { get; }
        public string ThirdFieldConverter { //exposes the third column in the search results, as this could be AlbumName or NumberOfSongs
            get {
                if (PlaylistID == 0)
                    return AlbumName;
                else {return $"{NumberOfSongs} songs";}
            }
        }

        public SearchItem(Playlist p){ //override for Playlist
            ItemImage = p.PlaylistImage;
            Title = p.Title;
            Artist = "";
            Length = p.Songs.Sum(s => s.Length);
            AlbumName = "";
            NumberOfSongs = p.Songs.Count;
            SongID = 0;
            PlaylistID = p.PlaylistID;

        }
        public SearchItem(Song s){ //override for Song
            ItemImage = s.Image;
            Title = s.Title;
            Artist = s.Artist;
            Length = s.Length;
            AlbumName = s.FirstAlbum;
            NumberOfSongs = -1;
            SongID = s.SongID;
            PlaylistID = 0;
        }
    }
}