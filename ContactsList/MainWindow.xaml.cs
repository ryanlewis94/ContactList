using System.Linq;
using System.Windows;
using System.Windows.Controls;


namespace ContactsList
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            LoadDb();
        }

        private void LoadDb()
        {
            var db = new ContactDb();

            //queries the db for all the contacts
            var query = from contact in db.Contacts
                select contact;

            //adds the result of the query to the data grid
            ContactList.ItemsSource = query.ToList();

            //resets the inputs and buttons
            TxtId.Text = "";
            TxtFirst.Text = "";
            TxtLast.Text = "";
            TxtPhone.Text = "";
            TxtEmail.Text = "";
            DelBtn.IsEnabled = false;
        }

        private void AddDb()
        {
            var db = new ContactDb();

            //initialize a new contact
            var ContactNew = new Contact();

            //fill the new contact with the details input by the user
            ContactNew.FirstName = TxtFirst.Text;
            ContactNew.LastName = TxtLast.Text;
            ContactNew.Phone = TxtPhone.Text;
            ContactNew.Email = TxtEmail.Text;


            //adds the new contact to the db
            db.Contacts.Add(ContactNew);
            db.SaveChanges();
        }

        private void UpdateDb()
        {
            var db = new ContactDb();
            var Id = int.Parse(TxtId.Text);

            //queries the db for contact that has been selected
            var UpdatedContact = 
                db.Contacts.First(g=>g.Id == Id);

            //changes the contact details with the details input by the user
            UpdatedContact.FirstName = TxtFirst.Text;
            UpdatedContact.LastName = TxtLast.Text;
            UpdatedContact.Phone = TxtPhone.Text;
            UpdatedContact.Email = TxtEmail.Text;

            //updates the contact details in the db
            db.SaveChanges();
        }

        private void DeleteDb()
        {
            var db = new ContactDb();
            var Id = int.Parse(TxtId.Text);

            //queries the db for contact that has been selected
            var DeleteContactDetails =
                from contact in db.Contacts
                where contact.Id == Id
                select contact;

            //Loop through the query
            foreach (var contact in DeleteContactDetails)
            {
                //delete the selected contact
                db.Contacts.Remove(contact);
            }

            //confirm the deleted row and update the db
            db.SaveChanges();
        }

        private void AddRecord(object sender, RoutedEventArgs e)
        {
            //if a contact has been selected then update their details
            if (TxtId.Text != "")
            {
                UpdateDb();
            }
            //otherwise add the details as a new contact
            else
            {
                AddDb();
            }
            
            //updates the data grid with the new changes
            LoadDb();
        }

        private void DelBtn_Click(object sender, RoutedEventArgs e)
        {
            DeleteDb();
            LoadDb();
        }

        private void OnTextChanged(object sender, RoutedEventArgs e)
        {
            //gets rid of any white spaces at the start and end of the user inputs
            TxtFirst.Text = TxtFirst.Text.Trim();
            TxtLast.Text = TxtLast.Text.Trim();
            TxtPhone.Text = TxtPhone.Text.Trim();
            TxtEmail.Text = TxtEmail.Text.Trim();
            
            //if the user has entered all the details allows them to add the contact details
            if (TxtFirst.Text != "" && TxtLast.Text != "" && TxtPhone.Text != "" && TxtEmail.Text != "")
            {
                AddBtn.Content = TxtId.Text != "" ? "Update" : "Add";
                AddBtn.IsEnabled = true;
            }
            //don't let the user add a contact if they haven't filled out all the details
            else
            {
                AddBtn.IsEnabled = false;
            }
        }

        private void ContactList_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //when a contact is selected it disables the option to add and enables the option to delete
            AddBtn.IsEnabled = false;
            DelBtn.IsEnabled = true;
        }

        private void ClearBtn_Click(object sender, RoutedEventArgs e)
        {
            //resets the text boxes and data grid
            LoadDb();
        }
    }
}
