using System.Collections.Generic;
using System.Windows.Input;
using MusicPlayer.Models;
using ReactiveUI;

namespace MusicPlayer.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public ICommand TogglePlayPause { get; }
    public MainWindowViewModel(){
        List<Song> SongList;
        List<Playlist> PlaylistList;
        (SongList, PlaylistList) = Database.LoadData();
        Player player = new(0, SongList[0]);
        player.LoadNew();
        TogglePlayPause = ReactiveCommand.Create<bool>((bool s) => {
            if (s) 
                player.Play(); 
            else 
                player.Pause();
            });
    }
}
