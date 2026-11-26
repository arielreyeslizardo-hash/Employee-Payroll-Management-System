using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Xml.Linq;

namespace Payroll {
    internal class EmployeeList {
        private readonly List<Employee> employees;
        private const string FILE_NAME = "Employees.txt";
        private const char DELIMITER = '|';

        public decimal AverageSalary {
            get {
                try {
                    return employees.Average(e => e.WeeklySalary);
                } catch {
                    return 0;
                }
            }
        }

        public decimal SumSalary {
            get {
                try {
                    return employees.Sum(e => e.WeeklySalary);
                } catch {
                    return 0;
                }
            }
        }

        public EmployeeList() {
            employees = new List<Employee>();
        }

        public static EmployeeList Load() {
            EmployeeList employees = new EmployeeList();

            using (StreamReader reader = new StreamReader(FILE_NAME)) {
                string? line;
                int lineNumber = 0;

                while ((line = reader.ReadLine()) != null) {
                    lineNumber++;
                    string[] fields = line.Split(DELIMITER);

                    if (fields.Length != 5) {
                        throw new FormatException($"Invalid employee record on line {lineNumber}");
                    }

                    uint id = uint.Parse(fields[0]);
                    string name = fields[1];
                    decimal hourlyRate = decimal.Parse(fields[2], CultureInfo.InvariantCulture);
                    decimal hoursWorked = decimal.Parse(fields[3], CultureInfo.InvariantCulture);
                    PaymentMethod paymentMethod = Enum.Parse<PaymentMethod>(fields[4]);

                    Employee employee = new Employee(id, name, hourlyRate, paymentMethod) {
                        HoursWorked = hoursWorked
                    };
                    employees.employees.Add(employee);
                }
            }
            return employees;
        }

        public void Save() {
            using (StreamWriter writer = new StreamWriter(FILE_NAME)) {
                foreach (Employee employee in employees) {
                    string line = employee.Id + DELIMITER + employee.Name + DELIMITER + employee.HourlyRate.ToString(CultureInfo.InvariantCulture) + DELIMITER + employee.HoursWorked.ToString(CultureInfo.InvariantCulture) + DELIMITER + employee.PaymentMethod;
                    writer.WriteLine(line);
                }
            }
        }

        public void GenerateReport(TextWriter output) {
            var nameWidth = 13;
            var sumSalary = SumSalary;
            try {
                nameWidth = employees.Max(e => e.Name.Length);
            } catch {
                // Leave nameWidth as default.
            }
            output.WriteLine(" ID   " + "Employee Name".PadRight(nameWidth) + "   Hours Worked   Overtime Hours   Regular Rate   Overtime Rate   Weekly Salary   % of Total Expense");
            output.WriteLine(new string('-', nameWidth + 106));

            foreach (Employee employee in employees) {
                output.WriteLine($"{employee.Id,3}   " + employee.Name.PadRight(nameWidth) + $"   {employee.HoursWorkedRegular,12}   {employee.HoursWorkedOvertime,14}   {employee.HourlyRate,12:C2}   {employee.HourlyRateOvertime,13:C2}   {employee.WeeklySalary,13:C2}   {employee.WeeklySalary/sumSalary,18:P}");
            }
            output.WriteLine();
            output.WriteLine("Average Salary   Total Expense");
            output.WriteLine(new string('-', nameWidth + 17));
            output.WriteLine($"{AverageSalary,14:C2}   {sumSalary,13:C2}");
        }
    }
}
