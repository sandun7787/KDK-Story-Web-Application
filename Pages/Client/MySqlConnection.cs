
namespace Story.Pages.Client
{
    internal class MySqlConnection
    {
        private string connectionString;

        public MySqlConnection(string connectionString)
        {
            this.connectionString = connectionString;
        }

        internal void Open()
        {
            throw new NotImplementedException();
        }
    }
}