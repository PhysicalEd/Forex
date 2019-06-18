


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// CAUTION - AUTOMATICALLY GENERATED
// These classes have been automatically generated from the core database. Use partial classes to create custom properties
// Code Generation Template developed by Ben Liebert, 12 Jun 2019 
namespace Contracts.Entities.Data {

	/// <summary>
	/// Interface of our generic Candle object
	/// </summary>
	public partial class Candle
	{
		public int CandleID { get; set; }
		public int CandleTypeID { get; set; }
		public DateTime FromTime { get; set; }
		public int HighTickID { get; set; }
		public int LowTickID { get; set; }
		public int OpenTickID { get; set; }
		public int CloseTickID { get; set; }
		public int PairID { get; set; }
		public bool IsNew { get { return (this.CandleID == 0); } }
	}
		
	/// <summary>
	/// Interface of our generic CandleType object
	/// </summary>
	public partial class CandleType
	{
		public int CandleTypeID { get; set; }
		public string CandleTypeCode { get; set; }
		public string Description { get; set; }
		public int NumberOfMinutes { get; set; }
		public bool IsNew { get { return (this.CandleTypeID == 0); } }
	}
		
	/// <summary>
	/// Interface of our generic CSVImport object
	/// </summary>
	public partial class CSVImport
	{
		public int CSVImportID { get; set; }
		public string DateDescription { get; set; }
		public int ImportedRows { get; set; }
		public int PairID { get; set; }
		public string CSVFileName { get; set; }
		public DateTime? FromDate { get; set; }
		public DateTime? ToDate { get; set; }
		public bool IsNew { get { return (this.CSVImportID == 0); } }
	}
		
	/// <summary>
	/// Interface of our generic FileDownloadStatus object
	/// </summary>
	public partial class FileDownloadStatus
	{
		public int FileDownloadStatusID { get; set; }
		public string FileName { get; set; }
		public bool? IsCompleted { get; set; }
		public string DateDescription { get; set; }
		public bool? IsDownloadable { get; set; }
		public int? PairID { get; set; }
		public bool? IsUnzipped { get; set; }
		public bool IsNew { get { return (this.FileDownloadStatusID == 0); } }
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




