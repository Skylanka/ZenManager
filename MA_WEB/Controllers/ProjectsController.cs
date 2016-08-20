using System;
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
using Novacode;

namespace MA_WEB.Controllers
{
    [RoutePrefix("api/projects")]
    public class ProjectsController : BaseController
    {
        private readonly ProjectManager _manager = new ProjectManager();

        [Route("saveProject")]
        [HttpPost]
        public async Task<int> SaveProject(ProjectViewModel project)
        {
            if (project == null)
                throw new ArgumentNullException("project");

            var user = await ValidateCurrentUser();
            if (project.Id == 0)
            {
                ProjectBO projectBo = project;
                projectBo.Created = DateTime.Now;
                projectBo.Updated = DateTime.Now;
                projectBo.CreatedBy = user;
                projectBo.Users.Add(user);
                projectBo.Docs = new List<DocumentationBO>();
                projectBo.Invitations = new List<InvitationBO>();
                project = await _manager.Create(projectBo);
                return project.Id;
            }
            else
            {
                await _manager.Update(project);
                return project.Id;
            }
        }

        [Route("getProject")]
        [Route("getProject/{Id}")]
        [HttpGet]
        public async Task<ProjectViewModel> GetProject([FromUri] IdParams parameters)
        {
            if (parameters == null)
                throw new ArgumentNullException("parameters");

            ValidateParameters(parameters);

            var project = await _manager.GetProject(parameters.Id);
            return project;
        }

        [Route("getProjects")]
        [Route("getProjects/{PageSize}/{Page}")]
        [HttpGet]
        public async Task<List<ProjectViewModel>> GetProjects([FromUri] PagingParams parameters)
        {
            if (parameters == null)
                throw new ArgumentNullException("parameters");

            ValidateParameters(parameters);

            var u = User.Identity.Name;

            var user = await ValidateCurrentUser();

            var projects = await _manager.GetProjects(parameters.Page, parameters.PageSize, user);

            var result = new List<ProjectViewModel>();
            for (int i = 0; i < projects.Count; i++)
            {
                result.Add(new ProjectViewModel()
                {
                    Id = projects[i].Id,
                    Name = projects[i].Name,
                    Description = projects[i].Description,
                    Created = projects[i].Created.ToShortDateString(),
                    Author = projects[i].CreatedBy.FirstName + ' ' + projects[i].CreatedBy.LastName,
                    IssueCount = await _manager.GetIssueCount(projects[i].Id),
                    RequirementCount = await _manager.GetRequirementCount(projects[i].Id),
                    Status = Enum.GetName(typeof (ProjectStatus), projects[i].Status)
                });
            }

            return result;
        }


        [Route("getActiveProjects")]
        [HttpGet]
        public async Task<List<ProjectViewModel>> GetActiveProjects()
        {
            var u = User.Identity.Name;

            var user = await ValidateCurrentUser();

            var projects = await _manager.GetProjects(user);

            var result = new List<ProjectViewModel>();
            for (int i = 0; i < projects.Count; i++)
            {
                result.Add(new ProjectViewModel()
                {
                    Id = projects[i].Id,
                    Name = projects[i].Name,
                    Updated = projects[i].Updated.ToShortDateString(),
                    IssueCount = await _manager.GetIssueCount(projects[i].Id),
                    RequirementCount = await _manager.GetRequirementCount(projects[i].Id),
                    Status = Enum.GetName(typeof (ProjectStatus), projects[i].Status)
                });
            }

            return result;
        }


        public List<ProjectBO> GetProjectsForUser(int id)
        {

            return _manager.GetProjects(id);
        }

        [Route("getUsersForProject")]
        [HttpGet]
        public List<UserViewModel> GetUsersForProject(int id)
        {
            return new List<UserViewModel>();
        }


        // [Authorize(Roles = "Admin, CompanyAdmin, Engineer")]
        [Route("getProjetsCount")]
        [HttpGet]
        public async Task<int> GetProjectsCount()
        {
            var user = await ValidateCurrentUser();

            return await _manager.GetCount(user);
        }

        [Route("deleteProject")]
        [HttpPost]
        public async Task DeleteProject(IdParams parameters)
        {
            ValidateParameters(parameters);
            var user = await ValidateCurrentUser();
            await _manager.Delete(parameters.Id);
        }


