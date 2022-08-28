using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace InventMS.Data
{
    public class ApplicationUser : IdentityUser
    {
        public ICollection<Till> Tills { get; set; }

        public ICollection<Transaction> Transactions { get; set; }
    }
}
