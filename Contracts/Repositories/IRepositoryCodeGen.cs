using System;
using System.Data.Entity.Core.Objects;
using Contracts.Entities.Data;

// CAUTION - AUTOMATICALLY GENERATED
// These classes have been automatically generated from the core database. Use partial classes to create custom properties
// Code Generation Template developed by Ben Liebert, 8 May 2018 
namespace Contracts.Repositories {

	 /// <summary>
    /// Abstracts the data-storage mode away from the caller
    /// </summary>
    public partial interface IRepository
    {
    
		Entities.Data.CSVImport GetOrCreateCSVImport(int? CSVImportID);
        IObjectSet<Entities.Data.CSVImport> CSVImport { get; }
		Entities.Data.Pair GetOrCreatePair(int? PairID);
        IObjectSet<Entities.Data.Pair> Pair { get; }
		Entities.Data.Tick GetOrCreateTick(int? TickID);
        IObjectSet<Entities.Data.Tick> Tick { get; }
	}
}
