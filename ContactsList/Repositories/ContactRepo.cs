using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsList.Repositories
{
    public class ContactRepo : IContactRepo
    {
        ContactDb _context = new ContactDb();
    }
}
