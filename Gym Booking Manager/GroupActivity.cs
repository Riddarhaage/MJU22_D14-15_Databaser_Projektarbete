using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Gym_Booking_Manager
{
    internal class GroupActivity
    {
        private string activityID;
        private int participantLimit;
        private List<Customer> participants;
        private TimeSlot timeSlot;
        private Instructor instructor;
        private Space space;
        public Equipment equipment { get; set; }

        public Instructor Instructor
        {
            get => instructor;
        }

        public List<Customer> Participants
        { 
            get => participants;
        }
        public TimeSlot TimeSlot
        {
            get => timeSlot;
        }
        public GroupActivity()
        {
            
        }
        public GroupActivity(string activityID, int participantLimit, TimeSlot timeSlot, Instructor instructor, Space space, Equipment equipment)
        {
            this.activityID = activityID;
            this.participantLimit = participantLimit;
            this.timeSlot = new TimeSlot(timeSlot.Start);
            this.instructor = instructor;
            this.space = space;
            this.equipment = equipment;
            this.participants = new List<Customer>();
        }
        public void SignUp(Customer customer)
        {
            if (participants.Count < participantLimit)
            {
                participants.Add(customer);
            }
            else
            {
                Console.WriteLine("Activity is full");
            }
        }
        public void SignOut(Customer customer)
        {
            participants.Remove(customer);
        }
        public string PartisipantsToString()
        {
            string partisipants = "";
            foreach (Customer customer in participants)
            {
                partisipants += customer.name + "|";
            }
            return partisipants;
        }
        //For future use 
        public override string ToString()
        {
            return $"Activity: {activityID}\ninstructor: {Instructor.name}\n{timeSlot}\nSpace: {space.Name}" +
                $"\nParticipants: {PartisipantsToString()} (Free slots left: {participantLimit - participants.Count})\nEquipment: {equipment.name}";
        }
        public  GroupActivity CreateActivity(GymDatabaseContext DB)
        {
            Console.WriteLine("Enter activity ID:");
            activityID = Console.ReadLine();
            Console.WriteLine("Enter participant limit:");
            participantLimit = int.Parse(Console.ReadLine());
            Console.WriteLine("Enter start time:");
            timeSlot = new TimeSlot(DateTime.Parse(Console.ReadLine()));
            Console.WriteLine("Enter instructor name:");
            int i = 1;
            foreach (Instructor instructor in DB.Read<Instructor>())
            {
                Console.WriteLine($"{i}. {instructor.name}");
                i++;
            }
            int instructorChoice = int.Parse(Console.ReadLine());
            instructor = DB.Read<Instructor>()[instructorChoice - 1];
            i = 1;
            foreach (Space space in DB.Read<Space>())
            {
                Console.WriteLine($"{i}. {space.Name}");
                i++;
            }
            int spaceChoice = int.Parse(Console.ReadLine());
            space = DB.Read<Space>()[spaceChoice - 1];
            i = 1;
            foreach (Equipment equipment in DB.Read<Equipment>())
            {
                Console.WriteLine($"{i}. {equipment.name}");
                i++;
            }
            int equipmentChoice = int.Parse(Console.ReadLine());
            equipment = DB.Read<Equipment>()[equipmentChoice - 1];
            var GA = new GroupActivity(activityID, participantLimit, timeSlot, instructor, space, equipment);
            GroupSchedule groupSchedule = new GroupSchedule(GA);
            groupSchedule.ViewSchedule();
            return GA;
        }
    }
   
}
