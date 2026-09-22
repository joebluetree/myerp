using MyErp.Common.Interfaces;

namespace MyErp.Common.Time;

public sealed class SystemDateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}
