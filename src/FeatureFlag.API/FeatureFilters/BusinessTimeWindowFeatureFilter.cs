using Microsoft.Extensions.Configuration;
using Microsoft.FeatureManagement;
using System;
using System.Threading.Tasks;

namespace FeatureFlag.API.FeatureFilters
{
    [FilterAlias("BusinessTimeWindow")]
    public class BusinessTimeWindowFeatureFilter : IFeatureFilter
    {
        public async Task<bool> EvaluateAsync(FeatureFilterEvaluationContext context)
        {
            var parameters = context.Parameters.Get<BusinessTimeWindowParameters>();

            var hourStart = int.Parse(parameters.Start);
            var hourEnd = int.Parse(parameters.End);
            var hourNow = int.Parse($"{DateTime.Now.Hour}{DateTime.Now.Minute}");

            if (hourNow >= hourStart && hourNow <= hourEnd)
            {
                return true;
            }

            return false;            
        }
    }

    public class BusinessTimeWindowParameters
    {
        public string Start { get; set; }
        public string End { get; set; }
    }
}
