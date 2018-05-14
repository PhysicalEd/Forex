using System;
using System.Collections.Generic;
using System.Linq;

namespace Contracts.Entities.Data
{
	public class AttendanceSummary
	{
        public int AttendanceID { get; set; }
        public int TermID { get; set; }
        public int CasualsRate { get; set; }
        public string TermName { get; set; }
	    public List<PlayerAttendanceSummary> PlayerAttendances { get; set; } = new List<PlayerAttendanceSummary>();

        public List<PlayerAttendanceSummary> TermPlayerAttendances { get; set; } = new List<PlayerAttendanceSummary>();
        public List<PlayerAttendanceSummary> CasualPlayerAttendances { get; set; } = new List<PlayerAttendanceSummary>();
	    public int TotalPlayersAttended => this.TermPlayerAttendances.Count;
	    public int CasualsAttended => this.CasualPlayerAttendances.Count;
	    public int TermPlayersAttended => this.TermPlayerAttendances.Count;
        public DateTime AttendanceDate { get; set; }
        public bool IsDisabled { get; set; }
	    public int ExpectedAmountFromCasuals => this.CasualsAttended * this.CasualsRate;
	    public int ActualAmountFromCasuals => this.CasualPlayerAttendances.Sum(x => x.AmountPaid);

	}
}
