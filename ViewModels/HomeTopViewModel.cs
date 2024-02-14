using System.Collections.Generic;
using System.Windows.Input;
using MusicPlayer.Models;

namespace MusicPlayer.ViewModels;

public class HomeTopViewModel(List<Song> sl, ICommand s) : ViewModelBase
{
    public ICommand NewSong { get; } = s; //exposes command to play new song without playlist
    public List<Song> SongTableau { get => songTableau; } //exposes home page songs as a list
    private readonly List<Song> songTableau = sl;
}