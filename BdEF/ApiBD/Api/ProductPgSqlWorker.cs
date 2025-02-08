using ApiBD.Infractructure;
using ApiBD.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ApiBD.Api
{
    public class ProductPgSqlWorker : PgSqlConnection
    {
        public int QuantityInStock = 0;
        public List<Product> LowQuantityInStock = new List<Product>();
        Product _product;
        public ProductPgSqlWorker(string connectString, Product product) : base(connectString)
        {
            _product = product;
        }
        public override async Task ExecAsync(string command)
        {
            try
            {
                _npgsqlConnection.Open();
                Console.WriteLine($"{this.ToString}: Connection.Open");
                switch (command)
                {
                    case "AddProduct":
                        Console.WriteLine($"{this.ToString}: Try to exec AddProduct");
                        await AddProduct();
                        Console.WriteLine($"{this.ToString}: Exec have done.");
                        break;

                    case "UpdatePriceProduct":
                        Console.WriteLine($"{this.ToString}: Try to exec UpdatePriceProduct");
                        await UpdatePriceProduct();
                        Console.WriteLine($"{this.ToString}: Exec have done.");
                        break;

                    case "GetQuantityInStock":
                        Console.WriteLine($"{this.ToString}: Try to exec GetQuantityInStock");
                        await GetQuantityInStock();
                        Console.WriteLine($"{this.ToString}: Exec have done.");
                        break;

                    case "GetMostExpensive":
                        Console.WriteLine($"{this.ToString}: Try to exec GetMostExpensive");
                        await GetMostExpensive();
                        Console.WriteLine($"{this.ToString}: Exec have done.");
                        break;

                    case "GetLowQuantityInStock":
                        Console.WriteLine($"{this.ToString}: Try to exec GetLowQuantityInStock");
                        await GetLowQuantityInStock();
                        Console.WriteLine($"{this.ToString}: Exec have done.");
                        break;

                    default:
                        Console.WriteLine($"{this.ToString}: Uncorrect command.");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{this.ToString()}: Message = {ex.Message}; Trc = {ex.StackTrace}");
            }
            finally
            {
                _npgsqlConnection?.Close();
                Console.WriteLine($"{this.ToString}: Connection.Close");
            }
        }

        private async Task AddProduct()
        {
            try
            {
                Console.WriteLine($"{this.ToString}: AddProduct... ");
                _npgsqlCommand.CommandText = "INSERT INTO \"Products\" (\"Price\", \"QuantityInStock\", \"Description\", \"ProductName\") VALUES (@price, @quantity, @descript, @name) ; ";
                _npgsqlCommand.Parameters.Add(new NpgsqlParameter("name", _product.ProductName));
                _npgsqlCommand.Parameters.Add(new NpgsqlParameter("price", _product.Price));
                _npgsqlCommand.Parameters.Add(new NpgsqlParameter("quantity", _product.QuantityInStock));
                _npgsqlCommand.Parameters.Add(new NpgsqlParameter("descript", _product.Description));
                int resp = await _npgsqlCommand.ExecuteNonQueryAsync();
                Console.WriteLine($"{this.ToString}: insert {resp} row(s)... ");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{this.ToString()}: Message = {ex.Message}; TRC = {ex.StackTrace}");
            }
        }

        private async Task UpdatePriceProduct()
        {
            try
            {
                Console.WriteLine($"{this.ToString}: UpdatePriceProduct... ");
                _npgsqlCommand.CommandText = "UPDATE  \"Products\" SET \"Price\" = @newPrice WHERE \"ProductName\" = @name ; ";
                _npgsqlCommand.Parameters.Add(new NpgsqlParameter("name", _product.ProductName));
                _npgsqlCommand.Parameters.Add(new NpgsqlParameter("newPrice", _product.Price));
                int resp = await _npgsqlCommand.ExecuteNonQueryAsync();
                Console.WriteLine($"{this.ToString}: update {resp} row(s)... ");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{this.ToString()}: Message = {ex.Message}; TRC = {ex.StackTrace}");
            }
        }

        private async Task GetQuantityInStock()
        {
            try
            {
                Console.WriteLine($"{this.ToString}: GetQuantityInStock... ");
                _npgsqlCommand.CommandText = "select \"QuantityInStock\" FROM \"Products\" p WHERE p.\"ProductName\" = @name; ";
                _npgsqlCommand.Parameters.Add(new NpgsqlParameter("name", _product.ProductName));
                NpgsqlDataReader reader = await _npgsqlCommand.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    QuantityInStock = int.Parse($"{reader.GetValue(0)}");
                    Console.WriteLine($"{this.ToString()}: QuantityInStock = {QuantityInStock}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{this.ToString()}: Message = {ex.Message}; TRC = {ex.StackTrace}");
            }
        }

        private async Task GetMostExpensive()
        {
            try
            {
                Console.WriteLine($"{this.ToString}: GetMostExpensive... ");
                _npgsqlCommand.CommandText = "SELECT * FROM public.\"Products\" where \"Price\" = (SELECT MAX(\"Price\") FROM public.\"Products\") ;";
                NpgsqlDataReader reader = await _npgsqlCommand.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    if(!reader.HasRows)
                    {
                        Console.WriteLine($"{this.ToString()}: there is no max price.");
                        return;
                    }
                    _product.ProductID = (int)reader["ProductID"];
                    _product.Price = (int)reader["Price"];
                    _product.QuantityInStock = (int)reader["QuantityInStock"];
                    _product.ProductName = reader["ProductName"] as string;
                    _product.Description = reader["Description"] as string;
                    Console.WriteLine($"{this.ToString()}: Most expensive product is = {_product.ProductName} for cost = {_product.Price}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{this.ToString()}: Message = {ex.Message}; TRC = {ex.StackTrace}");
            }
        }

        private async Task GetLowQuantityInStock()
        {
            try
            {
                Product product = new Product();
                Console.WriteLine($"{this.ToString}: GetLowQuantityInStock... ");
                _npgsqlCommand.CommandText = "SELECT * FROM public.\"Products\" where \"QuantityInStock\" < 20 ;";
                NpgsqlDataReader reader = await _npgsqlCommand.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    if (!reader.HasRows)
                    {
                        Console.WriteLine($"{this.ToString()}: there is no max price.");
                        return;
                    }
                    
                    product.ProductID = (int)reader["ProductID"];
                    product.Price = (int)reader["Price"];
                    product.QuantityInStock = (int)reader["QuantityInStock"];
                    product.ProductName = reader["ProductName"] as string;
                    product.Description = reader["Description"] as string;
                    LowQuantityInStock.Add(product);
                    Console.WriteLine($"{this.ToString()}: Product {product.ProductName} is low in stock {product.QuantityInStock}");
                }
                Console.WriteLine($"{this.ToString()}: Count low quantity in stock = {LowQuantityInStock.Count}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{this.ToString()}: Message = {ex.Message}; TRC = {ex.StackTrace}");
            }
        }
    }
}
