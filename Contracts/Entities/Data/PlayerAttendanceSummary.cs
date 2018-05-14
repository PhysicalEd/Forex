namespace Contracts.Entities.Data
{
	public class PlayerAttendanceSummary
	{
	    public int PlayerAttendanceID { get; set; }
        public int AttendanceID { get; set; }
        public int? TermID { get; set; }
        public int PlayerID { get; set; }
        public bool IsCasual { get; set; }
        public int AmountPaid { get; set; }
        public bool HasAttended { get; set; }

        // Player details
	    public string FirstName { get; set; } = "";
	    public string LastName { get; set; } = "";
        public string DisplayName {
            get { return this.FirstName + " "+this.LastName; }
        }
        

    }
}
