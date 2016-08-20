using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Model.BusinessObjects;

namespace MA_WEB.Models
{
    public class CommentViewModel
    {
        public Int32 Id { get; set; }
        public String Text { get; set; }
        public String Author { get; set; }
        public String Avatar { get; set; }
        public String Date { get; set; }
        public Int32 ReqId { get; set; }
        public Int32 IssueId { get; set; }

        public static implicit operator CommentBO(CommentViewModel commentView)
        {
            return new CommentBO()
            {
                Id = commentView.Id,
                Text = commentView.Text,
                Created = DateTime.Now
            };
        }

        public static implicit operator CommentViewModel(CommentBO commentBo)
        {
            return new CommentViewModel()
            {
                Id = commentBo.Id,
                Text = commentBo.Text,
                Date = GetPrettyDate(commentBo.Created),
                Author = commentBo.CreatedBy.FirstName + " " + commentBo.CreatedBy.LastName,
                Avatar = commentBo.CreatedBy.Avatar
            };
        }

        public static string GetPrettyDate(DateTime d)
        {
            // 1.
            // Get time span elapsed since the date.
            TimeSpan s = DateTime.Now.Subtract(d);

            // 2.
            // Get total number of days elapsed.
            int dayDiff = (int)s.TotalDays;

            // 3.
            // Get total number of seconds elapsed.
            int secDiff = (int)s.TotalSeconds;

            // 4.
            // Don't allow out of range values.
            if (dayDiff < 0 || dayDiff >= 31)
            {
                return null;
            }

            // 5.
            // Handle same-day times.
            if (dayDiff == 0)
            {
                // A.
                // Less than one minute ago.
                if (secDiff < 60)
                {
                    return "just now";
                }
                // B.
                // Less than 2 minutes ago.
                if (secDiff < 120)
                {
                    return "1 minute ago";
                }
                // C.
                // Less than one hour ago.
                if (secDiff < 3600)
                {
                    return string.Format("{0} minutes ago",
                        Math.Floor((double)secDiff / 60));
                }
                // D.
                // Less than 2 hours ago.
                if (secDiff < 7200)
                {
                    return "1 hour ago";
                }
                // E.
                // Less than one day ago.
                if (secDiff < 86400)
                {
                    return string.Format("{0} hours ago",
                        Math.Floor((double)secDiff / 3600));
                }
            }
            // 6.
            // Handle previous days.
            if (dayDiff == 1)
            {
                return "yesterday";
            }
            if (dayDiff < 7)
            {
                return string.Format("{0} days ago",
                dayDiff);
            }
            if (dayDiff < 31)
            {
                return string.Format("{0} weeks ago",
                Math.Ceiling((double)dayDiff / 7));
            }
            return null;
        }

    }
}