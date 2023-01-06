using Models.Model;
using Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.EmailAlert
{
    public interface IEmailAlert
    {
        Task<Response<Employee>> AddEmployeeAsync();
    }
}
