using System.Collections.Generic;
using System.Windows.Input;
using MusicPlayer.Models;

namespace MusicPlayer.ViewModels;

public class PlaylistsBottomViewModel : ViewModelBase
{
    public ICommand EnterPlaylistCommand { get; }
    public List<Playlist> PlaylistList { get => playlistList; }
    private List<Playlist> playlistList;
    public PlaylistsBottomViewModel(List<Playlist> pl, ICommand p){
        playlistList = pl;
        EnterPlaylistCommand = p;
    }
}