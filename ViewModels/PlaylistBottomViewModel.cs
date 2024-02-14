using System;
using System.Windows.Input;
using MusicPlayer.Models;
using ReactiveUI;

namespace MusicPlayer.ViewModels;

public class PlaylistBottomViewModel : ViewModelBase
{
    public ICommand NewPlaylistSong { get; } //exposes command to play new song in a playlist
    public Playlist DisplayPlaylist { get => displayPlaylist; set => this.RaiseAndSetIfChanged(ref displayPlaylist, value); } //exposes the current playlist to be displayed
    public int CurrentSongID { get => currentSongID; set => this.RaiseAndSetIfChanged(ref currentSongID, value); } //exposes the currently playing song for the "currently playing" icon
    private Playlist displayPlaylist;
    private int currentSongID;
    public PlaylistBottomViewModel(Playlist p, ICommand newPlaylistSong, MainWindowViewModel context){
        NewPlaylistSong = newPlaylistSong;
        displayPlaylist = p;
        //pull SongID from parent context, and ensure update
        CurrentSongID = context.SongID;
        context.WhenAnyValue(x => x.SongID).Subscribe(x => {CurrentSongID = x;}); 
    }
}