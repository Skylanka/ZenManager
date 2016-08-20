function RequirementsByTypeViewModel(app, dataModel) {
    var self = this;

    self.isLoaded = ko.observable(false);
    self.noRequirements = ko.observable(false);
    self.content = ko.observable();
    self.description = ko.observable();
    self.projectTitle = ko.observable();
    self.pages = 0;
    self.id = ko.observable();
    self.projectId = ko.observable();
    self.atrrTypes = ko.observableArray();

    self.rows = ko.observableArray();
    self.title = ko.observable();
    self.editable = ko.observable(false);

    self.load = function(id, projectId) {
        self.id(id);
        self.projectId(projectId);
        if (id != null) {
            app.sendRequest(app.dataModel.getRequirementsByType, 'GET',
            {
                Id: id,
                ProjectId: projectId
            }, function(data) {
                self.rows(data);
                if (data.length === 0) self.noRequirements(true);
                self.atrrTypes(data[0].attributes);
                app.sendRequest(app.dataModel.getRequirementType, 'GET',
                {
                    Id: id
                }, function(data) {
                    self.content(data);
                    self.description(data.name);
                    self.title(data.name + "<small> Requirements type</small>");
                    self.isLoaded(true);
                    $(document).ready(function () {
                        $('.dropdown-toggle').dropdown();
                    });
                });
            });
        }

        app.sendRequest(app.dataModel.getProject, 'GET', { Id: projectId }, function (data) {
            if (data != null) {
                self.projectTitle(data.name);
            }
        });
    };

    self.edit = function () {

        $('input').each(function () {
            if ($(this).attr('disabled')) {
                $(this).removeAttr('disabled');
                self.editable(true);
            }
        });
        $('select').each(function () {
            if ($(this).attr('disabled')) {
                $(this).removeAttr('disabled');
                self.editable(true);
            }
        });
    };

    self.save = function() {
        $('input').each(function () {
                $(this).attr({
                    'disabled': 'disabled'
                });
                self.editable(false);
        });
        $('select').each(function () {
            $(this).attr({
                'disabled': 'disabled'
            });
            self.editable(false);
        });
    };

    return self;
}

app.addViewModel({
    name: "RequirementsByType",
    bindingMemberName: "requirementsByType",
    factory: RequirementsByTypeViewModel,
    navigatorFactory: function (app) {
        return function (projectId, id) {
            app.view(app.Views.RequirementsByType);
            app.requirementsByType().load(id, projectId);
        }
    }
});