        //generate documentation
        [Route("generateDoc")]
        [Route("generateDoc/{Id}")]
        [HttpGet]
        public async Task<string> GenerateDoc([FromUri] IdParams parameters)
        {
            if (parameters == null)
                throw new ArgumentNullException("parameters");
            ValidateParameters(parameters);

            ProjectBO project = await _manager.GetProject(parameters.Id);
            String docName = project.Name  + "_Specification_" + DateTime.Now.ToString("hhmm")+ "_" + DateTime.Now.ToString("ddMMyyyy") + ".docx";

            // Title Formatting:
            var titleFormat = new Formatting();
            titleFormat.FontFamily = new System.Drawing.FontFamily("Arial Black");
            titleFormat.Size = 18D;
            titleFormat.Position = 12;

            var title2Format = new Formatting();
            title2Format.FontFamily = new System.Drawing.FontFamily("Arial Black");
            title2Format.Size = 12D;
            title2Format.Bold = true;

            var title3Format = new Formatting();
            title3Format.FontFamily = new System.Drawing.FontFamily("Calibri");
            title3Format.Size = 12D;
            title3Format.Bold = true;

            // Body Formatting
            var paraFormat = new Formatting();
            paraFormat.FontFamily = new System.Drawing.FontFamily("Calibri");
            paraFormat.Size = 10D;
            //paraFormat.Position = 12;

            var bodyFormat = new Formatting();
            bodyFormat.FontFamily = new System.Drawing.FontFamily("Calibri");
            bodyFormat.Size = 12D;

            var currentPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
            string d = System.IO.Path.GetDirectoryName(currentPath)  + "\\Docs\\" + docName;
            var path = d.Replace("file:\\", "");

            using (DocX document = DocX.Create(path))
            {
                document.AddHeaders();
                document.AddFooters();
                // Get the first Header for this document.
                Paragraph f_header = document.Headers.first.InsertParagraph().Append("ZenManager").Bold().FontSize(12);
                f_header.Alignment = Alignment.right;

                for (int i = 0; i < 15; i++)
                    document.InsertParagraph("", false, titleFormat);
                // Insert a Paragraph into the document.
                Paragraph projectName = document.InsertParagraph(project.Name, false, titleFormat);
                projectName.Alignment = Alignment.right;
                Paragraph version = document.InsertParagraph("Version: " + project.Version, false, paraFormat);
                version.Alignment = Alignment.right;


                version.InsertPageBreakAfterSelf();

                // Get the first Footer for this document.
                Paragraph f_footer = document.Footers.first.InsertParagraph().Append("@ZenManager," + DateTime.Now.Year);
                f_footer.Alignment = Alignment.center;

                document.DifferentFirstPage = true;      

                Table h_table = document.Headers.odd.InsertTable(2, 2);
                h_table.Design = TableDesign.TableGrid;
                h_table.AutoFit = AutoFit.Window;
                h_table.Rows[0].Cells[0].Paragraphs[0].InsertText(project.Name);
                h_table.Rows[0].Cells[1].Paragraphs[0].InsertText("Version: " + project.Version);
                h_table.Rows[1].Cells[1].Paragraphs[0].InsertText("Date: " + DateTime.Now.ToShortDateString());
                h_table.MergeCellsInColumn(0, 0, 1);

                var footer = document.Footers.odd.InsertParagraph();
                footer.Alignment = Alignment.right;
                footer.AppendPageNumber(PageNumberFormat.normal);
                //Second page

                document.InsertParagraph("Revision History", false, titleFormat).Alignment = Alignment.center;

                Table revision_table = document.InsertTable(4, 4);
                revision_table.Design = TableDesign.TableGrid;
                revision_table.AutoFit = AutoFit.Window;
                revision_table.Rows[0].Cells[0].Paragraphs[0].InsertText("Date");
                revision_table.Rows[0].Cells[1].Paragraphs[0].InsertText("Version");
                revision_table.Rows[0].Cells[2].Paragraphs[0].InsertText("Description");
                revision_table.Rows[0].Cells[3].Paragraphs[0].InsertText("Author");
                revision_table.InsertPageBreakAfterSelf();

                document.InsertParagraph();

                document.InsertParagraph("Project Description", false, titleFormat).Alignment = Alignment.center;
                document.InsertParagraph(project.Description, false, bodyFormat).Alignment = Alignment.both;

                document.InsertParagraph();
                document.InsertParagraph("Specifications", false, titleFormat).Alignment = Alignment.center;
                //All specifications go here
                List<RequirementsDescriptionBO> specifications =
                    await new RequirementManager().GetRequirementsTypes(project.Id);
                if (specifications.Count > 0)
                {
                    for (int i = 1; i < specifications.Count + 1; i++)
                    {
                        document.InsertParagraph(
                            i + ". [" + specifications[i - 1].Tag + "] " + specifications[i - 1].Name, false,
                            title2Format).Alignment = Alignment.left;
                        document.InsertParagraph(specifications[i - 1].Description, false, bodyFormat).Alignment =
                            Alignment.both;
                        document.InsertParagraph();

                        List<RequirementsByTypeViewModel> reqs =
                            await new RequirementsController().GetRequirementsByType(new IdProjectParams()
                            {
                                Id = specifications[i - 1].Id,
                                ProjectId = project.Id
                            });
                        if (reqs.Count > 0)
                        {
                            for (int j = 1; j < reqs.Count + 1; j++)
                            {
                                document.InsertParagraph(
                                    /*"\t" +*/ i + "." + j + " " + reqs[j - 1].UniqueTag + ": " + reqs[j - 1].Name, false,
                                    title3Format).Alignment = Alignment.left;
                                document.InsertParagraph(/*"\t" +*/ reqs[j - 1].Description, false, bodyFormat).Alignment =
                                    Alignment.left;
                                document.InsertParagraph();
                            }

                            if (reqs[0].Attributes.Count > 0)
                            {
                                document.InsertParagraph("Attributes:", false, title3Format).Alignment = Alignment.left;
                                //document.InsertParagraph();
                                Table attrTable = document.InsertTable(reqs.Count + 1, reqs[0].Attributes.Count + 1);
                                attrTable.Design = TableDesign.LightListAccent3;
                                attrTable.AutoFit = AutoFit.Window;
                                attrTable.Alignment = Alignment.center;
                                for (int j = 0; j < reqs.Count + 1; j++)
                                {
                                    for (int k = 0; k < reqs[0].Attributes.Count + 1; k++)
                                    {
                                        if (j == 0)
                                        {
                                            if (k == 0)
                                                attrTable.Rows[0].Cells[k].Paragraphs[0].InsertText("Name");
                                            else
                                                attrTable.Rows[0].Cells[k].Paragraphs[0].InsertText(
                                                    reqs[j].Attributes.ElementAt(k - 1).Name);
                                        }
                                        else
                                        {
                                            if(k==0)
                                            attrTable.Rows[j].Cells[k].Paragraphs[0].InsertText(reqs[j - 1].UniqueTag + ": " + reqs[j-1].Name);
                                            else
                                                attrTable.Rows[j].Cells[k].Paragraphs[0].InsertText(
                                               reqs[j - 1].Attributes.ElementAt(k - 1).Value);
                                        }
                                    }
                                }
                            }
                            document.InsertParagraph();
                        }
                    }


                    // Save to the output directory:
                    document.Save();
                }

            }
            DocumentationBO doc = new DocumentationBO();
            doc.Name = docName;
            doc.Description = "";
            await _manager.AddDoc(doc, project);
            return docName;
        }

        [Route("sendInvitation")]
        [Route("sendInvitation/{Email}")]
        [HttpPost]
        public async void SendInvitation(InvitationViewModel invtVM)
        {
            if (invtVM == null)
                throw new ArgumentNullException("invtVM");

            var user = await ValidateCurrentUser(invtVM.Email);
            var project = await _manager.GetProject(invtVM.ProjectId);
            InvitationBO invt = new InvitationBO()
            {
                Email = invtVM.Email,
                Status = false,
                Code = new Guid()
            };
            if (user != null && project!= null)
                await _manager.AddInvitation(invt, user, project);
            else
                await _manager.AddInvitation(invt);//TODO: Sending invitation code via mail
        }

        [Route("acceptInvitation")]
        [HttpPost]
        public async Task AcceptInvitation(InvitationViewModel invt)
        {
            if (invt == null)
                throw new ArgumentNullException("invt");
            ProjectBO project = _manager.GetProjectForInvitation(invt.Id);
            var user = await ValidateCurrentUser();
            await _manager.UpdateInvitation(invt, user, project);
        }
    }
}
