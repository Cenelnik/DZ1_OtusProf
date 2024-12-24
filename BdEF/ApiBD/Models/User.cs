
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace ApiBD.Models
{
    /// <summary>
    /// Класс для таблицы Users. 
    /// OrdersList - навигационное своейство для таблицы Orders
    /// </summary>

    [Table("Users")]
    public class User
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("UserName")]
        public string UserName { get; set; } = string.Empty;
        [Column("Email")]
        public string Email { get; set; } = string.Empty;
        [Column("RegistrationDate")]
        public DateTime RegistrationDate { get; set; }
        public List<Order> OrdersList { get; set; } = new();
    }
}
