"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/watchDogHub").build();

connection.on("UpdateWatchDogErrors", function (watchDogErrorList) {
    document.getElementById('watchDogErrorCount').innerHTML = watchDogErrorList.length;
    if (watchDogErrorList.length > 0) {
        document.getElementById('watchDogErrorCount').className = "btn btn-danger";
    }
    else {
        document.getElementById('watchDogErrorCount').className = "btn btn-success";
    }

});

connection.start().then(function () {
}).catch(function (err) {
    return console.error(err.toString());
    });
