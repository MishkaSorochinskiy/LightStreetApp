var defaultPosition = { lat: 49.8404945, lng: 24.0213335 };
var auditMap, auditMarker;

function initAuditMap() {
    auditMap = new google.maps.Map(document.getElementById("audit-map"), {
        zoom: 16,
        center: defaultPosition,
        mapTypeId: 'satellite'
    });

    auditMarker = new google.maps.Marker({
        map: auditMap,
    });

    var urlSearchParams = new URLSearchParams(window.location.search);
    var id = Object.fromEntries(urlSearchParams.entries()).id;
    if (id) {
        changeMarkerLocation(id);
    }
}

function changeMarkerLocation(id){
    $.ajax({
        type: 'GET',
        url: `${url}/camera?$filter=id eq ${id}`,
        contentType: 'application/json',
        complete: (res, success) => {
            if (res.status == 200) {
                var url = new URL(window.location);
                url.searchParams.set('id', id);
                window.history.pushState(null, '', url.toString());

                document.getElementById('identifier-lbl').innerHTML = res.responseJSON[0].identifier;
                position = { lat: res.responseJSON[0].latitude, lng: res.responseJSON[0].longtitude };
                auditMarker.setPosition(position);
                auditMap.setCenter(position);
            }
        }
    }); 
}
