namespace Contracts.Entities.Data
{
	public class TermPlayerSummary
	{
        public int TermPlayerID { get; set; }
        public int PlayerID { get; set; }
        public int TermID { get; set; }
        public int PersonID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string DisplayName { get { return string.Format("{0} {1}", this.FirstName, this.LastName); } }
        public bool IsCasual { get { return !(this.TermPlayerID > 0); } }
        public bool BondPaid { get; set; }
        public int? TermDue { get; set; }
        public int? TermOwing { get; set; }
    }
}
