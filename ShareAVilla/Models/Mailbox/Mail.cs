using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShareAVilla.Models
{
    public class Mail
    {
        public int ID { get; set; }

        public virtual Mailbox Sender { get; set; }

        public virtual List<Mailbox> Receiver { get; set; }

        public string Subject { get; set; }

        public string Text { get; set; }

        private DateTime Timestamp { get; set; }

        public virtual List<Attachement> Attachements { get; set; }

        public bool isRecieved { get; set; }

        public bool isRead { get; set; }

        public bool isAnswered { get; set; }

        public Mail()
        {
            
        }
    }
}