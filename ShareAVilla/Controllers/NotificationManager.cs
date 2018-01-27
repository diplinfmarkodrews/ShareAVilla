using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShareAVilla
{
    public static class NotificationManager
    {
        private static List<Notification> Notifications = new List<Notification>();
        
        public static void AddException(Exception e)
        {
            Notifications.Add(new Notification(NotificationType.Error, e.Message, e));
            Elmah.Error er = new Elmah.Error(e);
            using (var db = new Models.ApplicationDbContext())
            {                   

                //db.ElmahErr.Add(er);
                //db.SaveChangesAsync();
            }
    }
        public static bool HasNotifications()
        {
            return Notifications.Count > 0;
        }
        public static void AddNotification(NotificationType type, string message)
        {
            Notifications.Add(new Notification (type, message));
        }
        public static List<Notification> GetAndClearNotifications()
        {
            List<Notification> ret = new List<Notification>();
            foreach (Notification n in Notifications)
            {
                ret.Add(n);
                Notifications.Remove(n);
            }
            return ret;
        }
        public static Notification ReadNotification()
        {
            if (Notifications.Count > 0)
            {
                Notification ret = Notifications[Notifications.Count - 1];
                Notifications.RemoveAt(Notifications.Count - 1);
                return ret;
            }
            else
            {
                return null;
            }
        }
    }
}