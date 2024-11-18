using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;

namespace Story.Pages.Client
{
    public class CreateModel : PageModel
    {

        public ClientInfo clientInfo = new ClientInfo();
        private string errorMessage;
        public string SuccessMessage;

        public ClientInfo ClientInfo { get; private set; }

        public void OnGet()
        {
        }

        public void OnPost()
        {
            clientInfo.Name = Request.Form["name"];
            clientInfo.Email = Request.Form["email"];
            clientInfo.Phone = Request.Form["phone"];
            clientInfo.Address = Request.Form["address"];

            if (string.IsNullOrWhiteSpace(clientInfo.Name) ||
                string.IsNullOrWhiteSpace(clientInfo.Email) ||
                string.IsNullOrWhiteSpace(clientInfo.Phone) ||
                string.IsNullOrWhiteSpace(clientInfo.Address))
            {
                errorMessage = "All the fields are required.";
                return;
            }

            // Database connection string (update credentials as needed)
            string connectionString = "Server=localhost;Database=mystore;User ID=root;Password=yourpassword;Port=3306;";

            // Insert data into the database
            using (MySqlConnection connection = new(connectionString))
            {
                connection.Open();
                string sqlQuery = "INSERT INTO clients (name, email, phone, address) VALUES (@name, @email, @phone, @address)";
                using (MySqlCommand command = new MySqlCommand(sqlQuery, connection))
                {
                    command.Parameters.AddWithValue("@name", ClientInfo.Name);
                    command.Parameters.AddWithValue("@email", ClientInfo.Email);
                    command.Parameters.AddWithValue("@phone", ClientInfo.Phone);
                    command.Parameters.AddWithValue("@address", ClientInfo.Address);

                    command.ExecuteNonQuery();
                }
            }

            // Set success message and clear client info
            SuccessMessage = "New client created successfully!";
            ClientInfo = new ClientInfo();
        }

    }
}
