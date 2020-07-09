namespace Contact_Manager
{
    class Contact
    {
        public string Name { get; set; }

        public string LastName { get; set; }

        public int PhoneNumber { get; set; }

        public string Address { get; set; }

        public Contact (string name, string lastName, int phoneNumber, string address)
        {
            Name = name;
            LastName = lastName;
            PhoneNumber = phoneNumber;
            Address = address;
        }
    }
}
