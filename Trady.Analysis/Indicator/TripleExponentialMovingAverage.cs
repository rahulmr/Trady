using System;
using System.Collections.Generic;
using System.Linq;
using Trady.Analysis.Infrastructure;
using Trady.Core;
using Trady.Core.Infrastructure;

namespace Trady.Analysis.Indicator
{
    public class TripleExponentialMovingAverage<TInput, TOutput> : NumericAnalyzableBase<TInput, decimal?, TOutput>
    {
        private readonly GenericMovingAverage _ema1;
        private readonly GenericMovingAverage _ema2;
        private readonly GenericMovingAverage _ema3;

        public TripleExponentialMovingAverage(IEnumerable<TInput> inputs, Func<TInput, decimal?> inputMapper, int periodCount) : base(inputs, inputMapper)
        {
            _ema1 = new GenericMovingAverage(
                i => inputs.Select(inputMapper).ElementAt(i),
                Smoothing.Ema(periodCount),
                inputs.Count());

            var _ema1Result = _ema1.Compute();
            _ema2 = new GenericMovingAverage(i => _ema1Result.ElementAt(i),
                Smoothing.Ema(periodCount),
                _ema1Result.Count);

            var _ema2Result = _ema2.Compute();
            _ema3 = new GenericMovingAverage(i => _ema2Result.ElementAt(i),
                Smoothing.Ema(periodCount),
                _ema2Result.Count);

            PeriodCount = periodCount;
        }

        public int PeriodCount { get; }

        protected override decimal? ComputeByIndexImpl(IReadOnlyList<decimal?> mappedInputs, int index) => 3 * (_ema1[index] - _ema2[index]) + _ema3[index];
    }

    public class TripleExponentialMovingAverageByTuple : TripleExponentialMovingAverage<decimal?, decimal?>
    {
        public TripleExponentialMovingAverageByTuple(IEnumerable<decimal?> inputs, int periodCount)
            : base(inputs, i => i, periodCount)
        {
        }

        public TripleExponentialMovingAverageByTuple(IEnumerable<decimal> inputs, int periodCount)
            : this(inputs.Cast<decimal?>().ToList(), periodCount)
        {
        }
    }

    public class TripleExponentialMovingAverage : TripleExponentialMovingAverage<IOhlcv, AnalyzableTick<decimal?>>
    {
        public TripleExponentialMovingAverage(IEnumerable<IOhlcv> inputs, int periodCount)
            : base(inputs, i => i.Close, periodCount)
        {
        }
    }
    public class TripleExponentialMovingAverageByTick : TripleExponentialMovingAverage<ITickTrade, AnalyzableTick<decimal?>>
    {
        public TripleExponentialMovingAverageByTick(IEnumerable<ITickTrade> inputs, int periodCount)
            : base(inputs, i => i.Price, periodCount)
        {
        }
    }
    
}
