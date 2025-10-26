using System;
using System.Collections.Generic;
using System.Net;
namespace HealthCareSystem;


public class Appointment
{
    public User Patient;
    public User Personnel;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public TradeStatus Status = TradeStatus.Pending;


    public Appointment(User patient, User personnel)
    {
        Patient = patient;
        Personnel = personnel;
    }

    public string ToSaveString()
    {
        string result = $"{patient.Email}, {Personnel.Email}, {StartDate: 2025-10-20, 09:00}";
        foreach (Journal journal in Journals)
        {
            result += $"{journal.Name}, {journal.Owner.Email}";
        }
        result += Status;
        return result;
    }


    public void Accept()
    {
        Status = TradeStatus.Accepted;

        foreach (Journal journal in Journals)
        {
            if (journal.Owner == Patient)
            {
                journal.Owner = Personnel;
            }
            else if (journal.owner == Personnel)
            {
                journal.Owner = Patient;
            }
        }
    }

    public void Deny()
    {
        Status = TradeStatus.Denied;
    }
}

enum TradeStatus
{
    Pending,
    Accepted,
    Denied,
}
