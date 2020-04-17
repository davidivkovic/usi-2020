
using System;
using HospitalCalendar.Domain.Models;

public class User : DomainObject{

	public User() {
	}

    public string Username { get; set; }

	public string Password { get; set; }

	public string FirstName { get; set; }

	public string LastName { get; set; }

	//public bool IsActive { get; set; }

}