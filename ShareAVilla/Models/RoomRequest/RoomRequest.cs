using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
namespace ShareAVilla.Models.RoomRequest
{
    public enum RoomRequestResponseStatus
    {
        Canceled,
        Declined,
        Pending,        
        Proposed,
        Approved,
        Reserved,
    };
   
    
    public class RoomRequestResponse:BaseDataObject
    {
        
        public string Message { get; set; }
        public RoomRequestResponseStatus responseStatus { get; set; }
        public string StatusString { get; set; }
        public DateTime TimeStamp { get; set; }
        public bool IsApproved()
        {
            if (responseStatus == RoomRequestResponseStatus.Approved)
            {
                return true;
            }
            return false;
        }
        public bool IsDeclined()
        {
            if (responseStatus == RoomRequestResponseStatus.Declined)
            {
                return true;
            }
            return false;
        }
       
        public bool IsPending()
        {
            if (responseStatus == RoomRequestResponseStatus.Pending)
            {
                return true;
            }
            return false;
        }
        public bool IsProposed()
        {
            if (responseStatus == RoomRequestResponseStatus.Proposed)
            {
                return true;
            }
            return false;
        }
        public bool IsCanceled()
        {
            if (responseStatus == RoomRequestResponseStatus.Canceled)
            {
                return true;
            }
            return false;
        }
        public bool IsReserved()
        {
            if (responseStatus == RoomRequestResponseStatus.Reserved)
            {
                return true;
            }
            return false;
        }
        public RoomRequestResponse() 
        {
            
        }
        public void Init()
        {
            Message = "Please wait! Requestowner has not yet responded";
            responseStatus = RoomRequestResponseStatus.Pending;
            StatusString = "Pending";
            TimeStamp = DateTime.Now;
        }
        public void Decline()
        {
            Message = "Sorry, your roomrequest was declined!";
            responseStatus = RoomRequestResponseStatus.Declined;
            StatusString = "Declined";
            TimeStamp = DateTime.Now;
        }
        
        public void Approve() //Accepted by RqOwner, but waiting for reservation
        {
            Message = "Congrats, your application was accepted!";
            responseStatus = RoomRequestResponseStatus.Approved;
            StatusString = "Approved";
            TimeStamp = DateTime.Now;
        }
        public void Propose()
        {
            Message = "The request owner has made a proposal: \n";
            responseStatus = RoomRequestResponseStatus.Proposed;
            StatusString = "Proposed";
            TimeStamp = DateTime.Now;

        }
        public void Reserve()
        {
            Message = "Congratulations, your room is reserved";
            responseStatus = RoomRequestResponseStatus.Reserved;
            StatusString = "Reserved";
            TimeStamp = DateTime.Now;
        }
            public void Canceled()
        {
            Message = "Too bad! The room request was canceled";
            responseStatus = RoomRequestResponseStatus.Canceled;
            StatusString = "Canceled";
            TimeStamp = DateTime.Now;
        }

    }
   
    public class RoomRequest : BaseDataObject
    {


        public int ApplyingTraveler_ID { get; set; }
        public int Request_ID { get; set; }
        public int DoubleBed { get; set; }
        public int SingleBed { get; set; }
        public bool ShareRoom { get; set; }
        public bool RequestOwner { get; set; }
        public int DoubleBedPropose { get; set; }
        public int SingleBedPropose { get; set; }
        public bool ShareRoomPropose { get; set; }

        public int Room_ID { get; set; }
        public DateTime CheckInUser { get; set; } //User wanted checkDates
        public DateTime CheckOutUser { get; set; }
        public DateTime CheckInOwner { get; set; } //Proposal by Requestowner
        public DateTime CheckOutOwner { get; set; }
        public string PriceUser { get; set; }        //User wanted  Price
        public string PriceOwner { get; set; }       //Price Proposed by RequestOwner
        public Accommodation.RoomType RoomType { get; set; }
        public bool Nevermind { get; set; }
        [ForeignKey("RoomRequestResponse")]
        public int RoomRequestResponse_ID { get; set; }
        public RoomRequestResponse RoomRequestResponse { get; set; }

