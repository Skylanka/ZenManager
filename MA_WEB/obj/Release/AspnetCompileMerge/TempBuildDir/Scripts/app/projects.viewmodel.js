function ProjectsViewModel(app, dataModel) {
    var self = this;

    self.title = "Projects";

       self.gridViewModel = new ko.simpleGrid.viewModel({
        dataUrl: app.dataModel.getProjects,
        dataCountUrl: app.dataModel.getProjectsCount,
        columns: [
        {
            headerText: "Status",
            rowText: function (item) {
                if(item.status === "Active")
                    return "<span class=\"label bg-olive\">Active</span>";
                else if (item.status === "Finished")
                    return "<span class=\"label label-warning\">Finished</span>";
                else if(item.status === "Archived")
                    return "<span class=\"label bg-yellow\">Archived</span>";
                return "<span class=\"label bg-olive\">Active</span>";
            } 
        },
            {
                headerText: "Name",
                rowText: function (item) {
                    return "<a href=\"#/project/" + item.id + " \">" + item.name + "</a>" + "<br/><small>Created: " + item.created + "</small>";
                }
            },
            {
                headerText: "Issue count",
                rowText: function (item) {
                    return "<span>"+ item.issueCount +" </span>" +
                        "<small>Issue(s)</small>";
                }
            },
            {
                headerText: "Requirement count",
                rowText: function (item) {
                    return "<span>" + item.requirementCount + " </span>" +
                        "<small>Requirement(s)</small>";
                }
            },
            {
                headerText: "Created by", rowText: "author"
            }
        ],
        pageSize: 5
    });  
    
    return self;
}

app.addViewModel({
    name: "Projects",
    bindingMemberName: "projects",
    factory: ProjectsViewModel,
    navigatorFactory: function (app) {
        return function () {
            app.view(app.Views.Projects);
            //app.projects().load();
        }
    }
});
