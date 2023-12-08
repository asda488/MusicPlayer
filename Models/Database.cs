using System.Collections.Generic;
using Microsoft.Data.Sqlite;

namespace MusicPlayer.Models {
    class Database {
        public (List<Song>, List<Playlist>) LoadData(){
            using (SqliteConnection connection = new SqliteConnection("Data Source=Assets/music.db")){
                connection.Open();
                SqliteCommand command = connection.CreateCommand();
                command.CommandText = @"SELECT * FROM Songs";
            }

        }
    }
}