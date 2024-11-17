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
                // Connection string to the database
                string connectionString = "Data Source=.;Initial Catalog=mystore;Integrated Security=True;Trust Server Certificate=True;User Instance=False";

                // Using block to ensure the connection is properly closed
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sqlQuery = "SELECT id, name, email, phone, address FROM clients";

                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ClientInfo client = new ClientInfo
                                {
                                    Id = reader.GetInt32(0),
                                    Name = reader.GetString(1),
                                    Email = reader.GetString(2),
                                    Phone = reader.IsDBNull(3) ? null : reader.GetString(3),
                                    Address = reader.IsDBNull(4) ? null : reader.GetString(4)
                                };

                                ListClients.Add(client);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it accordingly
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }

    // ClientInfo class with properties
    public class ClientInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
    }
}

