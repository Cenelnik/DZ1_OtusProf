using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiBD.Models
{
    /// <summary>
    /// Класс для таблицы Products
    /// orderDetailsList - это навигационное свойство для таблицы OrderDetails
    /// </summary>
    [Table("Products")]
    public class Product
    {
        [Key]
        public int ProductID { get; set; } = 0;
        public string ProductName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Price { get; set; } = 0;
        public int QuantityInStock { get; set; } = 0;
        public List<OrderDetails> orderDetailsList { get; set; } = new();
    }
}
