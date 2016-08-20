function RequirementViewModel(app, dataModel) {
    var self = this;
    self.content = ko.observable();
    self.isLoaded = ko.observable(false);
    self.projectTitle = ko.observable();
    self.description = ko.observable();
    self.id = ko.observable();
    self.projectId = ko.observable();
    self.noAttributes = ko.observable(false);
    self.comments = ko.observableArray();
    self.atrrTypes = ko.observableArray();
    self.rows = ko.observableArray();
    self.editable = ko.observable(false);
    self.reqDesc = ko.observable();
    self.reqLinks = ko.observableArray();
    self.noLinks = ko.observable(false);
    self.editableLinks = ko.observable(true);
    self.specifications = ko.observableArray();
    self.requirements = ko.observableArray();
    self.contentOld = ko.observableArray();
    
    self.load = function (id, projectId) {
        self.id(id);
        self.projectId(projectId);

        if (id != null) {
            app.sendRequest(app.dataModel.getRequirementWithAttr, 'GET', { Id: id },
                function (data) {
                self.description(data.uniqueTag);
                self.content(data);
                self.contentOld(self.content().attributes);
                self.getSpecifications();
                var str = self.content().description.replace(/(?:\r\n|\r|\n)/g, '<br />');
                self.reqDesc(str);
                if (data.attributes.length === 0) {
                    self.noAttributes(true);
                } else {
                    self.rows(data);
                    self.noAttributes(false);
                    self.atrrTypes(data.attributes);
                }
                app.sendRequest(app.dataModel.getProject, 'GET',
                {
                    Id: projectId
                }, function (data) {
                    self.projectTitle(data.name);
                    
                    self.loadReqLinks();
                    self.startComments();

                });
            });
        }
    };

    self.loadReqLinks = function() {
        app.sendRequest(app.dataModel.getReqLinks, 'GET',
               {
                   Id: self.id()
               }, function (data) {
            if (data.length === 0) self.noLinks(true);
                   self.reqLinks(data);
               });
    };


    self.startComments = function () {
        self.getComments();
        $('#comment').keypress(function (e) {
            if (e.which == 13) {
                var message = $('#comment').val();
                app.sendRequest(app.dataModel.saveComment, 'POST', { Text: message, Id: 0, ReqId: self.id()}, function (data) {
                    if (data != null) {
                        /*var url = '#/project/' + data;
                        Finch.navigate(url);*/
                    }
                });
                $('#comment').val('').focus();
            }
        });


    };

    self.getComments = function() {
        app.sendRequest(app.dataModel.getReqComments, 'GET',
                {
                    Id: self.id()
                }, function (data) {
            self.comments(data);

        });
    };


    self.edit = function () {

        $('#attributes input').each(function () {
            if ($(this).attr('disabled')) {
                $(this).removeAttr('disabled');
                self.editable(true);
            }
        });
        $('#attributes select').each(function () {
            if ($(this).attr('disabled')) {
                $(this).removeAttr('disabled');
                self.editable(true);
            }
        });
    };

    /*self.editLinks = function () {
        if (self.editableLinks()) self.editableLinks(false);
        else self.editableLinks(true);
    };*/

    self.save = function () {
        $('#attributes input').each(function () {
            $(this).attr({
                'disabled': 'disabled'
            });
            self.editable(false);
        });
        $('#attributes select').each(function () {
            $(this).attr({
                'disabled': 'disabled'
            });
            self.editable(false);
        });

        if (self.contentOld() !== null) {

            for (var i = 0; i < self.contentOld().length; i++) {
                    //if (self.contentOld()[i].value !== self.atrrTypes()[i].value) {
                        app.sendRequest(app.dataModel.updateReqAttributes, 'POST', {
                            Id: self.atrrTypes()[i].id,
                            Value: self.atrrTypes()[i].value
                        }, function (data) {
                            /*var url = '#/project/' + self.projectId();
                            Finch.navigate(url);*/
                        });
                   // }
                
            }
        }
    };
    var linksTemplateData = {
    };
    self.modalVisible = ko.observable(false);
    self.modalSize = ko.observable('modal-lg');
    self.headerLabel = ko.observable('Some header text');
    self.bodyTemplate = ko.observable('editLinkTemplate');
    self.startModal = function () {
        linksTemplateData = {
            specification: self.specifications(),
            selectedSpecification: ko.observable(self.specifications()[0]),
            selectedRequirement: ko.observable(),
            type: ["Parent", "Child"],
            selectedType: ko.observable()
        };
        self.bodyData = ko.computed(function () {
            return linksTemplateData;
        });
        self.isLoaded(true);
    };


    self.getSpecifications = function() {
        app.sendRequest(app.dataModel.getRequirementsTypesWithRequirements, 'GET',
        {
            Id: self.projectId()
        }, function (data) {
            self.specifications(data);
            self.startModal();
        });
    };

    self.addLink = function () {

        if (!linksTemplateData.selectedRequirement()) {
            $("#requirement").css("border color", "red");
        } else {
            app.sendRequest(app.dataModel.addRequirementLink, 'POST', {
                ReqId: self.id(),
                SpecId: linksTemplateData.selectedRequirement().id,
                Type: linksTemplateData.selectedType
            }, function(data) {
                /*var url = '#/project/' + self.projectId();
            Finch.navigate(url);*/
                location.reload();
            });
        }

    };


    self.getRequirements = function () {
           /* app.sendRequest(app.dataModel.getRequirementsByType, 'GET',
            {
                Id: linksTemplateData.selectedSpecification.type,
                ProjectId: self.projectId()
            }, function(data) {
                self.requirements(data);
                //if (data.length === 0) self.noRequirements(true);
                return data;
            });*/

    };

    self.addLinkDialog = function () {
            self.modalVisible(true);

    };

    self.deleteLinkDialog = function(data) {
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
    name: "Requirement",
    bindingMemberName: "requirement",
    factory: RequirementViewModel,
    navigatorFactory: function (app) {
        return function (projectId, id) {
            app.view(app.Views.Requirement);
            app.requirement().load(id, projectId);
        }
    }
});
