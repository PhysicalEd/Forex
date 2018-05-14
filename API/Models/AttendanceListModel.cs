using System.Collections.Generic;
using Contracts.Entities.Data;

namespace API.Models
{
    public class AttendanceListModel
    {
        public List<AttendanceSummary> Attendances { get; set; }
    }
}
