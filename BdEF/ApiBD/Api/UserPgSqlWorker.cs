using ApiBD.Infractructure;
using ApiBD.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiBD.Api
{
    public class UserPgSqlWorker:PgSqlConnection
    {
        public int GetOrdersPrice = 0;
        public List<Order> GetOrdersList = new List<Order>();
        string _name;
        public UserPgSqlWorker(string connectString, string userName) : base(connectString)
        {
            _name = userName;
        }

        public override async Task ExecAsync( string command)
        {
            try
            {
                _npgsqlConnection.Open();
                Console.WriteLine($"{this.ToString}: Connection.Open");
                switch (command)
                {
                    case "GetOrdersPriceAsync":
                        Console.WriteLine($"{this.ToString}: Try to exec GetOrdersPriceAsync");
                        await GetOrdersPriceAsync();
                        Console.WriteLine($"{this.ToString}: Exec have done.");
                        break;

                    case "GetOrdersList":
                        Console.WriteLine($"{this.ToString}: Try to exec GetOrdersPriceAsync");
                        await GetOrdersListAsync();
                        Console.WriteLine($"{this.ToString}: Exec have done.");
                        break;

                    default:
                        Console.WriteLine($"{this.ToString}: Uncorrect command.");
                        break;
                }
            }catch (Exception ex)
            {
                Console.WriteLine($"{this.ToString()}: Message = {ex.Message}; Trc = {ex.StackTrace}");
            }finally
            {
                _npgsqlConnection?.Close();
                Console.WriteLine($"{this.ToString}: Connection.Close");
            }
        }

        private async Task GetOrdersPriceAsync()
        {
            try
            {
                Console.WriteLine($"{this.ToString}: GetOrdersPriceAsync... ");
                _npgsqlCommand.CommandText = "select SUM (od.\"TotalCost\") from \"Orders\" o LEFT JOIN \"Users\" u ON u.\"id\" = o.\"UserID\" LEFT JOIN \"OrderDetails\" od ON od.\"OrderID\" = o.\"OrderID\" WHERE u.\"UserName\" = @Name AND o.\"Status\" != 'Done'; ";
                NpgsqlParameter param = new NpgsqlParameter();
                param.ParameterName = "Name";
                param.Value = _name;
                _npgsqlCommand.Parameters.Add(param);
                NpgsqlDataReader reader = await _npgsqlCommand.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    GetOrdersPrice = int.Parse($"{reader.GetValue(0)}");
                    Console.WriteLine($"{this.ToString()}: {GetOrdersPrice}");
                }
            }catch(Exception ex) 
            { 
                Console.WriteLine($"{this.ToString()}: Message = {ex.Message}; TRC = {ex.StackTrace}"); 
            }


        }

        private async Task  GetOrdersListAsync()
        {
            try
            {
                Console.WriteLine($"{this.ToString}: GetOrdersPriceAsync... ");
                _npgsqlCommand.CommandText = "select o.* from \"Orders\" o LEFT JOIN \"Users\" u ON u.\"id\" = o.\"UserID\" WHERE u.\"UserName\" = @Name ; ";
                NpgsqlParameter param = new NpgsqlParameter();
                param.ParameterName = "Name";
                param.Value = _name;
                _npgsqlCommand.Parameters.Add(param);
                NpgsqlDataReader reader = await _npgsqlCommand.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    if(!reader.HasRows)
                    {
                        Console.WriteLine($"{this.ToString()}: responce is empty.");
                        return;
                    }
                    Order order = new Order();
                    order.OrderID = (int)reader["OrderID"];
                    order.UserID = (int)reader["UserID"];
                    order.Status = reader["Status"] as string;
                    order.OrderDate = reader.GetDateTime(2);
                    Console.WriteLine($"{this.ToString()}: {order.OrderID} {order.UserID} {order.Status} {order.OrderDate}.");
                    GetOrdersList.Add(order);
                }
                Console.WriteLine($"{this.ToString()}: GetOrdersList.Count = {GetOrdersList.Count}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{this.ToString()}: Message = {ex.Message}; TRC = {ex.StackTrace}");
            }
        }

    }
}
