using ApiBD.Infractructure;
using ApiBD.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiBD.Api
{
    public static class UserExtension
    {
        public static List<Order> GetOrdersList(this ConnectToBD context, string userName)
        {
            try
            {
                User? user = context.Users.FirstOrDefault<User>(x => x.UserName == userName);
                if (user != null)
                {
                    return user.OrdersList;
                }else
                {
                    throw new Exception($"ApiBD.Api.GetOrdersList: Did not find user with userName = {userName}.");
                }
            }catch (Exception ex) 
            {
                Console.WriteLine($"Product.API.GetOrdersList: MSG = {ex.Message}; TRC = {ex.StackTrace}.");
                return new List<Order>();
            }

        }

        public static int GetOrdersPrice(this ConnectToBD context, string userName)
        {
            int sum = 0;
            try
            {
                User user = context.Users.Where(u => u.UserName == $"{userName}").FirstOrDefault<User>();

                if (user.UserName != "")
                {
                    Order? order  = context.Orders.Include(l => l.orderDetailsList).Where(o => o.UserID == user.Id && o.Status != "Done" ).FirstOrDefault<Order>();
                    if (order != null)
                    {
                        sum += order.orderDetailsList.Sum(s => s.TotalCost);
                    }else
                    {
                        Console.WriteLine($"ApiBD.Api.GetOrdersList: there are no orders for user {userName}.");
                    }
                }else
                {
                    Console.WriteLine($"ApiBD.Api.GetOrdersList: there is no user {userName}");
                }
                return sum;
            }catch (Exception ex)
            {
                Console.WriteLine($"ApiBD.Api.GetOrdersList: error.");
                throw ex;
            }

        }
    }
}

