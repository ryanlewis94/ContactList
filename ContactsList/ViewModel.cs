using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ContactsList.Annotations;
using ContactsList.Repositories;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

namespace ContactsList
{
    public class ViewModel: ViewModelBase
    {
        private IContactRepo repository;

        public ICommand SaveCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand ClearCommand { get; set; }

        public MetroWindow metroWindow = (MetroWindow)Application.Current.MainWindow;

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

        private string saveButtonContent;

        public string SaveButtonContent
        {
            get { return saveButtonContent; }
            set { SetProperty(ref saveButtonContent, value); }
        }

        private bool gridContacts;
        public bool GridContacts
        {
            get { return gridContacts; }
            set { SetProperty(ref gridContacts, value); }
        }

        private bool noContacts;
        public bool NoContacts
        {
            get { return noContacts; }
            set { SetProperty(ref noContacts, value); }
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

        private async void SaveContact(object obj)
        {
            //check for bad name inputs
            if ((SelectedContact.FirstName.All(char.IsWhiteSpace) || SelectedContact.FirstName.Any(char.IsDigit)) ||
                (!string.IsNullOrEmpty(SelectedContact.LastName) && (SelectedContact.LastName.All(char.IsWhiteSpace) || SelectedContact.LastName.Any(char.IsDigit))))
            {
                await metroWindow.ShowMessageAsync("Warning", "Please enter a valid name!");
            }
            else
            {
                //check for correct phone number input
                if (SelectedContact.Phone.All(char.IsDigit) &&
                SelectedContact.Phone.Length >= 3 &&
                SelectedContact.Phone.Length <= 15)
                {
                    //check for bad email address input
                    if (!string.IsNullOrEmpty(SelectedContact.Email) && (SelectedContact.Email.All(char.IsWhiteSpace) || !SelectedContact.Email.Contains("@")))
                    {
                        await metroWindow.ShowMessageAsync("Warning", "Please enter a valid Email Address!");
                    }
                    else
                    {
                        //check if a user has been selected
                        if (SelectedContact.Id != 0)
                        {
                            try
                            {
                                await repository.UpdateContact(SelectedContact);
                            }
                            catch (Exception ex)
                            {
                                if (ex.Message == "Precondition Failed")
                                {
                                    await metroWindow.ShowMessageAsync("Warning", 
                                        "This contact list has been updated elsewhere and is now out of date, " +
                                        "your list will now be updated");
                                }
                            }
                        }
                        else
                        {
                            await repository.AddContact(SelectedContact);
                        }

                        LoadDb();
                    }
                }
                else
                {
                    await metroWindow.ShowMessageAsync("Warning", "Please enter a valid phone number!");
                }
            }
        }

        private bool CanSaveContact(object obj)
        {
            if (SelectedContact != null)
            {
                if (SelectedContact.Id != 0)
            {
                SaveButtonContent = "Save";
            }
            else
            {
                SaveButtonContent = "Add";
            }
            return !string.IsNullOrEmpty(SelectedContact.FirstName) &&
                   !string.IsNullOrEmpty(SelectedContact.Phone);
            }
            else
            {
                return !string.IsNullOrEmpty(SelectedContact.FirstName) &&
                   !string.IsNullOrEmpty(SelectedContact.Phone);
            }
        }

        private async void DeleteContact(object obj)
        {
            var settings = new MetroDialogSettings()
            {
                AffirmativeButtonText = "Yes",
                NegativeButtonText = "No"
            };

            var result = await metroWindow.ShowMessageAsync("Delete Contact",
                $"Are you sure you want to delete {SelectedContact.FirstName}'s contact details?",
                MessageDialogStyle.AffirmativeAndNegative, settings);
            if (result == MessageDialogResult.Affirmative)
            {
                await repository.DeleteContact(SelectedContact);
                LoadDb();
            }
        }

        private bool CanDeleteContact(object obj)
        {
            return SelectedContact.Id != 0;
        }

        private void ClearContact(object obj)
        {
            LoadDb();
        }

        private bool CanClearContact(object obj)
        {
            return !string.IsNullOrEmpty(SelectedContact.FirstName) ||
                   !string.IsNullOrEmpty(SelectedContact.LastName) ||
                   !string.IsNullOrEmpty(SelectedContact.Phone) ||
                   !string.IsNullOrEmpty(SelectedContact.Email);
        }

        private async void LoadDb()
        {
            SelectedContact = null;
            SelectedContact = new Contact();

            Contacts = await repository.GetContacts();
            if (Contacts.Count == 0)
            {
                GridContacts = false;
                NoContacts = true;
            }
            else
            {
                GridContacts = true;
                NoContacts = false;
            }
        }
    }
}
