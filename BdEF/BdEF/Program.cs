using ApiBD.Api;
using ApiBD.Infractructure;
using ApiBD.Models;
using Microsoft.EntityFrameworkCore;

class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("Hello, World!");
        try
        {
            using (ConnectToBD connect = new ConnectToBD())
            {
                Console.WriteLine($"Main: all in the stock {connect.GetQuantityInStock()} products.");

                int coins = connect.GetOrdersPrice($"Alice");

                Console.WriteLine($"Main: Alice has orders on {coins} coins (without Done-orders)");

                Console.WriteLine($"Main: get most expensive list. And the most expensive is {connect.GetMostExpensive().First().ProductName} product");

                Console.WriteLine($"Main: count of low quantity in stock {connect.GetLowQuantityInStock().Count}.");
            }
            Product  pr = new Product();
            pr.ProductName = "Test";
            pr.Price = 2222222;
            pr.Description = "xcvtybuniq3";
            pr.QuantityInStock = 10;

            PgSqlConnection pg = new ProductPgSqlWorker("Host=localhost;Port=5432;Database=Shop;Username=postgres;Password=1HUF!zLRnCKM-kV0", pr);
            //await pg.ExecAsync("AddProduct");
            //await pg.ExecAsync("UpdatePriceProduct");
            await pg.ExecAsync("GetQuantityInStock");
            await pg.ExecAsync("GetMostExpensive");
            await pg.ExecAsync("GetLowQuantityInStock");

            pg = new UserPgSqlWorker("Host=localhost;Port=5432;Database=Shop;Username=postgres;Password=1HUF!zLRnCKM-kV0", "Alice");
            await pg.ExecAsync("GetOrdersPriceAsync");
            await pg.ExecAsync("GetOrdersList");
            Console.ReadKey();

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Main: {ex.Message}; {ex.StackTrace}.");
        }
    }
}