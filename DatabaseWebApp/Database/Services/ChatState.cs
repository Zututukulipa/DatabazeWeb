using System;
using DatabaseAdapter.OracleLib.Models;

namespace Database.Services
{
    public class ChatState
    {
            public User SelectedUser { get; set; }

            public event Action OnChange;

            public void NotifyStateChanged() => OnChange?.Invoke();
    }
}