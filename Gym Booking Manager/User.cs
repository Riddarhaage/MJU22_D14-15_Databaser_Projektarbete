using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static Gym_Booking_Manager.Space;
using static Gym_Booking_Manager.LocalStorage;
using static Gym_Booking_Manager.Equipment;

#if DEBUG
[assembly: InternalsVisibleTo("Tests")]
#endif
namespace Gym_Booking_Manager
{
    internal abstract class User : ICSVable, IReservingEntity
    {
        public string name { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        protected int perm;
        public Guid Id { get; set; }

        protected User(string name)
        {
            this.name = name;
            this.phone = "0";
            this.email = "test@test";
            this.perm = 0;
            this.Id = new Guid();
        }
        protected User()
        {
            this.name = name;
            this.phone = "0";
            this.email = "test@test";
            this.perm = 0;
            this.Id = new Guid();
        }
        public string CSVify()
        {
            throw new NotImplementedException();
        }

        public static User Create(GymDatabaseContext DB)
        {
            Console.WriteLine("Enter your name: ");
            string name = Console.ReadLine();
            Console.WriteLine("Enter your phone number: ");
            string phone = Console.ReadLine();
            Console.WriteLine("Enter your email: ");
            string email = Console.ReadLine();
            Guid id = Guid.NewGuid();
            Console.WriteLine("Enter your choice (0 for Customer, 1 for Staff, 2 for Admin, 3 for Service): ");
            int choice = int.Parse(Console.ReadLine());
            //Console.WriteLine(userDB.Read<Customer>("Id", "00e19739-d644-4f05-a042-fec4a9ca946a"));
            User user = ChooseUserType(name, phone, email, id, choice, DB);

            return user;
        }
        public static void Remove(GymDatabaseContext DB, string id)
        {
            // throw new NotImplementedException();
            bool DEL = true;
            GetUserById(DB, id, DEL);
        }

        public static User GetUserById(GymDatabaseContext DB, string id, bool DEL = false)
        {
            //throw new NotImplementedException();
            // Search for the User with the right Id. "ArgumentOutOfRangeException"
            // I could replace this by checking if 'customers.count != 0'..  ¯\_(ツ)_/¯
            try
            {
                List<Customer> customers = DB.Read<Customer>("Id", id);
                User user = customers[0];
                if (DEL == true) { DB.Delete<Customer>(user as Customer); }
                return user;
            }
            catch (ArgumentOutOfRangeException)
            {
                try
                {
                    List<Staff> staff = DB.Read<Staff>("Id", id);
                    User user = staff[0];
                    if (DEL == true) { DB.Delete<Staff>(user as Staff); }
                    return user;
                }
                catch (ArgumentOutOfRangeException)
                {
                    try
                    {
                        List<Admin> admins = DB.Read<Admin>("Id", id);
                        User user = admins[0];
                        if (DEL == true) { DB.Delete<Admin>(user as Admin); }
                        return user;
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        try
                        {
                            List<Service> services = DB.Read<Service>("Id", id);
                            User user = services[0];
                            if (DEL == true) { DB.Delete<Service>(user as Service); }
                            return user;
                        }
                        catch (ArgumentOutOfRangeException)
                        {
                            // the ID was not found in any of the tables
                            Console.WriteLine("Not found");
                            User user = null;
                            return user;
                        }
                    }
                }
            }

        }

        public static User ChooseUserType(string name, string phone, string email, Guid Id, int choice, GymDatabaseContext DB)
        {
            switch (choice)
            {
                case 0:
                    User C_User = new Customer(name, phone, email, Id);
                    DB.Create<Customer>(C_User as Customer);
                    return C_User;
                case 1:
                    User S_User = new Staff(name, phone, email, Id);
                    DB.Create<Staff>(S_User as Staff);
                    return S_User;
                case 2:
                    User A_User = new Admin(name, phone, email, Id);
                    DB.Create<Admin>(A_User as Admin);
                    return A_User;
                case 3:
                    User SE_User = new Service(name, phone, email, Id);
                    DB.Create<Service>(SE_User as Service);
                    return SE_User;
                default:
                    throw new ArgumentException("Invalid choice");
            }
        }
        public virtual int GetPerm()
        {
            return perm;
        }
    }

