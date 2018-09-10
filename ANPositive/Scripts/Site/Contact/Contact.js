$(function() {
    "use strict";

    var marker,
        map;

    google.maps.event.addDomListener(window, "load", initialize);

    function initialize() {
        var $main_color = "#1696e7",
            $saturation = -20,
            $brightness = 10;

        var style = [
            {
                elementType: "labels",
                stylers: [
                    { saturation: $saturation }
                ]
            },
            {
                featureType: "poi",
                elementType: "labels",
                stylers: [
                    { visibility: "off" }
                ]
            },
            {
                featureType: "road.highway",
                elementType: "labels",
                stylers: [
                    { visibility: "off" }
                ]
            },
            {
                featureType: "road.local",
                elementType: "labels.icon",
                stylers: [
                    { visibility: "off" }
                ]
            },
            {
                featureType: "road.arterial",
                elementType: "labels.icon",
                stylers: [
                    { visibility: "off" }
                ]
            },
            {
                featureType: "road",
                elementType: "geometry.stroke",
                stylers: [
                    { visibility: "off" }
                ]
            },
            {
                featureType: "transit",
                elementType: "geometry.fill",
                stylers: [
                    { hue: $main_color },
                    { visibility: "on" },
                    { lightness: $brightness },
                    { saturation: $saturation }
                ]
            },
            {
                featureType: "poi",
                elementType: "geometry.fill",
                stylers: [
                    { hue: $main_color },
                    { visibility: "on" },
                    { lightness: $brightness },
                    { saturation: $saturation }
                ]
            },
            {
                featureType: "poi.government",
                elementType: "geometry.fill",
                stylers: [
                    { hue: $main_color },
                    { visibility: "on" },
                    { lightness: $brightness },
                    { saturation: $saturation }
                ]
            },
            {
                featureType: "poi.sport_complex",
                elementType: "geometry.fill",
                stylers: [
                    { hue: $main_color },
                    { visibility: "on" },
                    { lightness: $brightness },
                    { saturation: $saturation }
                ]
            },
            {
                featureType: "poi.attraction",
                elementType: "geometry.fill",
                stylers: [
                    { hue: $main_color },
                    { visibility: "on" },
                    { lightness: $brightness },
                    { saturation: $saturation }
                ]
            },
            {
                featureType: "poi.business",
                elementType: "geometry.fill",
                stylers: [
                    { hue: $main_color },
                    { visibility: "on" },
                    { lightness: $brightness },
                    { saturation: $saturation }
                ]
            },
            {
                featureType: "transit",
                elementType: "geometry.fill",
                stylers: [
                    { hue: $main_color },
                    { visibility: "on" },
                    { lightness: $brightness },
                    { saturation: $saturation }
                ]
            },
            {
                featureType: "transit.station",
                elementType: "geometry.fill",
                stylers: [
                    { hue: $main_color },
                    { visibility: "on" },
                    { lightness: $brightness },
                    { saturation: $saturation }
                ]
            },
            {
                featureType: "landscape",
                stylers: [
                    { hue: $main_color },
                    { visibility: "on" },
                    { lightness: $brightness },
                    { saturation: $saturation }
                ]

            },
            {
                featureType: "road",
                elementType: "geometry.fill",
                stylers: [
                    { hue: $main_color },
                    { visibility: "on" },
                    { lightness: $brightness },
                    { saturation: $saturation }
                ]
            },
            {
                featureType: "road.highway",
                elementType: "geometry.fill",
                stylers: [
                    { hue: $main_color },
                    { visibility: "on" },
                    { lightness: $brightness },
                    { saturation: $saturation }
                ]
            },
            {
                featureType: "water",
                elementType: "geometry",
                stylers: [
                    { hue: $main_color },
                    { visibility: "on" },
                    { lightness: $brightness },
                    { saturation: $saturation }
                ]
            }
        ];

        var mapProp = {
            center: new google.maps.LatLng(38.442094, 27.143574),
            zoom: 16,
            panControl: false,
            zoomControl: false,
            mapTypeControl: false,
            scaleControl: true,
            streetViewControl: false,
            overviewMapControl: false,
            rotateControl: true,
            scrollwheel: false,
            mapTypeId: google.maps.MapTypeId.ROADMAP,
            styles: style
        };

        map = new google.maps.Map(document.getElementById("contact-map"), mapProp);

        marker = new google.maps.Marker({
            position: new google.maps.LatLng(38.442094, 27.143574),
            animation: google.maps.Animation.DROP,
            icon: "/images/Site/pin.png"
        });

        marker.setMap(map);
        map.panTo(marker.position);
    }

    $("#contact-form").validate({
        rules: {
            "emailAddress": {
                required: true
            },
            "firstNameLastName": {
                required: true,
                minlength: 5
            },
            "emailSubject": {
                required: true,
                minlength: 3
            },
            "emailMessage": {
                required: true,
                minlength: 3
            }
        },
        messages: {
            "emailAddress": {
                required: function() {
                    toastr.error("Geçerli bir eposta adresi girmeniz gerekiyor!");
                    return false;
                }
            },
            "firstNameLastName": {
                required: function() {
                    toastr.error("Adınızı, soyadınızı girmeniz gerekiyor!");
                    return false;
                }
            },
            "emailSubject": {
                required: function () {
                    toastr.error("Mesajın konusunu girmeniz gerekiyor!");
                    return false;
                }
            },
            "emailMessage": {
                required: function () {
                    toastr.error("Mesajınızı girmeniz gerekiyor!");
                    return false;
                }
            }
        },
        submitHandler: function (form) {
            $(".loadingContainer").toggleClass("d-none");
            var data = {
                firstNameLastName: $("#firstNameLastName").val(),
                emailAddress: $("#emailAddress").val(),
                emailSubject: $("#emailSubject").val(),
                emailMessage: $("#emailMessage").val()
            };

            $.ajax({
                type: "POST",
                url: ajaxUrl,
                data: JSON.stringify(data),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var result = msg.result;
                    if (result.success) {
                        toastr.success(result.message);
                    } else {
                        toastr.error(result.message);
                    }
                    $(".loadingContainer").toggleClass("d-none");
                },
                error: function () {
                    console.log("Bir sorun oluştu. Lütfen daha sonra tekrar deneyin.");
                    $(".loadingContainer").toggleClass("d-none");
                }
            });
            return false;
        },
        errorPlacement: function(error, element) {
            console.log(element);
            return false;
        },
        success: function() {
            console.log("Success");
        }
    });
});