using System.Linq;
using System.Collections.Generic;
using System.Windows.Input;
using MusicPlayer.Models;
using ReactiveUI;
using Avalonia.Media.Imaging;
using System;
using Avalonia.Platform;

namespace MusicPlayer.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public ICommand TogglePlayPause { get; }
    public ICommand ChangeViewCommand { get; }
    public ICommand ShuffleCommand { get; }
    public ICommand EnterPlaylistCommand { get; }

    private List<Song> songTableau = new();
    private List<Playlist> playlistTableau = new();
    private List<Song> songList;
    private List<Playlist> playlistList;
    private Random rand = new();
    private Bitmap playerImage;
    private string title;
    private string artist;
    private int length;
    private int songID;
    private bool playIsChecked = true;
    private ObservableAsPropertyHelper<int> elapsedConverter;
    private int elapsed;
    private Player player;
    private bool enable = false;
    private bool shuffle = false;
    private bool loop = false;
    private ViewModelBase topViewModel;
    private ViewModelBase bottomViewModel;
    private Playlist? playlist;
    private List<int> shuffleOrder;

    public Bitmap PlayerImage { get => playerImage; set => this.RaiseAndSetIfChanged(ref playerImage, value); }
    public string Title { get => title; set => this.RaiseAndSetIfChanged(ref title, value); }
    public string Artist { get => artist; set => this.RaiseAndSetIfChanged(ref artist, value); }
    public int SongID { get => songID; set => this.RaiseAndSetIfChanged(ref songID, value); }
    public bool PlayIsChecked { get => playIsChecked; set => this.RaiseAndSetIfChanged(ref playIsChecked, value); }
    public int Length { get => length; set => this.RaiseAndSetIfChanged(ref length, value); }
    public int Elapsed { get => elapsed; set => this.RaiseAndSetIfChanged(ref elapsed, value); }
    public int SetElapsed { set => player.SetTime = value; }
    public int ElapsedConverter { get => elapsedConverter.Value; set => SetElapsed = value; }
    public bool Enable { get => enable; set => this.RaiseAndSetIfChanged(ref enable, value); }
    public bool Shuffle { get => shuffle; set => this.RaiseAndSetIfChanged(ref shuffle, value); }
    public bool Loop {get => loop; set => this.RaiseAndSetIfChanged(ref loop, value);}
    public ViewModelBase TopViewModel { get => topViewModel; set => this.RaiseAndSetIfChanged(ref topViewModel, value); }
    public ViewModelBase BottomViewModel { get => bottomViewModel; set => this.RaiseAndSetIfChanged(ref bottomViewModel, value); }

    public MainWindowViewModel(){
        //startup
        (songList, playlistList) = Database.LoadData();
        PopulateTableaus();
        playlistList.Add(new Playlist(playlistList.Count+1, "All Media", "All tracks in your library.", new(AssetLoader.Open(new Uri("avares://MusicPlayer/Assets/default-image.png"))), [.. songList]));
        player = new(0, null);
        player.PlayerFinished += NextSong;

        //command bindings
        ShuffleCommand = ReactiveCommand.Create(() => {
            if (shuffle && playlist != null && shuffleOrder == null){
                NewShuffleOrder();
            }
        });
        TogglePlayPause = ReactiveCommand.Create(() => {
            if (playIsChecked)
                player.Pause();
            else
                player.Play();
            });
        ChangeViewCommand = ReactiveCommand.Create((string s) => {ChangeView(s);});
        EnterPlaylistCommand = ReactiveCommand.Create((int pid) => {EnterPlaylist(pid);});

        //bindings to view
        elapsedConverter = this.WhenAnyValue(x => x.Elapsed).ToProperty(this, x => x.ElapsedConverter); //manually implement the RaiseAndSetIfChanged

        //bindings from model
        this.WhenAnyValue(x => x.player.Time).Subscribe(x => Elapsed = x);

        //init ui
        ChangeView("Home");
        enable = true;
    }

    private void EnterPlaylist(int pid){
        ICommand NewPlaylistSong = ReactiveCommand.Create((int sid) => {LoadPlaylist(playlistList[pid-1]); LoadSong(songList[sid-1]);});
        TopViewModel = new PlaylistTopViewModel(playlistList[pid-1]);
        BottomViewModel = new PlaylistBottomViewModel(playlistList[pid-1], NewPlaylistSong, this);
    }
    private void ChangeView(string s){
        switch (s) {
            case "Home":
                ICommand NewSong = ReactiveCommand.Create((int s) => {playlist = null; LoadSong(songList[s-1]);});
                TopViewModel = new HomeTopViewModel(songTableau, NewSong);
                BottomViewModel = new HomeBottomViewModel(playlistTableau, EnterPlaylistCommand);
                break;
            case "Playlists":
                TopViewModel = new PlaylistsTopViewModel();
                BottomViewModel = new PlaylistsBottomViewModel(playlistList, EnterPlaylistCommand);
                break;              
        }
    }
    private void LoadSong(Song s){
        if (enable){ //not-blocking will cause mayhem in ui, but blocking may affect unsafe parent functions
            Enable = false;
            Elapsed = 0;
            player.Song = s;
            player.LoadNew(!playIsChecked);
            PlayerImage = player.Song.Image;
            Title = player.Song.Title;
            Artist = player.Song.Artist;
            Length = player.Song.Length;
            SongID = player.Song.SongID;
            Enable = true;
        }

    }

    private void LoadPlaylist(Playlist p){
        playlist = p;
        if (shuffle) {
            NewShuffleOrder();
        }
    }

    private void NewShuffleOrder(){
        if (playlist != null){ //suppress CS 8602
            //create shuffleOrder queue
            Queue<int> q = new(Enumerable.Range(0, playlist.Songs.Count).ToList().ToList().OrderBy(x => rand.Next()));
            //rotate so current song is head of list
            while (playlist.Songs[q.Peek()] != player.Song){
                q.Enqueue(q.Dequeue());
            }
            //cast to list
            shuffleOrder = [.. q];
        }
    }
    private void PopulateTableaus(){ //it is recommended to implement one's own sort function via selection sampling
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

    private void NextSong(object? o, EventArgs? e){
        if (playlist != null){
            bool flag = false;
            if (shuffle){
                if (playlist.Songs.FindIndex(a => a == player.Song) != shuffleOrder[^1] || Loop){
                    flag = true;
                }
                //magic!, tl;dr we go into a list, into another one, and then change something, and back out in that order
                LoadSong(playlist.Songs[shuffleOrder[(shuffleOrder.FindIndex(a => a == playlist.Songs.FindIndex(a => a == player.Song)) + 1)%shuffleOrder.Count]]);
                if (flag){
                    player.Play();
                }
            }
            else {
                if (player.Song != playlist.Songs[^1] || Loop){
                    flag = true;
                }
                LoadSong(playlist.Songs[(playlist.Songs.FindIndex(a => a == player.Song) + 1)%playlist.Songs.Count]);
                if (flag){
                    player.Play();
                }
            }
        }
        else {
            Enable = false;
            Elapsed = 0;
            if (loop){
                PlayIsChecked = false;
                player.Play();
            }
            else {PlayIsChecked = true;}
            Enable = true;
        }
    }

}