    internal class Staff : User, ICSVable, IComparable<Staff>
    {
        public Staff()
        {

        }
        public Staff(string name, string phone, string email, Guid Id) : base(name)
        {
            this.perm = 1;
            this.phone = phone;
            this.email = email;
            this.Id = Id;
        }
        public Staff(Dictionary<string, string> dict) : base(dict[nameof(name)])
        {
            this.name = dict[nameof(name)];
            this.phone = dict[nameof(phone)];
            this.email = dict[nameof(email)];
            this.Id = Guid.Parse(dict[nameof(Id)]);
            this.perm = int.Parse(dict[nameof(perm)]);
        }
        public new string CSVify()
        {
            return $"{nameof(name)}:{this.name},{nameof(phone)}:{this.phone},{nameof(email)}:{this.email},{nameof(Id)}:{this.Id},{nameof(perm)}:{this.perm}";
        }
        public override string ToString()
        {
            return $"{name}, {phone}, {email}, {Id}";
        }
        public int CompareTo(Staff? other)
        {
            // If other is not a valid object reference, this instance is greater.
            if (other == null) return 1;
            // Sort primarily on category.
            if (this.Id != other.Id) return this.Id.CompareTo(other.Id);
            // When category is the same, sort on name.
            return this.name.CompareTo(other.name);
        }
        public static void StaffMenu(GymDatabaseContext DB, User user)
        {
            int option = 0;
            do
            {
                Console.WriteLine("Option 1");
                Console.WriteLine("Option 2");
                Console.WriteLine("Option 3");
                Console.WriteLine("4. Exit");
                Console.Write("Enter your option: ");
                option = Convert.ToInt32(Console.ReadLine());

                switch (option)
                {
                    case 1:
                        //BookEquipment(DB, user);
                        break;
                    case 2:
                        Console.WriteLine("Option 2 TEST");
                        GroupActivity activity = new GroupActivity();
                        Console.WriteLine();
                        activity.CreateActivity(DB);
                        break;
                    case 3:
                        Console.WriteLine("Option 3");
                        break;
                    case 4:
                        Console.WriteLine("Exiting the menu...");
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Invalid option, no workie.");
                        break;
                }
                Console.WriteLine();
            } while (option != 4);
        }
    }

    internal class Admin : User, ICSVable, IComparable<Admin>
    {
        public Admin()
        {

        }
        public Admin(string name, string phone, string email, Guid Id) : base(name)
        {
            this.perm = 2;
            this.phone = phone;
            this.email = email;
            this.Id = Id;
        }
        public Admin(Dictionary<string, string> dict) : base(dict[nameof(name)])
        {
            this.name = dict[nameof(name)];
            this.phone = dict[nameof(phone)];
            this.email = dict[nameof(email)];
            this.Id = Guid.Parse(dict[nameof(Id)]);
            this.perm = int.Parse(dict[nameof(perm)]);
        }
        public string CSVify()
        {
            return $"{nameof(name)}:{this.name},{nameof(phone)}:{this.phone},{nameof(email)}:{this.email},{nameof(Id)}:{this.Id},{nameof(perm)}:{this.perm}";
        }
        public override string ToString()
        {
            return $"{name}, {phone}, {email}, {Id}";
        }
        public int CompareTo(Admin? other)
        {
            // If other is not a valid object reference, this instance is greater.
            if (other == null) return 1;
            // Sort primarily on category.
            if (this.Id != other.Id) return this.Id.CompareTo(other.Id);
            // When category is the same, sort on name.
            return this.name.CompareTo(other.name);
        }
        public static void AdminMenu(GymDatabaseContext DB, User user)
        {
            int option = 0;
            do
            {
                Console.WriteLine("Option 1");
                Console.WriteLine("Option 2");
                Console.WriteLine("Option 3");
                Console.WriteLine("4. Exit");
                Console.Write("Enter your option: ");
                option = Convert.ToInt32(Console.ReadLine());

                switch (option)
                {
                    case 1:
                        //BookEquipment(DB, user);
                        break;
                    case 2:
                        Console.WriteLine("Option 2");
                        
                        break;
                    case 3:
                        Console.WriteLine("Option 3");
                        break;
                    case 4:
                        Console.WriteLine("Exiting the menu...");
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Invalid option, no workie.");
                        break;
                }
                Console.WriteLine();
            } while (option != 4);
        }
        // ViewLogg() method NYI
    }

