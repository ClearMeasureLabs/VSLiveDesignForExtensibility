using System;
using System.Collections.Generic;
using System.Linq;

namespace ClearMeasure.Bootcamp.Core.Model
{
    public class ExpenseReport
    {
        private IList<AuditEntry> _auditEntries = new List<AuditEntry>();
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public ExpenseReportStatus Status { get; set; }
        public Employee Submitter { get; set; }
        public Employee Approver { get; set; }
        public string Number { get; set; }
        // New Properties
        public int? MilesDriven { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? FirstSubmitted { get; set; }
        public DateTime? LastSubmitted { get; set; }
        public DateTime? LastWithdrawn { get; set; }
        public DateTime? LastCancelled { get; set; }
        public DateTime? LastApproved { get; set; }
        public DateTime? LastDeclined { get; set; }
        public decimal? Total { get; set; }

        public ExpenseReport()
        {
            Status = ExpenseReportStatus.Draft;
            Description = "";
            Title = "";
        }

        public string FriendlyStatus
        {
            get { return GetTextForStatus(); }
        }

        protected string GetTextForStatus()
        {
            return Status.ToString();
        }

        public override string ToString()
        {
            return "ExpenseReport " + Number;
        }

        public void ChangeStatus(ExpenseReportStatus status)
        {
            Status = status;
        }

        public void ChangeStatus(Employee employee, DateTime date, ExpenseReportStatus beginStatus, ExpenseReportStatus endStatus)
        {
            var auditItem = new AuditEntry(employee, date, beginStatus, endStatus, this);
            _auditEntries.Add(auditItem);
            Status = endStatus;
        }

        public IEnumerable<AuditEntry> AuditEntries => _auditEntries.ToArray();

        public void AddAuditEntry(AuditEntry auditEntry)
        {
            auditEntry.ExpenseReport = this;
            _auditEntries.Add(auditEntry);
        }

        protected bool Equals(ExpenseReport other)
        {
            return Id.Equals(other.Id);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ExpenseReport) obj);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        private sealed class IdEqualityComparer : IEqualityComparer<ExpenseReport>
        {
            public bool Equals(ExpenseReport x, ExpenseReport y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (ReferenceEquals(x, null)) return false;
                if (ReferenceEquals(y, null)) return false;
                if (x.GetType() != y.GetType()) return false;
                return x.Id.Equals(y.Id);
            }

            public int GetHashCode(ExpenseReport obj)
            {
                return obj.Id.GetHashCode();
            }
        }

        public static IEqualityComparer<ExpenseReport> IdComparer { get; } = new IdEqualityComparer();
    }
}