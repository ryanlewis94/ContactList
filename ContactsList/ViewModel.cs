using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ContactsList.Annotations;
using ContactsList.Repositories;

namespace ContactsList
{
    public class ViewModel: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private IContactRepo repository;

        public ICommand SaveCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand ClearCommand { get; set; }
        public ICommand SelectedCommand { get; set; }

        private void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        private List<Contact> contacts;

        public List<Contact> Contacts
        {
            get { return contacts; }
            set
            {
                contacts = value;
                RaisePropertyChanged("Contacts");
            }
        }

        private Contact selectedContact;

        public Contact SelectedContact
        {
            get { return selectedContact; }
            set
            {
                selectedContact = value;
                RaisePropertyChanged("SelectedContact");
            }
        }

        private Contact currentContact;

        public Contact CurrentContact
        {
            get { return currentContact; }
            set
            {
                currentContact = value;
                RaisePropertyChanged("CurrentContact");
            }
        }


        public ViewModel()
        {
            repository = new ContactRepo();
            LoadDb();
            LoadCommands();
        }

        private void LoadCommands()
        {
            SaveCommand = new CustomCommand(SaveContact, CanSaveContact);
            DeleteCommand = new CustomCommand(DeleteContact, CanDeleteContact);
            ClearCommand = new CustomCommand(ClearContact, CanClearContact);
            SelectedCommand = new CustomCommand(SelectContact, CanSelectContact);
        }

        private void SelectContact(object obj)
        {
            currentContact = SelectedContact;
        }
        private bool CanSelectContact(object obj)
        {
            return true;
        }

        private void SaveContact(object obj)
        {
            if (SelectedContact != null)
            {
                
                SelectedContact = CurrentContact;
                repository.UpdateContact(SelectedContact);
            }
            else
            {
                repository.AddContact(CurrentContact);
            }
            
            LoadDb();
        }

        private bool CanSaveContact(object obj)
        {
            if ((CurrentContact.FirstName != null && CurrentContact.LastName != null && CurrentContact.Phone != null) || (SelectedContact != null))
                return true;
            return false;
        }

        private void DeleteContact(object obj)
        {
            repository.DeleteContact(SelectedContact);
            LoadDb();
        }

        private bool CanDeleteContact(object obj)
        {
            if (SelectedContact != null)
                return true;
            return false;
        }

        private void ClearContact(object obj)
        {
            LoadDb();
        }

        private bool CanClearContact(object obj)
        {
            if (CurrentContact.FirstName != null || CurrentContact.LastName != null || CurrentContact.Phone != null || SelectedContact != null)
                return true;
            return false;
        }

        private void LoadDb()
        {
            SelectedContact = null;
            Contacts = repository.GetContacts();
            CurrentContact = new Contact();
            
        }
    }
}
