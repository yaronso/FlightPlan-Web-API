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
var flight = function (flight_id, longitude, latitude, passengers, company_name, date_time, is_external) {
    this.flight_id = flight_id;
    this.longitude = longitude;
    this.latitude = latitude;
    this.passengers = passengers;
    this.company_name = company_name;
    this.date_time = date_time;
    this.is_external = is_external;
    return this;
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

// Sampling the whole flights
var xhttp = new XMLHttpRequest();
xhttp.onreadystatechange = function () {
    if (this.readyState == 4 && this.status == 200) {
        flights.flightsArr = [];
        const words = this.responseText.replace("]", "").replace("[", "").concat(",").split("},");
        for (i = 0; i < words.length - 1; i++) {
            var obj = JSON.parse(words[i].concat('}'));
            // Store the current flight
            var currentFlight = new flight(obj.flight_id, obj.longitude, obj.latitude, obj.passengers, obj.company_name,
                obj.date_time, obj.landing_time, obj.is_external);
            // Store the object in flights array
            flights.flightsArr.push(new flight(obj.flight_id, obj.longitude, obj.latitude, obj.passengers, obj.company_name,
                obj.date_time, obj.landing_time, obj.is_external));
            //console.log("Current Flight ID is " + currentFlight.flight_id);
            setFlightsTable(currentFlight);
            
        }
        if (flights.flightsArr.length != null) {
            drawAirPlanes(flights.flightsArr);
        }
        
    }
};

setInterval(function () {
    // Set the current date time
    var d = new Date();
    var now = d.toLocaleString();
    document.getElementById("time").innerHTML = now;
    var currentTime = new Date(new Date().toString().split('GMT')[0] + 'UTC').toISOString().split('.')[0] + "Z";
    xhttp.open("GET", "/api/Flights?relative_to=".concat(currentTime), true);
    xhttp.send();
}, 3000);




// Fullfil the 'My Flights Table' accroding to the input.
var row;
function setFlightsTable(flight) {
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
    li2.appendChild(document.createTextNode(flight.company_name));
    let btn = document.createElement("button");
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

    btn.addEventListener('click', function () {
        document.getElementById('fid').innerHTML = flight.flight_id;
        document.getElementById('fcn').innerHTML = flight.company_name;
        document.getElementById('fpas').innerHTML = flight.passengers;
        drawLines(flight);


    })

    btn2.addEventListener('click', function () {
        var row = btn2.parentNode.parentNode;
        row.parentNode.removeChild(row);
    })

}



// Draw the airplanes icons according the Flights
function drawAirPlanes(myFlights) {
    for (var j = 0; j < myFlights.length; j++) {
        // add some markers to the map
        var marker = new google.maps.Marker({
            position: {
                lat: myFlights[j].latitude,
                lng: myFlights[j].longitude
            },
            icon: 'css/NotactiveAP.png'
        });


        var duplicatePlane = 0;
        for (var k = 0; k < markers.markersArr.length; k++) {
            if (myFlights[j].flight_id == markers.markersArr[k].id) {
                duplicatePlane = 1;
                break;
            }
        }
        if (duplicatePlane == 1) {
            continue;
        }

        // set the marker to our map.
        marker.setMap(map);
        markers.markersArr.push(new mark(marker, myFlights[j].flight_id));

        // when a click on a marker occured
        marker.addListener('click', function () {
            // set the marker icon to the following picture
            for (var i = 0; markers.markersArr.length; i++) {
                // find the specific id airplane
                if (this == markers.markersArr[i].pin) {
                    id = markers.markersArr[i].id;
                    markers.markersArr[i].pin.setIcon('css/blueAP.png');
                    break;
                }
                
            }
        });
    }

    map.addListener('click', function () {
        for (var i = 0; i < markers.markersArr.length; i++) {
            // set the image of the marker to NOT active AP of the marker
            markers.markersArr[i].pin.setIcon('css/NotactiveAP.png');
        }
    });
}

// Draw the segments lines according the Flight Plan
function drawLines(flight) {
    var flightPlan;
    var xhttp = new XMLHttpRequest();
    xhttp.onreadystatechange = function () {
        if (this.readyState == 4 && this.status == 200) {
            flightPlan = JSON.parse(this.responseText);
            var initLat = flightPlan.initial_location.latitude;
            var initLon = flightPlan.initial_location.longitude;
            var segArr = [];
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
        }
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





