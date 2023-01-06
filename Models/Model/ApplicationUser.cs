using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Model
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        [StringLength(100)]
        public string? FirstName { get; set; } 
        [StringLength(100)]
        public string? LastName { get; set; }
        public bool isActive { get; set; }
    }
}
