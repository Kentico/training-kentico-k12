/* Languages dropsown */
$(document).ready(function(){
    $(".dropdown-trigger").dropdown({
        hover: false,
    });
});



  /* Contact us page - Google map */
  function medioClinicMap(){

    var myLatLng = { lat: 51.5, lng: -0.15};

    var mapOptions = {
        zoom: 10,
        center: myLatLng,
        mapTypeId: google.maps.MapTypeId.HYBRID
    };
    
    var map = new google.maps.Map(document.getElementById("map"), mapOptions);

    var marker = new google.maps.Marker({
        position: myLatLng,
        map: map
    });

    var devwindow = new google.maps.InfoWindow({
        content: "Just an example of a map."
    });

    devwindow.open(map, marker);
}

