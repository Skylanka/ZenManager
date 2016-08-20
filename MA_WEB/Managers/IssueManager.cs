using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading.Tasks;
using Model.Base;
using Model.BusinessObjects;

namespace MA_WEB.Managers
{
    public class IssueManager : BaseManager
    {


        public async Task<int> GetCount(int id)
        {
            var t = await Context.Issues.Where(p => p.Project.Id == id && !p.IsDeleted).CountAsync();
            return t;
        }
        public async Task Delete(int id)
        {
            if (id <= 0)
                throw new ArgumentNullException("issue");

            IssueBO issue = await GetIssue(id);
            if(issue == null)
                throw new InvalidOperationException("issue doesn't exist");

            issue.IsDeleted = true;
            Context.Issues.AddOrUpdate(issue);
            await Context.SaveChangesAsync();

        }

        public async Task<IssueBO> GetRequirementLinks(int id)
        {
           var t = await Context.Issues.Where(i=>i.Id == id && !i.IsDeleted).Include(i=>i.Requirements.Select(m=>m.TypeDescription)).FirstOrDefaultAsync();
           return t;
        }


        public async Task<IssueBO> GetIssue(int id)
        {
            if (id <= 0)
                throw new ArgumentNullException("id");
            return
                await
                    Context.Issues.Include(p=>p.CreatedBy).Include(p=>p.AssignedTo).Include(p=>p.UpdatedBy).Include(p =>p.Requirements).Include(p=>p.Project)
                        .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);
        }

        public async Task<List<IssueBO>> GetIssues(int project, int pageIndex, int pageSize, ApplicationUser user)
        {
            if (user == null)
                throw new ArgumentNullException("user");
            var result = await
                    Context.Issues.OrderByDescending(p => p.Priority)
                        .Where(p => p.Project.Id == project && !p.IsDeleted).Include(p => p.AssignedTo).Include(p => p.Project).Include(p => p.CreatedBy).Include(p => p.UpdatedBy)
                        .Skip(pageSize * pageIndex)
                        .Take(pageSize)
                        .ToListAsync();
            return result;
        }

        public List<IssueBO> GetIssues(int id)
        {
            if (id <= 0)
                throw new ArgumentNullException("id");
            return Context.Issues.OrderByDescending(p => p.Priority)
                        .Where(p => p.Project.Id == id && !p.IsDeleted).Include(p => p.AssignedTo).Include(p => p.Project).Include(p => p.CreatedBy).Include(p => p.UpdatedBy)
                        .ToList();
        }

        public List<IssueBO> GetIssues(ApplicationUser user)
        {
            return Context.Issues.OrderByDescending(p => p.Id).Include(i=>i.Project).Include(i=>i.AssignedTo).Include(i=>i.CreatedBy)
                        .Where(p => !p.IsDeleted && p.AssignedTo.Id == user.Id).Take(10)
                        .ToList();
        }

        public Task Update(IssueBO issue)
        {
            if (issue == null)
                throw new ArgumentNullException("issue");
            if (issue.Id == 0)
                throw new ArgumentException("InvalidId");
            if (String.IsNullOrWhiteSpace(issue.Name))
                throw new ArgumentNullException("issue name");

            Context.Issues.AddOrUpdate(issue);
            return Context.SaveChangesAsync();
        }

        public async Task<IssueBO> Create(IssueBO isuue)
        {
            if (isuue == null)
                throw new ArgumentNullException("isuue");
            if (isuue.Id != 0)
                throw new ArgumentException("InvalidId");
            if (String.IsNullOrWhiteSpace(isuue.Name))
                throw new ArgumentNullException("issue name");

            isuue = Context.Issues.Add(isuue);
            await Context.SaveChangesAsync();
            return isuue;
        }

        public async Task<ProjectBO> AddUser(ProjectBO project)
        {
            if (project == null)
                throw new ArgumentNullException("project");
            if (project.Id != 0)
                throw new ArgumentException("InvalidId");
            if (String.IsNullOrWhiteSpace(project.Name))
                throw new ArgumentNullException("project name");

            Context.Projects.AddOrUpdate(project);
            await Context.SaveChangesAsync();
            return project;
        }
    }
}