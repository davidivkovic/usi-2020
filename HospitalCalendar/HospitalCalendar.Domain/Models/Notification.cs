
using System;
using System.Collections.Generic;
using System.Text;
using HospitalCalendar.Domain.Models;

public class Notification : DomainObject{

	public Notification() {
	}

	public string Message { get; set; }

	public NotificationStatus Status { get; set; }

	public DateTime Timestamp { get; set; }

}