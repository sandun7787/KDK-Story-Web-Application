using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Story.Pages.Client
{
    public class CreateModel : PageModel
    {

        public ClientInfo clientInfo = new ClientInfo();
        private string errorMessage;

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
        }

    }
}
