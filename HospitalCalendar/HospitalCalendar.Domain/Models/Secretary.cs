
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class Secretary : User {

	public Secretary() {
	}

	public virtual ICollection<AppointmentRequestNotification> AppointmentRequestNotifications { get; set; }
				   
	public virtual ICollection<SurgeryNotification> SurgeryNotifications { get; set; }
				   
	public virtual ICollection<AppointmentChangeRequestNotification> AppointmentChangeRequestNotifications { get; set; }

}