using System;
using System.Collections.Generic;
using System.Text;
using HospitalCalendar.Domain.Models;

namespace HospitalCalendar.Domain.Models
{
    public class Specialization : DomainObject
    {

        public Specializations SingleSpecialization { get; set; }
    }
}
