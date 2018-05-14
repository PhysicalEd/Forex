using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using Contracts.Entities.Data;

// CAUTION - AUTOMATICALLY GENERATED
// These classes have been automatically generated from the core database. Use partial classes to create custom properties
// Code Generation Template developed by Ben Liebert, 8 May 2018 
namespace Data.EF.Database {

	/// <summary>
    /// Creates lists of the various data-bound entities, which can later be injected into test routines etc - bypassing any database dependencies
    /// </summary>
    public partial class CodeFirstModel
    {

		private void LoadTables(DbModelBuilder modelBuilder)
		{

			// CSVImport
			modelBuilder.Entity<CSVImport>().HasKey(x => x.CSVImportID);
			modelBuilder.Entity<CSVImport>().ToTable("CSVImport");
			modelBuilder.Entity<CSVImport>().Property(x => x.CSVImportID);
			modelBuilder.Entity<CSVImport>().Property(x => x.CSVFileName);
			modelBuilder.Entity<CSVImport>().Property(x => x.DateDescription);
			modelBuilder.Entity<CSVImport>().Property(x => x.ImportedRows);
			modelBuilder.Entity<CSVImport>().Property(x => x.PairID);
		
			// Pair
			modelBuilder.Entity<Pair>().HasKey(x => x.PairID);
			modelBuilder.Entity<Pair>().ToTable("Pair");
			modelBuilder.Entity<Pair>().Property(x => x.PairID);
			modelBuilder.Entity<Pair>().Property(x => x.PairDescription);
		
			// Tick
			modelBuilder.Entity<Tick>().HasKey(x => x.TickID);
			modelBuilder.Entity<Tick>().ToTable("Tick");
			modelBuilder.Entity<Tick>().Property(x => x.TickID);
			modelBuilder.Entity<Tick>().Property(x => x.TickTime);
			modelBuilder.Entity<Tick>().Property(x => x.Bid);
			modelBuilder.Entity<Tick>().Property(x => x.Ask);
			modelBuilder.Entity<Tick>().Property(x => x.PairID);
			modelBuilder.Entity<Tick>().Property(x => x.CSVImportID);
		
		}

        public CSVImport GetOrCreateCSVImport(int? CSVImportID) {
            if (CSVImportID.GetValueOrDefault(0) > 0) return this.CSVImport.FirstOrDefault(x => x.CSVImportID == CSVImportID);
            var newItem = new CSVImport();
			this.CSVImport.AddObject(newItem);
            return newItem;
        }

		public IObjectSet<Contracts.Entities.Data.CSVImport> CSVImport        {
            get { 
				return Core.CreateObjectSet<CSVImport>();
				// var set = Core.CreateObjectSet<CSVImport>();
	            // set.MergeOption = MergeOption.NoTracking;
	            // return set;
			}
        }
	

        public Pair GetOrCreatePair(int? PairID) {
            if (PairID.GetValueOrDefault(0) > 0) return this.Pair.FirstOrDefault(x => x.PairID == PairID);
            var newItem = new Pair();
			this.Pair.AddObject(newItem);
            return newItem;
        }

		public IObjectSet<Contracts.Entities.Data.Pair> Pair        {
            get { 
				return Core.CreateObjectSet<Pair>();
				// var set = Core.CreateObjectSet<Pair>();
	            // set.MergeOption = MergeOption.NoTracking;
	            // return set;
			}
        }
	

        public Tick GetOrCreateTick(int? TickID) {
            if (TickID.GetValueOrDefault(0) > 0) return this.Tick.FirstOrDefault(x => x.TickID == TickID);
            var newItem = new Tick();
			this.Tick.AddObject(newItem);
            return newItem;
        }

		public IObjectSet<Contracts.Entities.Data.Tick> Tick        {
            get { 
				return Core.CreateObjectSet<Tick>();
				// var set = Core.CreateObjectSet<Tick>();
	            // set.MergeOption = MergeOption.NoTracking;
	            // return set;
			}
        }
	
	}
}
