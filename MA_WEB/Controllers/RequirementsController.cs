using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using MA_WEB.Controllers.Parameters;
using MA_WEB.Managers;
using MA_WEB.Models;
using Model.Base;
using Model.BusinessObjects;

namespace MA_WEB.Controllers
{
    [RoutePrefix("api/requirements")]
    public class RequirementsController : BaseController
    {
        private readonly RequirementManager _manager = new RequirementManager();

        [Route("saveRequirement")]
        [HttpPost]
        public async Task<int> SaveRequirement(RequirementViewModel requirementView)
        {
            if (requirementView == null)
                throw new ArgumentNullException("requirementView");
            
            var user = await ValidateCurrentUser();
            if (requirementView.Id == 0)
            {
                RequirementBO requirementBo = requirementView;
                requirementBo.Project = await new ProjectManager().GetProject(requirementView.ProjectId);
                requirementBo.TypeDescription = await _manager.GetRequirementType(requirementView.RequirementType);
                requirementBo.Created = DateTime.Now;
                requirementBo.CreatedBy = user;
                requirementBo.Updated = DateTime.Now;
                requirementBo.UpdatedBy = user;
                requirementBo.Comments = new List<CommentBO>();
                requirementBo.UniqueTag = requirementView.UniqueTag;
                requirementBo.Attributes = new List<AttributeBO>();
                List<AttributeDescriptionBO> attrTypes = await new AttributeManager().GetAttributeTypes(requirementBo.Project.Id, requirementBo.TypeDescription.Id);
                for (int i = 0; i < attrTypes.Count; i++)
                {
                    requirementBo.Attributes.Add(new AttributeBO()
                    {
                        Id = 0,
                        Type = attrTypes[i],
                        Value = attrTypes[i].DefaultValue
                    });
                }
                requirementView = await _manager.Create(requirementBo);
                //Add default Attributes
                return requirementView.Id;
            }
            else
            {
                RequirementBO req = await _manager.GetRequirement(requirementView.Id);
                req.Name = requirementView.Name;
                req.Description = requirementView.Description;
                req.Updated = DateTime.Now;
                req.UpdatedBy = user;
                await _manager.Update(req);
                return requirementView.Id;
            }
        }

        [Route("deleteRequirement")]
        [HttpPost]
        public async Task DeleteRequirement(IdParams parameters)
        {
            ValidateParameters(parameters);

            var user = await ValidateCurrentUser();
            await _manager.Delete(parameters.Id);
        }

        [Route("deleteRequirementLink")]
        [HttpPost]
        public async Task DeleteRequirementLink(IdParams parameters)
        {
            ValidateParameters(parameters);
            var user = await ValidateCurrentUser();
            await _manager.DeleteRequirementLink(parameters.Id);
        }

        [Route("deleteRequirementType")]
        [HttpPost]
        public async Task DeleteRequirementType(IdParams parameters)
        {
            ValidateParameters(parameters);

            var user = await ValidateCurrentUser();

            await _manager.DeleteType(parameters.Id);
        }


        [Route("addRequirementLink")]
        [HttpPost]
        public async Task AddRequirementLink(RequirementLinkViewModel reqlink)
        {
            RequirementsLinkBO link = new RequirementsLinkBO();
            RequirementBO req1 = await _manager.GetRequirement(reqlink.ReqId);
            RequirementBO req2 = await _manager.GetRequirement(reqlink.SpecId);
            if (reqlink.Type == Enum.GetName(typeof(RelationType), RelationType.Child))
            {
                link = new RequirementsLinkBO()
                {
                    RequirementFrom = req1,
                    RequirementTo = req2
                };
            }
            else if (reqlink.Type == Enum.GetName(typeof(RelationType), RelationType.Parent))
            {
                link = new RequirementsLinkBO()
                {
                    RequirementFrom = req2,
                    RequirementTo = req1
                };
            }
            await _manager.AddLink(link);
        }

