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
        List<Contact> GetContacts();
        Contact GetContact(Contact contact);
        Contact AddContact(Contact contact);
        Contact UpdateContact(Contact contact);
        Contact DeleteContact(Contact contact);
    }
}
