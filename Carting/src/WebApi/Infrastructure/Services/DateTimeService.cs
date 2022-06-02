using Carting.WebApi.Application.Common.Interfaces;

namespace Carting.WebApi.Infrastructure.Services;

public class DateTimeService : IDateTime
{
    public DateTime Now => DateTime.Now;
}
