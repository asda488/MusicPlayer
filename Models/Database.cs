using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using Avalonia.Media.Imaging;
using System;
using System.IO;
using Avalonia.Platform;

namespace MusicPlayer.Models {
    class Database {
        public static (List<Song>, List<Playlist>) LoadData(){ //loads data from database and returns, fulfills F1
            //create initial lists, fulfills F2(a)
            List<Song> SongList = [];
            List<Playlist> PlaylistList = [];
            //copy database from assembly to openable location
            var _currentPath = Path.GetDirectoryName(AppContext.BaseDirectory) ?? "";
            using (Stream db = AssetLoader.Open(new Uri ("avares://MusicPlayer/Assets/music.db"))){
                using var f = new FileStream(Path.Combine(_currentPath, "music.db"), FileMode.Create, FileAccess.Write);
                db.CopyTo(f);
            }
            //open database connection with using block to handle closure
            using (var connection = new SqliteConnection($"Data Source={Path.Combine(_currentPath, "music.db")};")){
                connection.Open();
                SqliteCommand command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM Songs";
                //execute command "SELECT * FROM Songs"
                using (var d = command.ExecuteReader()){
                    //read results into SongList
                    while (d.Read()){
                        Bitmap image;
                        if (d[4] is DBNull){
                            //if no image, load default
                            image = new(AssetLoader.Open(new Uri("avares://MusicPlayer/Assets/default-image.png")));
                        }
                        else {
                            using var ms = new MemoryStream((byte[])d[4]);
                            image = new Bitmap(ms);
                        }
                        SongList.Add(new Song((int)(long)d[0], (string)d[1], (int)(long)d[2], (string)d[3], image, null, null));
                    }
                }
                command.CommandText = $"SELECT * FROM ArtistSong";
                //execute command "SELECT * FROM ArtistSong"
                using (var d = command.ExecuteReader()){
                    //edit results into SongList
                    while (d.Read()){
                        SongList[(int)(long)d[1]-1].Artists.Add((string)d[0]);
                    }
                }
                command.CommandText = $"SELECT * FROM AlbumSong";
                //execute command "SELECT * FROM AlbumSong"
                using (var d = command.ExecuteReader()){
                    //edit results into SongList
                    while (d.Read()){
                        SongList[(int)(long)d[1]-1].Albums.Add((string)d[0]);
                    }
                }
                command.CommandText = "SELECT * FROM Playlists";
                //execute command "SELECT * FROM Playlists"
                using (var d = command.ExecuteReader()){
                    //add results into PlaylistList
                    while (d.Read()){
                        Bitmap? image;
                        if (d[3] is DBNull){
                            //if no image, load default
                            image = new(AssetLoader.Open(new Uri("avares://MusicPlayer/Assets/default-image.png")));
                        }
                        else {
                            using var ms = new MemoryStream((byte[])d[3]);
                            image = new Bitmap(ms);
                        }
                        PlaylistList.Add(new Playlist((int)(long)d[0], (string)d[1], (string)d[2], image, null));
                    }
                }
                command.CommandText = $"SELECT * FROM PlaylistSong";
                //execute command "SELECT * FROM PlaylistSong"
                using (var d = command.ExecuteReader()){
                    //edit results into PlaylistList
                    while (d.Read()){
                        PlaylistList[(int)(long)d[0]-1].Songs.Add(SongList[(int)(long)d[1]-1]);
                    }
                }
            } //using block automatically handles connection object closure
            return (SongList, PlaylistList);
        }
    }
}