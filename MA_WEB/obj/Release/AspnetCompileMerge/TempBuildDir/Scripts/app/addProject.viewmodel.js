function AddProjectViewModel(app, dataModel) {
    var self = this;
    self.isLoaded = ko.observable(false);
    self.isNew = ko.observable(false);
    self.name = ko.observable().extend({ required: true });
    self.id = ko.observable();
    self.title = ko.observable();
    self.description = ko.observable();
    self.content = ko.observable();
    self.status = ko.observable("Active");
    self.version = ko.observable();
    self.statusList = ["Active", "Finished", "Archived"];
    var invitationTemplateData = {};

    self.deleteDialog = function () {
        bootbox.dialog({
            message: "Are you sure you want to delete this project?",
            title: "Delete Project",
            buttons: {
                success: {
                    label: "Yes",
                    className: "btn-success",
                    callback: function () {
                        self.deleteProject();
                    }
                },
                danger: {
                    label: "No",
                    className: "btn-danger"
                }
            }
        });
    };
    self.deleteProject = function () {
        if (app.isValid(self)) {

            app.sendRequest(app.dataModel.deleteProject, 'POST', { Id: self.id() }, function (data) {
                    var url = '#/projects';
                    Finch.navigate(url);
            });
        }
    };

    self.save = function () {
        if (app.isValid(self)) {

            app.sendRequest(app.dataModel.saveProject, 'POST', { Name: self.name(), Id: self.id(), Description: self.description(), Status: self.status(), Version: self.version() }, function (data) {
                if (data != null) {
                    var url = '#/project/' + data;
                    Finch.navigate(url);
                }
            });
        }
    };

    self.load = function (id) {
        self.id(id);
        self.startModal();
        if (id != null) {
            self.title("Edit Project");

            app.sendRequest(app.dataModel.getProject, 'GET', { id: id }, function (data) {
                self.name(data.name);
                self.description(data.description);
                self.content(data);
                self.version(data.version);
                self.status(data.status);
                self.isLoaded(true);
            });
        } else {
            self.title("Create Project");
            self.isNew(true);
            self.isLoaded(true);
        }
    };

    self.startModal = function () {

        invitationTemplateData = {
            email: ko.observable()
        };
        self.modalVisible = ko.observable(false);
        self.modalSize = ko.observable('modal-lg');
        self.headerLabel = ko.observable('Some header text');
        self.bodyTemplate = ko.observable('invitationTemplate');
        self.bodyData = ko.computed(function () {
            return invitationTemplateData;
        });

        self.invitationDialog = function () {
            self.modalVisible(true);
        };
    };

    self.sendInvitation = function() {
        if (app.isValid(self)) {

            app.sendRequest(app.dataModel.sendInvitation, 'POST', {
                Email: invitationTemplateData.email,
                ProjectId: self.id()
            }, function () {
                self.modalVisible(false);
                invitationTemplateData.email = "";
            $('.close').click();
          });
        }
    };
    return self;


}

app.addViewModel({
    name: "AddProject",
    bindingMemberName: "addProject",
    factory: AddProjectViewModel,
    navigatorFactory: function (app) {
        return function (id) {
            app.view(app.Views.AddProject);
            app.addProject().load(id);
        }
    }
});
