using Data.AppContext;
using Microsoft.EntityFrameworkCore;
using Models.Model;
using Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.StoredProcedures
{
    public class StoredProcedureService : IStoredProcedureService
    {
        private readonly AppDbContext context;
        public StoredProcedureService(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<Response<Employee>> SPGetEmployeeAsync()
        {
            try
            {
                var data = context.Set<Employee>().FromSqlRaw<Employee>($"exec sp_GetEmployee");
                if (data is null)
                    return new Response<Employee>
                    {
                        Message = "No data found against this ID",
                        Status = false
                    };
                return new Response<Employee>
                {
                    Message = "Employee data found successfully",
                    Status = true,
                    List = data.ToList()
                };
            }
            catch
            {
                throw;
            }
        }

        public async Task<Response<Employee>> SPAddEmployeeAsync(Employee model)
        {
            try
            {
                if (model is null)
                    throw new NullReferenceException("Model is Null Here....");
                model.JoiningDate = DateTime.Now;
                await context.Database.ExecuteSqlRawAsync($"exec sp_InsertEmployee @Name='{model.Name}',@Designation='{model.Designation}',@Salary='{model.Salary}',@IsActive='{model.IsActive}',@JoiningDate='{model.JoiningDate}'");
                return new Response<Employee>
                {
                    Message = "Record Submitted Successfully",
                    Status = true
                };
            }
            catch
            {
                throw;
            }
        }

        public async Task<Response<Employee>> SPEditEmployeeAsync(Employee model)
        {
            try
            {
                if (model is null)
                    throw new NullReferenceException("Model is Null Here....");
                model.JoiningDate = DateTime.Now;
                await context.Database.ExecuteSqlRawAsync($"exec sp_UpdateEmployee @EmpID='{model.EmpID}',@Name='{model.Name}',@Designation='{model.Designation}',@Salary='{model.Salary}',@IsActive='{model.IsActive}',@JoiningDate='{model.JoiningDate}'");
                return new Response<Employee>
                {
                    Message = "Record Updated Successfully",
                    Status = true
                };
            }
            catch
            {
                throw;
            }
        }

        public async Task<Response<Employee>> SPDeleteByIdEmployeeAsync(int EmpId)
        {
            try
            {
                context.Database.ExecuteSqlRaw($"exec sp_DeleteEmployee @EmpID='{EmpId}'");
                return new Response<Employee>
                {
                    Message = "Record Is Deleted Successfully",
                    Status = true
                };
            }
            catch
            {
                throw;
            }
        }

        public async Task<Response<Employee>> SPEmployeeGetByIdAsync(int EmpId)
        {
            try
            {
                var data = context.Set<Employee>().FromSqlRaw<Employee>($"exec sp_GetEmployeeById @EmpID='{EmpId}'");
                if (data is null)
                    return new Response<Employee>
                    {
                        Message = "No data found against this ID",
                        Status = false
                    };
                return new Response<Employee>
                {
                    Message = "Employee data found successfully",
                    Status = true,
                    List = data.ToList()
                };
            }
            catch
            {
                throw;
            }
        }
    }
}