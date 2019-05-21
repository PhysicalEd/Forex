using System;
using System.Threading.Tasks;
using Contracts.Entities.Data;
using Contracts.Enums;

namespace Contracts
{
    public interface IFileDownloader
    {
        void Initialize(DateTime fromDate, DateTime toDate, BasePair pair);
        Task ProcessFiles();
        Task UnzipFiles();

    }
}
