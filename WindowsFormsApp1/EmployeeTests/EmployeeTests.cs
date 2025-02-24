using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    [TestClass()]
    public class EmployeeTests
    {
        [TestMethod()]
        public void EmployeeTest()
        {
            Employee emp1 = new Employee("Name", "Pos", new DateTime(2025, 1, 1));
            Assert.IsNotNull(emp1);
        }

        [TestMethod]
        public void EmployeeIsOnVacationTest()
        {
            Employee emp1 = new Employee("Name", "Pos", new DateTime(2025, 1, 1));
            emp1.VacationStart = emp1.HireDate;
            emp1.VacationEnd = new DateTime(2025, 5, 1);
            Assert.AreEqual(true, emp1.IsOnVacation);
        }
    }
}