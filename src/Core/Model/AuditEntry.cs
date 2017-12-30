using System;

namespace ClearMeasure.Bootcamp.Core.Model
{
    public class AuditEntry
    {
        private string _beginStatusCode;
        private string _endStatusCode;

        public AuditEntry()
        {
        }

        public AuditEntry(Employee employee, DateTime date, ExpenseReportStatus beginStatus, ExpenseReportStatus endStatus, ExpenseReport report)
        {
            Employee = employee;
            EmployeeName = Employee.GetFullName();
            Date = date;
            _beginStatusCode = beginStatus.Code;
            _endStatusCode = endStatus.Code;
            ExpenseReport = report;
        }

        public Employee Employee { get; set; }
        public DateTime Date { get; set; }
        public string EmployeeName { get; set; }

        public ExpenseReportStatus BeginStatus
        {
            get { return ExpenseReportStatus.FromCode(_beginStatusCode); }
            set { _beginStatusCode = value.Code; }
        }

        public ExpenseReportStatus EndStatus
        {
            get { return ExpenseReportStatus.FromCode(_endStatusCode); }
            set { _endStatusCode = value.Code; }
        }

        public ExpenseReport ExpenseReport { get; set; }
        public Guid Id { get; set; }
    }
}