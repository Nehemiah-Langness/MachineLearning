using Analysis.Services;
using Domain.Contracts;

namespace Analysis
{
    public class AnalysisChoice
    {
        public int MatchRate { get; set; }
        public int SuccessRate { get; set; }
        public IRule Rule { get; set; }

        public bool IsGoodChoice()
        {
            if (SuccessRate <= 50)
                return false;
            if (MatchRate <= 75)
                return false;

            return true;
        }
    }
}