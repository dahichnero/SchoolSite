using SchkalkaB.Domain.Services;
using SchkalkaB.Models;

namespace SchkalkaB.Infrastructure
{
    public class TimetableService : ITimetableService
    {
        private readonly IRepository<TimeTable>? timetables;

        public async Task UpdateTimetable(TimeTable timeTable)
        {
            await timetables.UpdateAsync(timeTable);
        }
    }
}
