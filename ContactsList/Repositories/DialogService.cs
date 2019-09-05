using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

namespace ContactsList.Repositories
{
    internal class DialogService : IDialogService
    {
        private readonly MetroWindow metroWindow;

        public DialogService(MetroWindow metroWindow)
        {
            this.metroWindow = metroWindow;
        }

        public Task<MessageDialogResult> AskQuestionAsync(string title, string message)
        {
            var settings = new MetroDialogSettings()
            {
                AffirmativeButtonText = "Yes",
                NegativeButtonText = "No",
            };
            return metroWindow.ShowMessageAsync(title, message,
                MessageDialogStyle.AffirmativeAndNegative, settings);
        }
    }
}
