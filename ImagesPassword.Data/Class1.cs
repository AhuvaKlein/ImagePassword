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
        public int UserId { get; set; }
    }

    public class User
    {
        public User()
        {
            Images = new List<Image>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public List<Image> Images { get; set; }
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
            cmd.CommandText = "INSERT INTO Images (fileName, password, views, userId) VALUES (@fileName, @password, @views, @userId) SELECT SCOPE_IDENTITY()";
            cmd.Parameters.AddWithValue("@fileName", image.FileName);
            cmd.Parameters.AddWithValue("@password", image.Password);
            cmd.Parameters.AddWithValue("@views", 0);
            cmd.Parameters.AddWithValue("@userId", image.UserId);
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
            conn.Open();
            cmd.CommandText = "UPDATE Images SET Views=views + 1 WHERE Id= @id SELECT Views FROM Images WHERE Id = @id";
            cmd.Parameters.AddWithValue("@id", id);
            int views = (int)cmd.ExecuteScalar();
            conn.Close();
            conn.Dispose();
            return views;
        }

        public void SignUp(User user, string password)
        {
            SqlConnection conn = new SqlConnection(_connectionString);
            SqlCommand cmd = conn.CreateCommand();
            string hash = BCrypt.Net.BCrypt.HashPassword(password);
            cmd.CommandText = "INSERT INTO Users(name, email, passwordhash) VALUES (@name, @email, @password)";
            cmd.Parameters.AddWithValue("@name", user.Name);
            cmd.Parameters.AddWithValue("@email", user.Email);
            cmd.Parameters.AddWithValue("@password", hash);
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
            conn.Dispose();

        }

        public User Login(User user, string password)
        {
            User u = GetUserByEmail(user.Email);
            if (u == null)
            {
                return null;
            }

            bool IsUser = BCrypt.Net.BCrypt.Verify(password, u.PasswordHash);
            if (!IsUser)
            {
                return null;
            }
            return u;
        }

        public User GetUserByEmail(string email)
        {
            var conn = new SqlConnection(_connectionString);
            var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM Users WHERE Email=@email";
            cmd.Parameters.AddWithValue("@email", email);
            conn.Open();
            var reader = cmd.ExecuteReader();
            User u = new User();
            if (reader.Read())
            {
                u.Id = (int)reader["Id"];
                u.Name = (string)reader["Name"];
                u.Email = (string)reader["Email"];
                u.PasswordHash = (string)reader["PasswordHash"];
            }

            conn.Close();
            conn.Dispose();
            return u;
        }

        public IEnumerable<Image> GetImageForUser(int userId)
        {
            var conn = new SqlConnection(_connectionString);
            var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM Images WHERE UserId=@id";
            cmd.Parameters.AddWithValue("@id", userId);
            conn.Open();
            var reader = cmd.ExecuteReader();
            List<Image> images = new List<Image>();

            while (reader.Read())
            {
                images.Add(new Image
                {
                    Id = (int)reader["Id"],
                    FileName = (string)reader["FileName"],
                    Password = (string)reader["Password"],
                    Views = (int)reader["Views"],
                });

            }
            conn.Close();
            conn.Dispose();
            return images;
        }

    }
}
