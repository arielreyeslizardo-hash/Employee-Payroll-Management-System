using System;
using System.Collections.Generic;
using System.Text;

namespace Payroll {
    /// <summary>
    /// A simple wrapper for an unsized integer that cannot be zero.
    /// </summary>
    internal readonly struct EmployeeID {
        public uint Value { get; }
        public EmployeeID(uint id) {
            if (id == 0) {
                throw new ArgumentException("Employee ID must be non-zero.");
            }
            Value = id;
        }

        public static implicit operator uint(EmployeeID id) => id.Value;
    }
}
