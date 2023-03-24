using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gym_Booking_Manager
{
    internal class Instructor: IReservable, ICSVable, IComparable<Instructor>
    {
        private Category category { get; set; }
        public string name { get; set; }
        private readonly Calendar calendar;
        private int quantity = 1;


        public Instructor()
        {

        }
        public Instructor(Category category, String name)
        {
            this.name = name;
            this.category = category;
            this.calendar = new Calendar();
        }
        // Every class T to be used for DbSet<T> needs a constructor with this parameter signature. Make sure the object is properly initialized.
        public Instructor(Dictionary<String, String> constructionArgs)
        {
            if (!Enum.TryParse(constructionArgs[nameof(category)], out Category parsedCategory))
            {
                throw new ArgumentException("Couldn't parse a valid Space.Category value.", nameof(category));
            }
            this.category = parsedCategory;

            this.calendar = new Calendar();
        }
        public int CompareTo(Instructor? other)
        {
            // If other is not a valid object reference, this instance is greater.
            if (other == null) return 1;
            // Sort primarily on category.
            if (this.category != other.category) return this.category.CompareTo(other.category);
            // When category is the same, sort on name.
            return this.name.CompareTo(other.name);
        }
        public override string ToString()
        {
            return $"{name}, {category}"; // TODO: Don't use CSVify. Make it more readable.
        }

        public string CSVify()
        {
            return $"{nameof(category)}:{category.ToString()},{nameof(name)}:{name}";
        }

        public enum Category
        {
            Coach,
            PT
        }
        public void ViewTimeTable()
        {
            // Fetch
            List<Reservation> tableSlice = this.calendar.GetSlice();
            // Show?
            foreach (Reservation reservation in tableSlice)
            {
                Console.WriteLine(reservation);
            }

        }
        public void MakeReservation(Reservation.Category category, IReservingEntity owner, TimeSlot timeslot)
        {
            // Check if the reservation is valid
            if (timeslot.Start > timeslot.End)
            {
                throw new ArgumentException("Start time must be before end time.");
            }
            // Check if the reservation is available
            if (this.calendar.isAvailable(timeslot.Start, timeslot.End))
            {
                // Make the reservation
                this.calendar.AddReservation(new Reservation(category, this.quantity, owner, timeslot));
            }
            else
            {
                throw new ArgumentException("The reservation is not available.");
            }
        }

        public void CancelReservation()
        {

        }
        //added to be able to print instructor name
        //public override string ToString()
        //{
        //    return $"{name}";
        //}
    }
}
