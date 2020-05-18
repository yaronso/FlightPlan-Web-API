

// Map pin object
var mark = function (pin, id) {
    // The marker object
    this.pin = pin;
    // The flight Plan ID
    this.id = id;
};

// markersArr
var markers = {
    start: '',
    end: '',
    length: '',
    markersArr: []
};

// Array of flights
var flights = {
    start: '',
    end: '',
    length: '',
    flightsArr: []
};

// Flight object
function flight (flight_id, longitude, latitude, passengers, final_location, starting_datails, initial_location, landing_details, company_name, date_time, is_external) {
    this.flight_id = flight_id;
    this.longitude = longitude;
    this.latitude = latitude;
    this.passengers = passengers;
    this.final_location = final_location;
    this.starting_datails = starting_datails;
    this.initial_location = initial_location;
    this.landing_details = landing_details;
    this.company_name = company_name;
    this.date_time = date_time;
    this.is_external = is_external;
    //return this;
};


var counter = 0;
var map;

// Initialize map
function initMap() {
    var options = {
        zoom: 2,
        center: { lat: 42.236, lng: -71.056 }
    }
    map = new google.maps.Map(document.getElementById('map'), options);
}

var flightsArr = [];
var currentFlight;
// Sampling the whole flights
var xhttp = new XMLHttpRequest();
xhttp.onreadystatechange = function () {
    
    if (this.readyState == 4 && this.status == 200) {

        flightsArr = [];
        const words = this.responseText.replace("]", "").replace("[", "").concat(",").split("},");
        for (i = 0; i < words.length - 1; i++) {
            var obj = JSON.parse(words[i].concat('}'));
            // Store the current flight
            currentFlight = new flight(obj.flight_id, obj.longitude, obj.latitude, obj.passengers, obj.final_location, obj.starting_datails, obj.initial_location, obj.landing_details,
                obj.company_name,
                obj.date_time, obj.landing_time, obj.is_external);

            console.log(words.length);
            console.log("cur lat " + currentFlight.latitude);            
            //console.log("cur lon " + currentFlight.longitude);
            setFlightsTable(currentFlight);
            flightsArr.push(currentFlight);
            


            /*
            var isExist = false;
            // Store the object in flights array
            for (var i = 0; i < flights.flightsArr.length; i++)
            {                
                if (currentFlight.flight_id == flights.flightsArr[i].flight_id) {
                    isExist = true;
                    break;
                }
            }
            if (!isExist) {

                console.log("middle lat " + currentFlight.latitude);
               flights.flightsArr.push(currentFlight);
               setFlightsTable(currentFlight);
            }

        }
        
        if (flights.flightsArr.length != null) {

            console.log("new lat " + currentFlight.latitude);
            drawAirPlanes(flights.flightsArr);
        }*/

        }
        
        drawAirPlanes(flightsArr);
    }
    
};

setInterval(function () {
    // Set the current date time

    var today = new Date();
    var h = today.getHours();
    if (h < 10) {
        h = "0" + h;
    }
    var m = today.getMinutes();
    if (m < 10) {
        m = "0" + m;
    }
    var s = today.getSeconds();
    if (s < 10) {
        s = "0" + s;
    }
    var day = today.getDate();
    if (day < 10) {
        day = "0" + day;
    }
    var month = today.getMonth() + 1;
    if (month < 10) {
        month = "0" + month;
    }
    var year = today.getFullYear();

   

    var format = year + '-' + month + '-' + day + 'T' + h + ':' + m + ':' + s + 'Z';
    



    
    var d = new Date();
    var now = d.toLocaleString();
    document.getElementById("time").innerHTML = now;
  
   


    let currentTime = new Date(new Date().toString()).toISOString().split(".")[0] + "Z";
    xhttp.open("GET", "/api/Flights?relative_to=".concat(format.toString()), true);
    xhttp.send();
}, 3000);



function buttons(flight, i) {
    document.getElementById('Company').innerHTML = flight[i].company_name;
    document.getElementById('Passengers').innerHTML = flight[i].passengers;
    document.getElementById('TakeOff').innerHTML = flight[i].starting_datails;
    document.getElementById('Landing').innerHTML = flight[i].landing_details;
    document.getElementById('Start').innerHTML = flight[i].initial_location;
    document.getElementById('End').innerHTML = flight[i].final_location;

    //removeDraw = drawLines(flight[i]));
}


