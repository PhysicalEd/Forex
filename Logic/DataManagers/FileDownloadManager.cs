using System.Collections.Generic;
using System.Linq;
using Contracts.DataManagers;
using Contracts.Entities.Data;
using Contracts.Enums;
using Contracts.Exceptions;

namespace Logic.DataManagers
{
    public class FileDownloadManager : IFileDownloadManager
    {
        public FileDownloadStatus GetFileDownloadStatusUsingDateDescriptionAndBasePair(string dateDesc, BasePair pair)
        {
            using (var cxt = DataStore.CreateDataStore())
            {
                var pairDesc = pair.ToString();
                //var result = cxt.FileDownloadStatus.FirstOrDefault(x => x.DateDescription == dateDesc && x. == (int)pair);
                var data = (
                    from fds in cxt.FileDownloadStatus
                    join p in cxt.Pair on fds.PairID equals p.PairID
                    where p.PairDescription == pairDesc
                    select fds
                ).FirstOrDefault();
                return data;
            }
        }

        public List<FileDownloadStatus> GetallUnzippedFiles()
        {
            using (var cxt = DataStore.CreateDataStore())
            {
                var result = cxt.FileDownloadStatus.Where(x => x.IsUnzipped == null || x.IsUnzipped == false).ToList();
                return result;
            }
        }

        public FileDownloadStatus SaveFileDownloadStatus(int? fileDownloadStatusID, BasePair pair, string fileName, bool? isCompleted, string dateDesc, bool? isDownloadable, bool? isUnzipped)
        {
            using (var cxt = DataStore.CreateDataStore())
            {
                // Check if pair exists first...
                var cxtPair = cxt.Pair.FirstOrDefault(x => x.PairID == (int)pair);
                if (cxtPair == null) cxtPair = Dependency.Dependency.Resolve<IPairManager>().SavePair(pair);

                var status = cxt.FileDownloadStatus.FirstOrDefault(x => x.FileDownloadStatusID == fileDownloadStatusID);
                if (status == null) status = cxt.GetOrCreateFileDownloadStatus(null);
                status.FileName = fileName;
                status.IsCompleted = isCompleted;
                status.DateDescription = dateDesc;
                status.IsDownloadable = isDownloadable;
                status.PairID = cxtPair.PairID;
                status.IsUnzipped = isUnzipped;
                cxt.SubmitChanges();
                return status;
            }
        }

        public void SetFileDownloadStatusToComplete(int fileDownloadStatusID)
        {
            using (var cxt = DataStore.CreateDataStore())
            {
                var status = cxt.FileDownloadStatus.FirstOrDefault(x => x.FileDownloadStatusID == fileDownloadStatusID);
                if (status == null) throw new UserException("The record you have attempted to update does not exist");
                status.IsCompleted = true;
                cxt.SubmitChanges();
            }
        }

        public void SetFileDownloadStatusToUnzipped(int fileDownloadStatusID)
        {
            using (var cxt = DataStore.CreateDataStore())
            {
                var status = cxt.FileDownloadStatus.FirstOrDefault(x => x.FileDownloadStatusID == fileDownloadStatusID);
                if (status == null) throw new UserException("The record you have attempted to update does not exist");
                status.IsUnzipped = true;
                cxt.SubmitChanges();
            }
        }
    }
}


