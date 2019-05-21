using Contracts.Enums;

namespace Contracts.Entities.Candle
{
	public class CandleTypeSummary
	{
        public CandleTypes CandleType { get; set; }
        public int NumberOfMinutes { get; set; }
	    public string Description { get; set; }
    }
}
