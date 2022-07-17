using Identity.Application.Common.Interfaces;

namespace Identity.Infrastructure.Services;

public class DateTimeService : IDateTime
{
    public DateTime Now => DateTime.Now;
}
