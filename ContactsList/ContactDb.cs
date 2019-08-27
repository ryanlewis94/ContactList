using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsList
{
    public class ContactDb : DbContext
    {
        public DbSet<Contact> Contacts { get; set; }
    }
}
