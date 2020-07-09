using System;
using System.IO;

namespace Contact_Manager
{
    class Program
    {
        //Contact data file path
        const string contactData = "ContactData.txt";

        static void Main(string[] args)
        {
            Program p = new Program();
            Contact[] contacts = new Contact[100];
            int contactCount = 0;
            string action = null;

            //Reading and printing starting data
            contactCount = p.ReadContactData(contacts);
            Console.WriteLine("Starting contacts data");
            p.PrintContacts(contacts, contactCount);
            Console.WriteLine();

            //Instructions for the available actions
            Console.WriteLine("Add a contact. Enter: 1");
            Console.WriteLine("Update a contact. Enter: 2");
            Console.WriteLine("Delete a contact. Enter: 3");
            Console.WriteLine("View all contacts. Enter: 4");
            Console.WriteLine("To exit program type: 0");
            Console.WriteLine();
            Console.Write("Choose an action: ");
            action = Console.ReadLine();
            Console.WriteLine();

            //Cheking if action was chosen correctly, if not asking to choose again
            while (action != "0")
            {
                contacts  = p.Action(action, contacts, contactCount);
                contactCount = p.CountContacts(contacts);
                Console.WriteLine("Add: 1");
                Console.WriteLine("Update: 2");
                Console.WriteLine("Delete: 3");
                Console.WriteLine("View: 4");
                Console.WriteLine("To exit program type: 0");
                Console.WriteLine();
                Console.Write("Choose an other action: ");
                action = Console.ReadLine();
            }

            //Rewriting the data to the same file when closing the program
            p.WriteContactsToFile(contacts, contactCount);
        }

        //Reading contact data file
        public int ReadContactData (Contact[] contacts)
        {
            int count = 0;

            string[] lines = File.ReadAllLines(contactData);
            foreach (string line in lines)
            {
                string[] values = line.Split(',');
                string name = values[0];
                string lastName = values[1];
                int phoneNumber = int.Parse(values[2]);
                string address = "-";

                if (values.Length == 5)
                {
                    address = values[3];
                }

                Contact contact = new Contact(name, lastName, phoneNumber, address);   //Creating new contacts

                contacts[count++] = contact;
            }

            return count;
        }

        //Directing actions to other functions
        public Contact[] Action (string input, Contact[] contacts, int contactCount)
        {
            Program p = new Program();

            //Add action
            if (input == "1")
            {
                contacts = p.AddContact(contacts, contactCount);
                Console.WriteLine("A contact has been added.");
                Console.WriteLine();

                return contacts;
            }

            //Update action
            else if (input == "2")
            {
                p.UpdateContact(contacts, contactCount);
                Console.WriteLine("A contact has been updated.");
                Console.WriteLine();

                return contacts;
            }

            //Delete action
            else if (input == "3")
            {
                contacts = p.DeleteContact(contacts, contactCount);
                Console.WriteLine("A contact has been deleted.");
                Console.WriteLine();

                return contacts;
            }

            //View action
            else if (input == "4")
            {
                Console.WriteLine("All contacts");
                p.PrintContacts(contacts, contactCount);

                return contacts;
            }

            //False input message
            else
            {
                Console.WriteLine("Choose an action from by writing a number between 1 and 4");
                Console.WriteLine();

                return contacts;
            }
        }

        //Creating a new contact
        public Contact CreateContact(Contact[] contacts, int contactCount)
        {
            string name = null;
            string lastName = null;
            int phoneNumber;
            string address = "-";

            Console.Write("Write contacts name: ");     //Writing a name
            name = Console.ReadLine();
            while (name == "")                          //Cheking if anything was entered, if not asking to enter something
            {
                Console.Write("Write contacts name: ");
                name = Console.ReadLine();
            }
            Console.WriteLine();

            Console.Write("Write contacts last name: ");        //Writing a last name
            lastName = Console.ReadLine();
            while (lastName == "")                      //Cheking if anything was entered, if not asking to enter something
            {
                Console.Write("Write contacts last name: ");
                lastName = Console.ReadLine();
            }
            Console.WriteLine();

            Console.Write("Write contacts phone number: ");                 //Writing a phone number
            while (!int.TryParse(Console.ReadLine(), out phoneNumber))      //Cheking if a type integer was entered and if anything was entered
            {
                Console.Write("Phone number should be made out of numbers, enter a correct phone number: ");
            }
            while (CheckForDuplicateNumbers(contacts, contactCount, phoneNumber))                   //Cheking if the entered number isnt already used by an other contact
            {
                Console.Write("This phone number is owned by another contact. Write an other one: ");
                while (!int.TryParse(Console.ReadLine(), out phoneNumber))                          //Cheking if a type integer was entered and if anything was entered
                {
                    Console.Write("Phone number should be made out of numbers, enter a correct phone number: ");
                }
            }
            Console.WriteLine();

            Console.Write("Write contacts address or press space to leave it blank: ");         //Writing a address or leaving it blank
            address = Console.ReadLine();
            if (address == "")
            {
                address = "-";
            }
            Console.WriteLine();

            Contact contact = new Contact(name, lastName, phoneNumber, address);

            return contact;
        }

