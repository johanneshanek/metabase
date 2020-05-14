using System;
using System.Collections.Generic;
using Icon.Events;

namespace Icon.Infrastructure.Aggregate
{
    public interface IEventSourcedAggregate : IAggregate, IValidatable
    {
        public DateTime Timestamp { get; set; }

        // For protecting the state, i.e. conflict prevention
        public int Version { get; }

        public bool Deleted { get; }

        public bool IsVirgin();
    }
}