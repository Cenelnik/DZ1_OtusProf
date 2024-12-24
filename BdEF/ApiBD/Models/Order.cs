using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiBD.Models
{
    /// <summary>
    /// Класс для таблицы Orders. 
    /// User - навигационное своейство для таблицы Users
    /// orderDetails - это навигационное свойство для таблицы OrderDetails
    /// </summary>
    [Table("Orders")]
    public class Order
    {
        [Key]
        public int OrderID { get; set; } = 0;
        public int UserID { get; set; } = 0;
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public string Status { get; set; } = string.Empty;

        [ForeignKey("UserID")]
        public User? User { get; set; }
        public List<OrderDetails> orderDetailsList { get; set; } = new();
    }
}