        //Adding a new contact to the array of existing ones
        public Contact[] AddContact(Contact[] contacts, int contactCount)
        {
            Program p = new Program();

            contacts[contactCount++] = p.CreateContact(contacts, contactCount);

            return contacts;
        }

        //Updating the contact
        public void UpdateContact(Contact[] contacts, int contactCount)
        {
            Program p = new Program();
            int contactNumber = 0;
            int updateInput = 4;

            p.PrintContacts(contacts, contactCount);                                    //Printing current contacts and asking which one is going to be updated
            Console.Write("Write number of the contact which you want to update: ");
            while (!int.TryParse(Console.ReadLine(), out contactNumber))                //Cheking if a type integer was entered and if anything was enered
            {
                Console.Write("Write number of the contact which you want to update: ");
            }
            Console.WriteLine();

            while (contactNumber <= 0 || contactNumber > contactCount)                  //Cheking if the inputed number coresponds to a contact
            {
                Console.Write("There is no such contact, choose an existing one: ");
                while (!int.TryParse(Console.ReadLine(), out contactNumber))            //Cheking if a type integer was entered and if anything was enered at all
                {
                    Console.Write("There is no such contact, choose an existing one: ");
                }
            }

            Console.WriteLine("Name: 1");                                               //Asking what part of the contact is going to be updated
            Console.WriteLine("Last name: 2");
            Console.WriteLine("Phone number: 3");
            Console.WriteLine("Address: 4");
            Console.WriteLine("Everything: 0");
            Console.Write("Choose what do you want to update: ");
            while (!int.TryParse(Console.ReadLine(), out updateInput))                  //Cheking if a type integer was entered and if anything was enered
            {
                Console.Write("Choose what do you want to update: ");
            }
            while (updateInput < 0 || updateInput > 4)                                  //Cheking if the input corresponds to the changing parts number
            {
                Console.Write("Choose what do you want to update: ");
                updateInput = int.Parse(Console.ReadLine());
            }
            Console.WriteLine();

            for (int i = 0; i < contactCount; i++)                                      //Updating the contact
            {
                if (contactNumber == i+1)                                               //Selecting the contact that the user wants to change
                {
                    if (updateInput == 1)                                               //Updating the contacts name
                    {
                        string name = null;
                        Console.Write("Change {0} to: ", contacts[i].Name);
                        name = Console.ReadLine();
                        while (name.Length == 0)                                        //Cheking if anything was entered
                        {
                            Console.Write("Change {0} to: ", contacts[i].Name);
                            name = Console.ReadLine();
                        }
                        contacts[i].Name = name;
                        Console.WriteLine();
                    }

                    else if (updateInput == 2)                                          //Updating the last name
                    {
                        string lastName = null;
                        Console.Write("Change {0} to: ", contacts[i].LastName);
                        lastName = Console.ReadLine();;
                        while (lastName.Length == 0)                                    //Cheking if anything was entered
                        {
                            Console.Write("Change {0} to: ", contacts[i].LastName);
                            lastName = Console.ReadLine();
                        }
                        contacts[i].LastName = lastName;
                        Console.WriteLine();
                    }

                    else if (updateInput == 3)                                          //Updating the number
                    {
                        int phoneNumber;
                        Console.Write("Change {0} to: ", contacts[i].PhoneNumber);
                        while (!int.TryParse(Console.ReadLine(), out phoneNumber))      //Cheking if the input is a type integer and if anything was entered
                        {
                            Console.Write("Phone number should be made out of numbers, enter a correct phone number: ");
                        }
                        Console.WriteLine();
                        while (CheckForDuplicateNumbers(contacts, contactCount, phoneNumber))                       //Cheking if the phone number isnt used by other contacts
                        {
                            Console.Write("This phone number is owned by another contact. Write an other one: ");
                            while (!int.TryParse(Console.ReadLine(), out phoneNumber))                              //Cheking if the input is a type integer and if anything was entered
                            {
                                Console.Write("Phone number should be made out of numbers, enter a correct phone number: ");
                            }
                        }
                        contacts[i].PhoneNumber = phoneNumber;
                        Console.WriteLine();
                    }

                    else if (updateInput == 4)                                          //Updating the address
                    {
                        string address = null;
                        Console.Write("Leave address blank by pressing space or change {0} to: ", contacts[i].Address);
                        address = Console.ReadLine();
                        if (address == "")                              //Inputing "-" if nothing was entered
                        {
                            address = "-";
                        }
                        contacts[i].Address = address;
                        Console.WriteLine();
                    }

                    else if (updateInput == 0)                                      //Updating everything
                    {
                        string name = null;
                        string lastName = null;
                        int phoneNumber;
                        string address = null;

                        Console.Write("Change {0} to: ", contacts[i].Name);         //Name
                        name = Console.ReadLine();
                        while (name.Length == 0)                                    //Cheking if anything was entered
                        {
                            Console.Write("Change {0} to: ", contacts[i].Name);
                            name = Console.ReadLine();
                        }
                        contacts[i].Name = name;
                        Console.WriteLine();

                        Console.Write("Change {0} to: ", contacts[i].LastName);     //Last name
                        lastName = Console.ReadLine();
                        while (lastName.Length == 0)                                //Cheking if anything was entered
                        {
                            Console.Write("Change {0} to: ", contacts[i].LastName);
                            lastName = Console.ReadLine();
                        }
                        contacts[i].LastName = lastName;
                        Console.WriteLine();

                        Console.Write("Change {0} to: ", contacts[i].PhoneNumber);      //Phone number
                        while (!int.TryParse(Console.ReadLine(), out phoneNumber))      //Cheking if anything was entered and if the input is type integer
                        {
                            Console.Write("Phone number should be made out of numbers, enter a correct phone number: ");
                        }
                        while (CheckForDuplicateNumbers(contacts, contactCount, phoneNumber))                                       //Cheking if the phone number isnt used by an other contact
                        {
                            Console.Write("This phone number is owned by another contact. Write an other one: ");
                            while (!int.TryParse(Console.ReadLine(), out phoneNumber))                                              //Cheking if anything was entered and if the input is type integer
                            {
                                Console.Write("Phone number should be made out of numbers, enter a correct phone number: ");
                            }
                        }
                        contacts[i].PhoneNumber = phoneNumber;
                        Console.WriteLine();

                        Console.Write("Leave address blank by pressing space or change {0} to: ", contacts[i].Address);     //Address
                        address = Console.ReadLine();
                        if (address == "")
                        {
                            address = "-";
                        }
                        contacts[i].Address = address;
                        Console.WriteLine();
                    }
                }
            }
        }