        public string Message { get; set; }
        
        public DateTime TimeStamp {get;set;}
       
        public RoomRequest()
        {
           // RoomRequestResponse = new RoomRequestResponse();
        }
        /// <summary>
        /// Update from User
        /// </summary>
        /// <param name="roomRequestVM"></param>
        public void SetValues(RoomRequestVMUser roomRequestVM)
        {
            
            CheckInUser = roomRequestVM.CheckIn;
            CheckOutUser = roomRequestVM.CheckOut;
            ShareRoom = roomRequestVM.ShareRoom;
            PriceUser = roomRequestVM.Price;
            Nevermind = roomRequestVM.Nevermind;
            DoubleBed = SingleBed = 0;
            if (roomRequestVM.DoubleBed)
            {
                DoubleBed++;
            }
            if (roomRequestVM.SingleBed)
            {
                SingleBed++;                    
            }
            RoomType = Accommodation.Accommodation.DetectRoomType(SingleBed, DoubleBed, ShareRoom, Nevermind);

            TimeStamp = DateTime.Now;
            Message = roomRequestVM.Text;
            
            RoomRequestResponse.Init();
            SetValid(); //Todo has to be checked in get Methods
            
            
        }
        /// <summary>
        /// Update when a proposal from RqOwner was made
        /// </summary>
        public void SetValues(RoomRequestVMOwner proposal)
        {
            CheckInOwner = proposal.CheckIn;
            CheckOutOwner = proposal.CheckOut;
            if (proposal.SingleBed)
            {
                SingleBedPropose = 1;
            }
            if (proposal.DoubleBed)
            {
                DoubleBedPropose = 1;
            }
            ShareRoomPropose = proposal.ShareRoom;
            PriceOwner = proposal.Price;
            Message = Message + "\nHost has made a proposal:\n" + proposal.Message;
            RoomRequestResponse.Propose();

        }
        public void AcceptProposal()
        {
            CheckInUser = CheckInOwner;
            CheckOutUser = CheckOutOwner;
            SingleBed = SingleBedPropose;
            DoubleBed = DoubleBedPropose;
            ShareRoom = ShareRoomPropose;
            PriceUser = PriceOwner;
            RoomRequestResponse.Approve();
        }
        public void Reserve(int roomid)
        {
            Room_ID = roomid;
            RoomRequestResponse.Reserve();

        }
        /// <summary>
        /// Constructor to create a RoomRequest
        /// </summary>
        /// <param name="roomRequestVM"></param>
        public RoomRequest(RoomRequestVMUser roomRequestVM)
        {

            CheckInUser = roomRequestVM.CheckIn;
            CheckOutUser = roomRequestVM.CheckOut;
            ShareRoom = roomRequestVM.ShareRoom;
            PriceUser = roomRequestVM.Price;
            DoubleBed = SingleBed = 0;
            Request_ID = roomRequestVM.Request_ID;
            Nevermind = roomRequestVM.Nevermind;
            if (roomRequestVM.DoubleBed)
            {
                DoubleBed++;
            }
            if (roomRequestVM.SingleBed)
            {
                SingleBed++;
            }
            RoomType = Accommodation.Accommodation.DetectRoomType(SingleBed, DoubleBed, ShareRoom, Nevermind);
           
            Message = roomRequestVM.Text;
            RequestOwner = roomRequestVM.RequestOwner;
            TimeStamp = DateTime.Now;
            SetValid();
            RoomRequestResponse = new RoomRequestResponse();
            RoomRequestResponse.Init(); 
        }
        public string RoomTypeToString()
        {
            switch (RoomType)
            {
                case Accommodation.RoomType.None: return "None";
                case Accommodation.RoomType.SingleBed: return "Single Bed";
                case Accommodation.RoomType.DoubleBed: return "Double Bed";
                case Accommodation.RoomType.SingleDoubleBed: return "Single + Double Bed";
                case Accommodation.RoomType.MultiBed: return "Multi Bed";
                case Accommodation.RoomType.All: return "Nevermind";
                default:return "Multi Bed";
            }
        }
       
    }

}