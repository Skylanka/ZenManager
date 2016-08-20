function HomeViewModel(app, dataModel) {
    var self = this;
    self.isLoaded = ko.observable(false);
    self.activeProjects = ko.observableArray();
    self.latestIssues = ko.observableArray();
    self.latestRequirements = ko.observableArray();
    self.noProjects = ko.observable(false);
    self.noIssues = ko.observable(false);
    self.noRequirements = ko.observable(false);

   self.myHometown = ko.observable("");
   self.load = function() {
       app.sendRequest(app.dataModel.getActiveProjects, 'GET',
       {
       }, function (data) {
           if (data != null)
               self.activeProjects(data);
           else self.noProjects(true);
       });
       app.sendRequest(app.dataModel.getLatestIssues, 'GET',
       {
       }, function (data) {
           if (data != null)
               self.latestIssues(data);
           else self.noIssues(true);
       });
       app.sendRequest(app.dataModel.getLatestRequirements, 'GET',
       {
       }, function (data) {
           if (data != null)
               self.latestRequirements(data);
           else self.noRequirements = ko.observable(true);
           self.isLoaded(true);
       });
       
   }

    self.projects = ko.observable("");
    return self;
}

app.addViewModel({
    name: "Home",
    bindingMemberName: "home",
    factory: HomeViewModel,
    navigatorFactory: function (app) {
        return function () {
            app.view(app.Views.Home);
            app.home().load();
        }
    }
});
