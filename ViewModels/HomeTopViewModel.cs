using System.Collections.Generic;
using System.Windows.Input;
using MusicPlayer.Models;

namespace MusicPlayer.ViewModels;

public class HomeTopViewModel : ViewModelBase
{
    public ICommand NewSong { get; }
    public List<Song> SongTableau { get => songTableau; }
    private List<Song> songTableau;
    public HomeTopViewModel(List<Song> sl, ICommand s){
        songTableau = sl;
        NewSong = s;
    }
}