
using HospitalCalendar.Domain.Services;
using HospitalCalendar.Domain.Services.CalendarEntryServices;

namespace HospitalCalendar.EntityFramework.Services
{
    public class SynchronizationService : ISynchronizationService
    {
        private readonly IRenovationService _renovationService;
        public SynchronizationService(IRenovationService renovationService)
        {
            _renovationService = renovationService;
        }

        public async void Synchronize()
        {
            await _renovationService.Synchronize();
        }
    }
}
