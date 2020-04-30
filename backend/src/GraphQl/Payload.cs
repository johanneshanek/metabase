using GreenDonut;
using ValueObjects = Icon.ValueObjects;

namespace Icon.GraphQl
{
    public abstract class Payload
    {
        public ValueObjects.Timestamp RequestTimestamp { get; } // TODO? Better name it `timestamp` or `fetchTimestamp` or `loadTimestamp` or `queryTimestamp` or ...

        public Payload(ValueObjects.Timestamp requestTimestamp)
        {
            RequestTimestamp = requestTimestamp;
        }
    }
}