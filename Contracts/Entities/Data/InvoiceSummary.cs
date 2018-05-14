using System;

namespace Contracts.Entities.Data
{
    public class InvoiceSummary
    {
        public int InvoiceID { get; set; }
        public int TermID { get; set; }
        public string TermName { get; set; }

        public DateTime TermStart { get; set; }
        public DateTime? TermEnd { get; set; }

        public bool OrganizerIncluded { get; set; }

        public DateTime InvoiceDate { get; set; }
        public int TotalAmount { get; set; }
        public int NumberOfSessions { get; set; }

        public DateTime? WhenPaid { get; set; }
        public string TermStartValue => this.TermStart.ToString("MMMM d yyyy");
        public string TermEndValue => TermEnd?.ToString("MMMM d yyyy") ?? "";

        public string TermRange => string.Format("{0} - {1}", this.TermStartValue, this.TermEndValue);
    }
}
