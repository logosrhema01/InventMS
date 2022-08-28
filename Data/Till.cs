using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace InventMS.Data
{
    public class Till
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public DateTime Start { get; set; }

        public DateTime? End { get; set; }

        [Required]
        public ApplicationUser User { get; set; }

        public ICollection<Transaction> Transactions { get; set; }
    }
}
