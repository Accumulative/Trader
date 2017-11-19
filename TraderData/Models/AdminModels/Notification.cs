using System;
namespace TraderData.Models.AdminModels
{
    public class Notification
    {
        public int NotificationID { get; set; }

        public string Link { get; set; }

        public DateTime timePosted { get; set; }

        public string Poster { get; set; }

        public string Summary { get; set; }

        public Notification()
        {
            timePosted = DateTime.Now; 
        }
    }
}
