using System.Collections.Generic;
using Contracts.Entities.Data;

namespace API.Models
{
    public class PlayerListModel
    {
        public List<TermPlayerSummary> TermPlayers { get; set; }
    }
}
