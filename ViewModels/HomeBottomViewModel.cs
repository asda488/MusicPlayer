using System.Collections.Generic;
using System.Windows.Input;
using MusicPlayer.Models;

namespace MusicPlayer.ViewModels;

public class HomeBottomViewModel : ViewModelBase
{
    public ICommand NewPlaylist { get; }
    public List<Playlist> PlaylistTableau { get => playlistTableau; }
    private List<Playlist> playlistTableau;
    public HomeBottomViewModel(List<Playlist> pl, ICommand p){
        playlistTableau = pl;
        NewPlaylist = p;
    }
}