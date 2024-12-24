using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiBD.Models
{
    /// <summary>
    /// Класс для таблицы OrderDetails
    /// Order - навигационное свойство для таблицы Order
    /// Product - навигационное свойство для таблицы Product
    /// </summary>
    public class OrderDetails
    {
        [Key]
        public int OrderDetailsID { get; set; } = 0;
        public int ProductID { get; set; } = 0;
        public int OrderID { get; set; } = 0;
        public int Quantity { get; set; } = 0;
        public int TotalCost { get; set; } = 0;
        [ForeignKey("OrderID")]
        public Order ?Order { get; set; }
        [ForeignKey("ProductID")]
        public Product ?Product { get; set; }
    }
}
