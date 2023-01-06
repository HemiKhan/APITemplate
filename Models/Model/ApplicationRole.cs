using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Model
{
    public class ApplicationRole : IdentityRole<Guid>
    {
        public bool Deleted { get; set; }
        public bool Hidden { get; set; }
    }
}
