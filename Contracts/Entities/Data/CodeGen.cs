


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// CAUTION - AUTOMATICALLY GENERATED
// These classes have been automatically generated from the core database. Use partial classes to create custom properties
// Code Generation Template developed by Ben Liebert, 8 May 2018 
namespace Contracts.Entities.Data {

	/// <summary>
	/// Interface of our generic CSVImport object
	/// </summary>
	public partial class CSVImport
	{
		public int CSVImportID { get; set; }
		public string CSVFileName { get; set; }
		public string DateDescription { get; set; }
		public int ImportedRows { get; set; }
		public int PairID { get; set; }
		public bool IsNew { get { return (this.CSVImportID == 0); } }
	}
		
	/// <summary>
	/// Interface of our generic Pair object
	/// </summary>
	public partial class Pair
	{
		public int PairID { get; set; }
		public string PairDescription { get; set; }
		public bool IsNew { get { return (this.PairID == 0); } }
	}
		
	/// <summary>
	/// Interface of our generic Tick object
	/// </summary>
	public partial class Tick
	{
		public int TickID { get; set; }
		public DateTime TickTime { get; set; }
		public decimal Bid { get; set; }
		public decimal Ask { get; set; }
		public int PairID { get; set; }
		public int CSVImportID { get; set; }
		public bool IsNew { get { return (this.TickID == 0); } }
	}
		

}




