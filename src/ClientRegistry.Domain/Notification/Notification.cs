﻿namespace ClientRegistry.Domain.Notification
{
    public class Notification
    {
        public Notification(string message)
        {
            Message = message;
        }

        public string Message { get; }
    }
}
