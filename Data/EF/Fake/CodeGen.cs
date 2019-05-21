using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using Contracts.Entities.Data;

// CAUTION - AUTOMATICALLY GENERATED
// These classes have been automatically generated from the core database. Use partial classes to create custom properties
// Code Generation Template developed by Ben Liebert, 21 May 2019 
namespace Data.EF.Fake {

	/// <summary>
    /// Creates lists of the various data-bound entities, which can later be injected into test routines etc - bypassing any database dependencies
    /// </summary>
    public partial class FakeData
    {

		private void CreateIdentityvalues()
		{
			this.Candle.Where(x => x.CandleID == 0).ToList().ForEach(x => x.CandleID = this.Candle.Max(y => y.CandleID) + 1);
			this.CandleType.Where(x => x.CandleTypeID == 0).ToList().ForEach(x => x.CandleTypeID = this.CandleType.Max(y => y.CandleTypeID) + 1);
			this.CSVImport.Where(x => x.CSVImportID == 0).ToList().ForEach(x => x.CSVImportID = this.CSVImport.Max(y => y.CSVImportID) + 1);
			this.FileDownloadStatus.Where(x => x.FileDownloadStatusID == 0).ToList().ForEach(x => x.FileDownloadStatusID = this.FileDownloadStatus.Max(y => y.FileDownloadStatusID) + 1);
			this.Pair.Where(x => x.PairID == 0).ToList().ForEach(x => x.PairID = this.Pair.Max(y => y.PairID) + 1);
			this.Tick.Where(x => x.TickID == 0).ToList().ForEach(x => x.TickID = this.Tick.Max(y => y.TickID) + 1);
		}
    
		public Candle GetOrCreateCandle(int? CandleID) {
            Candle item = this.Candle.FirstOrDefault(x => x.CandleID == CandleID);
			if (item == null){
				item = new Candle();
				this.Candle.AddObject(item);
			}
			return item;
        }

		private IObjectSet<Candle> _Candle = null;
        public IObjectSet<Candle> Candle {
            get {
                if (_Candle == null) {
                    var result = new List<Candle>();
                    _Candle = new FakeObjectSet<Candle>(result);
                }
                return _Candle;
            }
        }
	
		public CandleType GetOrCreateCandleType(int? CandleTypeID) {
            CandleType item = this.CandleType.FirstOrDefault(x => x.CandleTypeID == CandleTypeID);
			if (item == null){
				item = new CandleType();
				this.CandleType.AddObject(item);
			}
			return item;
        }

		private IObjectSet<CandleType> _CandleType = null;
        public IObjectSet<CandleType> CandleType {
            get {
                if (_CandleType == null) {
                    var result = new List<CandleType>();
                    _CandleType = new FakeObjectSet<CandleType>(result);
                }
                return _CandleType;
            }
        }
	
		public CSVImport GetOrCreateCSVImport(int? CSVImportID) {
            CSVImport item = this.CSVImport.FirstOrDefault(x => x.CSVImportID == CSVImportID);
			if (item == null){
				item = new CSVImport();
				this.CSVImport.AddObject(item);
			}
			return item;
        }

		private IObjectSet<CSVImport> _CSVImport = null;
        public IObjectSet<CSVImport> CSVImport {
            get {
                if (_CSVImport == null) {
                    var result = new List<CSVImport>();
                    _CSVImport = new FakeObjectSet<CSVImport>(result);
                }
                return _CSVImport;
            }
        }
	
		public FileDownloadStatus GetOrCreateFileDownloadStatus(int? FileDownloadStatusID) {
            FileDownloadStatus item = this.FileDownloadStatus.FirstOrDefault(x => x.FileDownloadStatusID == FileDownloadStatusID);
			if (item == null){
				item = new FileDownloadStatus();
				this.FileDownloadStatus.AddObject(item);
			}
			return item;
        }

		private IObjectSet<FileDownloadStatus> _FileDownloadStatus = null;
        public IObjectSet<FileDownloadStatus> FileDownloadStatus {
            get {
                if (_FileDownloadStatus == null) {
                    var result = new List<FileDownloadStatus>();
                    _FileDownloadStatus = new FakeObjectSet<FileDownloadStatus>(result);
                }
                return _FileDownloadStatus;
            }
        }
	
		public Pair GetOrCreatePair(int? PairID) {
            Pair item = this.Pair.FirstOrDefault(x => x.PairID == PairID);
			if (item == null){
				item = new Pair();
				this.Pair.AddObject(item);
			}
			return item;
        }

		private IObjectSet<Pair> _Pair = null;
        public IObjectSet<Pair> Pair {
            get {
                if (_Pair == null) {
                    var result = new List<Pair>();
                    _Pair = new FakeObjectSet<Pair>(result);
                }
                return _Pair;
            }
        }
	
		public Tick GetOrCreateTick(int? TickID) {
            Tick item = this.Tick.FirstOrDefault(x => x.TickID == TickID);
			if (item == null){
				item = new Tick();
				this.Tick.AddObject(item);
			}
			return item;
        }

		private IObjectSet<Tick> _Tick = null;
        public IObjectSet<Tick> Tick {
            get {
                if (_Tick == null) {
                    var result = new List<Tick>();
                    _Tick = new FakeObjectSet<Tick>(result);
                }
                return _Tick;
            }
        }
	
	}
}
