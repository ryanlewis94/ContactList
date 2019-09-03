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
    public class ViewModel: ViewModelBase
    {
        private IContactRepo repository;

        public ICommand SaveCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand ClearCommand { get; set; }

        private List<Contact> contacts;

        public List<Contact> Contacts
        {
            get { return contacts; }
            set { SetProperty(ref contacts, value); }
        }

        private Contact selectedContact;

        public Contact SelectedContact
        {
            get { return selectedContact; }
            set { SetProperty(ref selectedContact, value); }
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
        }


        private void SaveContact(object obj)
        {
            if (SelectedContact.Id != 0)
            {
                repository.UpdateContact(SelectedContact);
            }
            else
            {
                repository.AddContact(SelectedContact);
            }

            LoadDb();
        }

        private bool CanSaveContact(object obj)
        {
            return !string.IsNullOrEmpty(SelectedContact.FirstName) &&
                   !string.IsNullOrEmpty(SelectedContact.LastName) &&
                   !string.IsNullOrEmpty(SelectedContact.Phone);
        }

        private void DeleteContact(object obj)
        {
            repository.DeleteContact(SelectedContact);
            LoadDb();
        }

        private bool CanDeleteContact(object obj)
        {
            return SelectedContact.Id != 0;
        }

        private void ClearContact(object obj)
        {
            repository.DiscardChanges();
            LoadDb();
        }

        private bool CanClearContact(object obj)
        {
            return !string.IsNullOrEmpty(SelectedContact.FirstName) ||
                   !string.IsNullOrEmpty(SelectedContact.LastName) ||
                   !string.IsNullOrEmpty(SelectedContact.Phone) ||
                   !string.IsNullOrEmpty(SelectedContact.Email);
        }

        private void LoadDb()
        {
            SelectedContact = null;
            Contacts = repository.GetContacts();
            SelectedContact = new Contact();
            
        }
    }
}
