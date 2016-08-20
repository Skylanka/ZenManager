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
    public class AttributeManager : BaseManager
    {
        /* public async Task<ProjectBO> GetProject(int id, ApplicationUser user)
         {
             if (id <= 0)
                 throw new ArgumentNullException("id");
             if (user == null)
                 throw new ArgumentNullException("user");

             return
                 await
                     Context.Projects.FirstOrDefaultAsync(
                         p => p.Id == id && !p.IsDeleted && p.Users.FirstOrDefault(u => u.Id == user.Id) != null);
         }*/

         public async Task<List<AttributeDescriptionBO>> GetAttributeTypes(int attrId, int projectId, int reqId)
        {
            if (attrId <= 0)
                throw new ArgumentNullException("attrId");
            if (projectId <= 0)
                throw new ArgumentNullException("projectId");
            if (reqId <= 0)
                throw new ArgumentNullException("reqIdid");

             var result = await
                     Context.AttributeDescription.Where(
                         a =>
                             a.ProjectsRequirenmentAttributeTypes.Contains(new ProjectsRequirenmentAttributeTypesBO()
                             {
                                 AttributeDescriptionId = attrId
                             })).ToListAsync();
             return result;
             /*Context.Attributes.Include(p => p.Type)
                        .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);*/
        }

        public async Task<List<AttributeDescriptionBO>> GetAttributeTypes(int projectId)
        {
            if (projectId <= 0)
                throw new ArgumentNullException("projectId");

            var result = await Context.AttributeDescription.Where(a => a.Project.Id == projectId).ToListAsync();
            return result;
        }
        public async Task<AttributeDescriptionBO> GetAttributeType(int typeId)
        {
            if (typeId <= 0)
                throw new ArgumentNullException("typeId");

            var result = await Context.AttributeDescription.Where(a => a.Id == typeId).FirstOrDefaultAsync();
            return result;
        }

        public async Task<List<AttributeDescriptionBO>> GetAttributeTypes(int projectId, int reqTypeId)
        {
            if (projectId <= 0)
                throw new ArgumentNullException("projectId");

            var result = await Context.ProjectsRequirenmentAttributeTypes.Where(a=>a.ProjectId==projectId && a.RequirementsDescriptionId == reqTypeId).OrderBy(a=>a.AttributeDescriptionId).Select(a=>a.AttributeDescription).ToListAsync();
            return result;
        }

        public async Task<AttributeBO> GetAttribute(int id)
        {
            if (id <= 0)
                throw new ArgumentNullException("id");
            return
                await
                    Context.Attributes.Include(p => p.Type)
                        .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);
        }

       /* public async Task<List<AttributeBO>> GetAttributes(int id)
        {
            if (id <= 0)
                throw new ArgumentNullException("id");
            return
                await
                    Context.Attributes.Include(p => p.Type)
                        .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);
        }*/

        /*  public async Task<List<AttributeBO>> GetAttributes(int reqId)
          {
              if (reqId <= 0)
                  throw new ArgumentNullException("reqId");
              return
                  await
                      Context.Attributes.Include(p => p.Type).Where(a=>a.Type.Id == reqId && a.IsDeleted)
                          .ToListAsync();
          }*/

        public async Task<List<ProjectBO>> GetProjects(int pageIndex, int pageSize, ApplicationUser user)
        {
            if (user == null)
                throw new ArgumentNullException("user");
            return
                await
                    Context.Projects.OrderByDescending(p => p.Id)
                        .Where(p => p.Users.FirstOrDefault(u => u.Id == user.Id) != null && !p.IsDeleted)
                        .Skip(pageSize * pageIndex)
                        .Take(pageSize)
                        .ToListAsync();
        }

        public Task Update(AttributeBO attribute)
        {
            if (attribute == null)
                throw new ArgumentNullException("attribute");
            if (attribute.Id == 0)
                throw new ArgumentException("InvalidId");

            Context.Attributes.AddOrUpdate(attribute);
            return Context.SaveChangesAsync();
        }

        public async Task<AttributeBO> Create(AttributeBO attribute)
        {
            if (attribute == null)
                throw new ArgumentNullException("attribute");
            if (attribute.Id != 0)
                throw new ArgumentException("InvalidId");

            attribute = Context.Attributes.Add(attribute);
            await Context.SaveChangesAsync();
            return attribute;
        }

        public async Task<AttributeDescriptionBO> Create(AttributeDescriptionBO attribute)
        {
            if (attribute == null)
                throw new ArgumentNullException("attribute");
            if (attribute.Id != 0)
                throw new ArgumentException("InvalidId");

            attribute = Context.AttributeDescription.Add(attribute);
            await Context.SaveChangesAsync();
            return attribute;
        }

        public Task Update(AttributeDescriptionBO attribute)
        {
            if (attribute == null)
                throw new ArgumentNullException("attribute");
            if (attribute.Id == 0)
                throw new ArgumentException("InvalidId");

            Context.AttributeDescription.AddOrUpdate(attribute);
            return Context.SaveChangesAsync();
        }
    }
}