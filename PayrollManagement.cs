using System;
using static System.Console;
using System.IO;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Net;
using System.Collections.Generic;
class PayrollManagement 
{
	static void Main()
	{
		
	}
}

enum PaymentMethod
{
	//Defines the available payment methods for employees.
	check,
	DirectDeposit
}

class Employee
{
	public int Id {get; private set;}
	public string Name {get; private set;}
	public double HourlyRate {get; private set;}
	public double HoursWorked {get; private set;}
	public PaymentMethod PaymentMethod {get; set;}

	public Employee (int id, string name, double hourlyRate, PaymentMethod paymentMethod)
	{
		// HoursWorked is not included in the constructor because when a new employee is registered, we do not yet know how many hours they have worked.
		

		if(id <= 0)
		{
			throw new ArgumentException("Employee ID must be greater than zero.");
		}
		if (hourlyRate < 0)
		{
			throw new ArgumentException("Hourly rate cannot be negative.");
		}
		if (!Enum.IsDefined(typeof(PaymentMethod), paymentMethod))
		{
			throw new ArgumentException("Invalid payment method.");
		}

		ValidateName(name);

		Id = id;
		Name = name;
		HourlyRate = hourlyRate;
		PaymentMethod = paymentMethod;
		HoursWorked = 0;
		// All new employees start with 0 hours worked. Their hours will be registered later using the RegisterHoursWorked() method.
	}	

		private static void ValidateName(string name)
		{
		if (string.IsNullOrWhiteSpace(name))
		{
			throw new ArgumentException("Employee name cannot be empty.");
		}
		}

		public void RegisterHoursWorked(double hours)
		{
			if (hours < 0 || hours > 168)
		{
			throw new ArgumentException("Hours mus be between ) and 168.");
		}
			HoursWorked = hours;
		}
	
	public void UpdateName(string name)
	{
		ValidateName(name);
		Name = name;
	}

	public void UpdateHourlyRate(double hourlyRate)
	{
		if(hourlyRate < 0)
		{
			throw new ArgumentException("Hourly rate cannot be negative.");
		}
		HourlyRate = hourlyRate;
	}

	public void UpdatePaymentMethod(PaymentMethod paymentMethod)
	{
		if (!Enum.IsDefined(typeof(PaymentMethod), paymentMethod))
		{
			throw new ArgumentException("Invalid payment method. ");
		}
		PaymentMethod = paymentMethod;
	}
    public override string ToString()
    {
        return $"ID: {Id} | Name: {Name} | Hourly rate: {HourlyRate:C} | Hours: {HoursWorked} | Payment: {PaymentMethod}";
    }	
	
}

class EmployeeFile
{
	private const string FILE_NAME = "Employees.txt";
	private const char DELIMITER = '|';

	public static void SaveEmployees(List<Employee> employees)
	{
		using (StreamWriter writer = new StreamWriter(FILE_NAME))
		{
			foreach(Employee employee in employees)
			{
				string line = employee.Id + DELIMITER + employee.Name + DELIMITER + employee.HourlyRate.ToString(CultureInfo.InvariantCulture) + DELIMITER + employee.HoursWorked.ToString(CultureInfo.InvariantCulture) + DELIMITER + employee.PaymentMethod;
				writer.WriteLine(line);
			}
		}
	}

	public static List<Employee> LoadEmployees()
	{
		List<Employee> employees = new List<Employee>();

		if (!File.Exists(FILE_NAME))
		{
			return employees;
		}

		using (StreamReader reader = new StreamReader(FILE_NAME))
		{
			string? line;
			int lineNumber = 0;

			while((line = reader.ReadLine()) != null)
			{
				lineNumber++;
				string[] fields = line.Split(DELIMITER);

				if(fields.Length != 5)
				{
					throw new FormatException($"Invalid employee record on line {lineNumber}");
				}

				int id = int.Parse(fields[0]);
				string name = fields[1];

				double hourlyRate = double.Parse(fields[2],(CultureInfo.InvariantCulture));
				double hoursWorked = double.Parse(fields[3], CultureInfo.InvariantCulture);
				PaymentMethod paymentMethod = (PaymentMethod)Enum.Parse(typeof(PaymentMethod), fields[4]);

				Employee.RegisterHoursWorked(hoursWorked);
				employees.Add(Employee);
			}
		}
		return employees;
	}
}