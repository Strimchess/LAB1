using System.Windows.Forms;
using System;
using WindowsFormsApp1;

public class EmployeeForm : Form
{
    private EmployeeManager employeeManager;
    private TextBox nameTextBox;
    private TextBox positionTextBox;
    private DateTimePicker hireDatePicker;
    private DateTimePicker vacationStartPicker;
    private DateTimePicker vacationEndPicker;
    private Button addEmployeeButton;
    private Button removeEmployeeButton;
    private Button updateVacationButton;
    private ListBox employeesListBox;
    public EmployeeForm()
    {
        this.Text = "Управление сотрудниками";
        this.Width = 600;
        this.Height = 500;
        nameTextBox = new TextBox
        {
            Location = new System.Drawing.Point(10, 10),
            Width = 150
        };
        positionTextBox = new TextBox
        {
            Location = new System.Drawing.Point(170, 10),
            Width = 150
        };
        hireDatePicker = new DateTimePicker
        {
            Location = new System.Drawing.Point(330, 10)
        };
        vacationStartPicker = new DateTimePicker
        {
            Location = new System.Drawing.Point(10, 40)
        };
        vacationEndPicker = new DateTimePicker
        {
            Location = new System.Drawing.Point(170, 40)
        };
        addEmployeeButton = new Button
        {
            Location = new System.Drawing.Point(10, 70),
            Text = "Добавить",
            Width = 100
        };
        addEmployeeButton.Click += AddEmployeeButton_Click;
        removeEmployeeButton = new Button
        {
            Location = new System.Drawing.Point(120, 70),
            Text = "Удалить",
            Width = 100
        };
        removeEmployeeButton.Click += RemoveEmployeeButton_Click;
        updateVacationButton = new Button
        {
            Location = new System.Drawing.Point(220, 70),
            Text = "Обновить отпуск",
            Width = 120
        };
        updateVacationButton.Click += UpdateVacationButton_Click;
        employeesListBox = new ListBox
        {
            Location = new System.Drawing.Point(10, 100),
            Width = 560,
            Height = 250
        };
        this.Controls.Add(nameTextBox);
        this.Controls.Add(positionTextBox);
        this.Controls.Add(hireDatePicker);
        this.Controls.Add(vacationStartPicker);
        this.Controls.Add(vacationEndPicker);
        this.Controls.Add(addEmployeeButton);
        this.Controls.Add(removeEmployeeButton);
        this.Controls.Add(updateVacationButton);
        this.Controls.Add(employeesListBox);
        employeeManager = new EmployeeManager();
        UpdateEmployeesList();
    }
    private void UpdateEmployeesList()
    {
        employeesListBox.Items.Clear();
        foreach (var employee in employeeManager.Employees)
        {
            string vacationStatus = employee.IsOnVacation ? "В отпуске" : "На работе";
            employeesListBox.Items.Add($"{employee.Name} - {employee.Position}({ vacationStatus})");
        }
    }
    private void AddEmployeeButton_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(nameTextBox.Text) ||
        string.IsNullOrEmpty(positionTextBox.Text))
        {
            MessageBox.Show("Заполните все поля!");
            return;
        }
        DateTime hireDate = hireDatePicker.Value;
        Employee newEmployee = new Employee(nameTextBox.Text, positionTextBox.Text,
        hireDate);
        try
        {
            employeeManager.AddEmployee(newEmployee);
            nameTextBox.Clear();
            positionTextBox.Clear();
            UpdateEmployeesList();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }
    private void RemoveEmployeeButton_Click(object sender, EventArgs e)
    {
        if (employeesListBox.SelectedIndex == -1)
        {
            MessageBox.Show("Выберите сотрудника для удаления!");
            return;
        }
        string selectedItem = employeesListBox.SelectedItem.ToString();
        string[] parts = selectedItem.Split(new[] { '-' }, StringSplitOptions.None);
        if (parts.Length >= 2)
        {
            string name = parts[0].Trim();
            string position = parts[1].Trim();
            var employeeToRemove = employeeManager.Employees.Find(x => x.Name == name && x.Position == position);
            if (employeeToRemove != null)
            {
                try
                {
                    employeeManager.RemoveEmployee(employeeToRemove);
                    UpdateEmployeesList();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
    private void UpdateVacationButton_Click(object sender, EventArgs e)
    {
        if (employeesListBox.SelectedIndex == -1)
        {
            MessageBox.Show("Выберите сотрудника для обновления отпуска!");
            return;
        }
        string selectedItem = employeesListBox.SelectedItem.ToString();
        string[] parts = selectedItem.Split(new[] { '-' }, StringSplitOptions.None);
        if (parts.Length >= 2)
        {
            string name = parts[0].Trim();
            string position = parts[1].Trim();
            var employeeToUpdate = employeeManager.Employees.Find(x => x.Name == name && x.Position == position);
            if (employeeToUpdate != null)
            {
                DateTime? vacationStart = vacationStartPicker.Value;
                DateTime? vacationEnd = vacationEndPicker.Value;
                if (vacationStart >= vacationEnd)
                {
                    MessageBox.Show("Дата начала должна быть раньше даты окончания!");
                    return;
                }
                try
                {
                    employeeManager.UpdateVacation(employeeToUpdate, vacationStart,
                    vacationEnd);
                    UpdateEmployeesList();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
  
    public static void Main()
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run(new EmployeeForm());
    }
}