        [Route("getRequirementWithAttr")]
        [Route("getRequirementWithAttr/{Id}")]
        [HttpGet]
        public async Task<RequirementViewModel> GetRequirementWithAttr([FromUri]IdParams parameters)
        {
            if (parameters == null)
                throw new ArgumentNullException("parameters");

            ValidateParameters(parameters);

            var user = await ValidateCurrentUser();
            var requirements = await _manager.GetRequirement(parameters.Id);
            var result1 = new RequirementViewModel()
            {
                Attr = requirements.Attributes.OrderBy(a => a.Type.Id).ToList()
            };

            var result = new RequirementViewModel()
            {
                Name = requirements.Name,
                Description = requirements.Description,
                Id = requirements.Id,
                UniqueTag = requirements.UniqueTag,
                Attributes = new List<Attributes>(),
                Created = requirements.Created.ToString(),
                CreatedBy = requirements.CreatedBy.FirstName + " " + requirements.CreatedBy.LastName,
                Updated = requirements.Updated.ToString(),
                UpdatedBy = requirements.UpdatedBy.FirstName + " " + requirements.UpdatedBy.LastName
            };
                for (int j = 0; j < result1.Attr.Count; j++)
                {
                    result.Attributes.Add(new Attributes()
                    {
                        Id = result1.Attr[j].Id,
                        AttrType = result1.Attr[j].Type.Id,
                        Name = result1.Attr[j].Type.Name,
                        Value = result1.Attr[j].Value,
                        EditType = result1.Attr[j].Type.Type,
                        Values = result1.Attr[j].Type.Values
                    });
                }

            return result;
        }

        [Route("getRequirement")]
        [Route("getRequirement/{Id}")]
        [HttpGet]
        public async Task<RequirementBO> GetRequirement([FromUri]IdParams parameters)
        {
            if (parameters == null)
                throw new ArgumentNullException("parameters");

            ValidateParameters(parameters);

            var user = await ValidateCurrentUser();
            var result = await _manager.GetRequirement(parameters.Id);
            
            return result;
        }

        [Route("getRequirementType")]
        [Route("getRequirementType/{Id}")]
        [HttpGet]
        public async Task<RequirementTypeViewModel> GetRequirementType([FromUri]IdParams parameters)
        {
            if (parameters == null)
                throw new ArgumentNullException("parameters");

            ValidateParameters(parameters);

            var type = await _manager.GetRequirementType(parameters.Id);
            int[] attr = new int[type.ProjectsRequirenmentAttributeTypes.Count];
            for (int i = 0; i < type.ProjectsRequirenmentAttributeTypes.Count; i++)
                attr[i] = type.ProjectsRequirenmentAttributeTypes.ElementAt(i).AttributeDescriptionId;

            return new RequirementTypeViewModel()
            {
                Id = type.Id,
                Name = type.Name,
                Description = type.Description,
                Tag = type.Tag,
                Attributes = attr
            };
        }

        [Route("getRequirements")]
        [Route("getRequirements/{PageSize}/{Page}")]
        [HttpGet]
        public async Task<List<RequirementBO>> GetRequirements([FromUri]PagingParams parameters)
        {
            if (parameters == null)
                throw new ArgumentNullException("parameters");

            ValidateParameters(parameters);

            var u = User.Identity.Name;

            var user = await ValidateCurrentUser();

            return await _manager.GetRequirements(parameters.Page, parameters.PageSize);
        }

        [Route("getLatestRequirements")]
        [HttpGet]
        public async Task<List<RequirementViewModel>> GetLatestRequirements()
        {
           var u = User.Identity.Name;

            var user = await ValidateCurrentUser();

            List<RequirementBO> reqs = await _manager.GetRequirements(user);
            return reqs.Select(t => (RequirementViewModel) t).ToList();
        }

        [Route("getRequirementLinks")]
        [Route("getRequirementLinks/{Id}")]
        [HttpGet]
        public async Task<List<RequirementLinkViewModel>> GetRequirementLinks([FromUri]IdParams parameters)
        {
            if (parameters == null)
                throw new ArgumentNullException("parameters");

            ValidateParameters(parameters);

            var u = User.Identity.Name;

            var user = await ValidateCurrentUser();

            List<RequirementsLinkBO> links = await _manager.GetRequirementLinks(parameters.Id);

            List<RequirementLinkViewModel> result = new List<RequirementLinkViewModel>();

            for (int i = 0; i < links.Count; i++)
            {

                    result.Add(new RequirementLinkViewModel()
                    {
                        LinkId = links[i].Id,
                        Name = links[i].RequirementFrom.Id == parameters.Id ?  links[i].RequirementTo.Name : links[i].RequirementFrom.Name,
                        ReqId = links[i].RequirementFrom.Id == parameters.Id ?  links[i].RequirementTo.Id : links[i].RequirementFrom.Id,
                        SpecId = links[i].RequirementFrom.Id == parameters.Id ?  links[i].RequirementTo.TypeDescription.Id : links[i].RequirementFrom.TypeDescription.Id,
                        Specification = links[i].RequirementFrom.Id == parameters.Id ?  links[i].RequirementTo.TypeDescription.Name : links[i].RequirementFrom.TypeDescription.Name,
                        Tag = links[i].RequirementFrom.Id == parameters.Id ? links[i].RequirementTo.UniqueTag : links[i].RequirementFrom.UniqueTag,
                        Type = links[i].RequirementFrom.Id == parameters.Id ? Enum.GetName(typeof(RelationType), RelationType.Child) : Enum.GetName(typeof(RelationType), RelationType.Parent)
                    });
                
            }
            return result;
        }

