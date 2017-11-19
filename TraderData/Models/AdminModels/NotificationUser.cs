using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace TraderData.Models.AdminModels
{
    // Only Create this when the user clicks or removes the notification
    // Notifications display to everyone by default
    public class NotificationUser
    {
        public int NotificationUserID { get; set; }

        public virtual Notification Notification { get; set; }

        [ForeignKey("Notification")]
        public int NotificationID { get; set; }

        public int Clicked { get; set; }

        public bool Removed { get; set; }
    }
}
