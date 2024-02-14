using System.Collections.Generic;
using System.Windows.Input;
using MusicPlayer.Models;

namespace MusicPlayer.ViewModels;

public class PlaylistsBottomViewModel(List<Playlist> pl, ICommand p) : ViewModelBase
{
    public ICommand EnterPlaylistCommand { get; } = p; //exposes command allowing playlist page to be opened
    public List<Playlist> PlaylistList { get => playlistList; } //exposes list of all playlists to be displayed
    private readonly List<Playlist> playlistList = pl;
}