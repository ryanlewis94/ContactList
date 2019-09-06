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
    }
}
