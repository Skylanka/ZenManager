function ProjectViewModel(app, dataModel) {
    var self = this;
    self.content = ko.observable();
    self.isLoaded = ko.observable(false);
    self.description = ko.observable();
    self.id = ko.observable();
    self.widgetRequirements = ko.observableArray();
    self.participantsNames = ko.observableArray();
    self.participants = ko.observableArray();
    self.issuesLoaded = ko.observable(false);
    self.user = ko.observable();
    self.selectedFrom = ko.observable();
    self.selectedTo = ko.observable();
    self.traceability = ko.observableArray();
    self.noLinks = ko.observable(true);
    self.issues = ko.observableArray();

    self.load = function(id) {
        self.id(id);
        app.sendRequest(app.dataModel.getProject, 'GET',
        {
            Id: id
        }, function (data) {
            self.content(data);
            self.description(data.name + '<small>Details</small>');
            self.participants(data.participants);
            var res = [];
            for (var i = 0; i < self.participants().length; i++) {
                res[i] = self.participants()[i].name;
            }
            self.participantsNames(res);
            self.startTaceability();
            
            app.sendRequest(app.dataModel.userInfoUrl, 'GET',
               {

               }, function (data) {
                    self.user(data);
               });
            app.sendRequest(app.dataModel.getIssues, 'GET',
               {
                   Id: self.id()
               }, function (data) {
                   self.issues(data);
                   self.isLoaded(true);
                   $(document).ready(function () {
                       $('.dropdown-toggle').dropdown();
                       self.startChatting();
                       $('#issueTable').DataTable({
                           "paging": true,
                           "lengthChange": false,
                           "searching": false,
                           "ordering": true,
                           "info": true,
                           "autoWidth": true
                       });
                   });
               });
        });

        self.getWidget();
    }

    self.getWidget = function() {
        app.sendRequest(app.dataModel.getWidgetRequirements, 'GET',
        {
            Id: self.id()
        }, function(data) {
            self.widgetRequirements(data);
            $('.dropdown-toggle').dropdown();

        });
    };

    self.btnDeleteProject = function() {
        bootbox.dialog({
            message: "Are you sure you want to delete this project?",
            title: "Delete Project",
            buttons: {
                success: {
                    label: "Yes",
                    className: "btn-success",
                    callback: function() {
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

    var issueTemplateData = {
        name: ko.observable(''),
        description: ko.observable(''),
        opt: self.participants,
        selected: ko.observable()
    };

    self.modalVisible = ko.observable(false);
    self.modalSize = ko.observable('modal-lg');
    self.headerLabel = ko.observable('Some header text');
    self.bodyTemplate = ko.observable('issueTemplate');
    self.bodyData = ko.computed(function () {
        return issueTemplateData;
    });

    self.addIssueDialog = function() {
        self.modalVisible(true);
    };
    self.saveIssue = function () {
        var priority = $('#priority').find(":selected").text();
        app.sendRequest(app.dataModel.saveIssue, 'POST', { Name: issueTemplateData.name, Id: 0, Description: issueTemplateData.description, ProjectId: self.id(), AssignedToId: issueTemplateData.selected().id, Status: "Open", Priority: priority }, function (data) {
            self.modalVisible(false);
            issueTemplateData.name = "";
            issueTemplateData.description = "";
            $('.close').click();
        });
    };

    self.deleteProject = function () {
        app.sendRequest(app.dataModel.projectDeleteUrl, 'POST', {
            Id: self.id()
        }, function (data) {
            var url = '#/projects';
            Finch.navigate(url);
        });
    }

    self.lastScrollHeightValue = null;
    self.page = 0;
    self.endOfChat = false;

    self.chatRequest = function () {

        app.sendRequest(app.dataModel.chatUrl, 'GET', {
            Id: self.id(),
            Page: self.page,
            PageSize: 20
        }, function (data) {
            var length = data.length;
            var discussion = $('#discussion');
            if (length > 0) {
                var message = "";
                //self.deleteLoading();
                for (var i = 0; i < length; i++) {
                    if (data[i].userName === self.user().userName) {
                        message += '<div class="direct-chat-msg right"><div class="direct-chat-info clearfix"><span class="direct-chat-name pull-right">'
                            + data[i].name + '</span><span class="direct-chat-timestamp pull-left">'
                            + data[i].time + '</span></div><img class="direct-chat-img" src="Content/avatars/' + data[i].avatar + '" alt="message user image"><div class="direct-chat-text">'
                            + data[i].message + ' </div></div>';
                    } else {
                        message += '<div class="direct-chat-msg"><div class="direct-chat-info clearfix"><span class="direct-chat-name pull-left">'
                            + data[i].name + '</span><span class="direct-chat-timestamp pull-right">'
                            + data[i].time + '</span></div><img class="direct-chat-img" src="Content/avatars/' + data[i].avatar + '" alt="message user image"><div class="direct-chat-text">'
                            + data[i].message + ' </div></div>';
                    }
                }
                if ($('#discussion').find('.direct-chat-msg').length > 0) {
                    discussion.prepend(message);
                } else {
                    discussion.append(message);
                }
                /*if (discussion.height() > discussion.closest(".ibox-content").height()) {
                    discussion.prepend('<li class="loader"><img src="/Content/static_images/ajax loader.GIF"></li>');
                }*/
                self.chatScroll();
            } else {
                //self.deleteLoading();
                self.endOfChat = true;
            }
        });
    };

    /*self.deleteLoading = function () {
        $("#discussion li.loader").remove();
    };*/

    self.chatScroll = function () {
        $("#discussion").closest(".ibox-content").scrollTop($("#discussion")[0].scrollHeight - self.lastScrollHeightValue);
    };

    self.chatScrollDownOnMessageSent = function () {
        $("#discussion").closest(".ibox-content").scrollTop($("#discussion")[0].scrollHeight);
    };

    self.startChatting = function () {

        self.chatRequest();
        var closest = $("#discussion").closest(".ibox-content");

        //When scrollbar is at the top
        closest.scroll(function () {
            if (closest.scrollTop() === 0 && !self.endOfChat) {
                self.page++;
                self.lastScrollHeightValue = closest[0].scrollHeight;
                self.chatRequest();
            }
        });
        
        var chat = $.connection.chatHub;

        chat.client.addNewMessageToPage = function (name, message, time, avatar, userName) {
            if (userName === self.user().userName) {
                $('#discussion').append('<div class="direct-chat-msg right"><div class="direct-chat-info clearfix"><span class="direct-chat-name pull-right">'
                    + self.htmlEncode(name) + '</span><span class="direct-chat-timestamp pull-left">'
                    + time + '</span></div><img class="direct-chat-img" src="Content/avatars/' + avatar + '" alt="message user image"><div class="direct-chat-text">'
                    + self.htmlEncode(message) + ' </div></div>');
            } else {
                $('#discussion').append('<div class="direct-chat-msg"><div class="direct-chat-info clearfix"><span class="direct-chat-name pull-left">'
                    + self.htmlEncode(name) + '</span><span class="direct-chat-timestamp pull-right">'
                    + time + '</span></div><img class="direct-chat-img" src="Content/avatars/' + avatar + '" alt="message user image"><div class="direct-chat-text">'
                    + self.htmlEncode(message) + ' </div></div>');
            }
           
            //self.chatScrollDownOnMessageSent();
        };

       SignalRHelper.subscribeOnConnected("chatHub", function () {
           chat.server.connect(self.id());
           console.log('Now connected, connection ID=' + $.connection.hub.id);
           chat.server.connect(self.id());
           $('#message').keydown(function (e) {
               // Is enter key code
               if (e.keyCode === 13) {
                   var message = $('#message').val();
                   chat.server.send(self.id(), message);
                   $('#message').val('').focus();
               }
           });
           $('#sendmessage').click(
               function () {
                   var message = $('#message').val();
                   chat.server.send(self.id(), message);
                   $('#message').val('').focus();
               });
        });
       
    };

    self.startTaceability = function() {
        /*app.sendRequest(app.dataModel.getTraceability, 'GET', { IdFrom: 4 , IdTo: 3 }, function (data) {

        });*/
    };
    self.compare = function () {
        $('#alertLinks').hide();
        $('#alertChoose').hide();
        if (self.selectedFrom() !== self.selectedTo()) {
            app.sendRequest(app.dataModel.getTraceability, 'GET', { IdFrom: self.selectedFrom().type, IdTo: self.selectedTo().type },
                function(data) {
                    if (data != null) {
                        self.traceability(data);
                        self.noLinks(false);
                    } else {
                        self.noLinks(true);
                        $('#alertLinks').show();
                    }
                });
        } else {
            $('#alertChoose').show();
        }
        
    };

    self.htmlEncode = function (value) {
        var encodedValue = $('<div />').text(value).html();
        return encodedValue;
    };

    self.generate = function() {
        app.sendRequest(app.dataModel.generateDoc, 'GET',
            {
                Id: self.id()
            }, function (data) {
                if (data) {
                    window.open("/Docs/" + data);
                }
            });
    };

    return self;
}

app.addViewModel({
    name: "Project",
    bindingMemberName: "project",
    factory: ProjectViewModel,
    navigatorFactory: function (app) {
        return function (id) {
            app.view(app.Views.Project);
            app.project().load(id);
        }
    }
});
