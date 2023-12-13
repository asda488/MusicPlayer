using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Microsoft.Data.Sqlite;
using System.Configuration;

namespace MusicPlayer.Models {
    class Database {
        public static void LoadData(){
            List<int> SongId;
            List<Song> SongList;
            List<Playlist> PlaylistList;
            string dbLocation = ConfigurationManager.AppSettings["resourceLocation"] + @"music.db";
            using (var connection = new SqliteConnection($"Data Source={dbLocation};")){
                connection.Open();
                SqliteCommand command = connection.CreateCommand();
                command.CommandText = @"SELECT COUNT(*) FROM Songs";
                SqliteDataReader dataReader = command.ExecuteReader();
                while (dataReader.Read()){
                    Console.WriteLine(dataReader);
                }

            }

        }
    }
}