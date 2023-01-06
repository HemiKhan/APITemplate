using Models.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModel
{
    public class EmployeeViewModel
    {
        public int EmpID { get; set; }
        public string Name { get; set; }
        public string Designation { get; set; }
        public int Salary { get; set; }
        public bool IsActive { get; set; }
        public DateTime JoiningDate { get; set; }
    }

    public static class EmployeeExtensions
    {
        public static List<EmployeeViewModel> EmployeeMapperListing(this List<Employee> data)
        {
            List<EmployeeViewModel> users = new List<EmployeeViewModel>();
            foreach (var item in data)
            {
                EmployeeViewModel res = new EmployeeViewModel();
                res.EmpID = item.EmpID;
                res.Name = item.Name;
                res.Designation = item.Designation;
                res.Salary = item.Salary;
                res.IsActive = item.IsActive;
                res.JoiningDate = item.JoiningDate;
                users.Add(res);
            }
            return users;
        }

        public static EmployeeViewModel EmployeeMapper(this Employee data)
        {
            EmployeeViewModel res = new EmployeeViewModel();
            res.EmpID = data.EmpID;
            res.Name = data.Name;
            res.Designation = data.Designation;
            res.Salary = data.Salary;
            res.IsActive = data.IsActive;
            res.JoiningDate = data.JoiningDate;
            return res;
        }
    }
}
