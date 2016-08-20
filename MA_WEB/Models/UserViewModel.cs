using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MA_WEB.Managers;
using Model.Base;
using Model.BusinessObjects;
using Role = Model.BusinessModels.Role;

namespace MA_WEB.Models
{
    public class UserViewModel
    {
        public int Id { get; set; }
        public string FName { get; set; }
        public string LName { get; set; }
        public bool IsAuthenticated { get; set; }
        public List<ProjectBO> Projects { get; set; }
        public Role? Role { get; set; }
        public bool HasRole { get { return Role.HasValue; } }
        public string Avatar { get; set; }
        public string MemberSince { get; set; }
        public string Hometown { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public int ProjectCount { get; set; }
        public int IssueCount { get; set; }
        public List<InvitationViewModel> Invitations { get; set; } 
        //public HttpPostedFileBase File { get; set; }
        public String File { get; set; }
        /*public UserViewModel()
        {
            IsAuthenticated = false;
            Role = null;
        }*/
        public static implicit operator ApplicationUser(UserViewModel userView)
        {
            return new ApplicationUser()
            {
                Id = userView.Id,
                LastName = userView.LName,
                FirstName = userView.FName,
                Avatar = userView.Avatar

            };
        }

        public static implicit operator UserViewModel(ApplicationUser userBo)
        {
            List<InvitationViewModel> invts = new List<InvitationViewModel>();
            if (userBo.Invitations != null)
            {
                foreach (var invt in userBo.Invitations)
                {
                    ProjectBO project = new ProjectManager().GetProjectForInvitation(invt.Id);
                    invts.Add(new InvitationViewModel()
                    {
                        Email = invt.Email,
                        Id = invt.Id,
                        Status = invt.Status ? 1 : 0,
                        ProjectName = project.Name
                    });
                }
            } 
                

            return new UserViewModel()
            {
                Id = userBo.Id,
                FName = userBo.FirstName,
                LName = userBo.LastName,
                Avatar = userBo.Avatar,
                Hometown = userBo.Hometown,
                Phone = userBo.Phone,
                Email = userBo.Email,
                Invitations = invts
            };
        }
    }
}