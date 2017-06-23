using System;
using System.Reflection;

namespace Eventus.DocumentDb.Config
{
    public class AggregateConfig
    {
        public AggregateConfig(TypeInfo aggregateType, int offerThroughput, int snapshotThroughput)
        {
            if (offerThroughput <= 0) throw new ArgumentOutOfRangeException(nameof(offerThroughput));
            if (snapshotThroughput <= 0) throw new ArgumentOutOfRangeException(nameof(snapshotThroughput));

            AggregateType = aggregateType ?? throw new ArgumentNullException(nameof(aggregateType));
            OfferThroughput = offerThroughput;
            SnapshotOfferThroughput = snapshotThroughput;
        }

        public TypeInfo AggregateType { get; }

        public int SnapshotOfferThroughput { get; }

        public int OfferThroughput { get; }
    }
}