using System.Collections.Generic;
using Contracts.Entities.Data;

namespace API.Models
{
    public class AttendanceEditorModelResult
    {
        public AttendanceSummary Attendance { get; set; } = new AttendanceSummary();
        public List<PlayerAttendanceSummary> PlayerAttendances { get; set; } = new List<PlayerAttendanceSummary>();
        public List<PlayerAttendanceSummary> TermPlayerAttendances { get; set; } = new List<PlayerAttendanceSummary>();
        public List<PlayerAttendanceSummary> CasualPlayerAttendances { get; set; } = new List<PlayerAttendanceSummary>();
        public List<PlayerSummary> PlayerList { get; set; } = new List<PlayerSummary>();
    }
}