        [Route("saveRequirementLink")]
        [HttpPost]
        public async Task<int> SaveRequirementLink(RequirementViewModel requirementView)
        {
            if (requirementView == null)
                throw new ArgumentNullException("requirementView");

            var user = await ValidateCurrentUser();
            if (requirementView.Id == 0)
            {
                RequirementBO requirementBo = requirementView;
                requirementBo.Project = await new ProjectManager().GetProject(requirementView.ProjectId);
                requirementBo.TypeDescription = await _manager.GetRequirementType(requirementView.RequirementType);
                requirementBo.Created = DateTime.Now;
                requirementBo.CreatedBy = user;
                requirementBo.UniqueTag = requirementView.UniqueTag;

                requirementView = await _manager.Create(requirementBo);
                //Add default Attributes
                return requirementView.Id;
            }
            else
            {
                RequirementBO req = await _manager.GetRequirement(requirementView.Id);
                req.Name = requirementView.Name;
                req.Description = requirementView.Description;
                req.Updated = DateTime.Now;
                req.UpdatedBy = user;
                await _manager.Update(req);
                return requirementView.Id;
            }
        }


        [Route("getAllTypesOfRequirement")]
        [Route("getAllTypesOfRequirement/{Id}")]
        [HttpGet]
        public async Task<List<RequirementTypeViewModel>> GetAllTypesOfRequirement([FromUri]IdParams parameters)
        {
            if (parameters == null)
                throw new ArgumentNullException("parameters");

            var user = await ValidateCurrentUser();
            var types = await _manager.GetRequirementsTypes(parameters.Id);
            var model = new List<RequirementTypeViewModel>();
            for (int i = 0; i < types.Count; i++)
            {
                model.Add(new RequirementTypeViewModel()
                {
                    Id = types[i].Id,
                    Name = types[i].Name,
                    Count = _manager.CountRequirementsByType(types[i].Id, parameters.Id),
                    Description = types[i].Description,
                    Tag = types[i].Tag
                });
            }
            return model;
        }

        [Route("getTypesAndRequirements")]
        [Route("getTypesAndRequirements/{Id}")]
        [HttpGet]
        public async Task<List<RequirementTypeViewModel>> GetTypesAndRequirements([FromUri]IdParams parameters)
        {
            if (parameters == null)
                throw new ArgumentNullException("parameters");

            var user = await ValidateCurrentUser();
            var types = await _manager.GetRequirementsTypes(parameters.Id);
            var model = new List<RequirementTypeViewModel>();
            for (int i = 0; i < types.Count; i++)
            {
                model.Add(new RequirementTypeViewModel()
                {
                    Id = types[i].Id,
                    Name = types[i].Name,
                    Count = _manager.CountRequirementsByType(types[i].Id, parameters.Id),
                    Description = types[i].Description,
                    Tag = types[i].Tag
                });
            }
            return model;
        }

        [Route("getWidgetRequirements")]
        [HttpGet]
        public async Task<RequirementWidgetViewModel> GetWidgetRequirements([FromUri]IdParams parameters)
        {
            if (parameters == null)
                throw new ArgumentNullException("parameters");
            var user = await ValidateCurrentUser();
            var typesVM = new Collection<RequirementWidgetViewModelItem>();
            var types = await _manager.GetRequirementsTypes(parameters.Id);
            for (int i = 0; i < types.Count; i++)
            {
                typesVM.Add(new RequirementWidgetViewModelItem()
                {
                    Name = types[i].Tag + ": " +types[i].Name,
                    Count = _manager.CountRequirementsByType(types[i].Id, parameters.Id),
                    Description = types[i].Description,
                    LastUpdated = await _manager.CountLastUpdated(types[i].Id),
                    Type = types[i].Id
                });
            }
            return new RequirementWidgetViewModel()
            {
                Types = typesVM
            };
        }

