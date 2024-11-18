using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;

namespace Story.Pages.Client
{
    public class EditModel : PageModel
    {
        public ClientInfo ClientInfo { get; set; } = new ClientInfo();
        public string ErrorMessage { get; set; }
        public string SuccessMessage { get; set; }

        public void OnGet(int id)
        {
            try
            {
                // Database connection string
                string connectionString = "Server=localhost;Database=mystore;User ID=root;Password=yourpassword;Port=3306;";

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string sqlQuery = "SELECT id, name, email, phone, address FROM clients WHERE id = @id";

                    using (MySqlCommand command = new MySqlCommand(sqlQuery, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                ClientInfo.Id = reader.GetInt32("id");
                                ClientInfo.Name = reader.GetString("name");
                                ClientInfo.Email = reader.GetString("email");
                                ClientInfo.Phone = reader.IsDBNull(reader.GetOrdinal("phone")) ? null : reader.GetString("phone");
                                ClientInfo.Address = reader.IsDBNull(reader.GetOrdinal("address")) ? null : reader.GetString("address");
                            }
                            else
                            {
                                ErrorMessage = "Client not found.";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error: {ex.Message}";
            }
        }

        public void OnPost()
        {
            ClientInfo.Id = Convert.ToInt32(Request.Form["id"]);
            ClientInfo.Name = Request.Form["name"];
            ClientInfo.Email = Request.Form["email"];
            ClientInfo.Phone = Request.Form["phone"];
            ClientInfo.Address = Request.Form["address"];

            if (string.IsNullOrWhiteSpace(ClientInfo.Name) ||
                string.IsNullOrWhiteSpace(ClientInfo.Email) ||
                string.IsNullOrWhiteSpace(ClientInfo.Phone) ||
                string.IsNullOrWhiteSpace(ClientInfo.Address))
            {
                ErrorMessage = "All fields are required.";
                return;
            }

            try
            {
                // Database connection string
                string connectionString = "Server=localhost;Database=mystore;User ID=root;Password=yourpassword;Port=3306;";

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string sqlQuery = "UPDATE clients SET name = @name, email = @email, phone = @phone, address = @address WHERE id = @id";

                    using (MySqlCommand command = new MySqlCommand(sqlQuery, connection))
                    {
                        command.Parameters.AddWithValue("@id", ClientInfo.Id);
                        command.Parameters.AddWithValue("@name", ClientInfo.Name);
                        command.Parameters.AddWithValue("@email", ClientInfo.Email);
                        command.Parameters.AddWithValue("@phone", ClientInfo.Phone);
                        command.Parameters.AddWithValue("@address", ClientInfo.Address);

                        command.ExecuteNonQuery();
                    }
                }

                SuccessMessage = "Client updated successfully!";
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error: {ex.Message}";
            }
        }
    }

    public class ClientInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
    }
}

