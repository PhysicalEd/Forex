using System;

namespace Contracts.Entities.Data
{
	public class TermSummary
	{
        public int TermID { get; set; }
        public string Name { get; set; }
        public DateTime TermStart { get; set; }
        public DateTime? TermEnd { get; set; }
        public int BondAmount { get; set; }
        public int CasualRate { get; set; }
        public bool IncludeOrganizer { get; set; }
	    public int NumberOfPlayers { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public bool IsInvoiced { get; set; }
        public string TermStartStr { get; set; }
	    public string TermStartValue => this.TermStart.ToString("MMMM d yyyy");
	    public string TermEndValue => TermEnd?.ToString("MMMM d yyyy") ?? "";
	    public string TermRange => string.Format("{0} - {1}", this.TermStartValue, this.TermEndValue);





    }
}
