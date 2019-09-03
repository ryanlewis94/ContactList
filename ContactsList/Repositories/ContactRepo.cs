using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsList.Repositories
{
    public class ContactRepo : IContactRepo
    {
        ContactDb _context = new ContactDb();

        public List<Contact> GetContacts()
        {
            return _context.Contacts.ToList();
        }

        public Contact GetContact(Contact contact)
        {
            Contact contToDisplay = _context.Contacts.FirstOrDefault(c => c.Id == contact.Id);

            return contToDisplay;
        }

        public Contact AddContact(Contact contact)
        {
            _context.Contacts.Add(contact);
            _context.SaveChanges();
            return contact;
        }

        public Contact UpdateContact(Contact contact)
        {
            _context.Contacts.First(c => c.Id == contact.Id);
            
            _context.SaveChanges();
            return contact;
        }

        public Contact DeleteContact(Contact contact)
        {
            var contactToDelete = _context.Contacts.First(c => c.Id == contact.Id);
            if (contactToDelete != null)
            {
                _context.Contacts.Remove(contactToDelete);
            }

            _context.SaveChanges();
            return contact;
        }
    }
}
