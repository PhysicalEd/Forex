namespace Contracts.Entities.Data
{
	public class PlayerSummary
	{
        public int PlayerID { get; set; }
        public int PersonID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string DisplayName { get { return string.Format("{0} {1}", this.FirstName, this.LastName); } }
        public bool IsDeleted { get; set; }

        // Term player
        public int TermPlayerID { get; set; }
        public int TermID { get; set; }
        public bool IsCasual { get { return !(this.TermPlayerID > 0); } }
        public bool BondPaid { get; set; }
        public int? TermDue { get; set; }
        public int? TermOwing { get; set; }
    }
}
