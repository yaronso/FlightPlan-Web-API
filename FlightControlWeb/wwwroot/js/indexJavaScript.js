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
    // map options - defines the properties for the map.
    var options = {
        zoom: 3,
        center: new google.maps.LatLng(32.3232919, 34.85538661),
    };
    // make a new map
    var map = new google.maps.Map(document.getElementsByClassName("map-responsive"), options);
    return map;
}

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
                    lon: myFlights[j].initial_location.longitude
                },
                icon: 'css/NOTactiveAP.png'
            });

            // set the marker to our map.
            marker.setMap(map);
            markers.markersArr.push(new mark(marker, myFlights[j].flightPlanID));

            var infowindow = new google.maps.InfoWindow({
                content: "Hello World!"
            });
            google.maps.event.addListener(marker, 'click', function () {
                infowindow.open(map, marker);
            });

            alert("hey new map 4");
            // add the marker to the map
            marker.addListener('click', function () {
                // set the marker icon to the following picture
                for (var i = 0; markers.markersArr.length; i++) {
                    markers.markersArr[i].pin.setIcon('css/activeAP.png');
                }
            });
        }

        map.addListener('click', function () {
            for (var i = 0; i < markers.markersArr.length; i++) {
                // set the image of the marker to NOT active AP of the marker
                markers.markersArr[i].pin.setIcon('css/NOTactiveAP.png');
            }
        });

    // End of getJSON reading data
    });






