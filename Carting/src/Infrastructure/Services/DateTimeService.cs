using Carting.Application.Common.Interfaces;

namespace Carting.Infrastructure.Services;

public class DateTimeService : IDateTime
{
    public DateTime Now => DateTime.Now;
}
