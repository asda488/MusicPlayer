using System.Windows.Input;
using MusicPlayer.Models;
using ReactiveUI;

namespace MusicPlayer.ViewModels;

public class PlaylistBottomViewModel : ViewModelBase
{
    public ICommand NewPlaylistSong { get; }
    public Playlist DisplayPlaylist { get => displayPlaylist; set => this.RaiseAndSetIfChanged(ref displayPlaylist, value); }
    private Playlist displayPlaylist;
    public PlaylistBottomViewModel(Playlist p, ICommand newPlaylistSong){
        NewPlaylistSong = newPlaylistSong;
        DisplayPlaylist = p;
    }
}