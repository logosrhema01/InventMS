using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
namespace InventMS.Data
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string BarCode { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public double Price { get; set; } = 0.0;

        public Category Category { get; set; }

        public ICollection<Transaction> Transactions { get; set; }
    }
}