// Fullfil the 'My Flights Table' accroding to the input.
var row;
let btn;

var tableId = [];
var mapOfPaths = new Map();
var mapOfAirplane = new Map();
var mapOfMyFlights = new Map();


function updateStatus(flight) {

    for (var i = 0; i < tableId.length; i++) {
        if (flight.flight_id != tableId[i] &&
            mapOfMyFlights.get(tableId[i]).style.backgroundColor == "blue") {
            replace(tableId[i]);
        }
        
    }

    document.getElementById('Company').innerHTML = flight.company_name;
    document.getElementById('Passengers').innerHTML = flight.passengers;
    document.getElementById('TakeOff').innerHTML = flight.starting_datails;
    document.getElementById('Landing').innerHTML = flight.landing_details;
    document.getElementById('Start').innerHTML = flight.initial_location;
    document.getElementById('End').innerHTML = flight.final_location;

    var flightRow = mapOfMyFlights.get(flight.flight_id);
    flightRow.style.backgroundColor = "blue";
    var mark = mapOfAirplane.get(flight.flight_id);  
    mark.setIcon('css/blueAP.png');

    drawLines(flight);
}

function clickOnMap(flight) {

    for (var i = 0; i < tableId.length; i++) {
        if (flight.flight_id != tableId[i]) {
            var p = mapOfPaths.get(tableId[i]);
            if (p != null) {
                p.setMap(null);
            }
        }
    }
    removeDetails(flight);
    var flightRow = mapOfMyFlights.get(flight.flight_id);
    flightRow.style.backgroundColor = "";
    var mark = mapOfAirplane.get(flight.flight_id);
    mark.setIcon('css/NotactiveAP.png');

}

function replace(id) {
    console.log(id);
    var flightRow = mapOfMyFlights.get(id);
    flightRow.style.backgroundColor = "";

    var mark = mapOfAirplane.get(id);
    mark.setIcon('css/NotactiveAP.png');

    var removeDraw = mapOfPaths.get(id);
    removeDraw.setMap(null);

}

function removeDetails(flight) {
    document.getElementById('Company').innerHTML = "";
    document.getElementById('Passengers').innerHTML = "";
    document.getElementById('TakeOff').innerHTML = "";
    document.getElementById('Landing').innerHTML = "";
    document.getElementById('Start').innerHTML = "";
    document.getElementById('End').innerHTML = "";

    var removeDraw = mapOfPaths.get(flight.flight_id);
    removeDraw.setMap(null);
}


function setFlightsTable(flight) {

    var isExist = false;
    for (var i = 0; i < tableId.length; i++) {
        if (tableId[i] == flight.flight_id) {
            isExist = true;
            break;
        }
    }

    if (!isExist) {
        tableId.push(flight.flight_id);

        var listFlight = document.getElementById('flightTable');
        // Check if the current flight is already exists in the fligts table.

        let tr = document.createElement("tr");
        let th1 = document.createElement("th");
        let th2 = document.createElement("th");
        let th3 = document.createElement("th");
        let th4 = document.createElement("th");
        let li = document.createElement("div");
        li.appendChild(document.createTextNode(flight.flight_id));
        let li2 = document.createElement("div");
        console.log(flight.company_name);
        li2.appendChild(document.createTextNode(flight.company_name));
        btn = document.createElement("button");
        btn.appendChild(document.createTextNode("press"));
        btn.style.background = "#8FBC8F";
        let btn2 = document.createElement("button");
        btn2.appendChild(document.createTextNode("delete"));
        btn2.style.background = "red";

        th1.appendChild(li);
        th2.appendChild(li2);
        th3.appendChild(btn);
        th4.appendChild(btn2)
        tr.appendChild(th1);
        tr.appendChild(th2);
        tr.appendChild(th3);
        tr.appendChild(th4);
        listFlight.appendChild(tr);
        mapOfMyFlights.set(flight.flight_id, tr);
        

        btn.addEventListener('click', function () { updateStatus(flight); }); 
       

        btn2.addEventListener('click', function () {

            if (mapOfMyFlights.get(flight.flight_id).style.backgroundColor == "blue") {
                clickOnMap(flight);
            }
            else {
            var row = btn2.parentNode.parentNode;
            row.parentNode.removeChild(row);

            removeDetails(flight);

            var removeAirplane = mapOfAirplane.get(flight.flight_id);
            removeAirplane.setMap(null);

            var xhttpDel = new XMLHttpRequest();
            xhttpDel.open("DELETE", "/api/Flights/".concat(flight.flight_id), true);
            xhttpDel.send();
        }

        })
    }
}

