using System;
using System.Collections.Generic;
using MusicPlayer.Models;

namespace MusicPlayer.ViewModels;

public class HomeTopViewModel : ViewModelBase
{
    public List<Song> SongTableau { get => songTableau; }
    private List<Song> songTableau;
    public HomeTopViewModel(List<Song> sl){
        songTableau = sl;
    }
}