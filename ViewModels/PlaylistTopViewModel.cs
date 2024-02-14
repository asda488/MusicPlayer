using MusicPlayer.Models;
using System.Linq;
using ReactiveUI;

namespace MusicPlayer.ViewModels;

public class PlaylistTopViewModel : ViewModelBase
{
    public Playlist DisplayPlaylist { get => displayPlaylist; set => this.RaiseAndSetIfChanged(ref displayPlaylist, value); } //exposes the current playlist to be displayed
    public string InfoString { get => infoString.Value; } //exposes "number of songs and runtime" as single simple string
    private Playlist displayPlaylist;
    private readonly ObservableAsPropertyHelper<string> infoString;

    public PlaylistTopViewModel(Playlist p){
        displayPlaylist = p;
        //compounds number of songs and runtime into single simple string
        infoString = this.WhenAnyValue(x => x.DisplayPlaylist, displayPlaylist => $"{displayPlaylist.Songs.Count} tracks • {(from l in displayPlaylist.Songs select l.Length).Sum()/60}:{(from l in displayPlaylist.Songs select l.Length).Sum()%60} runtime").ToProperty(this, x => x.InfoString);

    }
}