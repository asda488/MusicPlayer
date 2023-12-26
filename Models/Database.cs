using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using System.Configuration;
using System;
using System.Text;

namespace MusicPlayer.Models {
    class Database {
        public static void LoadData(){
            List<Song> SongList = new();
            List<Playlist> PlaylistList = new();
            string dbLocation = ConfigurationManager.AppSettings["resourceLocation"] + @"\Assets\music.db";
            using (var connection = new SqliteConnection($"Data Source={dbLocation};")){
                connection.Open();
                SqliteCommand command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM Songs";
                using (var d = command.ExecuteReader()){
                    while (d.Read()){
                        UTF8Encoding utf8 = new();
                        byte[] blob = utf8.GetBytes((string)d[4]);
                        SongList.Add(new Song((int)(long)d[0], (string)d[1], (int)(long)d[2], (string)d[3], blob, null, null));
                    }
                }
                command.CommandText = $"SELECT * FROM ArtistSong";
                using (var d = command.ExecuteReader()){
                    while (d.Read()){
                        SongList[(int)(long)d[1]-1].artists.Add((string)d[0]);
                    }
                }
                command.CommandText = $"SELECT * FROM AlbumSong";
                using (var d = command.ExecuteReader()){
                    while (d.Read()){
                        SongList[(int)(long)d[1]-1].albums.Add((string)d[0]);
                    }
                }
                command.CommandText = "SELECT * FROM Playlists";
                using (var d = command.ExecuteReader()){
                    while (d.Read()){
                        UTF8Encoding utf8 = new();
                        byte[]? blob = d[3] is DBNull ? null : utf8.GetBytes((string)d[3]);
                        PlaylistList.Add(new Playlist((int)(long)d[0], (string)d[1], (string)d[2], blob, null));
                    }
                }
                command.CommandText = $"SELECT * FROM PlaylistSong";
                using (var d = command.ExecuteReader()){
                    while (d.Read()){
                        PlaylistList[(int)(long)d[0]-1].playlistSongs.Add(SongList[(int)(long)d[1]-1]);
                    }
                }
            } //using block automatically handles connection object closure
        }
    }
}