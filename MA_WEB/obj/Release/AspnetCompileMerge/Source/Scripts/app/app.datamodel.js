function AppDataModel() {
    var self = this;
    // Routes
    self.userInfoUrl = "/api/Me";
    self.siteUrl = "/";

    // Route operations

    // Other private operations

    // Operations
    // Data
    self.returnUrl = self.siteUrl;

    self.getReqComments = "api/comments/getComments";
    self.getIssueComments = "api/comments/getIssueComments";
    self.saveComment = "api/comments/saveComment";


    self.getProjects = "api/projects/getProjects";
    self.saveProject = "api/projects/saveProject";
    self.getProject = "api/projects/getProject";
    self.getUsersForProject = "api/projects/getUsersForProject";
    self.getProjectsCount = "api/projects/getProjetsCount";
    self.getActiveProjects = "api/projects/getActiveProjects";
    self.deleteProject = "api/projects/deleteProject";
    self.generateDoc = "api/projects/generateDoc";
    self.sendInvitation = "api/projects/sendInvitation";
    self.acceptInvitation = "api/projects/acceptInvitation";


    self.getRequirements = "api/requirements/getRequirements";
    self.getRequirement = "api/requirements/getRequirement";
    self.saveRequirement = "api/requirements/saveRequirement";
    self.getRequirementsTypes = "api/requirements/getAllTypesOfRequirement";
    self.getWidgetRequirements = "api/requirements/getWidgetRequirements";
    self.getRequirementsByType = "api/requirements/getRequirementsByType";
    self.getReqLinks = "api/requirements/getRequirementLinks";
    self.getRequirementType = "api/requirements/getRequirementType";
    self.getRequirementWithAttr = "api/requirements/getRequirementWithAttr";
    self.getTraceability = "api/requirements/getTraceability";
    self.getRequirementsTypesWithRequirements = "api/requirements/getRequirementsTypesWithRequirements";
    self.addRequirementLink = "api/requirements/addRequirementLink";
    self.deleteRequirementLink = "api/requirements/deleteRequirementLink";
    self.saveRequirementType = "api/requirements/saveRequirementType";
    self.getLatestRequirements = "api/requirements/getLatestRequirements";
    self.deleteRequirement = "api/requirements/deleteRequirement";
    self.deleteRequirementType = "api/requirements/deleteRequirementType";
    

    self.getAttributeTypes = "api/attributes/getAttributeTypes";
    self.getProjectAttributeTypes = "api/attributes/getProjectAttributeTypes";
    self.getAttrTypesForReq = "api/attributes/getAttrTypesForReq";
    self.updateReqAttributes = "api/attributes/updateReqAttributes";
    self.saveAttributeType = "api/attributes/saveAttributeType";


    self.getIssues = "api/issues/getIssues";
    self.saveIssue = "api/issues/saveIssue";
    self.getIssuesCount = "api/issues/getIssuesCount";
    self.getIssue = "api/issues/getIssue";
    self.addIssueRequirementLink = "api/issues/addIssueRequirementLink";
    self.getIssueLinks = "api/issues/getIssueLinks";
    self.deleteIssueLink = "api/issues/deleteIssueLink";
    self.deleteIssue = "api/issues/deleteIssue";
    self.getLatestIssues = "api/issues/getLatestIssues";


    self.chatUrl = "api/chat/getChat";


    self.getUserUrl = "api/user/getUser";
    self.saveUser = "api/user/saveUser";
    self.saveUserAvatar = "api/user/saveUserAvatar";
    
    // Data access operations
    self.setAccessToken = function (accessToken) {
        sessionStorage.setItem("accessToken", accessToken);
    };

    self.getAccessToken = function () {
        return sessionStorage.getItem("accessToken");
    };

    self.user = ko.observable(null);

}
