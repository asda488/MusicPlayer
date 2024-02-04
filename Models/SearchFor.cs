using System.Collections.Generic;

namespace MusicPlayer.Models {
    public class SearchFor {
        public static List<SearchItem> SearchForString(List<SearchItem> items, string search){
            List<SearchItem> foundItems = [];
            foreach (SearchItem item in items){
                if (item.Title.Contains(search, System.StringComparison.CurrentCultureIgnoreCase) 
                || item.Artist.Contains(search, System.StringComparison.CurrentCultureIgnoreCase) 
                || item.AlbumName.Contains(search, System.StringComparison.CurrentCultureIgnoreCase))
                {
                    foundItems.Add(item);
                }
            }
            return foundItems;
        } 

        public static List<SearchItem> SearchForType(List<SearchItem> items, System.Type t){
            List<SearchItem> foundItems = [];
            foreach(SearchItem item in items){
                if ((t.Name == "Playlist" && item.PlaylistID != 0) || (t.Name == "Song" && item.SongID != 0)){
                    foundItems.Add(item);
                }
            }
            return foundItems;
        }
    }
}