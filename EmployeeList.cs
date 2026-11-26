using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Payroll {
    internal class EmployeeList {
        private List<Employee> employees;
        private const string FILE_NAME = "Employees.txt";
        private const char DELIMITER = '|';

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
                    double hourlyRate = double.Parse(fields[2], (CultureInfo.InvariantCulture));
                    double hoursWorked = double.Parse(fields[3], CultureInfo.InvariantCulture);
                    PaymentMethod paymentMethod = (PaymentMethod)Enum.Parse(typeof(PaymentMethod), fields[4]);

                    Employee employee = new Employee(new EmployeeID(id), name, hourlyRate, paymentMethod);
                    employee.RegisterHoursWorked(hoursWorked);
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
    }
}
