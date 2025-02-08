using Npgsql;

namespace ApiBD.Infractructure
{
    /// <summary>
    /// Класс для написания SQL запросов 
    /// </summary>
    public class PgSqlConnection 
    {
        protected NpgsqlConnection _npgsqlConnection;
        protected NpgsqlCommand _npgsqlCommand = new NpgsqlCommand();
        public PgSqlConnection(string connectString) 
        {
            try
            {
                Console.WriteLine($"{this.ToString}: Try to connect to Base..");
                _npgsqlConnection = new NpgsqlConnection(connectString);
                _npgsqlCommand.Connection = _npgsqlConnection;
                Console.WriteLine($"{this.ToString}: Connection establish..");
            }
            catch (Exception ex)
            { 
                Console.WriteLine($"{this.ToString}: Message = {ex.Message}; StackTrace = {ex.StackTrace}");
            }
            
        }
        /// <summary>
        /// Класс для исполнения команд
        /// </summary>
        /// <returns></returns>
        public virtual async Task ExecAsync(string command)
        {

        }

        public virtual async Task GetResult()
        {

        }

    }
}
