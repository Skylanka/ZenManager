function RequirementsViewModel(app, dataModel) {
    var self = this;
    self.count = ko.observable();
    self.isLoaded = ko.observable(false);
    self.content = ko.observable();
    self.title = ko.observable();
    self.pages = 0;
    self.title = "Projects";
    self.load = function (id, projectId) {
        app.sendRequest(app.dataModel.getProjects, 'GET',
        {
            Page: 0,
            PageSize:10
        }, function(data) {
            self.content(data);
            self.isLoaded(true);
        });

        app.sendRequest(app.dataModel.getProjectsCount, 'GET',
        {
        }, function (data) {
            self.count(data);
            self.pages = Math.ceil(self.count / 10);
        });

        return self;
    }
}

app.addViewModel({
    name: "Requirements",
    bindingMemberName: "requirements",
    factory: RequirementsViewModel,
    navigatorFactory: function (app) {
        return function (projectId, id) {
            app.view(app.Views.Requirements);
            app.requirements().load(id, projectId);
        }
    }
});
