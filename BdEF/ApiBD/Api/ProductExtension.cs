using ApiBD.Models;
using ApiBD.Infractructure;
using System;

namespace ApiBD.Api
{
    public static class ProductExtension
    {
        public static void AddProduct(this ConnectToBD context, Product newProduct)
        {
            try
            {
                context.Products.Add(newProduct);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Product.API.Add: MSG = {ex.Message}; TRC = {ex.StackTrace}.");
            }
        }

        public static void UpdatePriceProduct(this ConnectToBD context, string lable, int newPrice)
        {
            try
            {
                Product? product = context.Products.FirstOrDefault<Product>(p => p.ProductName == lable);
                if (product != null)
                {
                    product.Price = newPrice;
                    context.SaveChanges();
                }
                else
                {
                    throw new Exception($"Error: Product with ProductName = {lable} dont find.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Product.API.UpdateCost: MSG = {ex.Message}; TRC = {ex.StackTrace}.");
            }
        }

        public static int GetQuantityInStock(this ConnectToBD context)
        {
            try
            {
                return context.Products.Sum(s => s.QuantityInStock);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Product.API.GetQuantityInStock: MSG = {ex.Message}; TRC = {ex.StackTrace}.");
                return 0;
            }
        }

        public static List<Product> GetMostExpensive(this ConnectToBD context)
        {
            try
            {
                return context.Products.OrderByDescending(n => n.Price).Take(5).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Product.API.GetQuantityInStock: MSG = {ex.Message}; TRC = {ex.StackTrace}.");
                return new List<Product>();
            }
        }

        public static List<Product> GetLowQuantityInStock(this ConnectToBD context)
        {
            try
            {
                return context.Products.Where(n => n.QuantityInStock < 5).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Product.API.GetQuantityInStock: MSG = {ex.Message}; TRC = {ex.StackTrace}.");
                return new List<Product>();
            }
        }
    }
}
