using System;
using System.Collections.Generic;
using System.Text;

namespace Payroll {
    internal class Employee {
        public uint Id { get; private set; }
        public string Name { get; private set; }
        public double HourlyRate { get; private set; }
        public double HoursWorked { get; private set; }
        public PaymentMethod PaymentMethod { get; set; }

        public Employee(uint id, string name, double hourlyRate, PaymentMethod paymentMethod) {
            // HoursWorked is not included in the constructor because when a new employee is registered, we do not yet know how many hours they have worked.


            if (id <= 0) {
                throw new ArgumentException("Employee ID must be greater than zero.");
            }
            if (hourlyRate < 0) {
                throw new ArgumentException("Hourly rate cannot be negative.");
            }
            if (!Enum.IsDefined(typeof(PaymentMethod), paymentMethod)) {
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

        private static void ValidateName(string name) {
            if (string.IsNullOrWhiteSpace(name)) {
                throw new ArgumentException("Employee name cannot be empty.");
            }
        }

        public void RegisterHoursWorked(double hours) {
            if (hours < 0 || hours > 168) {
                throw new ArgumentException("Hours must be between 0 and 168.");
            }
            HoursWorked = hours;
        }

        public void UpdateName(string name) {
            ValidateName(name);
            Name = name;
        }

        public void UpdateHourlyRate(double hourlyRate) {
            if (hourlyRate < 0) {
                throw new ArgumentException("Hourly rate cannot be negative.");
            }
            HourlyRate = hourlyRate;
        }

        public void UpdatePaymentMethod(PaymentMethod paymentMethod) {
            if (!Enum.IsDefined(typeof(PaymentMethod), paymentMethod)) {
                throw new ArgumentException("Invalid payment method. ");
            }
            PaymentMethod = paymentMethod;
        }
        public override string ToString() {
            return $"ID: {Id} | Name: {Name} | Hourly rate: {HourlyRate:C} | Hours: {HoursWorked} | Payment: {PaymentMethod}";
        }
    }

    enum PaymentMethod {
        //Defines the available payment methods for employees.
        check,
        DirectDeposit
    }
}
