using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShareAVilla
{
    public enum NotificationType
    {
        Success = 0,
        Error,
        Warning,
        Info 

    };

    public class Notification
    {
        public NotificationType Type { get; set; }
        public string Message { get; set; }
        public Exception Exception { get; set; }
        public Notification(NotificationType type, string message)
        {
            Type = type;
            Message = message;
        }
        public Notification(NotificationType type, string message, Exception exc)
        {
            Type = type;
            Message = message;
            Exception = exc;
        }
    }
}