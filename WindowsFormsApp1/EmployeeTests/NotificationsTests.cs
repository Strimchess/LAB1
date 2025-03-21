using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Windows.Forms;
using WindowsFormsApp1;
using System.Linq;


namespace NotifTests
{
    [TestClass]
    public class NotificationsTests
    {
        private EmployeeForm form;
        private string testSettingsFile = "test_settings.txt";

        [TestInitialize]
        public void Setup()
        {
            form = new EmployeeForm();
            typeof(EmployeeForm)
                .GetField("settingsFile", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                .SetValue(form, testSettingsFile);
            employeeManager = new EmployeeManager();

        }

        [TestMethod]
        public void SaveSettings_Should_CreateFile()
        {
            File.Delete(testSettingsFile);
            form.GetType().GetMethod("SaveSettings", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).Invoke(form, null);
            Assert.IsTrue(File.Exists(testSettingsFile));
        }

        private EmployeeManager employeeManager;


        [TestMethod]
        public void Notification_Should_TriggerBeforeVacation()
        {
            Employee emp = new Employee("Иван Иванов", "Менеджер", DateTime.Today.AddYears(-1))
            {
                VacationStart = DateTime.Today.AddDays(3),
                VacationEnd = DateTime.Today.AddDays(10)
            };
            employeeManager.AddEmployee(emp);

            bool shouldNotify = employeeManager.Employees.Any(e => e.VacationStart.HasValue &&
                                          (e.VacationStart.Value - DateTime.Today).Days == 3);
            Assert.IsTrue(shouldNotify);
        }

        [TestMethod]
        public void Notification_Should_TriggerBeforeVacationEnd()
        {
            Employee emp = new Employee("Анна Смирнова", "Разработчик", DateTime.Today.AddYears(-2))
            {
                VacationStart = DateTime.Today.AddDays(-5),
                VacationEnd = DateTime.Today.AddDays(1)
            };
            employeeManager.AddEmployee(emp);

            bool shouldNotify = employeeManager.Employees.Any(e => e.VacationEnd.HasValue &&
                                          (e.VacationEnd.Value - DateTime.Today).Days == 1);
            Assert.IsTrue(shouldNotify);
        }
    }
}