using ApiBD.Api;
using ApiBD.Infractructure;
using ApiBD.Models;
using Microsoft.EntityFrameworkCore;
class Program
{
    static void Main(string[] args)
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
        }catch (Exception ex)
        {
            Console.WriteLine($"Main: {ex.Message}; {ex.StackTrace}.");
        }
    }
}