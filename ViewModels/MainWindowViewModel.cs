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
    //objects belonging to this ViewModel, composition is used here over inheritance 
    private readonly Player player;

    //private variables for internal use
    private readonly List<Song> songTableau = [];
    private readonly List<Playlist> playlistTableau = [];
    private readonly List<Song> songList;
    private readonly List<Playlist> playlistList;
    private readonly List<SearchItem> allSearchItems;
    private readonly Random rand = new(); //it is advised to generate a singular random object during the application lifespan and reuse it
    private List<int> shuffleOrder;

    //command bindings to be passed to sub-viewmodels that let them interact with the player bar and media player.
    public ICommand TogglePlayPause { get; }
    public ICommand ChangeViewCommand { get; }
    public ICommand ShuffleCommand { get; }
    public ICommand EnterPlaylistCommand { get; }
    public ICommand NewSong { get; }
    public ICommand EnterKeyDown { get; }

    //private versions of exposed variables, storing their value, for the ViewModels to operate on...
    private Bitmap playerImage;
    private string title;
    private string artist;
    private int length;
    private int songID;
    private bool playIsChecked = true;
    private ObservableAsPropertyHelper<int> elapsedConverter;
    private int elapsed;
    private bool enable = false;
    private bool shuffle = false;
    private bool loop = false;
    private ViewModelBase topViewModel;
    private ViewModelBase bottomViewModel;
    private Playlist? playlist;
    private bool searchIsFocused;
    private string searchValue;

    //... and public versions of (references to) these variables, for the Views to bind to and update, implementing ReactiveUI's very helpful RaiseAndSetIfChanged
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
    public bool SearchIsFocused {get => searchIsFocused; set => this.RaiseAndSetIfChanged(ref searchIsFocused, value);}
    public string SearchValue {get => searchValue; set => this.RaiseAndSetIfChanged(ref searchValue, value);}


    public MainWindowViewModel(){
        //startup
        (songList, playlistList) = Database.LoadData();
        PopulateTableaus();
        //generate list of all items for searching
        allSearchItems = [.. from s in songList select new SearchItem(s)];
        allSearchItems.AddRange([.. from p in playlistList select new SearchItem(p)]);
        //add playlist of all media
        playlistList.Add(new Playlist(playlistList.Count+1, "All Media", "All tracks in your library.", new(AssetLoader.Open(new Uri("avares://MusicPlayer/Assets/default-image.png"))), [.. songList]));
        //initialise player
        player = new(0, null);
        player.PlayerFinished += NextSong;

        //ReactiveUI (MVVM) bindings
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
        EnterKeyDown = ReactiveCommand.Create(() =>{
            if (SearchIsFocused && SearchValue.Length >= 3) { //if the query is too short, cannot return too much lest the application crashes (for larger databases)
                EnterSearch(SearchValue);
            }
        });
        NewSong = ReactiveCommand.Create((int s) => {playlist = null; LoadSong(songList[s-1]);});
        ChangeViewCommand = ReactiveCommand.Create((string s) => {ChangeView(s);});
        EnterPlaylistCommand = ReactiveCommand.Create((int pid) => {EnterPlaylist(pid);});

        //bindings to view
        elapsedConverter = this.WhenAnyValue(x => x.Elapsed).ToProperty(this, x => x.ElapsedConverter); //manually implement the RaiseAndSetIfChanged

        //bindings from model
        this.WhenAnyValue(x => x.player.Time).Subscribe(x => Elapsed = x);

        //start UI
        ChangeView("Home");
    }

    private void EnterPlaylist(int pid){ //switches sub view to page of a playlist with PlaylistID = pid
        ICommand newPlaylistSong = ReactiveCommand.Create((int sid) => {LoadSong(songList[sid-1]); LoadPlaylist(playlistList[pid-1]);});
        TopViewModel = new PlaylistTopViewModel(playlistList[pid-1]);
        BottomViewModel = new PlaylistBottomViewModel(playlistList[pid-1], newPlaylistSong, this);
    }

    private void EnterSearch(string search){ //switches sub view to search results for Query = search
        var _ = new SearchBottomViewModel(EnterPlaylistCommand, NewSong, this);
        BottomViewModel = _;
        TopViewModel = new SearchTopViewModel(allSearchItems, search, _);

    }
    private void ChangeView(string s){ //handles change in view for navigation buttons
        switch (s) {
            case "Home":
                TopViewModel = new HomeTopViewModel(songTableau, NewSong);
                BottomViewModel = new HomeBottomViewModel(playlistTableau, EnterPlaylistCommand);
                break;
            case "Playlists":
                TopViewModel = new PlaylistsTopViewModel();
                BottomViewModel = new PlaylistsBottomViewModel(playlistList, EnterPlaylistCommand);
                break;              
        }
    }
    private void LoadSong(Song s){ //loads a song in the player
        if (enable || player.Song == null){ //only run if not loading, or if there is no song loaded
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

    private void LoadPlaylist(Playlist p){ //loads a playlist and shuffles it if needs be 
        playlist = p;
        if (shuffle) {
            NewShuffleOrder();
        }
    }

    private void NewShuffleOrder(){ //generates a new shuffleOrder
        if (playlist != null){ //suppress CS 8602
            //create queue of indexes of songs in the playlist
            Queue<int> q = new(Enumerable.Range(0, playlist.Songs.Count).ToList().OrderBy(x => rand.Next()));
            //rotate so current song is head of list
            while (playlist.Songs[q.Peek()] != player.Song){
                q.Enqueue(q.Dequeue());
            }
            //cast to list
            shuffleOrder = [.. q];
        }
    }
    private void PopulateTableaus(){ //it is recommended to implement one's own sort function via selection/resevoir sampling
        int i = 0;
        foreach (Song s in songList){ //for each item in all possible songs
            if ((((double)4-songTableau.Count)/(songList.Count-(double)i)) >= rand.NextDouble()){ //pick it with the probability (number of songs still needed, out of 4)/(number of songs left in the list of all songs)
                songTableau.Add(s);
            }
            if (4-songTableau.Count == 0){ //if we've got everything, stop
                break;
            }
            i += 1;
        }
        i = 0;
        foreach (Playlist s in playlistList){
            if ((((double)2-playlistTableau.Count)/(playlistList.Count-(double)i)) >= rand.NextDouble()){ //pick it with the probability (number of playlists still needed, out of 2)/(number of playlists left in the list of all playlists)
                playlistTableau.Add(s);
            }
            if (2-playlistTableau.Count == 0){ //if we've got everything, stop
                break;
            }
            i += 1;
        } 
    } 

    private void NextSong(object? o, EventArgs? e){ //handles the PlayerFinished event from player
        if (playlist != null){ //if there is a playlist
            bool flag = false;
            if (shuffle){ //if there is a shuffle, load the next song according to the shuffle order, and if you hit the end, stop/loop
                if (playlist.Songs.FindIndex(a => a == player.Song) != shuffleOrder[^1] || Loop){
                    flag = true;
                }
                LoadSong(playlist.Songs[shuffleOrder[(shuffleOrder.FindIndex(a => a == playlist.Songs.FindIndex(a => a == player.Song)) + 1)%shuffleOrder.Count]]);
                if (flag){
                    player.Play();
                }
            }
            else { //else load the next song according to playlist order (if you hit the end, stop/loop)
                if (player.Song != playlist.Songs[^1] || Loop){
                    flag = true;
                }
                LoadSong(playlist.Songs[(playlist.Songs.FindIndex(a => a == player.Song) + 1)%playlist.Songs.Count]);
                if (flag){
                    player.Play();
                }
            }
        }
        else { //else reset the song, possibly loop
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
