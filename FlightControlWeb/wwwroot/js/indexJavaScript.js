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

// Initialize map
function initMap() {
    var options = {
        zoom: 2,
        center: { lat: 42.236, lng: -71.056 }
    }
    map = new google.maps.Map(document.getElementById('map'), options);
    // Call the date displaying function.
    displayCurrDate();
}

// Display the current date.
function displayCurrDate() {
    var d = new Date();
    var now = d.toLocaleString();
    document.getElementById("time").innerHTML = now;
}


// Fullfil the 'My Flights Table' accroding to the input.
var productsUrl = "/api/FlightPlan";
$.getJSON(productsUrl, function (data) {
        var table = document.getElementById('flightTable');
        for (var i = 0; i < data.length; i++) {
            var row = `<tr>
                           <td>${data[i].flightPlanID}</td>
                           <td>${data[i].company_name}</td>
                       <tr>`
            table.innerHTML += row
        }
});


    var productsUrl = "/api/FlightPlan";
    $.getJSON(productsUrl, function (data) {
        var myFlights = [];
        var countFlights = 0;
        // iterate through the flight plans and store it on array
        for (var key in data) {
            if (data.hasOwnProperty(key)) {
                var element = data[key];
                myFlights[key] = element;
                countFlights++;
                //alert("hi there " + myFlights[key].flightPlanID);
            }
        }

       // store the map.
       var map = initMap();
        // iterate again
        for (var j = 0; j < countFlights; j++) {
            //alert("Hey its a debug " + myFlights[j].initial_location.latitude);
            // add some markers to the map
            var marker = new google.maps.Marker({
                position: {
                    lat: myFlights[j].initial_location.latitude,
                    lng: myFlights[j].initial_location.longitude
                },
               
                icon: 'css/NOTactiveAP.png'
            });

            // set the marker to our map.
            marker.setMap(map);
            alert("Setted the marker on the map");
            markers.markersArr.push(new mark(marker, myFlights[j].flightPlanID));

            // add the marker to the map
            marker.addListener('click', function () {
                // set the marker icon to the following picture
                for (var i = 0; markers.markersArr.length; i++) {
                    markers.markersArr[i].pin.setIcon('css/activeAP.png');
                }
            });
        }

            /*map.addListener('click', function () {
                for (var i = 0; i < markers.markersArr.length; i++) {
                    // set the image of the marker to NOT active AP of the marker
                    markers.markersArr[i].pin.setIcon('css/NOTactiveAP.png');
                }
            });*/
        

    // End of getJSON reading data
    });






