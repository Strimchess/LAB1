using Microsoft.VisualStudio.TestTools.UnitTesting;
using WindowsFormsApp1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace WindowsFormsApp1.Tests
{
    [TestClass()]
    public class EmployeeManagerTests
    {
        [TestMethod()]
        public void EmployeeManagerTest()
        {
            EmployeeManager manager = new EmployeeManager();
            Assert.IsNotNull(manager);
        }

        [TestMethod()]
        public void AddEmployeeTest()
        {
            EmployeeManager manager = new EmployeeManager();
            Employee employee =  new Employee("Name", "Pos", new DateTime(2025, 1, 1));
            manager.AddEmployee(employee);
            Assert.AreEqual(employee.Name, manager.Employees[0].Name);
        }

        [TestMethod()]
        public void RemoveEmployeeTest()
        {
            EmployeeManager manager = new EmployeeManager();
            EmployeeManager manager2 = new EmployeeManager();
            Employee employee = new Employee("Name", "Pos", new DateTime(2025, 1, 1));
            manager.AddEmployee(employee);
            manager.RemoveEmployee(employee);
            Assert.AreEqual(manager2.Employees.Count, manager.Employees.Count);
        }

        [TestMethod()]
        public void UpdateVacationTest()
        {
            EmployeeManager manager = new EmployeeManager();
            Employee employee = new Employee("Name", "Pos", new DateTime(2025, 1, 1));
            manager.AddEmployee(employee);
            manager.UpdateVacation(employee, new DateTime(2025, 2, 2), new DateTime(2025, 3, 3));
            Assert.AreEqual(employee.VacationStart, new DateTime(2025, 2, 2));
        }

        public string testFilePath = "employees.txt";
        public EmployeeManager employees = new EmployeeManager();

        [TestCleanup]
        public void Cleanup()
        {
            if (File.Exists(testFilePath))
            {
                File.Delete(testFilePath);
            }
        }

        [TestMethod]
        public void SaveEmployees_CreatesCorrectFile()
        {
            employees.AddEmployee(new Employee("Emp", "Pos", new DateTime(2025, 5, 1))
            {
                VacationStart = new DateTime(2025, 6, 1),
                VacationEnd = new DateTime(2025, 6, 2)
            });



            Assert.IsTrue(File.Exists(testFilePath));
            var lines = File.ReadAllLines(testFilePath);
            Assert.AreEqual("Emp|Pos|2025-05-01|2025 - 06 - 01|2025 - 06 - 02", lines[0]);
        }

        [TestMethod]
        public void LoadEmployees_LoadsCorrectData()
        {
            
        }

    }
}


