using SchkalkaB.Models;

namespace SchkalkaB.Domain.Services
{
    public interface ITimetableService
    {
        Task UpdateTimetable(TimeTable timeTable);
    }
}
