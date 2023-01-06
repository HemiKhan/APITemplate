using Models.Model;
using Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.StoredProcedures
{
    public interface IStoredProcedureService
    {
        Task<Response<Employee>> SPGetEmployeeAsync();
        Task<Response<Employee>> SPAddEmployeeAsync(Employee model);
        Task<Response<Employee>> SPEditEmployeeAsync(Employee model);
        Task<Response<Employee>> SPDeleteByIdEmployeeAsync(int EmpId);
        Task<Response<Employee>> SPEmployeeGetByIdAsync(int EmpId);
    }
}
