using Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Seed
{
    public interface ISeedService
    {
        Task<Response<string>> SeederAsync();
    }
}
