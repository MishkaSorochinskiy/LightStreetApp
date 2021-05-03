const photourl = "https://imgstorage123.blob.core.windows.net/partnersfiles1/";
const url = "https://localhost:44325";
const uiUrl = "https://localhost:44322";  

var types = [];

setTypes();

class Point {
    index;
    id;
    lampTypeId = types[0].id;
    constructor(marker, photo) {
        this.photo = photo;
        this.marker = marker;
        this.latitude = marker.position.lat();
        this.longtitude = marker.position.lng();
        this.setInfoWindow.bind(this);
    }

    getInfoContent(photo) {
        let selectHtml = `<select class='form-control form-control-sm' onchange="selectChanged(${this.index},this.options[this.selectedIndex].value)">`; 
        for (let i = 0; i < types.length; ++i) {
            if (types[i].id == this.lampTypeId) {
                selectHtml = selectHtml + `<option selected value=${types[i].id}>${types[i].name}</option>`;
            } else {
                selectHtml = selectHtml + `<option value=${types[i].id}>${types[i].name}</option>`;
            }
        }

        selectHtml = selectHtml + "</select>";
        let content = `<div class="info-window text-center"> ${selectHtml}
                            <div class="row mt-1">
                                <div class="col-sm text-right">
                                    <input id="loadphoto" class="none" type="file" accept=".jpeg,.jpg,.png">
                                 </div>
                            </div>
                        <img class="pointphoto" onclick="loadLinkClicked(${this.index})" src="${photo}">
                      </div>`;

        return content;
    }

    setInfoPhoto(camera) {
        this.id = camera.id;
        this.infoWindow.setContent(this.getInfoContent(camera.photo));
    }

    setInfoWindow(infoWindow, map) {
        this.infoWindow = infoWindow;

        this.infoWindow.setContent(this.getInfoContent(`${uiUrl}/photo.png`));

        this.lampTypeId = this.lampTypeId;

        var marker = this.marker;
        this.marker.addListener('click', function () {
            infoWindow.open(map, marker);
        });
    }
}

function selectChanged(index, value) {
    let point = newpoints[index];
    point.lampTypeId = Number.parseInt(value);
}

function loadLinkClicked(index) {
    let point = newpoints[index];
    if (point == undefined) {
        point = points[index];
    }
    let input = document.getElementById("loadphoto");

    input.onchange = async (event) => {
        let photoBase64 = await getBase64(event.target.files[0]);
        var data = { latitude: point.latitude, longtitude: point.longtitude, photo: photoBase64, lampTypeId: Number.parseInt(point.lampTypeId) };
        $.ajax({
            type: 'POST',
            url: `${url}/camera`,
            contentType: 'application/json',
            data: JSON.stringify(data),
            complete: async (res, success) => {
                point.setInfoPhoto(res.responseJSON);
                points.push(point);
                newpoints.splice(newpoints.indexOf(point), 1);
            }
        }); 
    }

    input.click();
}


function getBase64(file) {
    return new Promise((resolve, reject) => {
        const reader = new FileReader();
        reader.readAsDataURL(file);
        reader.onload = () => resolve(reader.result);
        reader.onerror = error => reject(error);
    });
}

function setTypes() {
    $.ajax({
        type: 'GET',
        url: `${url}/lamptype`,
        contentType: 'application/json',
        complete: async (res, success) => {
            types = res.responseJSON;
        }
    }); 
}