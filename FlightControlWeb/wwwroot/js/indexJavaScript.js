// Map pin object
var mark = function (pin, id) {
    this.pin = pin;
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
    // map options
    var options = {
        zoom: 1,
        center: { lat: 32.3232919, lon: 34.85538661 },
    }
    // make a new map
    map = new google.maps.Map(document.getElementsByClassName('.map-responsive'), options);
    //return map;
}

    //var map = initMap();
    var productsUrl = "/api/FlightPlan";
    $.getJSON(productsUrl, function (data) {
        // iterate through the flight plan and store it on array
        var myFlights = [];
        var countFlights = 0;
        for (var key in data) {
            if (data.hasOwnProperty(key)) {
                var element = data[key];
                myFlights[key] = element;
                countFlights++;
                alert("Lon: " + element.initial_location.longitude);
            }
        }

        //alert("Hello to " + myFlights[0].flightPlanID + " and to " + myFlights[1].flightPlanID + " and "
          //+ myFlights[2].flightPlanID + " and the last one " + myFlights[3].flightPlanID + " counter " + count);

        //var markers = [];
        // iterate again
        for (var j = 0; j < countFlights; j++) {
            // add markers
            var marker = new google.maps.Marker({
                position: {
                    lat: myFlights[j].initial_location.latitude,
                    lon: myFlights[j].initial_location.longitude
                },
                map: map,
                icon: 'css/NotActive-Airplane.png'
            });
            alert("he");
            marker.setMap(map);
            markers.markersArr.push(new mark(marker, myFlights[j].flightPlanID));
            // add the marker to the map
            marker.addListener('click', function () {
                // set the marker icon to the following picture
                for (var i = 0; markers.markersArr.length; i++) {

                    markers.markersArr[i].pin.setIcon('css/NotActive-Airplane.png');
                }
            });
        }
    });

    map.addListener('click', function () {
        for (var i = 0; i < markers.markersArr.length; i++) {
            markers.markersArr[i].pin.setIcon('css/NotActive-Airplane.png');
        }
    });




