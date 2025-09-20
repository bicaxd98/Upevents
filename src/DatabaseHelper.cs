using Microsoft.Data.Sqlite;
using System;

namespace Gopro360App
{
    public static class DatabaseHelper
    {
        private static string DbPath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "UPEvents360.db");

        public static void InitDatabase()
        {
            using var conn = new SqliteConnection($"Data Source={DbPath}");
            conn.Open();
            var cmd = conn.CreateCommand();
            cmd.CommandText = @"CREATE TABLE IF NOT EXISTS recordings (
  id INTEGER PRIMARY KEY AUTOINCREMENT,
  filename TEXT NOT NULL,
  filepath TEXT NOT NULL,
  created_at TEXT NOT NULL
);".Replace("\r\n","\n");
            cmd.ExecuteNonQuery();
        }
    }
}
