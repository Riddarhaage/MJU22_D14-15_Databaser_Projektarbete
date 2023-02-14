﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using static Gym_Booking_Manager.Space;

namespace Gym_Booking_Manager
{
    internal class Equipment: IReservable, ICSVable, IComparable<Equipment>
    {
        //private enum Category with 2 subcategories large equipment and sports equipment where we list the diffrenet kinds of each equipment subcategory?
        private Category category;
        private String name;
        private readonly Calendar calendar;
        private bool largeEquipment;

        public Equipment(Category category, String name)
        {
            this.name = name;
            this.category = category;
            this.calendar = new Calendar();
        }
        // Every class T to be used for DbSet<T> needs a constructor with this parameter signature. Make sure the object is properly initialized.
        public Equipment(Dictionary<String, String> constructionArgs)
        {
            this.name = constructionArgs[nameof(name)];
            if (!Category.TryParse(constructionArgs[nameof(category)], out this.category))
            {
                throw new ArgumentException("Couldn't parse a valid Space.Category value.", nameof(category));
            }

            this.calendar = new Calendar();
        }
        public int CompareTo(Equipment? other)
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
            return this.CSVify(); // TODO: Don't use CSVify. Make it more readable.
        }

        // Every class C to be used for DbSet<C> should have the ICSVable interface and the following implementation.
        public string CSVify()
        {
            return $"{nameof(category)}:{category.ToString()},{nameof(name)}:{name}";
        }

        //This enum contains just exampels of equipment
        public enum Category
        {
            Machines, 
            Gym, 
            Bench,
            Dumbbells,
            Treadmill,
            Rackets
        }
        public void ViewTimeTable()
        {
            // Fetch
            List<Reservation> tableSlice = this.calendar.GetSlice();
            // Show?
            foreach (Reservation reservation in tableSlice)
            {
                // Do something?
            }

        }

        public void MakeReservation(IReservingEntity owner)
        {

        }

        public void CancelReservation()
        {

        }
    }
}