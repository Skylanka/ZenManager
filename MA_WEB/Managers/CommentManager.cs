using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Model.BusinessObjects;

namespace MA_WEB.Managers
{
    public class CommentManager : BaseManager
    {
        public List<CommentBO> GetComments(int id)
        {
            return Context.Comments.OrderByDescending(p=>p.Created)
                        .Where(p=>p.Requirement.Id == id).Include(p=>p.CreatedBy)
                        .ToList();
        }

        public List<CommentBO> GetIssueComments(int id)
        {
            return Context.Comments.OrderByDescending(p => p.Created)
                        .Where(p => p.Issue.Id == id).Include(p => p.CreatedBy)
                        .ToList();
        }

        public CommentBO GetComment(int id)
        {
            return Context.Comments.FirstOrDefault(p => p.Id == id);
        }

        public Task Update(CommentBO comment)
        {
            if (comment == null)
                throw new ArgumentNullException("comment");
            if (comment.Id == 0)
                throw new ArgumentException("InvalidId");

            Context.Comments.AddOrUpdate(comment);
            return Context.SaveChangesAsync();
        }

        public async Task<CommentBO> Create(CommentBO comment)
        {
            if (comment == null)
                throw new ArgumentNullException("project");
            if (comment.Id != 0)
                throw new ArgumentException("InvalidId");

            comment = Context.Comments.Add(comment);
            await Context.SaveChangesAsync();
            return comment;
        }

    }
}