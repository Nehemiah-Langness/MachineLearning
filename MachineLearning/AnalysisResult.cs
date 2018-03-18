using Domain.Contracts;

namespace Analysis
{
    public class AnalysisResult<T>
    {
        public AnalysisResult(T outcome, ResultStatus result)
        {
            Outcome = outcome;
            Result = result;
        }

        public T Outcome { get; }
        public ResultStatus Result { get; }
    }
}