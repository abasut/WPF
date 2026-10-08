using System.Data;
using Microsoft.Data.SqlClient;
using WpfCrudApp.Models;

namespace WpfCrudApp.Services
{
    public class PersonService
    {
        // Измените строку подключения под ваш SQLExpress сервер при необходимости
        // Варианты:
        //   Server=.\\SQLEXPRESS;Database=WpfCrudDb;Trusted_Connection=True;TrustServerCertificate=True;
        //   Server=localhost\\SQLEXPRESS;Database=WpfCrudDb;Trusted_Connection=True;TrustServerCertificate=True;
        //   Server=(localdb)\\MSSQLLocalDB;Database=WpfCrudDb;Trusted_Connection=True;TrustServerCertificate=True;
        private readonly string _connectionString =
            @"Server=.\SQLEXPRESS;Database=WpfCrudDb;Trusted_Connection=True;TrustServerCertificate=True;";

        public List<Person> GetAll()
        {
            var list = new List<Person>();

            using var connection = new SqlConnection(_connectionString);
            connection.Open();

            using var command = new SqlCommand(
                "SELECT Id, FullName, Age, Email, Phone, CreatedAt FROM dbo.People ORDER BY Id",
                connection);

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new Person
                {
                    Id = reader.GetInt32(0),
                    FullName = reader.GetString(1),
                    Age = reader.IsDBNull(2) ? null : reader.GetInt32(2),
                    Email = reader.IsDBNull(3) ? null : reader.GetString(3),
                    Phone = reader.IsDBNull(4) ? null : reader.GetString(4),
                    CreatedAt = reader.GetDateTime(5)
                });
            }

            return list;
        }

        public int Insert(Person person)
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();

            using var command = new SqlCommand(
                @"INSERT INTO dbo.People (FullName, Age, Email, Phone)
                  OUTPUT INSERTED.Id
                  VALUES (@FullName, @Age, @Email, @Phone)",
                connection);

            command.Parameters.AddWithValue("@FullName", person.FullName);
            command.Parameters.AddWithValue("@Age", (object?)person.Age ?? DBNull.Value);
            command.Parameters.AddWithValue("@Email", (object?)person.Email ?? DBNull.Value);
            command.Parameters.AddWithValue("@Phone", (object?)person.Phone ?? DBNull.Value);

            return (int)command.ExecuteScalar()!;
        }

        public void Update(Person person)
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();

            using var command = new SqlCommand(
                @"UPDATE dbo.People
                  SET FullName = @FullName,
                      Age = @Age,
                      Email = @Email,
                      Phone = @Phone
                  WHERE Id = @Id",
                connection);

            command.Parameters.AddWithValue("@Id", person.Id);
            command.Parameters.AddWithValue("@FullName", person.FullName);
            command.Parameters.AddWithValue("@Age", (object?)person.Age ?? DBNull.Value);
            command.Parameters.AddWithValue("@Email", (object?)person.Email ?? DBNull.Value);
            command.Parameters.AddWithValue("@Phone", (object?)person.Phone ?? DBNull.Value);

            command.ExecuteNonQuery();
        }

        public void Delete(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            connection.Open();

            using var command = new SqlCommand(
                "DELETE FROM dbo.People WHERE Id = @Id",
                connection);

            command.Parameters.AddWithValue("@Id", id);
            command.ExecuteNonQuery();
        }
    }
}
