﻿"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/watchDogHub").build();

connection.on("UpdateWatchDogErrors", function (watchDogErrorList) {
    document.getElementById('watchDogErrorCount').innerHTML = watchDogErrorList.length;
    document.getElementById('watchDogErrorCount').className = "btn btn-danger";

    var rowCount = watchDogErrorsTable.rows.length;
    for (var i = rowCount - 1; i > 0; i--) {
        watchDogErrorsTable.deleteRow(i);
    }

    for (var i in watchDogErrorList) {

        if (i != 0) {
            var row = watchDogErrorsTable.insertRow(i);
        }
        else {
            continue;
        }

        var cell1 = row.insertCell(0);
        var cell2 = row.insertCell(1);
        var cell3 = row.insertCell(2);
        var cell4 = row.insertCell(3);
        var cell5 = row.insertCell(4);
        var cell6 = row.insertCell(5);
        var cell7 = row.insertCell(6);
        var cell8 = row.insertCell(7);
        var cell9 = row.insertCell(8);
        cell1.innerHTML = watchDogErrorList[i].methodName;
        cell2.innerHTML = watchDogErrorList[i].errorMessage;
        cell3.innerHTML = watchDogErrorList[i].status;
        cell4.innerHTML = watchDogErrorList[i].timeStamp;
        cell5.innerHTML = watchDogErrorList[i].parameter1;
        cell6.innerHTML = watchDogErrorList[i].parameter2;
        cell7.innerHTML = watchDogErrorList[i].parameter3;
        cell8.innerHTML = watchDogErrorList[i].parameter4;
        cell9.innerHTML = watchDogErrorList[i].parameter5;
    }
});

connection.start().then(function () {
}).catch(function (err) {
    return console.error(err.toString());
    });
