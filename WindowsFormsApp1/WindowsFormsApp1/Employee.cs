using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.IO;
using System.Linq;
public class Employee
{
    public string Name { get; set; }
    public string Position { get; set; }
    public DateTime HireDate { get; set; }
    public DateTime? VacationStart { get; set; }
    public DateTime? VacationEnd { get; set; }
    public Employee(string name, string position, DateTime hireDate)
    {
        Name = name;
        Position = position;
        HireDate = hireDate;
        VacationStart = null;
        VacationEnd = null;
    }
    public bool IsOnVacation
    {
        get
        {
            if (VacationStart == null || VacationEnd == null)
            {
                return false;
            }
            return DateTime.Now >= VacationStart.Value && DateTime.Now <=
            VacationEnd.Value;
        }
    }
}
