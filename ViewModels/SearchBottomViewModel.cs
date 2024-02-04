using MusicPlayer.Models;
using System;
using ReactiveUI;
using System.Collections.Generic;
using System.Windows.Input;
namespace MusicPlayer.ViewModels;

public class SearchBottomViewModel : ViewModelBase {
    public ICommand HandleSearchItem { get; }
    public List<SearchItem> SearchResults { get => searchResults; set => this.RaiseAndSetIfChanged(ref searchResults, value); }
    private List<SearchItem> searchResults;
    public int CurrentSongID { get => currentSongID; set => this.RaiseAndSetIfChanged(ref currentSongID, value); }
    private int currentSongID;
    public SearchBottomViewModel(ICommand enterPlaylist, ICommand newSong, MainWindowViewModel context){
        CurrentSongID = context.SongID;
        context.WhenAnyValue(x => x.SongID).Subscribe(x => {CurrentSongID = x;}); 
        HandleSearchItem = ReactiveCommand.Create((SearchItem s) => {
            if (s.SongID != 0){
                newSong.Execute(s.SongID);
            } 
            else {
                enterPlaylist.Execute(s.PlaylistID);
            }
            });
    }
}
