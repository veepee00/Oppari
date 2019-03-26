"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/watchDogHub").build();

//Disable send button until connection is established
document.getElementById("updateWatchDogErrors").disabled = true;

connection.on("ReceiveWatchDogErrorsUpdate", function (rng) {
	document.getElementById('updateWatchDogErrors').value = rng.toString();
});

connection.on("UpdateWatchDogErrors", function (watchDogErrorCount) {
	document.getElementById('watchDogErrorCount').innerHTML = watchDogErrorCount.toString();
});

connection.start().then(function(){
    document.getElementById("updateWatchDogErrors").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("updateWatchDogErrors").addEventListener("click", function (event) {
    connection.invoke("UpdateWatchDogErrors").catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});