// Draw the airplanes icons according the Flights
function drawAirPlanes(myFlights) {
    for (var j = 0; j < myFlights.length; j++) {
        var duplicatePlane = 0;
        for (var k = 0; k < markers.markersArr.length; k++) {
            if (myFlights[j].flight_id == markers.markersArr[k].id) {
                duplicatePlane = 1;                
                var changeMarker = markers.markersArr[k].pin;
               
                changeMarker.setPosition(new google.maps.LatLng(myFlights[j].latitude,
                    myFlights[j].longitude));
            }
        }
        if (duplicatePlane == 1) {
            continue;
        }

        // add some markers to the map
        var marker = new google.maps.Marker({
            position: {
                lat: myFlights[j].latitude,
                lng: myFlights[j].longitude,
            },
            icon: 'css/NotactiveAP.png'
        });
        //console.log("lat " + myFlights[j].latitude);

        // set the marker to our map.
        mapOfAirplane.set(myFlights[j].flight_id, marker);
        marker.setMap(map);
        markers.markersArr.push(new mark(marker, myFlights[j].flight_id));
        var f = myFlights[j];
        // when a click on a marker occured
        marker.addListener('click', function () { updateStatus(f); });     

        map.addListener('click', function () { clickOnMap(f); });

    }

    //map.addListener('click', function () { clickOnMap(f); });
    /*
    map.addListener('click', function () {
        for (var i = 0; i < markers.markersArr.length; i++) {
            // set the image of the marker to NOT active AP of the marker
            markers.markersArr[i].pin.setIcon('css/NotactiveAP.png');
        }
    });*/
}



// Draw the segments lines according the Flight Plan

function drawLines(flight) {
    var flightPlan;
    var segArr = [];    
    var xhttp = new XMLHttpRequest();
    xhttp.onreadystatechange = function () {
        if (this.readyState == 4 && this.status == 200) {
            flightPlan = JSON.parse(this.responseText);
            var initLat = flightPlan.initial_location.latitude;
            var initLon = flightPlan.initial_location.longitude;            
            var initSeg = { lat: initLat, lng: initLon };
            segArr.push(initSeg);
            for (var i = 0; i < flightPlan.segments.length; i++) {
                var segIn = { lat: flightPlan.segments[i].latitude, lng: flightPlan.segments[i].longitude };
                segArr.push(segIn);
            }
            var flightPath = new google.maps.Polyline({
                path: segArr,
                geodesic: true,
                strokeColor: '#FF0000',
                strokeOpacity: 1.0,
                strokeWeight: 2
            });
            flightPath.setMap(map);
            mapOfPaths.set(flight.flight_id, flightPath);
        }
        return flightPath;
    }
    xhttp.open("GET", "/api/FlightPlan/".concat(flight.flight_id), true);
    xhttp.send();
    
}






function clickFlightDetails(id) {
    var clickFlight;
    var productsUrl = "/api/Flights";
    $.getJSON(productsUrl, (data) => {
        data.filter(flights => {
            for (var f in flights) {
                if (f.flight_id === id)
                    clickFlight = f;
            }
            document.getElementById('fid').innerHTML = clickFlight.flight_id;
        });
    });
}


function createClickButton(flightNumber) {
    console.log("hi");
    console.log(flightNumber);
}


function h() {
    console.log("hi");
}


function createButton(context, func) {
    var button = document.createElement("input");

    button.type = "button";
    button.value = "im a button";
    button.onclick = func;
    context.appendChild(button);
}