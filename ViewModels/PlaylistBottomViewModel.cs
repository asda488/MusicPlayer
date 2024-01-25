using System;
using System.Windows.Input;
using MusicPlayer.Models;
using ReactiveUI;

namespace MusicPlayer.ViewModels;

public class PlaylistBottomViewModel : ViewModelBase
{
    public ICommand NewPlaylistSong { get; }
    public Playlist DisplayPlaylist { get => displayPlaylist; set => this.RaiseAndSetIfChanged(ref displayPlaylist, value); }
    public int CurrentSongID { get => currentSongID; set => this.RaiseAndSetIfChanged(ref currentSongID, value); }
    private Playlist displayPlaylist;
    private int currentSongID;
    public PlaylistBottomViewModel(Playlist p, ICommand newPlaylistSong, MainWindowViewModel context){
        NewPlaylistSong = newPlaylistSong;
        displayPlaylist = p;
        CurrentSongID = context.SongID;
        context.WhenAnyValue(x => x.SongID).Subscribe(x => {CurrentSongID = x;}); 
    }
}