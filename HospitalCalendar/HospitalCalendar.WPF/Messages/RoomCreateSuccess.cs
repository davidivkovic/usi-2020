using HospitalCalendar.Domain.Models;

namespace HospitalCalendar.WPF.Messages
{
    public class RoomCreateSuccess
    {
        public Room Room { get; }
        public RoomCreateSuccess(Room room)
        {
            Room = room;
        }
    }
}
