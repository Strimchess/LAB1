using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.IO;

namespace WindowsFormsApp1.Tests
{
    [TestClass()]
    public class EmployeeManagerTests
    {
        private string testFilePath = "employees.txt";
        private EmployeeManager manager;

        [TestInitialize]
        public void Setup()
        {
            if (File.Exists(testFilePath))
            {
                File.Delete(testFilePath);
            }
            manager = new EmployeeManager();
        }

        [TestMethod()]
        public void EmployeeManager_CreatesInstance()
        {
            Assert.IsNotNull(manager);
        }

        [TestMethod()]
        public void AddEmployee_AddsEmployeeToList()
        {
            Employee employee = new Employee("Khalid Kashmiri", "Developer", new DateTime(2025, 1, 1));
            manager.AddEmployee(employee);
            Assert.IsTrue(File.Exists(testFilePath));
            Assert.AreEqual(1, manager.Employees.Count);
            Assert.AreEqual(employee.Name, manager.Employees[0].Name);
        }

        

        [TestMethod()]
        public void RemoveEmployee_RemovesEmployeeFromList()
        {
            Employee employee = new Employee("Khalid Kashmiri", "Developer", new DateTime(2025, 1, 1));
            manager.AddEmployee(employee);
            manager.RemoveEmployee(employee);
            Assert.IsTrue(File.Exists(testFilePath));
            Assert.AreEqual(0, manager.Employees.Count);
        }

        

        [TestMethod()]
        public void UpdateVacation_UpdatesEmployeeVacationPeriod()
        {
            Employee employee = new Employee("Khalid Kashmiri", "Developer", new DateTime(2025, 1, 1));
            manager.AddEmployee(employee);
            Assert.IsTrue(File.Exists(testFilePath));
            DateTime vacationStart = new DateTime(2025, 2, 2);
            DateTime vacationEnd = new DateTime(2025, 3, 3);
            manager.UpdateVacation(employee, vacationStart, vacationEnd);
            Assert.AreEqual(vacationStart, employee.VacationStart);
            Assert.AreEqual(vacationEnd, employee.VacationEnd);
        }

        

        [TestMethod]
        public void SaveEmployees_CreatesFileWithCorrectData()
        {
            Employee employee = new Employee("Emp", "Pos", new DateTime(2025, 5, 1));
            manager.AddEmployee(employee);
            Assert.IsTrue(File.Exists(testFilePath));
            var lines = File.ReadAllLines(testFilePath);
            Assert.IsTrue(lines.Any(line => line.Contains("Emp|Pos|2025-05-01")));
        }

        [TestMethod]
        public void LoadEmployees_LoadsCorrectData()
        {
            File.WriteAllLines(testFilePath, new[] { "Name|Pos|2025-01-01||" });

            manager = new EmployeeManager();
            Assert.AreEqual(1, manager.Employees.Count);
            Assert.AreEqual("Name", manager.Employees[0].Name);
        }

        [TestCleanup]
        public void Cleanup()
        {
            if (File.Exists(testFilePath))
            {
                File.Delete(testFilePath);
            }
        }
    }
}