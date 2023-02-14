﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;




namespace Gym_Booking_Manager
{
    // Interface for classes of objects that Customers can reserve.
    // I.E MakeReservation(User that does the reservvation), CancelReservation(TODO), ViewTimeTable(TODO)
    internal interface IReservable
    {
        void MakeReservation(Reservation.Category category, IReservingEntity owner, DateTime start, DateTime end);
        void CancelReservation();
        void ViewTimeTable(); // start and end as arguments?
    }
}
