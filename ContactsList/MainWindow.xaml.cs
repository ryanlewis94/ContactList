using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using ContactsList.Repositories;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using MahApps.Metro.Controls;

namespace ContactsList
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        private IContactRepo _repository = new ContactRepo();
        private Contact _contact = new Contact();

        public MainWindow()
        {
            InitializeComponent();
            
        }

        private void OnTextChanged(object sender, RoutedEventArgs e)
        {
            //gets rid of any white spaces at the start and end of the user inputs
            TxtFirst.Text = TxtFirst.Text.Trim();
            TxtLast.Text = TxtLast.Text.Trim();
            TxtPhone.Text = TxtPhone.Text.Trim();
            TxtEmail.Text = TxtEmail.Text.Trim();

        }
    }
}
