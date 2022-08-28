using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace InventMS.Data
{
    public class Transaction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public DateTime TxnDateTime { get; set; }

        [Required]
        public double TotalPrice { get; set; }

        [Required]
        public ApplicationUser User { get; set; }

        [Required]
        public Till Till { get; set; }

        [Required]
        public ICollection<Product> Products { get; set; }
    }
}
