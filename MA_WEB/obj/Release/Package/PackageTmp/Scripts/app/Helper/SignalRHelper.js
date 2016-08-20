// Global SignalR connection helper
SignalRHelper = {};

SignalRHelper._signalrState = $.signalR.connectionState.disconnected;

SignalRHelper._connectionCallbacks = {};

SignalRHelper._connect = function () {
    $.connection.hub.qs = { "Bearer": app.dataModel.getAccessToken() };
    $.connection.hub.start().done(function () {
        for (var callback in SignalRHelper._connectionCallbacks) {
            if (SignalRHelper._connectionCallbacks.hasOwnProperty(callback)) {
                SignalRHelper._connectionCallbacks[callback]();
            }
        }
    });
};

// Create connection and reconnect handler
SignalRHelper.init = function() {

    $.connection.hub.url = '/signalr';
    $.connection.hub.logging = true;
    $.connection.hub.qs = { 'accessToken': app.dataModel.getAccessToken() };

    $.connection.hub.stateChanged(function (change) {
        SignalRHelper._signalrState = change.newState;
    });

    // Reconnect handler
    $.connection.hub.disconnected(function() {
        setTimeout(function() {
            SignalRHelper._connect();
        }, 5000);
    });

    // Open connection
    SignalRHelper._connect();
};



SignalRHelper.subscribeOnConnected = function (name, callback) {
    SignalRHelper._connectionCallbacks[name] = callback;

    $.connection.hub.stop();
    SignalRHelper._connect();
};

SignalRHelper.unsubscribeOnConnected = function (name) {
    delete SignalRHelper._connectionCallbacks[name];
}