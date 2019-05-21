using System;
using System.Collections.Generic;
using Contracts.Entities;
using Contracts.Entities.Data;
using Contracts.Enums;

namespace Contracts.DataManagers
{
    public interface IFileDownloadManager
    {
        FileDownloadStatus GetFileDownloadStatusUsingDateDescriptionAndBasePair(string dateDesc, BasePair pair);
        FileDownloadStatus SaveFileDownloadStatus(int? fileDownloadStatusID, BasePair pair, string fileName, bool? isCompleted, string dateDesc, bool? isDownloadable, bool? isUnzipped);
        void SetFileDownloadStatusToComplete(int fileDownloadStatusID);
        void SetFileDownloadStatusToUnzipped(int fileDownloadStatusID);
        List<FileDownloadStatus> GetallUnzippedFiles();


    }
}