        [Route("getRequirementsByType")]
        [Route("getRequirementsByType/{Id}/{ProjectId}")]
        [HttpGet]
        public async Task<List<RequirementsByTypeViewModel>> GetRequirementsByType([FromUri]IdProjectParams parameters)
        {
            if (parameters == null)
                throw new ArgumentNullException("parameters");

            AttributeManager attrManager = new AttributeManager();
            var attrTypes = await attrManager.GetAttributeTypes(parameters.ProjectId, parameters.Id);

            var user = await ValidateCurrentUser();
            var requirements = await _manager.GetRequirementsByType(parameters.Id, parameters.ProjectId);
            var result1 = new List<RequirementViewModel>();

            for (int i = 0; i < requirements.Count; i++)
            {
                result1.Add(new RequirementViewModel()
                {
                    /*                    Name = requirements[i].Name,
                                        Description = requirements[i].Description,
                                        Id = requirements[i].Id,
                                        ProjectId = parameters.ProjectId,
                                        RequirementType = parameters.Id,
                                        UniqueTag = requirements[i].UniqueTag,*/
                    Attr = requirements[i].Attributes.OrderBy(a => a.Type.Id).ToList()
                });
            }

            var result = new List<RequirementsByTypeViewModel>();

            for (int i = 0; i < requirements.Count; i++)
            {
                result.Add(new RequirementsByTypeViewModel()
                {
                    Name = requirements[i].Name,
                    Description = requirements[i].Description,
                    Id = requirements[i].Id,
                    UniqueTag = requirements[i].UniqueTag,
                    Attributes = new List<Attributes>()
                    
                });
                for (int j = 0; j < result1[i].Attr.Count; j++)
                {
                    result[i].Attributes.Add(new Attributes()
                    {
                        AttrType = result1[i].Attr[j].Type.Id,
                        Name = result1[i].Attr[j].Type.Name,
                        Value = result1[i].Attr[j].Value,
                        EditType = result1[i].Attr[j].Type.Type,
                        Values = result1[i].Attr[j].Type.Values
                    });
                }
            }


            return result;
        }

        [Route("getTraceability")]
        [Route("getTraceability/{Id}")]
        [HttpGet]
        public async Task<TraceabilityViewModel> GetTraceability([FromUri] TraceabilityParams parameters)
        {
            if (parameters == null)
                throw new ArgumentNullException("parameters");

            ValidateParameters(parameters);
            
            TraceabilityViewModel result = new TraceabilityViewModel();
            result.Columns = new List<TraceabilityObject>();
            result.Rows = new List<TraceabilityObject>();
            List<RequirementBO> rows = await _manager.GetRequirementsByType(parameters.IdFrom);
            List<RequirementBO> columns = await _manager.GetRequirementsByType(parameters.IdTo);
            foreach (var row in rows)
            {
                result.Rows.Add(row);
            }
            foreach (var column in columns)
            {
                result.Columns.Add(column);
            }
            
            for (int i = 0; i < rows.Count; i++)
            {
                List<RequirementsLinkBO> links = await _manager.GetRequirementLinks(rows[i].Id);
                result.Rows[i].Links = new int[columns.Count];
                if (links.Count == 0)
                    for (int j = 0; j < columns.Count; j++)
                        result.Rows[i].Links[j] = 0;
                else
                {
                    for (int j = 0; j < columns.Count; j++)
                    {
                        foreach (var link in links)
                        {
                            if (link.RequirementFrom.Id == rows[i].Id && link.RequirementTo.Id == columns[j].Id)
                            {
                                result.Rows[i].Links.SetValue(1,j);
                                break;
                            }
                            else if (link.RequirementFrom.Id == columns[j].Id && link.RequirementTo.Id == rows[i].Id)
                            {
                                result.Rows[i].Links.SetValue(2, j);
                                break;
                            }
                            else result.Rows[i].Links.SetValue(0, j);
                        }
                    }
                }               
            }

            return result;
        }


