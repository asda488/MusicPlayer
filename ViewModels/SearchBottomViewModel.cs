using MusicPlayer.Models;
using System.Collections.Generic;
using System.Windows.Input;
namespace MusicPlayer.ViewModels;

public class SearchBottomViewModel : ViewModelBase {
    public ICommand NewPlaylistSong { get => new(); }
    public List<SearchItem> SearchResults { get => searchResults;}
    public List<SearchItem> searchResults;
    public SearchBottomViewModel(){
        
    }
}
