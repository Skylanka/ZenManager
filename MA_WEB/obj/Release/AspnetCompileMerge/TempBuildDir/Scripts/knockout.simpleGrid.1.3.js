(function () {
    
    function loadData(self, dataUrl, params, page, pageSize) {
        if (params != null) {
            params.PageSize = pageSize;
            params.Page = page;
        } else
            params = { PageSize: pageSize, Page: page };

        app.sendRequest(dataUrl, 'GET', params, function(data) {
            if (typeof (data) == "object" && data.length <= pageSize) {
                self.itemsOnCurrentPage(data);
                self.loading(false);
                if (self.maxPageIndex() > 0)
                    self.showPaging(true);

                setTimeout(function() {
                    //app.initLocalization();
                }, 200);
            }
        });
    };

    function loadDataCount(self, dataCountUrl, params) {
        app.sendRequest(dataCountUrl, 'GET', params, function (data) {
            self.maxPageIndex(Math.ceil(data / self.pageSize) - 1);
            self.itemsCount(data);
            if (self.maxPageIndex() > 0)
                self.showPaging(true);
        });
    };

    ko.simpleGrid = {
        // Defines a view model class you can use to populate a grid
        viewModel: function (configuration) {

            var self = this;
            self.itemsCount = ko.observable(0);
            self.currentPageIndex = ko.observable(0);
            self.pageSize = configuration.pageSize || 20;
            self.itemsOnCurrentPage = ko.observableArray([]);
            self.maxPageIndex = ko.observable(0);
            self.showPaging = ko.observable(false);
            self.params = configuration.params;
            self.loading = ko.observable(true);

            if (typeof (configuration.loadDataOnInitialize) != "undefined")
                self.loadDataOnInitialize = configuration.loadDataOnInitialize;
            else
                self.loadDataOnInitialize = true;

            self.pagingLeft = ko.computed(function () {
                var currentPageIndex = self.currentPageIndex();
                var start = currentPageIndex - 5;

                if (currentPageIndex < 1)
                    return [];
                if (start < 1)
                    start = 1;

                return ko.utils.range(start, currentPageIndex - 1);
            });

            self.pagingRight = ko.computed(function () {
                var currentPageIndex = self.currentPageIndex();
                var finish = currentPageIndex + 5;

                if (currentPageIndex == self.maxPageIndex())
                    return [];
                if (finish > self.maxPageIndex())
                    finish = self.maxPageIndex();

                return ko.utils.range(currentPageIndex + 1, finish);
            });

            // If you don't specify columns configuration, we'll use scaffolding
            self.columns = configuration.columns;

            self.currentPageIndex.subscribe(function() {
                loadData(self, configuration.dataUrl, self.params, self.currentPageIndex(), self.pageSize);
            });

            //загружает данные
            self.loadData = function (param) {
                loadData(self, configuration.dataUrl, self.params, 0, self.pageSize);

                if (configuration.dataCountUrl != null)
                    loadDataCount(self, configuration.dataCountUrl, self.params);
            }
            
            if (configuration.data != null) {
                self.itemsOnCurrentPage = ko.computed(function () {
                    var startIndex = self.pageSize * self.currentPageIndex();
                    return ko.utils.unwrapObservable(configuration.data).slice(startIndex, startIndex + self.pageSize);
                }, this);
                self.loading(false);
            } else {
                if (self.loadDataOnInitialize == true) {
                    loadData(self, configuration.dataUrl, self.params, 0, self.pageSize);

                    if (configuration.dataCountUrl != null)
                        loadDataCount(self, configuration.dataCountUrl, self.params);
                }
            }
        }
    };

    // Templates used to render the grid
    var templateEngine = new ko.nativeTemplateEngine();

    templateEngine.addTemplate = function (templateName, templateMarkup) {
        document.write("<script type='text/html' id='" + templateName + "'>" + templateMarkup + "<" + "/script>");
    };

    /*<thead>\
                            <tr role=\"row\" data-bind=\"foreach: columns\">\
                               <th tabindex=\"0\"  rowspan=\"1\" colspan=\"1\" data-bind=\"text: headerText\"></th>\
                            </tr>\
                        </thead>\*/

    templateEngine.addTemplate("ko_simpleGrid_grid", "\
                    <table class=\"table table-bordered table-striped\" role=\"grid\" id=\"issueTable\">\<thead>\
                            <tr data-bind=\"foreach: columns\">\
                               <th data-bind=\"text: headerText\"></th>\
                            </tr>\
                        </thead>\
                        <tbody data-bind=\"foreach: itemsOnCurrentPage\">\
                           <tr role=\"row\" data-bind=\"foreach: $parent.columns\">\
                               <td><span data-bind=\"html: typeof rowText == 'function' ? rowText($parent) : $parent[rowText] \"></span>&nbsp;<span data-bind=\"visible: $data.editable != null && editable($parent), click: function() { if ($data.onEditClick != null) { onEditClick($parent); } }\" class=\"glyphicon glyphicon-pencil\"></span></td>\
                            </tr>\
                        </tbody>\
                        <tbody data-bind=\"visible: loading\">\
                           <tr>\
                               <td data-bind=\"attr: {'colspan': columns.length}\" class=\"text-center\">\
	                                <img src=\"\\Content\\static_images\\ajax loader.GIF\">\
	                                <span class=\"text\">Loading</span>\
                                </td>\
                           </tr>\
                        </tr>\
                        </tbody>\
                        <tbody data-bind=\"visible: itemsOnCurrentPage().length == 0 && !loading()\">\
                           <tr>\
                               <td class=\"text-center empty-area\">\
	                                <span class=\"text\">You don't have any items.</span>\
                                </td>\
                           </tr>\
                        </tr>\
                        </tbody>\
                    </table>");

   /* templateEngine.addTemplate("ko_simpleGrid_pageLinks", "\
                    <div class=\"btn-group\" data-bind=\"visible: showPaging\">\
                        <!-- ko foreach: pagingLeft -->\
                            <button type=\"button\" class=\"btn btn-default\" data-bind=\"text: $data + 1, click: function() { $root.currentPageIndex($data) }\">\
                            </button>\
                        <!-- /ko -->\
                        <!-- ko foreach: pagingRight -->\
                            <button type=\"button\" class=\"btn btn-default\" data-bind=\"text: $data + 1, click: function() { $root.currentPageIndex($data) }\">\
                            </button>\
                        <!-- /ko -->\
                    </div>");*/
/*   templateEngine.addTemplate("ko_simpleGrid_pageLinks", "\
                    <ul class=\"pagination pagination-sm no-margin pull-right\" data-bind=\"visible: showPaging\">\
                        <!-- ko foreach: pagingLeft -->\
                            <button type=\"button\" class=\"btn btn-default\" data-bind=\"text: $data + 1, click: function() { $root.currentPageIndex($data) }\">\
                            </button>\
                        <!-- /ko -->\
                        <!-- ko foreach: pagingRight -->\
                            <button type=\"button\" class=\"btn btn-default\" data-bind=\"text: $data + 1, click: function() { $root.currentPageIndex($data) }\">\
                            </button>\
                        <!-- /ko -->\
                    </ul>");*/
/*    self.itemsCount = ko.observable(0);
    self.currentPageIndex = ko.observable(0);
    self.pageSize = configuration.pageSize || 20;
    self.itemsOnCurrentPage = ko.observableArray([]);
    self.maxPageIndex = ko.observable(0);
    self.showPaging = ko.observable(false);
    self.params = configuration.params;
    self.loading = ko.observable(true);*/

    templateEngine.addTemplate("ko_simpleGrid_pageLinks", "\
            <div class=\"row\">\
                <div class=\"col-sm-5\">\
                     <div class=\"dataTables_info\" role=\"status\" aria-live=\"polite\" data-bind=\"text: 'Showing ' + (currentPageIndex()*pageSize+1) +' to ' + ((currentPageIndex()*pageSize+1) + itemsOnCurrentPage().length - 1) +' of ' + itemsCount() + ' entries'\">\
              </div></div>\
                <div class=\"col-sm-7\">\
                     <div class=\"dataTables_paginate paging_simple_numbers\" >\
                    <ul class=\"pagination pull-right\">\
                        <li class=\"paginate_button previous\"><a href=\"#/projects\" data-bind=\"click: function() { currentPageIndex(0) }\">&laquo;</a></li>\
                        <!-- ko foreach: ko.utils.range(0, maxPageIndex) -->\
                               <li class=\"paginate_button\">\<a href=\"#/projects\" data-bind=\"text: $data + 1, click: function() { $root.currentPageIndex($data) }, css: { selected: $data == $root.currentPageIndex() }\">\
                            </a>\</li>\
                        <!-- /ko -->\
                       <li class=\"paginate_button next\"><a href=\"#/projects\" data-bind=\"click: function() { currentPageIndex(maxPageIndex) }\">&raquo;</a></li>\
                    </ul>\
              </div></div></div>");


    // The "simpleGrid" binding 
    ko.bindingHandlers.simpleGrid = {
        init: function () {
            return { 'controlsDescendantBindings': true };
        },
        // This method is called to initialize the node, and will also be called again if you change what the grid is bound to
        update: function (element, viewModelAccessor, allBindings) {
            var viewModel = viewModelAccessor();

            // Empty the element
            while (element.firstChild)
                ko.removeNode(element.firstChild);

            // Allow the default templates to be overridden
            var gridTemplateName = allBindings.get('simpleGridTemplate') || "ko_simpleGrid_grid",
                pageLinksTemplateName = allBindings.get('simpleGridPagerTemplate') || "ko_simpleGrid_pageLinks";

            // Render the main grid
            var gridContainer = element.appendChild(document.createElement("DIV"));
            ko.renderTemplate(gridTemplateName, viewModel, { templateEngine: templateEngine }, gridContainer, "replaceNode");
            element.appendChild(gridContainer);

            // Render the page links
            var pageLinksContainer = element.appendChild(document.createElement("DIV"));
            ko.renderTemplate(pageLinksTemplateName, viewModel, { templateEngine: templateEngine }, pageLinksContainer, "replaceNode");
        }
    };
})();