        //Deleting a contact
        public Contact[] DeleteContact(Contact[] contacts, int contactCount)
        {
            Program p = new Program();
            int contactNumber = 0;
            int count = 0;
            Contact[] contactsWithoutDeleted = new Contact[100];                            //Creating a new contacts array

            p.PrintContacts(contacts, contactCount);                                        //Asking which contact is to be deleted
            Console.Write("Write number of the contact which you want to delete: ");
            while (!int.TryParse(Console.ReadLine(), out contactNumber))                    //Cheking if anythig was entered and if input is type integer
            {
                Console.Write("Write number of the contact which you want to delete: ");
            }
            while (contactNumber <= 0 || contactNumber > contactCount)                      //Cheking if the input coresponds to a contact
            {
                Console.Write("There is no such contact, choose an existing one: ");
                while (!int.TryParse(Console.ReadLine(), out contactNumber))                //Cheking if anythig was entered and if input is type integer
                {
                    Console.Write("Write number of the contact which you want to delete: ");
                }
            }

            for (int i = 0; i < contactCount; i++)                                          //Adding all contacts to the new array except the chosen one
            {
                if (contactNumber != i+1)
                {
                    contactsWithoutDeleted[count++] = contacts[i];
                }
            }
            Console.WriteLine();

            return contactsWithoutDeleted;
        }

        //Printing contacts to the console
        public void PrintContacts (Contact[] contacts, int contactCount)
        {

            Console.WriteLine(new string('-', 81));                                                                                                 //Creating the header
            Console.WriteLine("| {0, -5} | {1, -10} | {2, -10} | {3, -15} | {4, -25} |", "Nr.", "Name", "Last Name", "Phone number", "Address");

            for (int i = 0; i < contactCount; i++)                                                                                                  //Inputing the data
            {
                Console.WriteLine(new string('-', 81));
                Console.WriteLine("| {0, -5} | {1, -10} | {2, -10} | {3, -15} | {4, -25} |", i+1, contacts[i].Name, contacts[i].LastName, contacts[i].PhoneNumber, contacts[i].Address);
            }

            Console.WriteLine(new string('-', 81));
            Console.WriteLine();
        }

        //Cheking if the phone number duplicates and if the duplicates are from a diferent contact
        public bool CheckForDuplicateNumbers(Contact[] contacts, int contactCount, int phoneNumber)
        {
            bool duplicate = false;

            for (int i = 0; i < contactCount-1; i++)
            {
                if (contacts[i].PhoneNumber == phoneNumber)
                {
                    duplicate = true;
                    break;
                }
            }

            return duplicate;
        }

        //Counting the number of contacts in array
        public int CountContacts(Contact[] contacts)
        {
            int count = 0;

            foreach (var contact in contacts)
            {
                if (contact != null)
                {
                    count++;
                }
                else break;
            }

            return count;
        }

        //Writing all contact data to a file
        public void WriteContactsToFile(Contact[] contacts, int contactsCount)
        {
            using (StreamWriter writer = new StreamWriter(contactData))
            {
                for (int i = 0; i < contactsCount; i++)
                {
                    writer.WriteLine("{0},{1},{2},{3},", contacts[i].Name, contacts[i].LastName, contacts[i].PhoneNumber, contacts[i].Address);
                }
            }
        }
    }
}
