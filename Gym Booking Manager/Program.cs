using Gym_Booking_Manager;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;

#if DEBUG
[assembly: InternalsVisibleTo("Tests")]
#endif
namespace Gym_Booking_Manager
{
    internal class Program
    {

        static void Main(string[] args)
        {
            /* ---------------------------------------------------->START<------------------------------------------------------------------ */
            GymDatabaseContext DB = new GymDatabaseContext();
            Console.WriteLine("Read and print from DB Test\n");
            foreach (Admin s in DB.Read<Admin>())
            {
                Console.WriteLine(s);
            }
            Console.WriteLine("\nPRESS ANY KEY TO RUN THE NEXT TEST...");
            Console.ReadKey();
            Console.WriteLine("\nUpdate and print from DB Test\n");
            foreach (var c in DB.Read<Customer>())
            {
                if (c.name == "Kustömer")
                {
                    Console.WriteLine($"Original values:\n" +
                                      $"{c.name},{c.phone},{c.email},{c.Id}");
                }
            }
            foreach (var c in DB.Read<Customer>())
            {
                if (c.name == "Kustömer")
                {
                    DB.Update(new Customer("NewCust", "070-01 10 01", "Updatedmail@test.se", c.Id), c);
                }
            }
            Console.WriteLine();
            foreach (var c in DB.Read<Customer>())
            {
                if (c.name == "NewCust")
                {
                    Console.WriteLine($"Updated values:\n" +
                                      $"{c.name},{c.phone},{c.email},{c.Id}");
                }
            }
            Console.WriteLine("\nPRESS ANY KEY TO RUN THE NEXT TEST...");
            Console.ReadKey();
            Console.WriteLine("\nDELETE USER and print from DB Test\n");
            Console.WriteLine("Current users in DB:\n");
            foreach (var c in DB.Read<Customer>())
            {
                Console.WriteLine(c);
            }
            Console.WriteLine();
            foreach (var c in DB.Read<Customer>())
            {
                if (c.name == "NewCust")
                {
                    DB.Delete(c);
                }
            }
            Console.WriteLine("\nUsers after deletion:\n");
            foreach (var c in DB.Read<Customer>())
            {
                Console.WriteLine(c);
            }
            Console.WriteLine("\nALL TESTS COMPLETED, PRESS ANY KEY TO CONTINUE");
            Console.ReadKey();
            Console.Clear();
            User user = null;
            Console.WriteLine("Hi, there! Welcome to our program. Hope you will enjoy it.");
            Console.WriteLine("Do you want to create a new user(1) or select a existing one(2)?");
            int answer = Convert.ToInt32(Console.ReadLine());
            if (answer == 1)
            {
                user = User.Create(DB);
                Console.WriteLine($"This is your ID(save it--Or dont. see if i care.):{user.Id}");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }
            else if (answer == 2)
            {
                Console.WriteLine("Enter ID:");
                string Id = Console.ReadLine(); // <--- Green.
                user = User.GetUserById(DB, Id);
                if (user == null) { Console.WriteLine($"No user exist with a matching Id ({Id})"); }
            }
            else
            {  // Repeats until user write correct input
                while (answer != 1 && answer != 2)
               {
                    Console.WriteLine("Please choose option (1) or (2).");
                    int userChoice = Convert.ToInt32(Console.ReadLine());
                    if (userChoice == 1)
                    {
                        user = User.Create(DB);
                        Console.WriteLine($"This is your ID(save it--Or dont. see if i care.):{user.Id}");
                        Console.WriteLine("Press any key to continue...");
                        Console.ReadKey();
                    }
                    else if (userChoice == 2)
                    {
                        Console.WriteLine("Enter ID:");
                        string id = Console.ReadLine();
                        user = User.GetUserById(DB, id);
                        if (user == null) { Console.WriteLine("Get Bent.(No user)."); }
                    }
                }
            }

             /* ---------------------------------------------------->MENU<------------------------------------------------------------------ */
            // Make sure 'user' is not null.
            while (user != null)
            {
                int perm = user.GetPerm();
                do
                {
                    // Switch to select the correct menu depending on the 'user'.
                    switch (perm)
                    {
                        case 0:
                            // User.cs L.389
                            Customer.CustomerMenu(DB, user);
                            break;
                        case 1:
                            // User.cs L.174
                            Staff.StaffMenu(DB, user);
                            break;
                        case 2:
                            // User.cs L.242
                            Admin.AdminMenu(DB, user);
                            break;
                        case 3:
                            // User.cs L.310
                            Service.ServiceMenu(DB, user);
                            break;
                        default:
                            Console.WriteLine("Things but also stuff.");
                            break;
                    }
                } while (perm != -1);
            }
        }
    }
}
/*
            // user.GetType().Name;
            Customer ruben = new Customer("Ruben");
            Customer david = new Customer("David");
            Customer adam = new Customer("Adam");
            TimeSlot timeSlot = new TimeSlot(new DateTime(2023, 10, 10, 20, 30, 00), new DateTime(2023, 10, 10, 21, 00, 00));
            Console.WriteLine(timeSlot);
            Instructor instructor = new Instructor("Tim");
            Space hall = new Space(0, "Hall");
            Equipment treadmill = new Equipment(1);
            GroupActivity activity1 = new GroupActivity("A1", 2, timeSlot, instructor, hall, treadmill);
            activity1.SignUp(ruben);
            activity1.SignUp(david);
            activity1.SignUp(adam);
            foreach(Customer c in activity1.Participants)
                {
                    Console.WriteLine(c.name);
                }
            Customer ruben = new Customer("Ruben", "011-131313", "ruben@test.se", Guid.NewGuid());
            Customer david = new Customer("David", "011-131313", "asd", Guid.NewGuid());
            //userDB.Create<Customer>(ruben);
            //userDB.Create<Customer>(david);
            //userDB.Create<Customer>(ruben);
            DateTime t1 = new DateTime(year: 2023, month: 10, day: 10, hour: 12, minute: 00, second: 00);
            DateTime t2 = new DateTime(year: 2023, month: 10, day: 10, hour: 13, minute: 00, second: 00);
            DateTime t3 = new DateTime(year: 2023, month: 10, day: 10, hour: 14, minute: 00, second: 00);
            DateTime t4 = new DateTime(year: 2023, month: 10, day: 10, hour: 15, minute: 00, second: 00);
            TimeSlot time = new TimeSlot(t1);
            TimeSlot time2 = new TimeSlot(t2);
            Space space = new Space(Space.Category.Hall, "Hall");
            Space space2 = new Space(Space.Category.Studio, "Studio 2");
            //Equipment gym = new Equipment(Equipment.Category.Gym, "Gym", 10, true);
            //DB.Create<Equipment>(gym);
            //gym.MakeReservation(Reservation.Category.Gym, ruben, time);
            //gym.ViewTimeTable();
            //DB.Create<Equipment>(treadmill);
            //treadmill.MakeReservation(0,ruben, time);
            //treadmill.ViewTimeTable();
            Instructor instructor = new Instructor(Instructor.Category.PT, "Tom");
            Equipment gym = new Equipment(Equipment.Category.Gym, "Gym", 10, true);
            DB.Create<Equipment>(gym);
            //DB.Create<Instructor>(instructor);
            //instructor.MakeReservation(Reservation.Category.PT, ruben, time);
            //instructor.ViewTimeTable();
            Equipment equipmentT = new Equipment(Equipment.Category.Treadmill, "treadmill", 3);
            GroupActivity groupActivity = new GroupActivity("Activity 1", 2, time2, instructor, space,equipmentT);
            GroupSchedule groupSchedule = new GroupSchedule(groupActivity);
            //GroupActivity groupActivity2 = new GroupActivity("Activity 2", 3, time, instructor, space2, gym);
            //groupSchedule.AddActivity(groupActivity2);
            //groupActivity2.SignUp(ruben);
            //groupActivity.SignUp(ruben);
            //groupActivity.SignUp(david);
            //groupSchedule.ViewSchedule();
            Space space1 = new Space(Space.Category.Lane, "Lane");
            //GroupActivity groupActivity1 = new GroupActivity("test", 3, time2, instructor, space1, treadmill);
            //groupSchedule.AddActivity(groupActivity1);
            //groupActivity.SignUp(ruben);
            //groupSchedule.ViewSchedule();

            foreach (Equipment e in DB.Read<Equipment>())
            {
                Console.WriteLine(e);
            }
            foreach (Instructor i in DB.Read<Instructor>())
            {
                Console.WriteLine(i);
            }
            foreach (Space s in DB.Read<Space>())
            {
                Console.WriteLine(s);
            }
            foreach (Customer c in DB.Read<Customer>())
            {
                Console.WriteLine(c);
            }

            DateTime d1 = new DateTime(year: 2023, month: 10, day: 10, hour: 12, minute: 00, second:00);
            DateTime d2 = new DateTime(year: 2023, month: 10, day: 11);
            TimeSlot timeSlot = new TimeSlot(d1);
            TimeSlot timeSlot2 = new TimeSlot(d2);
            TimeSlot timeSlot3 = new TimeSlot(DateTime.Now);
            Console.WriteLine(timeSlot);
            Console.WriteLine(timeSlot2);
            Console.WriteLine(timeSlot3);
            //Console.WriteLine(userDB.Read<Customer>("Id", "00e19739-d644-4f05-a042-fec4a9ca946a"));
            //User user = User.ChooseUserType(name, phone, email, id, choice);
            //userDB.Create<Customer>(user as Customer);
*/