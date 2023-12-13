using MusicPlayer.Models;
using ReactiveUI;
using System;
using System.Reactive.Concurrency;

namespace MusicPlayer.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public MainWindowViewModel(){
        Database.LoadData();
        Console.WriteLine("a");
    }
}
