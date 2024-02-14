using MusicPlayer.Models;
using System;
using ReactiveUI;
using System.Collections.Generic;
using System.Windows.Input;
namespace MusicPlayer.ViewModels;

public class SearchBottomViewModel : ViewModelBase {
    public ICommand HandleSearchItem { get; } //exposes command for searchItem to be executed, e.g. song to be played or playlist to be open
    public List<SearchItem> SearchResults { get => searchResults; set => this.RaiseAndSetIfChanged(ref searchResults, value); } //exposes SearchResults as list to be displayed
    private List<SearchItem> searchResults = [];
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
