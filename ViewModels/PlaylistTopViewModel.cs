using MusicPlayer.Models;
using System.Linq;
using ReactiveUI;

namespace MusicPlayer.ViewModels;

public class PlaylistTopViewModel : ViewModelBase
{
    public Playlist DisplayPlaylist { get => displayPlaylist; set => this.RaiseAndSetIfChanged(ref displayPlaylist, value); }
    public string InfoString { get => infoString.Value; }
    private Playlist displayPlaylist;
    private ObservableAsPropertyHelper<string> infoString;

    public PlaylistTopViewModel(Playlist p){
        displayPlaylist = p;
        infoString = this.WhenAnyValue(x => x.DisplayPlaylist, displayPlaylist => $"{displayPlaylist.Songs.Count} tracks • {(from l in displayPlaylist.Songs select l.Length).Sum()/60}:{(from l in displayPlaylist.Songs select l.Length).Sum()%60} runtime").ToProperty(this, x => x.InfoString);

    }
}