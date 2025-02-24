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


        [TestMethod]
        public void SaveEmployees_CreatesCorrectFile()
        {
            employees.AddEmployee(new Employee("Emp", "Pos", new DateTime(2025, 5, 1)));



            Assert.IsTrue(File.Exists(testFilePath));
            var lines = File.ReadAllLines(testFilePath);
            Assert.AreEqual("Emp|Pos|2025-05-01||", lines[1]);
        }

        [TestMethod]
        public void LoadEmployees_LoadsCorrectData()
        {
            Assert.AreEqual(employees.Employees[0].Name, "Name");
        }

    }
}


