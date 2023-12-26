using MusicPlayer.Models;

namespace MusicPlayer.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public MainWindowViewModel(){
        Database.LoadData();
    }
}
