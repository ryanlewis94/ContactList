using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsList.Repositories
{
    public interface IContactRepo
    {
        Task<List<Contact>> GetContacts();
        Task<Contact> AddContact(Contact contact);
        Task<Contact> UpdateContact(Contact contact);
        Task DeleteContact(Contact contact);
        Task DiscardChanges();
    }
}
