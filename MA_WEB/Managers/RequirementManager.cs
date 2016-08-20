using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Model.Base;
using Model.BusinessObjects;

namespace MA_WEB.Managers
{
    public class RequirementManager : BaseManager
    {
        public async Task<RequirementBO> GetRequirement(int id)
        {
            if (id <= 0)
                throw new ArgumentNullException("id");
            return
                await
                    Context.Requirements.Include(p => p.TypeDescription).Include(r=>r.Attributes.Select(a=>a.Type)).Include(p=>p.CreatedBy).Include(p=>p.UpdatedBy)
                        .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);
        }

        public async Task<int> CountLastUpdated(int id)
        {
            if (id <= 0)
                throw new ArgumentNullException("id");
            DateTime date = DateTime.Now.AddDays(-7);
            return
                await
                    Context.Requirements.Where(r=> r.TypeDescription.Id == id && !r.IsDeleted && r.Updated.CompareTo(date) > 0)
                        .CountAsync();
        }

        public async Task<RequirementsLinkBO> GetRequirementLink(int id)
        {
            if (id <= 0)
                throw new ArgumentNullException("id");
            return
                await
                    Context.ReqLinks.FirstOrDefaultAsync(r => r.Id == id && !r.IsDeleted);
        } 

        public async Task<List<RequirementsLinkBO>> GetRequirementLinks(int id)
        {
            if (id <= 0)
                throw new ArgumentNullException("id");
            return
                await
                    Context.ReqLinks.Include(p => p.RequirementFrom.TypeDescription).Include(r => r.RequirementTo.TypeDescription).Where(p =>( p.RequirementFrom.Id == id || p.RequirementTo.Id == id)&& !p.IsDeleted)
                        .ToListAsync();
        }


        public async Task<List<RequirementsLinkBO>> GetRequirementLinksFrom(int fromId, int toId)
        {
            if (fromId <= 0)
                throw new ArgumentNullException("fromId");
            return
                await
                    Context.ReqLinks.Include(p => p.RequirementFrom.TypeDescription).Include(r => r.RequirementTo.TypeDescription).Where(p => p.RequirementFrom.TypeDescription.Id == fromId && p.RequirementTo.TypeDescription.Id == toId)
                        .ToListAsync();
        }

        public async Task<List<RequirementsLinkBO>> GetRequirementLinksTo(int fromId, int toId)
        {
            if (fromId <= 0)
                throw new ArgumentNullException("fromId");
            return
                await
                    Context.ReqLinks.Include(p => p.RequirementFrom.TypeDescription).Include(r => r.RequirementTo.TypeDescription).Where(p => p.RequirementFrom.TypeDescription.Id == toId || p.RequirementTo.TypeDescription.Id == fromId)
                        .ToListAsync();
        }


