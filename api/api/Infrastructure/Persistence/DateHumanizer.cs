using api.Domain.Persistence;
using Humanizer;

namespace api.Infrastructure.Persistence;

public class DateHumanizer : IDateHumanizer
{
    public string HumanizeDate(DateTime date)
    {
        return date.Humanize();
    }
}