        public async Task<List<RequirementLinkViewModel>> GetTraceability1([FromUri]TraceabilityParams parameters)
        {
            if (parameters == null)
                throw new ArgumentNullException("parameters");

            ValidateParameters(parameters);

            var u = User.Identity.Name;

            var user = await ValidateCurrentUser();

            List<RequirementsLinkBO> linksFrom = await _manager.GetRequirementLinksFrom(parameters.IdFrom, parameters.IdTo);
            List<RequirementsLinkBO> linksTo = await _manager.GetRequirementLinksTo(parameters.IdFrom, parameters.IdTo);

            List<RequirementLinkViewModel> result = new List<RequirementLinkViewModel>();

            for (int i = 0; i < linksFrom.Count; i++)
            {
                    result.Add(new RequirementLinkViewModel()
                    {
                        Name = linksFrom[i].RequirementTo.Name,
                        ReqId = linksFrom[i].RequirementTo.Id,
                        SpecId = linksFrom[i].RequirementTo.TypeDescription.Id,
                        Specification = linksFrom[i].RequirementTo.TypeDescription.Name,
                        Tag = linksFrom[i].RequirementTo.UniqueTag,
                        Type = Enum.GetName(typeof(RelationType), RelationType.Child)
                    });
            }
            for (int i = 0; i < linksTo.Count; i++)
            {
                result.Add(new RequirementLinkViewModel()
                {
                    Name = linksTo[i].RequirementFrom.Name,
                    ReqId = linksTo[i].RequirementFrom.Id,
                    SpecId = linksTo[i].RequirementFrom.TypeDescription.Id,
                    Specification = linksTo[i].RequirementFrom.TypeDescription.Name,
                    Tag = linksTo[i].RequirementFrom.UniqueTag,
                    Type = Enum.GetName(typeof (RelationType), RelationType.Parent)
                });
            }
            return result;
        }


        [Route("getRequirementsTypesWithRequirements")]
        [Route("getRequirementsTypesWithRequirements/{Id}")]
        [HttpGet]
        public async Task<List<RequirementTypeViewModel>> GetRequirementsTypesWithRequirements([FromUri]IdParams parameters)
        {
            if (parameters == null)
                throw new ArgumentNullException("parameters");

            ValidateParameters(parameters);

            var u = User.Identity.Name;

            var user = await ValidateCurrentUser();

            var requirementTypes = await _manager.GetRequirementsTypes(parameters.Id);
            var requirements = await _manager.GetRequirements(parameters.Id);
            List <RequirementTypeViewModel> result = new List<RequirementTypeViewModel>();

            for (int i = 0; i<requirementTypes.Count; i++)
            {
                var temp = new List<RequirementBO>();
                for (int j = 0; j < requirements.Count; j++)
                {
                    if (requirements[j].TypeDescription.Id == requirementTypes[i].Id)
                        temp.Add(new RequirementBO()
                        {
                            Id = requirements[j].Id,
                            Name = "[" + requirements[j].UniqueTag + "] " + requirements[j].Name
                        });
                }
                result.Add(new RequirementTypeViewModel()
                {
                    Id = requirementTypes[i].Id,
                    Name = requirementTypes[i].Tag + ": " +requirementTypes[i].Name,
                    Requirements = temp
                });
            }
            return result;
        }

        [Route("saveRequirementType")]
        [HttpPost]
        public async Task<int> SaveRequirementType(RequirementTypeViewModel model)
        {
            if(model == null)
                throw new ArgumentNullException("view model");
            if (model.Id == 0)
            {
                RequirementsDescriptionBO type = new RequirementsDescriptionBO();
                type.Id = model.Id;
                type.Name = model.Name;
                type.Description = model.Description;
                type.Tag = model.Tag;
                type.Project = await new ProjectManager().GetProject(model.ProjectId);
                type.ProjectsRequirenmentAttributeTypes = new List<ProjectsRequirenmentAttributeTypesBO>();
                if (model.Attributes.Length > 0)
                {
                    for (int i = 0; i < model.Attributes.Length; i++)
                    {
                        type.ProjectsRequirenmentAttributeTypes.Add(new ProjectsRequirenmentAttributeTypesBO()
                        {
                            ProjectId = type.Project.Id,
                            RequirementsDescriptionId = type.Id,
                            AttributeDescriptionId = model.Attributes[i]
                        });
                    }
                }
                type = await _manager.CreateRequirementType(type);
                return type.Id;
            }
            else
            {

                return 1;
            }
           
        } 

    }
}
