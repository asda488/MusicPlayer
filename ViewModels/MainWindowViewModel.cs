using System.Collections.Generic;
using System.Windows.Input;
using MusicPlayer.Models;
using ReactiveUI;
using Avalonia.Media.Imaging;
using System;
using System.Reactive.Linq;

namespace MusicPlayer.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public ICommand TogglePlayPause { get; }
    public ICommand ChangeView { get; }

    private List<Song> songTableau = new();
    private List<Playlist> playlistTableau = new();
    private List<Song> songList;
    private List<Playlist> playlistList;

    private Bitmap playerImage;
    private string title;
    private string artist;
    private int length;
    private ObservableAsPropertyHelper<string> lengthString;
    private int elapsed;
    private Player player;
    private bool enable = false;
    private ViewModelBase topViewModel;
    private ViewModelBase bottomViewModel;

    public Bitmap PlayerImage { get => playerImage; set => this.RaiseAndSetIfChanged(ref playerImage, value); }
    public string Title { get => title; set => this.RaiseAndSetIfChanged(ref title, value); }
    public string Artist { get => artist; set => this.RaiseAndSetIfChanged(ref artist, value); }
    public int Length { get => length; set => this.RaiseAndSetIfChanged(ref length, value); }
    public string LengthString { get => lengthString.Value; }
    public int Elapsed { get => elapsed; set => this.RaiseAndSetIfChanged(ref elapsed, value); }
    public bool Enable { get => enable; set => this.RaiseAndSetIfChanged(ref enable, value); }
    public ViewModelBase TopViewModel { get => topViewModel; set => this.RaiseAndSetIfChanged(ref topViewModel, value); }
    public ViewModelBase BottomViewModel { get => bottomViewModel; set => this.RaiseAndSetIfChanged(ref bottomViewModel, value); }

    public MainWindowViewModel(){
        //startup
        (songList, playlistList) = Database.LoadData();
        PopulateTableaus();
        player = new(0, null);
        LoadSong(songList[3]);

        //command bindings
        TogglePlayPause = ReactiveCommand.Create<bool>((bool s) => {
            if (s)
                player.Pause();
            else
                player.Play();
            });
        ChangeView = ReactiveCommand.Create<string>((string s) => {
            switch (s) {
                case "Home":
                    TopViewModel = new HomeTopViewModel(songTableau);
                    BottomViewModel = new HomeBottomViewModel(playlistTableau);
                    break;
            }});
        this.WhenAnyValue(x => x.Length, length => $"{(length / 60).ToString()}:{(length % 60).ToString().PadLeft(2, '0')}").ToProperty(this, x => x.LengthString, out lengthString);
        //this.WhenAnyValue(x => x.Length).Subscribe(_ => player.Reader.CurrentTime = new TimeSpan(length % 3600, length % 60, length / 60));
        //this.WhenAnyValue(x => x.player.Reader.CurrentTime.Seconds).Subscribe(x => x = Elapsed);
    }

    private void LoadSong(Song s){
        Enable = false;
        player.Song = s;
        player.LoadNew();
        PlayerImage = player.Song.Image;
        Title = player.Song.Title;
        Artist = player.Song.Artist;
        Length = player.Song.Length;
        Enable = true;
    }

    private void PopulateTableaus(){ //it is recommended to implement one's own sort function via selection sampling
        Random rand = new Random();
        int i = 0;
        foreach (Song s in songList){
            if ((((double)4-songTableau.Count)/(songList.Count-(double)i)) >= rand.NextDouble()){
                songTableau.Add(s);
            }
            if (4-songTableau.Count == 0){
                break;
            }
            i += 1;
        }
        i = 0;
        foreach (Playlist s in playlistList){
            if ((((double)2-playlistTableau.Count)/(playlistList.Count-(double)i)) >= rand.NextDouble()){
                playlistTableau.Add(s);
            }
            if (2-playlistTableau.Count == 0){
                break;
            }
            i += 1;
        } 
    } 

}
