function IssueViewModel(app, dataModel) {
    var self = this;
    self.content = ko.observable();
    self.projectId = ko.observable();
    self.id = ko.observable();
    self.isLoaded = ko.observable(false);
    self.projectTitle = ko.observable();
    self.description = ko.observable();
    self.reqDesc = ko.observable();
    self.noLinks = ko.observable(false);
    self.reqLinks = ko.observableArray();
    self.comments = ko.observableArray();
    self.editableLinks = ko.observable(true);
    self.specifications = ko.observableArray();
    self.requirements = ko.observableArray();
    self.editable = ko.observable(false);
    var issueTemplateData = {};
    var linksTemplateData = {};

    self.load = function (id, projectId) {
        self.id(id);
        self.projectId(projectId);
        app.sendRequest(app.dataModel.getIssue, 'GET',
       {
           Id:id
       }, function (data) {
           self.description(data.name);
           self.content(data);
            self.getSpecifications();
            self.startModal();
           var str = self.content().description.replace(/(?:\r\n|\r|\n)/g, '<br />');
           self.reqDesc(str);
           app.sendRequest(app.dataModel.getProject, 'GET',
               {
                   Id: projectId
               }, function (data) {
                   self.projectTitle(data.name);
                   //self.isLoaded(true);
                   self.getComments();
               self.getLinks();

           });
        });
    }

    self.startComments = function () {
        $('#comment').keypress(function (e) {
            if (e.which === 13) {
                var message = $('#comment').val();
                app.sendRequest(app.dataModel.saveComment, 'POST', { Text: message, Id: 0, IssueId: self.id() }, function (data) {
                    if (data != null) {
                        location.reload();
                    }
                });
                $('#comment').val('').focus();
            }
        });
    };

    self.getComments = function () {
        app.sendRequest(app.dataModel.getIssueComments, 'GET',
                {
                    Id: self.id()
                }, function (data) {
                    self.comments(data);
                });
    };


    self.getLinks = function() {
        app.sendRequest(app.dataModel.getIssueLinks, 'GET',
                {
                    Id: self.id()
                }, function (data) {
                    self.reqLinks(data);
                });
    };

    self.startModal = function () {

        issueTemplateData = {
            name: self.content().name,
            description: self.content().description,
            /*opt: self.participants,*/
            selected: ko.observable(),
            selectedStatus: self.content().status,
            selectedPriority: self.content().priority,
            status: ["Open", "InProgress", "ClosedNoReproduce", "FixedResolved"],
            priority: ["Low", "Medium", "High"]
        };
        self.modalVisible = ko.observable(false);
        self.modalSize = ko.observable('modal-lg');
        self.headerLabel = ko.observable('Some header text');
        self.bodyTemplate = ko.observable('editIssueTemplate');
        self.bodyData = ko.computed(function() {
            return issueTemplateData;
        });

        self.addIssueDialog = function() {
            self.modalVisible(true);
        };
    };

 self.startLinksModal = function () {
        self.modalLinksVisible = ko.observable(false);
        self.modalLinksSize = ko.observable('modal-lg');
        self.headerLinksLabel = ko.observable('Add Requirement Link');
        self.bodyLinksTemplate = ko.observable('editLinkTemplate');
        linksTemplateData = {
            specification: self.specifications(),
            selectedSpecification: ko.observable(self.specifications()[0]),
            selectedRequirement: ko.observable()
        };
        self.bodyLinksData = ko.computed(function () {
            return linksTemplateData;
        });
        self.isLoaded(true);
        self.startComments();
    };


    self.getSpecifications = function () {
        app.sendRequest(app.dataModel.getRequirementsTypesWithRequirements, 'GET',
        {
            Id: self.projectId()
        }, function (data) {
            self.specifications(data);
            self.startLinksModal();
        });
    };

    self.addLink = function () {
        self.editableLinks(false);
        if (!linksTemplateData.selectedRequirement()) {
            $("#requirement").css("border color", "red");
        } else {
            app.sendRequest(app.dataModel.addIssueRequirementLink, 'POST', {
                IssueId: self.id(),
                RequirementId: linksTemplateData.selectedRequirement().id
            }, function (data) {
                /*var url = '#/project/' + self.projectId();
            Finch.navigate(url);*/
                location.reload();
            });
        }

    };

    self.addLinkDialog = function () {
        self.modalLinksVisible(true);
    };
    

    self.saveIssue = function () {
        //var priority = $('#priority').find(":selected").text();
        app.sendRequest(app.dataModel.saveIssue, 'POST', {
            Name: issueTemplateData.name,
            Id: self.id(),
            Description: issueTemplateData.description,
            ProjectId: self.projectId(),
            AssignedToId: 1,
            Status: issueTemplateData.selectedStatus,
            Priority: issueTemplateData.selectedPriority
        }, function (data) {
            location.reload();
            /*self.modalVisible(false);*/
            /*issueTemplateData.name = "";
            issueTemplateData.description = "";
            $('.close').click();*/
        });
    };
    self.deleteLinkDialog = function (data) {
        bootbox.dialog({
            message: "Are you sure you want to delete this link?",
            title: "Delete Requirement link",
            buttons: {
                success: {
                    label: "Yes",
                    className: "btn-success",
                    callback: function () {
                        self.deleteLink(data);
                    }
                },
                danger: {
                    label: "No",
                    className: "btn-danger"
                }
            }
        });
    };

    self.deleteLink = function (data) {
        app.sendRequest(app.dataModel.deleteRequirementLink, 'POST', {
            Id: data.linkId
        }, function () {
            $('table#links tr#' + data.linkId).remove();
        });
    }
    return self;
}

app.addViewModel({
    name: "Issue",
    bindingMemberName: "issue",
    factory: IssueViewModel,
    navigatorFactory: function (app) {
        return function (projectId, id) {
            app.view(app.Views.Issue);
            app.issue().load(id, projectId);
        }
    }
});
