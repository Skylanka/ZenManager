function AppViewModel(dataModel) {
    // Private state
    var self = this;

    var returnUrlKey = '?returnUrl=';

    // Private operations
    function cleanUpLocation() {
        window.location.hash = "";

        if (typeof (history.pushState) !== "undefined") {
            history.pushState("", document.title, location.pathname);
        }
    }

    self.dataModel = dataModel;

    // Data
    self.Views = {
        Loading: {} // Other views are added dynamically by app.addViewModel(...).
    };


    // UI state
    self.view = ko.observable(self.Views.Loading);

    self.loading = ko.computed(function() {
        return self.view() === self.Views.Loading;
    });

    // UI operations

    // Other navigateToX functions are added dynamically by app.addViewModel(...).

    // Other operations
    self.addViewModel = function(options) {
        var viewItem = new options.factory(self, dataModel),
            navigator;

        // Add view to AppViewModel.Views enum (for example, app.Views.Home).
        self.Views[options.name] = viewItem;

        // Add binding member to AppViewModel (for example, app.home);
        self[options.bindingMemberName] = ko.computed(function() {
            if (!dataModel.getAccessToken()) {
                // The following code looks for a fragment in the URL to get the access token which will be
                // used to call the protected Web API resource
                var fragment = common.getFragment();

                if (fragment.access_token) {
                    // returning with access token, restore old hash, or at least hide token
                    window.location.hash = fragment.state || '';
                    dataModel.setAccessToken(fragment.access_token);
                } else {
                    // no token - so bounce to Authorize endpoint in AccountController to sign in or register
                    window.location = "/Account/Authorize?client_id=web&response_type=token&state=" + encodeURIComponent(window.location.hash);
                }
            }
            if (self.view() !== viewItem) {
                return null;
            }
            return new options.factory(self, dataModel);
        });

        if (typeof (options.navigatorFactory) !== "undefined") {
            navigator = options.navigatorFactory(self, dataModel);
        } else {
            navigator = function() {
                self.view(viewItem);
            };
        }

        // Add navigation member to AppViewModel (for example, app.NavigateToHome());
        self["navigateTo" + options.name] = navigator;
    };


    /* self.addViewModel = function (options) {
        var viewItem = new options.factory(self, dataModel),
            navigator;
        // Add view to AppViewModel.Views enum (for example, app.Views.Home).
        self.Views[options.name] = viewItem;

        // Add binding member to AppViewModel (for example, app.home);
        self[options.bindingMemberName] = ko.computed(function () {
            if (self.view() !== viewItem) {
                return null;
            }

            return new options.factory(self, dataModel);
        });

        if (typeof (options.navigatorFactory) !== "undefined") {
            navigator = options.navigatorFactory(self, dataModel);
        } else {
            navigator = function () {
                self.view(viewItem);
            };
        }

        // Add navigation member to AppViewModel (for example, app.NavigateToHome());
        self["navigateTo" + options.name] = navigator;
    };*/

    self.initialize = function() {
        //Sammy().run();
        self.spaContent = $('#spaContent');
        self.spaContent.show();
    }

    self.isValid = function(model) {
        return ko.validatedObservable(model).isValid();
    };


    self.sendRequest = function(url, method, data, success) {
        $.ajax({
            method: method,
            url: url,
            data: data,
            dataType: "json",
            headers: {
                'Authorization': 'Bearer ' + dataModel.getAccessToken(),
                'Accept': 'application/json'
            },
            success: success,
            statusCode: {
                403: function() {
                    window.location.replace("/error/_403");
                },
                404: function() {
                    //dataModel.user(null);
                    window.location.replace("/error/_404");
                } /*,
                500: function () {
                    //dataModel.user(null);
                    window.location.replace("/error/_500");
                }*/
            },
            error: function(xhr, ajaxOptions, thrownError) {
                var err = eval("(" + xhr.responseText + ")");
                var message = (err.Message == undefined ? "" : err.Message + ". ") + (err.ExceptionMessage == undefined ? "" : err.ExceptionMessage);
                //self.showErrorMessage(message);
            }
        });
        self.spaContent = null;
    }

    self.sendNotProcessedRequest = function (url, method, data, success) {
        $.ajax({
            method: method,
            url: url,
            data: data,
            dataType: "json",
            processData: false,
            headers: {
                'Authorization': 'Bearer ' + dataModel.getAccessToken(),
                'Accept': 'application/json'
            },
            success: success,
            statusCode: {
                403: function () {
                    window.location.replace("/error/_403");
                },
                404: function () {
                    //dataModel.user(null);
                    window.location.replace("/error/_404");
                } /*,
                500: function () {
                    //dataModel.user(null);
                    window.location.replace("/error/_500");
                }*/
            },
            error: function (xhr, ajaxOptions, thrownError) {
                var err = eval("(" + xhr.responseText + ")");
                var message = (err.Message == undefined ? "" : err.Message + ". ") + (err.ExceptionMessage == undefined ? "" : err.ExceptionMessage);
                //self.showErrorMessage(message);
            }
        });
        self.spaContent = null;
    }

}

var app = new AppViewModel(new AppDataModel());
