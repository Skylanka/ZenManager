function AddRequirementViewModel(app, dataModel) {
    var self = this;
    self.isLoaded = ko.observable(false);
    self.name = ko.observable().extend({ required: true });
    self.id = ko.observable();
    self.title = ko.observable();
    self.description = ko.observable();
    self.pageDescription = ko.observable();
    self.projectId = ko.observable();
    self.projectName = ko.observable();
    self.requirementsTypes = ko.observableArray();
    self.selectedType = ko.observable();
    self.uniqueTag = ko.observable();
    self.requirement = ko.observable();

    self.deleteReq = function () {
        bootbox.dialog({
            message: "Are you sure you want to delete this requirement?",
            title: "Delete Requirement",
            buttons: {
                success: {
                    label: "Yes",
                    className: "btn-success",
                    callback: function () {
                        self.deleteRequirement();
                    }
                },
                danger: {
                    label: "No",
                    className: "btn-danger"
                }
            }
        });
    };

    self.deleteRequirement = function() {
        app.sendRequest(app.dataModel.deleteRequirement, 'POST', {
            Id: self.id()
        }, function(data) {
            var url = '#/project/' + self.projectId();
            Finch.navigate(url);
        });
    };

    self.save = function () {
        if (app.isValid(self)) {
            if (!self.id()) {
                self.tag = self.selectedType().tag;
                var uniqueTag = self.selectedType().tag + (self.selectedType().count + 1);
                app.sendRequest(app.dataModel.saveRequirement, 'POST', { Name: self.name(), Id: self.id(), Description: self.description(), ProjectId: self.projectId(), RequirementType: self.selectedType().id, UniqueTag: uniqueTag }, function(data) {
                    if (data != null) {
                        var url = '#/project/' + self.projectId() + '/requirement/' + data;
                        Finch.navigate(url);
                    }
                });
            } else {
                app.sendRequest(app.dataModel.saveRequirement, 'POST', { Name: self.name(), Id: self.id(), Description: self.description(), ProjectId: self.projectId(), RequirementType: self.selectedType().id, UniqueTag: self.uniqueTag() }, function (data) {
                    if (data != null) {
                        var url = '#/project/' + self.projectId() + '/requirement/' + data;
                        Finch.navigate(url);
                    }
                });
            }
        }
    };

    self.load = function (id, projectId) {
        self.id(id);
        self.projectId(projectId);

        if (projectId != null) {
            app.sendRequest(app.dataModel.getProject, 'GET', { Id: projectId }, function (data) {
                if (data != null) {
                    self.projectName(data.name);
                    if (id != null) {
                        self.title("Edit Requirement");

                        app.sendRequest(app.dataModel.getRequirement, 'GET', { Id: id }, function (data) {
                            self.name(data.name);
                            self.description(data.description);
                            self.pageDescription(self.projectName() + "<small>" + self.title() + "</small>");
                            self.uniqueTag(data.uniqueTag);
                            self.getTypes();
                            self.requirement(data);

                        });
                    } else {
                        self.title("Add Requirement");
                        self.pageDescription(self.projectName() + "<small>" + self.title() + "</small>");
                        self.getTypes();
                    }
                }
            });

        }

    };

    self.getTypes = function() {
        app.sendRequest(app.dataModel.getRequirementsTypes, 'GET', { Id: self.projectId() }, function (data) {
            if (data != null) {
                self.requirementsTypes(data);
                if (self.id()) {
                    for (var i = 0; i < data.length; i++) {
                    if (data[i].id === self.requirement().typeDescription.id) {
                        self.selectedType(data[i]);
                    }
                }
                }
                
                self.isLoaded(true);
            }
        });
    };


    return self;


}

app.addViewModel({
    name: "AddRequirement",
    bindingMemberName: "addRequirement",
    factory: AddRequirementViewModel,
    navigatorFactory: function (app) {
        return function (projectId, id) {
            app.view(app.Views.AddRequirement);
            app.addRequirement().load(id, projectId);
        }
    }
});