        public async Task<RequirementsDescriptionBO> GetRequirementType(int id)
        {
            if (id <= 0)
                throw new ArgumentNullException("id");
            return
                await
                    Context.RequirementsDescription.Include(d=>d.ProjectsRequirenmentAttributeTypes).FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);
        }

        public async Task<List<RequirementBO>> GetRequirements(int pageIndex, int pageSize)
        {

            return
                await
                    Context.Requirements.OrderByDescending(p => p.Id)
                        .Where(p => !p.IsDeleted)
                        .Skip(pageSize * pageIndex)
                        .Take(pageSize)
                        .ToListAsync();
        }

        public async Task<List<RequirementBO>> GetRequirements(int projectId)
        {

            return
                await
                    Context.Requirements
                        .Where(p => !p.IsDeleted && p.Project.Id == projectId)
                        .ToListAsync();
        }

        public async Task<List<RequirementBO>> GetRequirements(ApplicationUser user)
        {

            return
                await
                    Context.Requirements.Include(r=>r.TypeDescription).Include(r=>r.Project)
                        .Where(p => !p.IsDeleted && p.Project.Users.FirstOrDefault(u => u.Id == user.Id).Id == user.Id).Take(10)
                        .ToListAsync();
        }

        public Task Update(RequirementBO requirement)
        {
            if (requirement == null)
                throw new ArgumentNullException("attribute");
            if (requirement.Id == 0)
                throw new ArgumentException("InvalidId");

            Context.Requirements.AddOrUpdate(requirement);
            return Context.SaveChangesAsync();
        }
        public async Task Delete(int id)
        {
            if (id <= 0)
                throw new ArgumentNullException("project");

            RequirementBO req = await GetRequirement(id);
            if (req == null)
                throw new InvalidOperationException("Project doesn't exist");

            req.IsDeleted = true;
            Context.Requirements.AddOrUpdate(req);
            await Context.SaveChangesAsync();

        }

        public async Task DeleteRequirementLink(int id)
        {
            if (id <= 0)
                throw new ArgumentNullException("project");

            RequirementsLinkBO req = Context.ReqLinks.FirstOrDefault(l=>l.Id == id);
            if (req == null)
                throw new InvalidOperationException("Project doesn't exist");

            req.IsDeleted = true;
            Context.ReqLinks.Remove(req);
            await Context.SaveChangesAsync();

        }

        public async Task DeleteType(int id)
        {
            if (id <= 0)
                throw new ArgumentNullException("project");

            RequirementsDescriptionBO req = await GetRequirementType(id);
            if (req == null)
                throw new InvalidOperationException("Project doesn't exist");

            req.IsDeleted = true;
            Context.RequirementsDescription.AddOrUpdate(req);
            await Context.SaveChangesAsync();

        }

        public Task AddLink(RequirementsLinkBO link)
        {
            if (link == null)
                throw new ArgumentNullException("link");
            if (link.RequirementFrom == null)
                throw new ArgumentNullException("RequirementFrom");
            if (link.RequirementTo == null)
                throw new ArgumentNullException("RequirementTo");

            Context.ReqLinks.Add(link);
            return Context.SaveChangesAsync();
        }

        public async Task<RequirementBO> Create(RequirementBO requirement)
        {
            if (requirement == null)
                throw new ArgumentNullException("requirement");
            if (requirement.Id != 0)
                throw new ArgumentException("InvalidId");

            requirement = Context.Requirements.Add(requirement);
            await Context.SaveChangesAsync();
            return requirement;
        }



        public async Task<RequirementsDescriptionBO> CreateRequirementType(RequirementsDescriptionBO requirementType)
        {
            if (requirementType == null)
                throw new ArgumentNullException("requirementType");
            if (requirementType.Id != 0)
                throw new ArgumentException("InvalidId");

            requirementType = Context.RequirementsDescription.Add(requirementType);
            await Context.SaveChangesAsync();
            return requirementType;
        }

        public async Task<RequirementsDescriptionBO> UpdateRequirementType(RequirementsDescriptionBO requirementType)
        {
            if (requirementType == null)
                throw new ArgumentNullException("requirementType");
            if (requirementType.Id != 0)
                throw new ArgumentException("InvalidId");

            Context.RequirementsDescription.AddOrUpdate(requirementType);
            await Context.SaveChangesAsync();
            return requirementType;
        }

        public async Task<List<RequirementsDescriptionBO>> GetRequirementsTypes(int id)
        {

            if (id <= 0)
                throw new ArgumentNullException("id");
            return
                await
                    Context.RequirementsDescription.Where(r=>r.Project.Id == id).ToListAsync();
        }

        public int CountRequirementsByType(int id, int projectId)
        {
            if (id <= 0)
                throw new ArgumentNullException("id");
            return Context.Requirements.Count(r => r.Project.Id == projectId && r.TypeDescription.Id == id);
        }

        public async Task<List<RequirementBO>> GetRequirementsByType(int id, int projectId)
        {
            if (id <= 0)
                throw new ArgumentNullException("id");
            if (projectId <= 0)
                throw new ArgumentNullException("projectId");

            return
                await
                    Context.Requirements.Include(r=>r.Attributes.Select(a=>a.Type))
                        .Where(r => r.Project.Id == projectId && r.TypeDescription.Id == id)
                        .ToListAsync();
        }

        public async Task<List<RequirementBO>> GetRequirementsByType(int id)
        {
            if (id <= 0)
                throw new ArgumentNullException("id");

            return
                await
                    Context.Requirements.Include(r => r.Attributes.Select(a => a.Type))
                        .Where(r =>  r.TypeDescription.Id == id)
                        .ToListAsync();
        }
    }
}