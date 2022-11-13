var directionsService = null;
var directionsRenderer = null;
var clickListenerHandler = null;
var map = null;
var directions = [];

var points = [];
var newpoints = [];
var iseditable = false;
var isadmin = true;
var coloredpolylines = [];

function initMap (){
    directionsService = new google.maps.DirectionsService();
    directionsRenderer = new google.maps.DirectionsRenderer();
    var lviv = new google.maps.LatLng(49.84070662559602, 24.026379088646518);
    var mapOptions = {
        zoom: 13,
        center: lviv,
        mapTypeId:google.maps.MapTypeId.SATELLITE
    }
    map = new google.maps.Map(document.getElementById('map'), mapOptions);
    directionsRenderer.setMap(map);

    $.ajax({
        type: 'GET',
        url: `${url}/camera`,
        contentType: 'application/json',
        complete: (res, success) => {
            points = [];
            res.responseJSON.forEach(cmr => addPoint(cmr));
        }
    }); 
}

function calcRoute() {
    clearRoutes();
    clearPolylines();

    var start = document.getElementById('start').value;
    var end = document.getElementById('end').value;
    var request = {
        origin: start,
        destination: end,
        travelMode: 'DRIVING',
        provideRouteAlternatives: true
    };
    directionsService.route(request, function (result, status) {
        if (status == 'OK') {
            let routespoints = [];
            let routes = [];
            for (var i = 0, len = result.routes.length; i < len; i++) {
                var polylineOptionsActual = new google.maps.Polyline({
                    strokeColor: "#6666ff",
                    strokeOpacity: 1.0,
                    strokeWeight: 6
                });

                directions.push(new google.maps.DirectionsRenderer({
                    map: map,
                    directions: result,
                    routeIndex: i,
                    zIndex: 50,
                    polylineOptions: polylineOptionsActual
                }));

                routespoints = routespoints.concat(pointsBelong(result.routes[i].overview_path));
                routes.push(result.routes[i].overview_path);
            }

            routespoints = [...new Set(routespoints)];

            getLightness(routespoints, routes);
        }
        else {
            alert(`request is invalid: ${status}`);
        }
    });
}

function clickListener(event) {
    var latitude = event.latLng.lat();
    var longitude = event.latLng.lng();

    var marker = new google.maps.Marker({
        position: { lat: latitude, lng: longitude },
        map: map
    });

    let newpoint = new Point(marker);
    newpoint.index = newpoints.length;
    newpoint.setInfoWindow(new google.maps.InfoWindow(), map);

    newpoints.push(newpoint);
}

function clearRoutes() {
    for (let i = 0; i < directions.length; ++i) {
        directions[i].setMap(null);
    }
}

function clearPolylines() {
    for (let i = 0; i < coloredpolylines.length; ++i) {
        coloredpolylines[i].poly.setMap(null);
    }

    coloredpolylines = [];
}

function clearPoints() {
    for (let i = 0; i < points.length; ++i) {
        points[i].marker.setMap(null);
    }
    points = [];
}

function switchEdit() {
    if (iseditable) {
        iseditable = false;
        clickListenerHandler.remove();
        for (let i = 0; i < newpoints.length; ++i) {
            newpoints[i].marker.setMap(null);
        }

        newpoints = [];
    }
    else {
        iseditable = true;
        clickListenerHandler = map.addListener("click", clickListener);
    }
}

function addPoint(point) {
    var marker = new google.maps.Marker({
        position: { lat: point.latitude, lng: point.longtitude },
        map: map
    });

    let newpoint = new Point(marker);
    newpoint.lampTypeId = point.lampTypeId;
    newpoint.index = points.length;
    newpoint.setInfoWindow(new google.maps.InfoWindow(), map);
    newpoint.setInfoPhoto(point)
    points.push(newpoint);
}

function pointsBelong(smoothroute) {
    let routepoints = [];

    let polyline = new google.maps.Polyline({
        path: smoothroute,
        strokeColor: '#000000',
        strokeWeight: 10
    })

    for (let i = 0; i < points.length; ++i) {
        let location = new google.maps.LatLng(points[i].latitude, points[i].longtitude);
        if (google.maps.geometry.poly.isLocationOnEdge(location, polyline, 0.01)) {
            routepoints.push(points[i].id);
        }
    }

    return routepoints;
}

function getLightness(cameraIds, routes) {
    (async () => {
        const rawResponse = await fetch(`${url}/Lamp/analyse-multiple`, {
            method: 'POST',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(cameraIds)
        });
        const responce = await rawResponse.json();

        showLightRoutes(responce, routes)
    })();
}

function showLightRoutes(lightpoints, routes) {

    for (let routeIndex = 0; routeIndex < routes.length; ++routeIndex) {
        for (let subPolyIndex = 0; subPolyIndex < routes[routeIndex].length; subPolyIndex += 1) {
            for (let pointIndex = 0; pointIndex < lightpoints.length; ++pointIndex) {

                let point = points.find(p => p.id == lightpoints[pointIndex].cameraId);

                if (point !== undefined) {
                    let latLng = new google.maps.LatLng(point.latitude, point.longtitude);
                    let polyline = new google.maps.Polyline({
                        path: routes[routeIndex].slice(subPolyIndex, subPolyIndex + 2),
                        strokeWeight: 9,
                        zIndex: 100
                    });

                    if (google.maps.geometry.poly.isLocationOnEdge(latLng, polyline, 0.001)) {
                        coloredpolylines.push({ isLight: lightpoints[pointIndex].isLight, poly: polyline });
                    }
                }
            }
        }
    }

    for (let i = 0; i < coloredpolylines.length; ++i) {
        if (coloredpolylines[i].isLight) {
            coloredpolylines[i].poly.setOptions({ strokeColor: "#00ff00" });
        } else {
            coloredpolylines[i].poly.setOptions({ strokeColor: "#4d4d00" });
        }

        coloredpolylines[i].poly.setMap(map);
    }
}

function bounds_changed() {
    let bounds = map.getBounds();
    console.log(bounds);
    for (let i = 0; i < points.length; ++i) {
        if (points[i].latitude >= bounds.Ya.i && points[i].latitude <= bounds.Ya.j) {
            if (points[i].longitude >= bounds.Ua.i && points[i].longitude <= bounds.Ua.j) {
                points[i].marker.setMap(null);
                points.splice(i, 1);
            }
        }
    }
    $.ajax({
        type: 'GET',
        url: `${url}/camera?$filter=latitude gt ${bounds.La.i}`,
        contentType: 'application/json',
        complete: (res, success) => {
            console.log(res);
        }
    }); 
}