    internal class Service : User, ICSVable, IComparable<Service>
    {
        public Service(string name, string phone, string email, Guid Id) : base(name)
        {
            this.perm = 3;
            this.phone = phone;
            this.email = email;
            this.Id = Id;
        }
        public Service(Dictionary<string, string> dict) : base(dict[nameof(name)])
        {
            this.name = dict[nameof(name)];
            this.phone = dict[nameof(phone)];
            this.email = dict[nameof(email)];
            this.Id = Guid.Parse(dict[nameof(Id)]);
            this.perm = int.Parse(dict[nameof(perm)]);
        }
        public string CSVify()
        {
            return $"{nameof(name)}:{this.name},{nameof(phone)}:{this.phone},{nameof(email)}:{this.email},{nameof(Id)}:{this.Id},{nameof(perm)}:{this.perm}";
        }
        public override string ToString()
        {
            return $"{name}, {phone}, {email}, {Id}";
        }
        public int CompareTo(Service? other)
        {
            // If other is not a valid object reference, this instance is greater.
            if (other == null) return 1;
            // Sort primarily on category.
            if (this.Id != other.Id) return this.Id.CompareTo(other.Id);
            // When category is the same, sort on name.
            return this.name.CompareTo(other.name);
        }
        public static void ServiceMenu(GymDatabaseContext DB, User user)
        {
            int option = 0;
            do
            {
                Console.WriteLine("Option 1");
                Console.WriteLine("Option 2");
                Console.WriteLine("Option 3");
                Console.WriteLine("4. Exit");
                Console.Write("Enter your option: ");
                option = Convert.ToInt32(Console.ReadLine());

                switch (option)
                {
                    case 1:
                        //BookEquipment(DB, user);
                        break;
                    case 2:
                        Console.WriteLine("Option 2");
                        break;
                    case 3:
                        Console.WriteLine("Option 3");
                        break;
                    case 4:
                        Console.WriteLine("Exiting the menu...");
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Invalid option, no workie.");
                        break;
                }
                Console.WriteLine();
            } while (option != 4);
        }
        //public method RepairItem() ------> NYI: Not Yet Implemented
    }

    internal class Customer : User, ICSVable, IComparable<Customer>, IReservingEntity
    {
        public Customer()
        {
            this.perm = 0;
            this.phone = phone;
            this.email = email;
            this.Id = Id;
        }
        public Customer(string name, string phone, string email, Guid Id) : base(name)
        {
            this.perm = 0;
            this.phone = phone;
            this.email = email;
            this.Id = Id;
        }
        public Customer(string name, string phone, string email) : base(name)
        {
            this.name = name;
            this.phone = phone;
            this.email = email;
        }
        public Customer(Dictionary<string, string> dict) : base(dict[nameof(name)])
        {
            this.name = dict[nameof(name)];
            this.phone = dict[nameof(phone)];
            this.email = dict[nameof(email)];
            this.Id = Guid.Parse(dict[nameof(Id)]);
            this.perm = int.Parse(dict[nameof(perm)]);
        }
        public int CompareTo(Customer? other)
        {
            // If other is not a valid object reference, this instance is greater.
            if (other == null) return 1;
            // Sort primarily on category.
            if (this.Id != other.Id) return this.Id.CompareTo(other.Id);
            // When category is the same, sort on name.
            return this.name.CompareTo(other.name);
        }

        public string CSVify()
        {
            return $"{nameof(name)}:{this.name},{nameof(phone)}:{this.phone},{nameof(email)}:{this.email},{nameof(Id)}:{this.Id},{nameof(perm)}:{this.perm}";
        }
        public void PrintAllInfo()
        {
            Console.WriteLine($"Name: {name}, Phone: {phone}, Email: {email}, Id: {Id}");
        }
        public override string ToString()
        {
            return $"{name}, Id:{Id}";
        }
        public static void CustomerMenu(GymDatabaseContext DB, User user)
        {
            int option = 0;
            do
            {
                Console.WriteLine("Option 1");
                Console.WriteLine("Option 2");
                Console.WriteLine("Option 3");
                Console.WriteLine("4. Exit");
                Console.Write("Enter your option: ");
                option = Convert.ToInt32(Console.ReadLine());

                switch (option)
                {
                    case 1:
                        Equipment.BookEquipment(DB, user);
                        break;
                    case 2:
                        Console.WriteLine("Option 2");
                        break;
                    case 3:
                        Console.WriteLine("Option 3");
                        break;
                    case 4:
                        Console.WriteLine("Exiting the menu...");
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Invalid option, no workie.");
                        break;
                }
                Console.WriteLine();
            } while (option != 4);
        }
    }

}