﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MahApps.Metro.Controls.Dialogs;

namespace ContactsList.Repositories
{
    public interface IDialogService
    {
        Task<MessageDialogResult> AskQuestionAsync(string title, string message);
    }
}
