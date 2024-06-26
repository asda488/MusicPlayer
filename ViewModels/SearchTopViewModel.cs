using System.Collections.Generic;
using System;
using MusicPlayer.Models;
using ReactiveUI;

namespace MusicPlayer.ViewModels;

public class SearchTopViewModel : ViewModelBase
{
    private readonly List<SearchItem> allSearchResults;
    public bool SongsChecked { get => songsChecked; set => this.RaiseAndSetIfChanged(ref songsChecked, value); } //exposes Songs checkbox
    public bool PlaylistsChecked { get => playlistsChecked; set => this.RaiseAndSetIfChanged(ref playlistsChecked, value); } //exposes Playlists checkbox
    public int CurrentSortIndex { get => currentSortIndex; set => this.RaiseAndSetIfChanged(ref currentSortIndex, value); } //exposes Sort combo box
    private bool songsChecked = true;
    private bool playlistsChecked = true;
    private int currentSortIndex = 0;

    public SearchTopViewModel(List<SearchItem> items, string search, SearchBottomViewModel context){
        //perform search and initial sort
        allSearchResults = SearchFor.SearchForString(items, search);
        context.SearchResults = Sort.BubbleSortItems(allSearchResults, currentSortIndex); //this will fire the property to update the bottom view model

        //listen to changes in search criteria and push to bottom view model
        this.WhenAnyValue(x => x.SongsChecked).Subscribe(x => context.SearchResults = UpdateSearch());
        this.WhenAnyValue(x => x.PlaylistsChecked).Subscribe(x => context.SearchResults = UpdateSearch());
        this.WhenAnyValue(x => x.CurrentSortIndex).Subscribe(x => context.SearchResults = UpdateSearch());
    }

    private List<SearchItem> UpdateSearch(){ //updates all search results and returns
        List<SearchItem> updatedSearchResults = [];
        if (songsChecked){
            updatedSearchResults.AddRange(SearchFor.SearchForType(allSearchResults, typeof(Song)));
        }
        if (playlistsChecked) {
            updatedSearchResults.AddRange(SearchFor.SearchForType(allSearchResults, typeof(Playlist)));
        }
        updatedSearchResults = Sort.BubbleSortItems(updatedSearchResults, currentSortIndex);
        return updatedSearchResults;
    }
}