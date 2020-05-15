
function myMap() {
  var mapProp= {
        center:new google.maps.LatLng(51.508742,-0.120850),
    zoom:5,
  };
  var map = new google.maps.Map(document.getElementById("googleMap"),mapProp);
}

function initMap() {
    // map options - defines the properties for the map.
    var options = {
        zoom: 3,
        center: new google.maps.LatLng(32.3232919, 34.85538661),
    };
    // make a new map
    //var map = new google.maps.Map(document.getElementsByClassName("map-responsive"), options);
    var map = new google.maps.Map(document.getElementById(m1), options);
    return map;
}


document.querySelector('.btn').addEventListener('click', function () {
    document.getElementById('head').textContent = 'vfvf';    

    var marker = new google.maps.Marker({
        position: myCenter,
        icon: 'css/activeAP.png'
    });

    marker.setMap(m1);
});





