using System.Windows.Forms;
using System;
using WindowsFormsApp1;
using System.Resources;

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
        this.Height = 420;

        nameTextBox = new TextBox
        {
            Location = new System.Drawing.Point(10, 30),
            Width = 150,
            Text = "Имя и фамилия",
            Tag = "Имя и фамилия"
        };

        positionTextBox = new TextBox
        {
            Location = new System.Drawing.Point(170, 30),
            Width = 150,
            Text = "Позиция",
            Tag = "Позиция"
        };

        hireDatePicker = new DateTimePicker
        {
            Location = new System.Drawing.Point(330, 30)
        };

        vacationStartPicker = new DateTimePicker
        {
            Location = new System.Drawing.Point(10, 60)
        };

        vacationEndPicker = new DateTimePicker
        {
            Location = new System.Drawing.Point(220, 60)
        };

        addEmployeeButton = new Button
        {
            Location = new System.Drawing.Point(10, 90),
            Text = "Добавить",
            Width = 100
        };
        addEmployeeButton.Click += AddEmployeeButton_Click;

        removeEmployeeButton = new Button
        {
            Location = new System.Drawing.Point(120, 90),
            Text = "Удалить",
            Width = 100
        };
        removeEmployeeButton.Click += RemoveEmployeeButton_Click;

        updateVacationButton = new Button
        {
            Location = new System.Drawing.Point(220, 90),
            Text = "Обновить отпуск",
            Width = 120
        };
        updateVacationButton.Click += UpdateVacationButton_Click;

        employeesListBox = new ListBox
        {
            Location = new System.Drawing.Point(10, 120),
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

        nameTextBox.GotFocus += new EventHandler(RemoveText);
        nameTextBox.LostFocus += new EventHandler(AddText);
        positionTextBox.GotFocus += new EventHandler(RemoveText);
        positionTextBox.LostFocus += new EventHandler(AddText);

        employeeManager = new EmployeeManager();
        UpdateEmployeesList();
    }

    private void RemoveText(object sender, EventArgs e)
    {
        var txtbox = (TextBox)sender;
        if (txtbox.Text == txtbox.Tag.ToString())
        {
            txtbox.Text = "";
        }
    }

    private void AddText(object sender, EventArgs e)
    {
        var txtbox = (TextBox)sender;
        if (string.IsNullOrWhiteSpace(txtbox.Text))
        {
            txtbox.Text = txtbox.Tag.ToString();
        }
    }

    private void UpdateEmployeesList()
    {
        employeesListBox.Items.Clear();
        foreach (var employee in employeeManager.Employees)
        {
            string vacationStatus = employee.IsOnVacation
                ? $"В отпуске ({employee.VacationStart:dd.MM.yyyy} - {employee.VacationEnd:dd.MM.yyyy})"
                : "На работе";
            employeesListBox.Items.Add($"{employee.Name} - {employee.Position} ({vacationStatus})");
        }
    }

    private void AddEmployeeButton_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(nameTextBox.Text) || string.IsNullOrWhiteSpace(positionTextBox.Text))
        {
            MessageBox.Show("Заполните все поля!");
            return;
        }

        DateTime hireDate = hireDatePicker.Value;
        DateTime vacationStart = vacationStartPicker.Value;
        DateTime vacationEnd = vacationEndPicker.Value;

        if (vacationStart < hireDate)
        {
            MessageBox.Show("Дата начала отпуска не может быть раньше даты приема на работу!");
            return;
        }

        if (vacationStart >= vacationEnd)
        {
            MessageBox.Show("Дата начала отпуска должна быть раньше даты окончания!");
            return;
        }

        Employee newEmployee = new Employee(nameTextBox.Text, positionTextBox.Text, hireDate);

        try
        {
            employeeManager.AddEmployee(newEmployee);
            nameTextBox.Text = "Имя и фамилия";
            positionTextBox.Text = "Позиция";
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
        string name = selectedItem.Split('-')[0].Trim();
        string position = selectedItem.Split('-')[1].Split('(')[0].Trim();

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

    private void UpdateVacationButton_Click(object sender, EventArgs e)
    {
        if (employeesListBox.SelectedIndex == -1)
        {
            MessageBox.Show("Выберите сотрудника для обновления отпуска!");
            return;
        }

        string selectedItem = employeesListBox.SelectedItem.ToString();
        string name = selectedItem.Split('-')[0].Trim();
        string position = selectedItem.Split('-')[1].Split('(')[0].Trim();

        var employeeToUpdate = employeeManager.Employees.Find(x => x.Name == name && x.Position == position);

        if (employeeToUpdate != null)
        {
            DateTime vacationStart = vacationStartPicker.Value;
            DateTime vacationEnd = vacationEndPicker.Value;

            if (vacationStart < employeeToUpdate.HireDate)
            {
                MessageBox.Show("Дата начала отпуска не может быть раньше даты приема на работу!");
                return;
            }

            if (vacationStart >= vacationEnd)
            {
                MessageBox.Show("Дата начала отпуска должна быть раньше даты окончания!");
                return;
            }

            try
            {
                employeeManager.UpdateVacation(employeeToUpdate, vacationStart, vacationEnd);
                UpdateEmployeesList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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