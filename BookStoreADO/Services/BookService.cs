using BookStoreADO.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace BookStoreADO.Services
{
    public class BookService
    {
        private readonly string _connectionString;

        public BookService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        // SqlDataReader: connected architecture
        public List<Book> GetAllBooks()
        {
            List<Book> books = new List<Book>();

            using SqlConnection con = new SqlConnection(_connectionString);

            string query = "SELECT * FROM Books";

            using SqlCommand cmd = new SqlCommand(query, con);

            con.Open();

            using SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                books.Add(new Book
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    Title = reader["Title"].ToString(),
                    Author = reader["Author"].ToString(),
                    Price = Convert.ToDecimal(reader["Price"])
                });
            }

            return books;
        }

        // Stored Procedure + parameterized values
        public void AddBook(Book book)
        {
            using SqlConnection con = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand("AddBook", con);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Title", book.Title);
            cmd.Parameters.AddWithValue("@Author", book.Author);
            cmd.Parameters.AddWithValue("@Price", book.Price);

            con.Open();
            cmd.ExecuteNonQuery();
        }

        public void UpdateBook(Book book)
        {
            using SqlConnection con = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand("UpdateBook", con);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Id", book.Id);
            cmd.Parameters.AddWithValue("@Title", book.Title);
            cmd.Parameters.AddWithValue("@Author", book.Author);
            cmd.Parameters.AddWithValue("@Price", book.Price);

            con.Open();
            cmd.ExecuteNonQuery();
        }

        public void DeleteBook(int id)
        {
            using SqlConnection con = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand("DeleteBook", con);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Id", id);

            con.Open();
            cmd.ExecuteNonQuery();
        }

        // SqlDataAdapter + DataSet: disconnected architecture
        public DataSet GetBooksDataSet()
        {
            using SqlConnection con = new SqlConnection(_connectionString);

            string query = "SELECT * FROM Books";

            using SqlDataAdapter adapter = new SqlDataAdapter(query, con);

            DataSet ds = new DataSet();

            adapter.Fill(ds, "Books");

            return ds;
        }
    }
}