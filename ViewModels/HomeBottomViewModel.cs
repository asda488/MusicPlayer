using System.Collections.Generic;
using System.Windows.Input;
using MusicPlayer.Models;

namespace MusicPlayer.ViewModels;

public class HomeBottomViewModel(List<Playlist> pl, ICommand p) : ViewModelBase
{
    public ICommand EnterPlaylistCommand { get; } = p; //exposes command allowing playlist page to be opened
    public List<Playlist> PlaylistTableau { get => playlistTableau; } //exposes home page playlists as a list
    private readonly List<Playlist> playlistTableau = pl;
}