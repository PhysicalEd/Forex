using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using Contracts.Entities.Data;

// CAUTION - AUTOMATICALLY GENERATED
// These classes have been automatically generated from the core database. Use partial classes to create custom properties
// Code Generation Template developed by Ben Liebert, 16 Feb 2019 
namespace Data.EF.Fake {

	/// <summary>
    /// Creates lists of the various data-bound entities, which can later be injected into test routines etc - bypassing any database dependencies
    /// </summary>
    public partial class FakeData
    {

		private void CreateIdentityvalues()
		{
			this.CSVImport.Where(x => x.CSVImportID == 0).ToList().ForEach(x => x.CSVImportID = this.CSVImport.Max(y => y.CSVImportID) + 1);
			this.CSVTest.Where(x => x.CSVTestID == 0).ToList().ForEach(x => x.CSVTestID = this.CSVTest.Max(y => y.CSVTestID) + 1);
			this.Pair.Where(x => x.PairID == 0).ToList().ForEach(x => x.PairID = this.Pair.Max(y => y.PairID) + 1);
			this.Tick.Where(x => x.TickID == 0).ToList().ForEach(x => x.TickID = this.Tick.Max(y => y.TickID) + 1);
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
	
		public CSVTest GetOrCreateCSVTest(int? CSVTestID) {
            CSVTest item = this.CSVTest.FirstOrDefault(x => x.CSVTestID == CSVTestID);
			if (item == null){
				item = new CSVTest();
				this.CSVTest.AddObject(item);
			}
			return item;
        }

		private IObjectSet<CSVTest> _CSVTest = null;
        public IObjectSet<CSVTest> CSVTest {
            get {
                if (_CSVTest == null) {
                    var result = new List<CSVTest>();
                    _CSVTest = new FakeObjectSet<CSVTest>(result);
                }
                return _CSVTest;
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
