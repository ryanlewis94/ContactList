using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using ContactsList.Repositories;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;


namespace ContactsList
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IContactRepo _repository = new ContactRepo();
        private Contact _contact = new Contact();

        public MainWindow()
        {
            InitializeComponent();
            LoadDb();
            this.DataContext = _contact;
        }

        private void LoadDb()
        {
            //adds the result of the query to the data grid
            ContactList.ItemsSource = _repository.GetContacts();

            DelBtn.IsEnabled = false;
            ContactList.SelectedItem = null;
            _contact = new Contact();
        }

        private void AddRecord(object sender, RoutedEventArgs e)
        {
            //if a contact has been selected then update their details
            if (TxtId.Text != "")
            {
                //updates the contact to the db
                _repository.UpdateContact(_contact);
            }
            //otherwise add the details as a new contact
            else
            {
                _contact.FirstName = TxtFirst.Text;
                _contact.LastName = TxtLast.Text;
                _contact.Phone = TxtPhone.Text;
                _contact.Email = TxtEmail.Text;

                //adds the new contact to the db
                _repository.AddContact(_contact);
            }

            //updates the data grid with the new changes
            LoadDb();
        }

        private void DelBtn_Click(object sender, RoutedEventArgs e)
        {
            _contact.Id = int.Parse(TxtId.Text);
            _repository.DeleteContact(_contact);

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
                //checks if the details entered are for adding a new contact or updating an existing one
                //and changes the add buttons text
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
            ////////////////////////////////create a get contact in the repo to get the contact that is selected//////////////////////////////////////////
            //_contact = new Contact();
            //_contact.Id = int.Parse(TxtId.Text);
            //_contact = _repository.GetContact(_contact);

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
