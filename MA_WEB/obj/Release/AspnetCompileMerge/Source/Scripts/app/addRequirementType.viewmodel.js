function AddRequirementTypeViewModel(app, dataModel) {
    var self = this;
    self.isLoaded = ko.observable(false);
    self.name = ko.observable().extend({ required: true });
    self.id = ko.observable();
    self.title = ko.observable();
    self.pageDescription = ko.observable();
    self.description = ko.observable();
    self.projectId = ko.observable();
    self.projectName = ko.observable(); 
    self.tag = ko.observable();
    self.selectedType = ko.observable();
    self.uniqueTag = ko.observable();
    self.attributeTypes = ko.observableArray();
    self.chosenAttributes = ko.observableArray();
    self.selectedAttributes = ko.observableArray();
    var attributeTemplateData = {};

    /*$('select').select2({
  data: [
    {
      id: 'value',
      text: 'Text to display'
    },
    // ... more data objects ...
  ]
});*/

    var select2data = [];

    self.deleteRTdialog = function () {
        bootbox.dialog({
            message: "Are you sure you want to delete this requirement type?",
            title: "Delete Requirement Type",
            buttons: {
                success: {
                    label: "Yes",
                    className: "btn-success",
                    callback: function () {
                        self.deleteRT();
                    }
                },
                danger: {
                    label: "No",
                    className: "btn-danger"
                }
            }
        });
    };

    self.deleteRT = function() {
        if (app.isValid(self)) {

            app.sendRequest(app.dataModel.deleteRequirementType, 'POST', { Id: self.id() }, function (data) {
                if (data != null) {
                    var url = '#/project/' + self.projectId();
                    Finch.navigate(url);
                }
            });
        }
    };

    self.save = function () {
        if (app.isValid(self)) {
            self.selectedAttributes($(".select2").select2("val"));

            app.sendRequest(app.dataModel.saveRequirementType, 'POST', { Name: self.name(), Id: self.id(), Description: self.description(), ProjectId: self.projectId(), Tag: self.tag(), Attributes: self.selectedAttributes() }, function (data) {
                if (data != null) {
                    var url = '#/project/'+ self.projectId() + '/requirements/type/' + data;
                    //var url = '#/project/' + self.projectId();
                    Finch.navigate(url);
                }
            });
       }
    };

    self.load = function (id, projectId) {
        self.id(id);
        self.projectId(projectId);
        if (projectId != null) {
            app.sendRequest(app.dataModel.getProject, 'GET', { Id: projectId }, function (data) {
                if (data != null) {
                    self.projectName(data.name);
                    app.sendRequest(app.dataModel.getProjectAttributeTypes, 'GET', { Id: projectId }, function (data) {
                        if (data != null) {
                            self.attributeTypes(data);
                            for (var i = 0; i < data.length; i++) {
                                select2data.push({id: data[i].id, text: data[i].name});
                            }
                            
                            if (id != null) {
                                self.title(data.name + "<small>Edit Requirement Type</small>");
                                self.pageDescription("Edit Requirement Type");
                                app.sendRequest(app.dataModel.getRequirementType, 'GET', { Id: id }, function (data) {
                                    if (data != null) {
                                        self.name(data.name);
                                        self.description(data.description);
                                        self.tag(data.tag);
                                        self.startModal();
                                        //self.isLoaded(true);
                                        self.selectedAttributes(data.attributes);
                                        $(".select2").select2();
                                        $(".select2").select2({ data: select2data});
                                        $(".select2").select2("val", data.attributes);
                                        
                                    }
                                });
                            } else {
                                self.title(self.projectName() + "<small>Add Requirement Type</small>");
                                self.pageDescription("Add Requirement Type");
                                self.startModal();

                                $(".select2").select2();
                                $(".select2").select2({ data: select2data });
                            }
                        }
                    });
                    
                    
                }
                
            });
        }
    };

    self.onOptionChanged = function() {
        $("select").change(function () {
            //self.tag(self.selectedType().tag);
            //self.uniqueTag(self.tag() + (self.selectedType().count + 1));
        });
    }

    self.startModal = function () {
        attributeTemplateData = {
            name: ko.observable(),
            defaultValue: ko.observable(),
            selectedType: ko.observable(),
            value: ko.observable(),
            type: ["Text", "List"],
            isList: ko.computed( function() {
                if (attributeTemplateData.selectedType && attributeTemplateData.selectedType === "List") return true;
                return false;
            })
        };
        self.modalVisible = ko.observable(false);
        self.modalSize = ko.observable('modal-lg');
        self.headerLabel = ko.observable('Add Attribute');
        self.bodyTemplate = ko.observable('addAttributeTemplate');
        self.bodyData = ko.computed(function () {
            return attributeTemplateData;
        });

        self.isLoaded(true);
        
    };

    self.addAttributeDialog = function () {
        self.modalVisible(true);
    };

    self.addAttribute = function () {
        var value;
        if (attributeTemplateData.selectedType() === "List") {
            value = (attributeTemplateData.value()).split(",");
        } else value = null;
        app.sendRequest(app.dataModel.saveAttributeType, 'POST', {
            Name: attributeTemplateData.name(),
            Id: 0,
            ProjectId: self.projectId(),
            Default: attributeTemplateData.defaultValue(),
            Type: attributeTemplateData.selectedType(),
            Values: value
    }, function (data) {
            if (data != null) {
                select2data.push({ id: data, text: attributeTemplateData.name() });
                $(".select2").select2({ data: select2data });
                attributeTemplateData.defaultValue("");
                attributeTemplateData.name("");
                attributeTemplateData.value("");
                self.modalVisible(false);
                $('.close').click();
            }
        });
    };
    return self;


}

app.addViewModel({
    name: "AddRequirementType",
    bindingMemberName: "addRequirementType",
    factory: AddRequirementTypeViewModel,
    navigatorFactory: function (app) {
        return function (projectId, id) {
            app.view(app.Views.AddRequirementType);
            app.addRequirementType().load(id, projectId);
        }
    }
});
