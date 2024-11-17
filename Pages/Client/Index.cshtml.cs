using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Story.Pages.Client
{
    public class IndexModel : PageModel
    {
        public List<ClientInfo> ListClients { get; set; } = new List<ClientInfo>();

        public void OnGet()
        {
            try
            {
                // Database connection string
                string connectionString = "Data Source=.;Initial Catalog=mystore;Integrated Security=True;Trust Server Certificate=True;";

                // Connect to the database
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sqlQuery = "SELECT id, name, email, phone, address, created_at FROM clients";

                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                // Populate the ListClients with data from the database
                                ClientInfo client = new ClientInfo
                                {
                                    Id = reader.GetInt32(0),
                                    Name = reader.GetString(1),
                                    Email = reader.GetString(2),
                                    Phone = reader.IsDBNull(3) ? null : reader.GetString(3),
                                    Address = reader.IsDBNull(4) ? null : reader.GetString(4),
                                    CreatedAt = reader.GetDateTime(5)
                                };

                                ListClients.Add(client);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log or handle exceptions
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }

    // ClientInfo model with properties
    public class ClientInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public DateTime CreatedAt { get; set; } // Added CreatedAt field for display
    }
}

