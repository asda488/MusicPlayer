using System.Collections.Generic;

namespace MusicPlayer.Models {
    public class Sort {
        private static readonly List<(SortCriteria, bool)> sortsList = [ //a list corresponding exactly to the ComboBox items in SearchTopViewModel.cs for Sorts, in order, as the index is retrieved rather than the actual string
            (SortCriteria.Title, true),  //order = true for ascending, false for descending
            (SortCriteria.Title, false),
            (SortCriteria.Artist, true),
            (SortCriteria.Artist, false),
            (SortCriteria.Length, true),
            (SortCriteria.Length, false),
            (SortCriteria.AlbumName, true),
            (SortCriteria.AlbumName, true)
        ];
        public static List<SearchItem> BubbleSortItems(List<SearchItem> list, SortCriteria criteria=SortCriteria.Title, bool order=true) { //implements bubble sort with custom comparison, fulfills F5(a)
            bool sorted = false;
            while (!sorted){
                sorted = true;
                for (int i = 0; i < list.Count-1; i++){
                    if (CompareItems(list[i], list[i+1], criteria, order)) {
                        (list[i+1], list[i]) = (list[i], list[i+1]);
                        sorted = false;
                    }
                }
            }
            return list;
        }

        public static List<SearchItem> BubbleSortItems(List<SearchItem> list, int sortStringIndex){ //extra overload utilizing the ComboBox index rather than the actual sort parameters
            return BubbleSortItems(list, sortsList[sortStringIndex].Item1, sortsList[sortStringIndex].Item2);
        }
        private static bool CompareItems(SearchItem a, SearchItem b, SortCriteria c, bool o){ //custom comparison based on SortCriteria and order, returns true if a swap is needed
            //o = order = true for asc, false for desc
            switch(c) {
                case SortCriteria.Title:
                    if (string.Compare(a.Title, b.Title) > 0){
                        return o;
                    }
                    else { return !o; }
                case SortCriteria.Artist:
                    if (string.Compare(a.Artist, b.Artist) > 0){
                        return o;
                    }
                    else { return !o; }                
                case SortCriteria.Length:
                    if (a.Length > b.Length){
                        return o;
                    }
                    else { return !o; }
                case SortCriteria.AlbumName:
                    if (string.Compare(a.AlbumName, b.AlbumName) > 0){
                        return o;
                    }
                    else { return !o; }
                default:
                    return false;
            }
        }
    }
}