using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Payroll {
    internal class Employee {
        public uint Id {
            get;
            set {
                if (value == 0) {
                    throw new ArgumentException("Employee ID must be non-zero.");
                }
                field = value;
            }
        }
        public string Name {
            get;
            set
            {
                if (string.IsNullOrWhiteSpace(value)) {
                    throw new ArgumentException("Employee name cannot be empty.");
                }
                field = value;
            }
        }
        public decimal HourlyRate {
            get;
            set {
                if (value < 0) {
                    throw new ArgumentException("Hourly rate cannot be negative.");
                }
                field = value;
            }
        }
        public decimal HourlyRateOvertime {
            get => 1.5m * HourlyRate;
        }
        public decimal HoursWorked {
            get;
            set {
                if (value < 0 || value > 168) {
                    throw new ArgumentException("Hours must be between 0 and 168.");
                }
                field = value;
            }
        }
        public decimal HoursWorkedRegular {
            get => decimal.Min(HoursWorked,40);
        }
        public decimal HoursWorkedOvertime {
            get => decimal.Max(HoursWorked - 40, 0);
        }
        public decimal WeeklySalary {
            get => HourlyRate * HoursWorkedRegular + HourlyRateOvertime * HoursWorkedOvertime;
        }
        public PaymentMethod PaymentMethod { get; set; }

        public Employee(uint id, string name, decimal hourlyRate, PaymentMethod paymentMethod) {
            Id = id;
            Name = name;
            HourlyRate = hourlyRate;
            PaymentMethod = paymentMethod;
            // HoursWorked is not included in the constructor because when a new employee is registered, we do not yet know how many hours they have worked.
            HoursWorked = 0;
        }

        public override string ToString() {
            return $"ID: {Id} | Name: {Name} | Hourly rate: {HourlyRate:C} | Hours: {HoursWorked} | Payment: {PaymentMethod}";
        }
    }


    /// <summary>
    /// The available payment methods for employees.
    /// </summary>
    enum PaymentMethod {
        Check,
        DirectDeposit
    }
}
