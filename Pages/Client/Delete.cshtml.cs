using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;

namespace Story.Pages.Client
{
    public class DeleteModel : PageModel
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

        public IActionResult OnPost()
        {
            int id = Convert.ToInt32(Request.Form["id"]);

            try
            {
                // Database connection string
                string connectionString = "Server=localhost;Database=mystore;User ID=root;Password=yourpassword;Port=3306;";

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string sqlQuery = "DELETE FROM clients WHERE id = @id";

                    using (MySqlCommand command = new MySqlCommand(sqlQuery, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        command.ExecuteNonQuery();
                    }
                }

                SuccessMessage = "Client deleted successfully!";
                return RedirectToPage("/Client/Index");
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error: {ex.Message}";
                return Page();
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
