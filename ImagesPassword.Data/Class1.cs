using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace ImagesPassword.Data
{
    public class Image
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string Password { get; set; }
        public int Views { get; set; }
    }

    public class ImageManager
    {
        private string _connectionString { get; set; }

        public ImageManager(string connectionString)
        {
            _connectionString = connectionString;
        }

        public int AddImage(Image image)
        {
            var conn = new SqlConnection(_connectionString);
            var cmd = conn.CreateCommand();
            cmd.CommandText = "INSERT INTO Images VALUES (@fileName, @password, @views) SELECT SCOPE_IDENTITY()";
            cmd.Parameters.AddWithValue("@fileName", image.FileName);
            cmd.Parameters.AddWithValue("@password", image.Password);
            cmd.Parameters.AddWithValue("@views", 0);
            conn.Open();
            int id = (int)(decimal)cmd.ExecuteScalar();
            conn.Close();
            conn.Dispose();
            return id;
        }

        public Image GetImage(int id)
        {
            var conn = new SqlConnection(_connectionString);
            var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM Images WHERE Id=@id";
            cmd.Parameters.AddWithValue("@id", id);
            conn.Open();
            var reader = cmd.ExecuteReader();
            Image image = new Image();

            if (reader.Read())
            {
                image.Id = (int)reader["Id"];
                image.FileName = (string)reader["FileName"];
                image.Password = (string)reader["Password"];
                image.Views = (int)reader["Views"];
            }
            conn.Close();
            conn.Dispose();
            return image;
        }

        public int IncrementCount(int id)
        {
            var conn = new SqlConnection(_connectionString);
            var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT Views FROM Images WHERE Id=@id";
            cmd.Parameters.AddWithValue("@id", id);
            conn.Open();
            int views = (int)cmd.ExecuteScalar();

            views++;
            cmd.Parameters.Clear();
            cmd.CommandText = "UPDATE Images SET Views=@views WHERE Id=3";
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@views", views);
            cmd.ExecuteNonQuery();
            conn.Close();
            conn.Dispose();
            return views;
        }
        
    }
}
