using System;
using System.Collections.Generic;

using Trady.Analysis.Indicator;
using Trady.Analysis.Infrastructure;
using Trady.Core.Infrastructure;

namespace Trady.Analysis.Extension
{
    public static class TickTradeExtension
    {
        public static IReadOnlyList<AnalyzableTick<decimal?>> Tema(this IEnumerable<ITickTrade> candles, int periodCount, int? startIndex = null, int? endIndex = null)
          => new TripleExponentialMovingAverageByTick(candles, periodCount).Compute(startIndex, endIndex);
    }
}
