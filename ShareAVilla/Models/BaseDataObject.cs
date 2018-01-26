using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace ShareAVilla
{

    public static class Check
    {
        public static bool IsNull(int? i)
        {
            if (i == 0) throw new ArgumentNullException();
            return false;
        }
        public static bool IsNull(Object i, string exc)
        {
            if (i == null) throw new ArgumentNullException(i.ToString(), exc);
            return false;
        }
       
    }
    public abstract class BaseDataObject 
    {
       public int ID { get; set; }
        public BaseDataObject()
        {
            objState = ObjectState.Valid;
        }
       
        public bool IsInValid( string excName)
        {
            if (objState == ObjectState.Invalid) { NotificationManager.AddException(new Exception(ToString()+excName)); return true; }
            return false;
        }
        public bool IsInValid()
        {
            if (objState == ObjectState.Invalid) { return true; }
            return false;
        }
        public bool IsValid()
        {
            if (objState == ObjectState.Valid) { return true; } return false;
        }
        public bool IsDeleted(string excName)
        {
            if (objState == ObjectState.Deleted) { NotificationManager.AddException(new Exception(ToString()+excName)); return true; }
            return false;
        }
        public bool IsClosed()
        {
            if (objState == ObjectState.Closed) { return true; }
            return false;
        }
        public bool IsClosed(string excName)
        {
            if (objState == ObjectState.Closed) { NotificationManager.AddException(new Exception(ToString() + excName)); return true; }
            return false;
        }
        public bool IsDeleted()
        {
            if (objState == ObjectState.Deleted) { return true; }
            return false;
        }
        public void Invalid() { objState = ObjectState.Invalid; }
        public void Delete() { objState=ObjectState.Deleted;  }
        public void Close() { objState = ObjectState.Closed; }
        public void SetValid() { objState = ObjectState.Valid; }
        public ObjectState objState { get; set; }
        public enum ObjectState
        {
            Valid,
            Invalid,
            Deleted,
            Closed
        };
    }
}