using Data.AppContext;
using Models.Model;
using Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.EmailAlert
{
    public class EmailAlert : IEmailAlert
    {
        private readonly AppDbContext context;

        public EmailAlert(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<Response<Employee>> AddEmployeeAsync()
        {
            try
            {
                Employee model = new Employee();
                model.Name = "Hammas";
                model.Designation = "Developer";
                model.Salary = 50000;
                model.IsActive = true;
                model.JoiningDate = DateTime.Now;

                await context.Employees.AddAsync(model);
                await context.SaveChangesAsync();

                return new Response<Employee>
                {
                    Message = "Employee data found successfully",
                    Status = true,
                    Data = model
                };
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
