using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShareAVilla.Controllers
{
   
    public class DayPilotWrapper
    {



        private DayPilot.Web.Ui.DayPilotScheduler InitScheduler(DateTime start, TimeSpan days)
        {
            DayPilot.Web.Ui.DayPilotScheduler DayPilotScheduler = new DayPilot.Web.Ui.DayPilotScheduler();
            DayPilotScheduler.TimeRangeSelectedHandling = DayPilot.Web.Ui.Enums.TimeRangeSelectedHandling.JavaScript;
            DayPilotScheduler.TimeRangeSelectedJavaScript = "create('{0}')";

            DayPilotScheduler.EventClickHandling = DayPilot.Web.Ui.Enums.EventClickHandlingEnum.JavaScript;
            DayPilotScheduler.EventClickJavaScript = "edit('{0}')";

            DayPilotScheduler.StartDate = start;
            DayPilotScheduler.Days = days.Days;
            return DayPilotScheduler;
        }

        private DayPilot.Web.Ui.DayPilotScheduler LoadEvents(DayPilot.Web.Ui.DayPilotScheduler scheduler, List<Models.RoomRequest.RoomRequestVMOwner> reservations)
        {

            scheduler.DataSource = from e in reservations select e;
            scheduler.DataTextField = "ApplyingTraveler.Name";
            scheduler.DataIdField = "ID";
            scheduler.DataStartField = "CheckIn";
            scheduler.DataEndField = "CheckOut";
            scheduler.DataBind();
            return scheduler;
        }

        private DayPilot.Web.Ui.DayPilotScheduler LoadResources(DayPilot.Web.Ui.DayPilotScheduler scheduler, List<Models.Accommodation.BedRoom> bedrooms)
        {

            foreach (var bedroom in bedrooms)
            {
                scheduler.Resources.Add(bedroom.ID.ToString(), bedroom.RoomNumber.ToString());
            }
            return scheduler;
        }


    }
}