using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using MA_WEB.Controllers.Parameters;
using MA_WEB.Managers;
using MA_WEB.Models;
using Model.BusinessObjects;

namespace MA_WEB.Controllers
{
    [RoutePrefix("api/attributes")]
    public class AttributesController : BaseController
    {
        private readonly AttributeManager _manager = new AttributeManager();

        [Route("saveAttributeType")]
        [HttpPost]
        public async Task<int> SaveAttributeType(AtrributeTypeViewModel model)
        {
            if (model == null)
                throw new ArgumentNullException("attribute type model");

            var user = await ValidateCurrentUser();
            if (model.Id == 0)
            {

                AttributeDescriptionBO attribute = model;
                //attribute.InternalValue = model.Values.ToString();
                attribute.Project = await new ProjectManager().GetProject(model.ProjectId);
                
                model = await _manager.Create(attribute);
                return model.Id;
            }
            else
            {
                await _manager.Update(model);
                return model.Id;
            }
        }

        [Route("updateReqAttributes")]
        [Route("updateReqAttributes/{Id}")]
        [HttpPost]
        public async Task UpdateReqAttributes(AttributeViewModel attribute)
        {
            if (attribute == null)
                throw new ArgumentNullException("attr");

            AttributeBO attr = await _manager.GetAttribute(attribute.Id);
            if (attr.Value != attribute.Value)
            {
                attr.Value = attribute.Value;
            await _manager.Update(attr);
            }
            
        }




        [Route("getAttribute")]
        [Route("getAttribute/{Id}")]
        [HttpGet]
        public async Task<AttributeBO> GetAttribute([FromUri]IdParams parameters)
        {
            if (parameters == null)
                throw new ArgumentNullException("parameters");

            ValidateParameters(parameters);

            return await _manager.GetAttribute(parameters.Id);
        }


        [Route("getAttributes")]
        [Route("getAttributes/{Id}")]
        [HttpGet]
        public async Task<List<AttributeBO>> GetAttributes([FromUri]IdParams parameters)
        {
            if (parameters == null)
                throw new ArgumentNullException("parameters");

            ValidateParameters(parameters);

           //return await _manager.GetAttribute(parameters.Id);
            return new List<AttributeBO>();
        }

        [Route("getAttributeTypes")]
        [Route("getAttributeTypes/{Id}")]
        [HttpGet]
        public async Task<List<AtrributeTypeViewModel>> GetAttributeTypes([FromUri]IdAttributeParams parameters)
        {
            if (parameters == null)
                throw new ArgumentNullException("parameters");

            ValidateParameters(parameters);

            var result =  await _manager.GetAttributeTypes(parameters.Id, parameters.ProjectId, parameters.RequirementTypeId);
            return new List<AtrributeTypeViewModel>();
        }

        [Route("getAttrTypesForReq")]
        [Route("getAttrTypesForReq/{projectId}/{reqTypeId}")]
        [HttpGet]
        public async Task<List<AttributeDescriptionBO>> GetAttrTypesForReq([FromUri]RequirementProjectParams parameters)
        {
            if (parameters == null)
                throw new ArgumentNullException("parameters");

            ValidateParameters(parameters);

            var result = await _manager.GetAttributeTypes(parameters.ProjectId, parameters.ReqTypeId);
            return result;
        }



        [Route("getProjectAttributeTypes")]
        [Route("getProjectAttributeTypes/{Id}")]
        [HttpGet]
        public async Task<List<AtrributeTypeViewModel>> GetProjectAttributeTypes([FromUri]IdParams parameters)
        {
            if (parameters == null)
                throw new ArgumentNullException("parameters");

            ValidateParameters(parameters);

            var types = await _manager.GetAttributeTypes(parameters.Id);
            var result = new List<AtrributeTypeViewModel>();
            for (int i = 0; i < types.Count; i++)
            {
                result.Add(new AtrributeTypeViewModel()
                {
                    Id = types[i].Id,
                    Name = types[i].Name,
                    Default = types[i].DefaultValue,
                    Values = types[i].Values
                });
            }     
            return result;
        }
    }
}
