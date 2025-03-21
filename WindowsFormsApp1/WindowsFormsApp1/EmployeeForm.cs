using System.Windows.Forms;
using System;
using System.IO;
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
    private Label hireDateLabel;
    private Label vacationStartLabel;
    private Label vacationEndLabel;
    private NumericUpDown notifyBeforeStartPicker;
    private NumericUpDown notifyBeforeEndPicker;
    private Label notifyBeforeStartLabel;
    private Label notifyBeforeEndLabel;
    private Timer notificationTimer;


    public EmployeeForm()
    {
        this.Text = "Управление сотрудниками";
        this.Width = 600;
        this.Height = 420;
        

        nameTextBox = new TextBox
        {
            Location = new System.Drawing.Point(10, 10),
            Width = 150,
            Text = "Имя и фамилия",
            Tag = "Имя и фамилия"
        };

        positionTextBox = new TextBox
        {
            Location = new System.Drawing.Point(170, 10),
            Width = 150,
            Text = "Должность",
            Tag = "Должность"
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

        hireDateLabel = new Label
        {
            Location = new System.Drawing.Point(330, 5),
            Text = "Дата приёма",
        };

        vacationStartLabel = new Label
        {
            Location = new System.Drawing.Point(10, 35),
            Text = "Начало отпуска",
        };

        vacationEndLabel = new Label
        {
            Location = new System.Drawing.Point(220, 35),
            Text = "Конец отпуска",
        };

        notifyBeforeStartLabel = new Label
        {
            Location = new System.Drawing.Point(420, 60),
            Text = "Увед. за (дн.):",
        };

        notifyBeforeStartPicker = new NumericUpDown
        {
            Location = new System.Drawing.Point(520, 60),
            Width = 50,
            Minimum = 1,
            Maximum = 30,
            Value = 3
        };

        notifyBeforeEndLabel = new Label
        {
            Location = new System.Drawing.Point(420, 90),
            Text = "Увед. о конце (дн.):",
        };

        notifyBeforeEndPicker = new NumericUpDown
        {
            Location = new System.Drawing.Point(520, 90),
            Width = 50,
            Minimum = 1,
            Maximum = 30,
            Value = 1
        };


        this.Controls.Add(notifyBeforeStartLabel);
        this.Controls.Add(notifyBeforeStartPicker);
        this.Controls.Add(notifyBeforeEndLabel);
        this.Controls.Add(notifyBeforeEndPicker);
        this.Controls.Add(hireDateLabel);
        this.Controls.Add(vacationStartLabel);
        this.Controls.Add(vacationEndLabel);
        this.Controls.Add(nameTextBox);
        this.Controls.Add(positionTextBox);
        this.Controls.Add(hireDatePicker);
        this.Controls.Add(vacationStartPicker);
        this.Controls.Add(vacationEndPicker);
        this.Controls.Add(addEmployeeButton);
        this.Controls.Add(removeEmployeeButton);
        this.Controls.Add(updateVacationButton);
        this.Controls.Add(employeesListBox);

        notifyBeforeStartPicker.ValueChanged += (s, e) => SaveSettings();
        notifyBeforeEndPicker.ValueChanged += (s, e) => SaveSettings();
        nameTextBox.GotFocus += new EventHandler(RemoveText);
        nameTextBox.LostFocus += new EventHandler(AddText);
        positionTextBox.GotFocus += new EventHandler(RemoveText);
        positionTextBox.LostFocus += new EventHandler(AddText);

        employeeManager = new EmployeeManager();
        UpdateEmployeesList();
        LoadSettings();
        NotifCheck();
    }

    private void NotifCheck()
    {
        int daysBeforeStart = (int)notifyBeforeStartPicker.Value;
        int daysBeforeEnd = (int)notifyBeforeEndPicker.Value;
        DateTime today = DateTime.Today;

        foreach (var employee in employeeManager.Employees)
        {
            if (employee.VacationStart.HasValue && (employee.VacationStart.Value - today).Days == daysBeforeStart)
            {
                MessageBox.Show($"Скоро отпуск у {employee.Name}! Начало {employee.VacationStart.Value:dd.MM.yyyy}.",
                                "Напоминание", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            if (employee.VacationEnd.HasValue && (employee.VacationEnd.Value - today).Days == daysBeforeEnd)
            {
                MessageBox.Show($"Скоро конец отпуска у {employee.Name}. Возвращается {employee.VacationEnd.Value:dd.MM.yyyy}.",
                                "Напоминание", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
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
            positionTextBox.Text = "Должность";
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

    private string settingsFile = "settings.txt";

    private void SaveSettings()
    {
        try
        {
            File.WriteAllLines(settingsFile, new string[]
            {
            notifyBeforeStartPicker.Value.ToString(),
            notifyBeforeEndPicker.Value.ToString()
            });
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ошибка при сохранении настроек: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void LoadSettings()
    {
        if (File.Exists(settingsFile))
        {
            try
            {
                var lines = File.ReadAllLines(settingsFile);
                if (lines.Length >= 2)
                {
                    if (int.TryParse(lines[0], out int startDays))
                    {
                        notifyBeforeStartPicker.Value = Math.Max(notifyBeforeStartPicker.Minimum, Math.Min(notifyBeforeStartPicker.Maximum, startDays));
                    }

                    if (int.TryParse(lines[1], out int endDays))
                    {
                        notifyBeforeEndPicker.Value = Math.Max(notifyBeforeEndPicker.Minimum, Math.Min(notifyBeforeEndPicker.Maximum, endDays));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке настроек: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
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