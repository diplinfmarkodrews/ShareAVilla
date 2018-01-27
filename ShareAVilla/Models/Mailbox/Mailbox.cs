using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShareAVilla.Models
{
    public class Mailbox
    {
        public int ID { get; set; }
        public bool hasNewMail { get; set; }
        public virtual List<Mail> Inbox { get; set; }
        public virtual List<Mail> Outbox { get; set; }
        public virtual List<Mail> Created { get; set; }
        public virtual List<Mail> Deleted { get; set; }
        
    }
}