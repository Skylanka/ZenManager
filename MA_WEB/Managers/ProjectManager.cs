using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading.Tasks;
using MA_WEB.Models;
using Model.Base;
using Model.BusinessObjects;

namespace MA_WEB.Managers
{
    public class ProjectManager : BaseManager
    {
        public async Task<ProjectBO> GetProject(int id, ApplicationUser user)
        {
            if (id <= 0)
                throw new ArgumentNullException("id");
            if (user == null)
                throw new ArgumentNullException("user");

            return
                await
                    Context.Projects.FirstOrDefaultAsync(
                        p => p.Id == id && !p.IsDeleted && p.Users.FirstOrDefault(u => u.Id == user.Id) != null);
        }

        public async Task<int> GetCount(ApplicationUser user)
        {
            return await Context.Projects.Where(p => p.Users.FirstOrDefault(u => u.Id == user.Id) != null && !p.IsDeleted).CountAsync();
        }
        public async Task Delete(int id)
        {
            if (id <= 0)
                throw new ArgumentNullException("project");

            ProjectBO project = await GetProject(id);
            if(project == null)
                throw new InvalidOperationException("Project doesn't exist");

            project.IsDeleted = true;
            Context.Projects.AddOrUpdate(project);
            await Context.SaveChangesAsync();

        }

        public async Task<ProjectBO> GetProject(int id)
        {
            if (id <= 0)
                throw new ArgumentNullException("id");
            return
                await
                    Context.Projects.Include(p=>p.CreatedBy).Include(p=>p.Users).Include(p=>p.Docs)
                        .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);
        }

        public ProjectBO GetProjectForInvitation(int id)
        {
            if (id <= 0)
                throw new ArgumentNullException("invt id");
            return
                    Context.Projects.Include(p => p.Users).Include(p=>p.Invitations)
                        .FirstOrDefault(p => p.Invitations.FirstOrDefault().Id == id && !p.IsDeleted);
        }

        public async Task<List<ProjectBO>> GetProjects(int pageIndex, int pageSize, ApplicationUser user)
        {
            if (user == null)
                throw new ArgumentNullException("user");
            var result = await
                    Context.Projects.OrderBy(p => p.Id).Include(p=>p.CreatedBy)
                        .Where(p => p.Users.FirstOrDefault(u => u.Id == user.Id) != null && !p.IsDeleted)
                        .Skip(pageSize * pageIndex)
                        .Take(pageSize)
                        .ToListAsync();
            return result;
        }

        public async Task<List<ProjectBO>> GetProjects(ApplicationUser user)
        {
            if (user == null)
                throw new ArgumentNullException("user");
            var result = await
                    Context.Projects.OrderBy(p => p.Id)
                        .Where(p => p.Users.FirstOrDefault(u => u.Id == user.Id) != null && !p.IsDeleted && p.Status == 0)
                        .Take(7)
                        .ToListAsync();
            return result;
        }

        public async Task<int> GetIssueCount(int id)
        {
            if(id <= 0)
                throw new ArgumentNullException("id");
            return await Context.Issues.Where(i => i.Project.Id == id).CountAsync();

        }

        public async Task<int> GetRequirementCount(int id)
        {
            if (id <= 0)
                throw new ArgumentNullException("id");
            return await Context.Requirements.Where(i => i.Project.Id == id).CountAsync();

        }

        public List<ProjectBO> GetProjects(int id)
        {
            return Context.Projects.OrderBy(p => p.Id)
                        .Where(p => p.Users.FirstOrDefault(u => u.Id == id) != null && !p.IsDeleted)
                        .ToList();
        }

        public Task Update(ProjectBO project)
        {
            if (project == null)
                throw new ArgumentNullException("project");
            if (project.Id == 0)
                throw new ArgumentException("InvalidId");
            if (String.IsNullOrWhiteSpace(project.Name))
                throw new ArgumentNullException("project name");

            Context.Projects.AddOrUpdate(project);
            return Context.SaveChangesAsync();
        }

        public async Task AddDoc(DocumentationBO doc, ProjectBO project)
        {
            if (project == null)
                throw new ArgumentNullException("project");
            if (project.Id == 0)
                throw new ArgumentException("InvalidId project");
            if (doc == null)
                throw new ArgumentNullException("doc");

            if (String.IsNullOrWhiteSpace(project.Name))
                throw new ArgumentNullException("project name");

            doc = Context.Documentations.Add(doc);
            if (project.Docs == null)
                project.Docs = new List<DocumentationBO>();
            project.Docs.Add(doc);
            await Update(project);
            await Context.SaveChangesAsync();
        }

        public async Task<ProjectBO> Create(ProjectBO project)
        {
            if (project == null)
                throw new ArgumentNullException("project");
            if (project.Id != 0)
                throw new ArgumentException("InvalidId");
            if (String.IsNullOrWhiteSpace(project.Name))
                throw new ArgumentNullException("project name");

            project = Context.Projects.Add(project);
            await Context.SaveChangesAsync();
            return project;
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

        public async Task AddInvitation(InvitationBO invt, ApplicationUser user, ProjectBO project)
        {
            if (invt == null)
                throw new ArgumentNullException("invt");
            if (String.IsNullOrWhiteSpace(invt.Email))
                throw new ArgumentNullException("invt email");
            if (user.Id <= 0)
                throw new ArgumentException("InvalidId user");
            if (project.Id <= 0)
                throw new ArgumentException("InvalidId project");

            invt = Context.Invitations.Add(invt);
            if (user.Invitations == null)
                user.Invitations = new List<InvitationBO>();
            if (project.Invitations == null)
                project.Invitations = new List<InvitationBO>();
            user.Invitations.Add(invt);
            Context.Users.AddOrUpdate(user);
            project.Invitations.Add(invt);
            await Update(project);
            await Context.SaveChangesAsync();
        }

        public async Task AddInvitation(InvitationBO invt)
        {
            if (invt == null)
                throw new ArgumentNullException("invt");
            if (String.IsNullOrWhiteSpace(invt.Email))
                throw new ArgumentNullException("invt email");

            invt = Context.Invitations.Add(invt);
            await Context.SaveChangesAsync();
        }

        public async Task UpdateInvitation(InvitationViewModel invt, ApplicationUser user, ProjectBO project)
        {
            if (invt == null)
                throw new ArgumentNullException("invt");
            if (invt.Id == 0)
                throw new ArgumentException("InvalidId");
            if (user.Id <= 0)
                throw new ArgumentException("InvalidId user");
            if (project.Id <= 0)
                throw new ArgumentException("InvalidId project");

            InvitationBO invtBo = await Context.Invitations.FirstOrDefaultAsync(i => i.Id == invt.Id);
            invtBo.Status = true;
            Context.Invitations.AddOrUpdate(invtBo);
            project.Users.Add(user);
            await Update(project);
            await Context.SaveChangesAsync();
        }
    }
}