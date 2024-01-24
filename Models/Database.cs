using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using Avalonia.Media.Imaging;
using System;
using System.IO;
using Avalonia.Platform;

namespace MusicPlayer.Models {
    class Database {
        public static (List<Song>, List<Playlist>) LoadData(){
            List<Song> SongList = new();
            List<Playlist> PlaylistList = new();
            using (Stream db = AssetLoader.Open(new Uri ("avares://MusicPlayer/Assets/music.db"))){
                using (FileStream f = new FileStream(Path.Combine(Path.GetDirectoryName(AppContext.BaseDirectory), "music.db"), FileMode.Create, FileAccess.Write)){
                    db.CopyTo(f);
                }
            }
            using (var connection = new SqliteConnection($"Data Source={Path.Combine(Path.GetDirectoryName(AppContext.BaseDirectory), "music.db")};")){
                connection.Open();
                SqliteCommand command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM Songs";
                using (var d = command.ExecuteReader()){
                    while (d.Read()){
                        Bitmap image;
                        if (d[4] is DBNull){
                            image = new(AssetLoader.Open(new Uri("avares://MusicPlayer/Assets/default-image.png")));
                        }
                        else {
                            using (var ms = new MemoryStream((byte[])d[4])){
                                image = new Bitmap(ms);
                            }
                        }
                        SongList.Add(new Song((int)(long)d[0], (string)d[1], (int)(long)d[2], (string)d[3], image, null, null));
                    }
                }
                command.CommandText = $"SELECT * FROM ArtistSong";
                using (var d = command.ExecuteReader()){
                    while (d.Read()){
                        SongList[(int)(long)d[1]-1].Artists.Add((string)d[0]);
                    }
                }
                command.CommandText = $"SELECT * FROM AlbumSong";
                using (var d = command.ExecuteReader()){
                    while (d.Read()){
                        SongList[(int)(long)d[1]-1].Albums.Add((string)d[0]);
                    }
                }
                command.CommandText = "SELECT * FROM Playlists";
                using (var d = command.ExecuteReader()){
                    while (d.Read()){
                        Bitmap? image;
                        if (d[3] is DBNull){
                            image = new(AssetLoader.Open(new Uri("avares://MusicPlayer/Assets/default-image.png")));
                        }
                        else {
                            using (var ms = new MemoryStream((byte[])d[3])){
                                image = new Bitmap(ms);
                            }
                        }
                        PlaylistList.Add(new Playlist((int)(long)d[0], (string)d[1], (string)d[2], image, null));
                    }
                }
                command.CommandText = $"SELECT * FROM PlaylistSong";
                using (var d = command.ExecuteReader()){
                    while (d.Read()){
                        PlaylistList[(int)(long)d[0]-1].Songs.Add(SongList[(int)(long)d[1]-1]);
                    }
                }
            } //using block automatically handles connection object closure
            return (SongList, PlaylistList);
        }
    }
}