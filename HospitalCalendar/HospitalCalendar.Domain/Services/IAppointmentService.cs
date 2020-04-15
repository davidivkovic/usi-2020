﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HospitalCalendar.Domain.Services
{
    interface IAppointmentService : IDataService<Appointment>
    {
        Task<Appointment> GetAllByTimeFrame(DateTime start, DateTime end);
    }
}
