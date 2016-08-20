using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Model.BusinessObjects;

namespace MA_WEB.Models
{
    public class ChatRecordViewModel
    {
        public ChatRecordViewModel(ChatRecordBO chatRecord)
        {
            Name = chatRecord.User.FirstName + " " + chatRecord.User.LastName;
            Message = chatRecord.Message;
            Time = chatRecord.Time.ToString("g");
            Avatar = chatRecord.User.Avatar;
            UserName = chatRecord.User.UserName;
        }

        public ChatRecordViewModel()
        {

        }

        public string Name { get; set; }

        public string Message { get; set; }

        public string Time { get; set; }
        public string Avatar { get; set; }
        public string UserName { get; set; }
    }
}