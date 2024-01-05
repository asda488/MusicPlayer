using System.Collections.Generic;
using MusicPlayer.Models;

namespace MusicPlayer.ViewModels;

public class HomeBottomViewModel : ViewModelBase
{
    public List<Playlist> PlaylistTableau { get => playlistTableau; }
    private List<Playlist> playlistTableau;
    public HomeBottomViewModel(List<Playlist> pl){
        playlistTableau = pl;
    }
}