function UserProfileViewModel(app, dataModel) {
    var self = this;

    self.content = ko.observable();
    self.isLoaded = ko.observable(false);
    self.fileData = ko.observable({
        dataURL: ko.observable(),
        binaryString: ko.observable(),
        base64String: ko.observable()
    });
    
    self.acceptInvitation = function (data) {
        app.sendRequest(app.dataModel.acceptInvitation, 'POST', {
            Id: data.id 
        }, function () {
            location.reload();
        });
    };

    self.saveProfile = function () {
        if (app.isValid(self)) {
            app.sendRequest(app.dataModel.saveUser, 'POST', {
                Id: self.content().id,
                FName: self.content().fName,
                LName: self.content().lName,
                Phone: self.content().phone,
                Email: self.content().email,
                Hometown: self.content().hometown,
                File: self.fileData().base64String()
            }, function (data) {
                if (data != null) {
                    /*var url = '#/user/' + self.content().id;
                    Finch.navigate(url);*/
                    location.reload();
                }
            });
        }
    };

    self.load = function(id) {
        app.sendRequest(app.dataModel.getUserUrl, 'GET',
        {
            Id: id
        }, function(data) {
            self.content(data);
            self.isLoaded(true);
        });
    };

   self.onClear = function(fileData){
            if(confirm('Are you sure?')){
                fileData.clear && fileData.clear();
            }                            
    };

    return self;
}

app.addViewModel({
    name: "UserProfile",
    bindingMemberName: "userProfile",
    factory: UserProfileViewModel,
    navigatorFactory: function (app) {
        return function (id) {
            app.view(app.Views.UserProfile);
            app.userProfile().load(id);
        }
    }
});
