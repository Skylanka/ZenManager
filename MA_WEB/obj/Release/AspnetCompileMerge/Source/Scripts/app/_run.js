$(function () {
    app.initialize();

    // Activate Knockout
    ko.validation.init({
        grouping: { observable: false },
        registerExtenders: true,
        messagesOnModified: true,
        insertMessages: true,
        parseInputAttributes: true,
        messageTemplate: null
    });


    Finch.route("/", function () {

      /*  var getParameterByName = function (name) {
            name = name.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
            var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
                results = regex.exec(location.search);
            return results === null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
        }

        var returnUrl = getParameterByName('returnUrl');

        if (returnUrl) {
            window.location.href = window.location.origin + '/#' + returnUrl;
        } else {*/
            app.navigateToHome();
        //}
    });

    Finch.route("/user/:id", function (bindings) {
        app.navigateToUserProfile(bindings.id);
    });

    Finch.route("/project", function () {
        app.navigateToProject();
    });

    Finch.route("/project/:id/edit", function (bindings) {
        app.navigateToAddProject(bindings.id);
    });

    Finch.route("/projects", function () {
        app.navigateToProjects();
    });

    Finch.route("/projects/add", function () {
        app.navigateToAddProject();
    });

    Finch.route("/project/:id", function (bindings) {
        app.navigateToProject(bindings.id);
    });

    Finch.route("/project/:projectId/requirement/:id/edit", function (bindings) {
        app.navigateToAddRequirement(bindings.projectId, bindings.id);
    });

    Finch.route("/project/:projectId/requirements/add", function (bindings) {
        app.navigateToAddRequirement(bindings.projectId);
    });

    Finch.route("/project/:projectId/requirement/:id", function (bindings) {
        app.navigateToRequirement(bindings.projectId, bindings.id);
    });

    Finch.route("/project/:id/requirements/", function (bindings) {
        app.navigateToRequirements(bindings.id);
    });

    Finch.route("/project/:projectId/requirements/type/:id", function (bindings) {
        app.navigateToRequirementsByType(bindings.projectId, bindings.id);
    });

    Finch.route("/project/:projectId/requirements/types/add", function (bindings) {
        app.navigateToAddRequirementType(bindings.projectId);
    });
    Finch.route("/project/:projectId/requirements/type/:id/edit", function (bindings) {
        app.navigateToAddRequirementType(bindings.projectId, bindings.id);
    });

    Finch.route("/project/:projectId/issue/:id", function (bindings) {
        app.navigateToIssue(bindings.projectId, bindings.id);
    });
    
    ko.applyBindings(app);
    Finch.listen();
});

