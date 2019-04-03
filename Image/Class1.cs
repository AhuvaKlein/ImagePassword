using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace Image
{
    public class Image
    {
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

        public void AddImage(Image image)
        {
            var conn = new SqlConnection(_connectionString);
            var cmd = conn.CreateCommand();
            cmd.CommandText = "INSERT INTO Images VALUES (@fileName, @password)";
            cmd.Parameters.AddWithValue("@fileName", image.FileName);
            cmd.Parameters.AddWithValue("@password", image.Password);
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
            conn.Dispose();
        }

        public Image GetImage(int id)
        {
            var conn = new SqlConnection(_connectionString);
            var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM Images WHERE Id=@id";
            cmd.Parameters.AddWithValue("@id", id);
            var reader = cmd.ExecuteReader();
            Image image = new Image();

            if(reader.Read())
            {
                image.FileName = (string)reader["FileName"];
                image.Password = (string)reader["Password"];
                image.Views = (int)reader["Views"];
            }
            conn.Close();
            conn.Dispose();
            return image;
        }

       
    }
}
