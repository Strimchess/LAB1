using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace WindowsFormsApp1
{
    public class EmployeeManager
    {
        public List<Employee> Employees { get; private set; }
        public EmployeeManager()
        {
            Employees = new List<Employee>();
            LoadEmployees();
        }
        public void AddEmployee(Employee employee)
        {
            if (employee == null)
            {
                throw new ArgumentNullException(nameof(employee));
            }
            Employees.Add(employee);
            SaveEmployees();
        }
        public void RemoveEmployee(Employee employee)
        {
            if (employee == null)
            {
                throw new ArgumentNullException(nameof(employee));
            }
            Employees.Remove(employee);
            SaveEmployees();
        }
        public void UpdateVacation(Employee employee, DateTime? vacationStart, DateTime?
        vacationEnd)
        {
            if (employee == null)
            {
                throw new ArgumentNullException(nameof(employee));
            }
            employee.VacationStart = vacationStart;
            employee.VacationEnd = vacationEnd;
            SaveEmployees();
        }
        private void SaveEmployees()
        {
            File.WriteAllLines("employees.txt", Employees.Select(e =>
            $"{e.Name}|{e.Position}|{e.HireDate.ToString("yyyy-MM-dd")}|{e.VacationStart?.ToString("yyyy - MM - dd")}|{e.VacationEnd?.ToString("yyyy - MM - dd")}"));
        }
        private void LoadEmployees()
        {
            if (File.Exists("employees.txt"))
            {
                var lines = File.ReadAllLines("employees.txt");
                foreach (var line in lines)
                {
                    var parts = line.Split('|');
                    if (parts.Length == 5)
                    {
                        DateTime hireDate;
                        DateTime? vacationStart = null;
                        DateTime? vacationEnd = null;
                        if (DateTime.TryParse(parts[2], out hireDate))
                        {
                            if (parts[3] != "")
                            {
                                if (DateTime.TryParse(parts[3], out DateTime temp))
                                {
                                    vacationStart = temp;
                                }
                            }
                            if (parts[4] != "")
                            {
                                if (DateTime.TryParse(parts[4], out DateTime temp))
                                {
                                    vacationEnd = temp;
                                }
                            }
                            Employees.Add(new Employee(parts[0], parts[1], hireDate)
                            {
                                VacationStart = vacationStart,
                                VacationEnd = vacationEnd
                            });
                        }
                    }
                }
            }
        }
    }

}
