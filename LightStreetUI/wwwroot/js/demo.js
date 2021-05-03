function upload() {
    document.getElementById("upload").click();
}

function handleUpload(files) {
    let formData = new FormData();
    formData.append('file', files[0], files[0].name);
    $.ajax({
        type: 'POST',
        url: `${url}/lamp/detect`,
        processData: false,
        contentType: false,
        data: formData,
        complete: async (res, success) => {
            document.getElementById("lightness").value = res.responseJSON.lightness;
            document.getElementById("upload-image").src = `data:image/jpeg;base64,${res.responseJSON.image64}`;
        }
    });
}

function submitted() {
    let formValues = {};
    formValues.lightness = `${document.getElementById("lightness").value}`;
    formValues.power = Number.parseFloat(document.getElementById("power").value);
    formValues.distance = `${document.getElementById("distance").value}`;
    formValues.type = Number.parseInt(document.getElementById("type").value);
    formValues.material = Number.parseInt(document.getElementById("material").value);

    $.ajax({
        type: 'POST',
        url: `${url}/lamp/analyse`,
        contentType: 'application/json',
        data: JSON.stringify(formValues),
        complete: async (res, success) => {
            if (res.responseJSON.prediction == '1') {
                document.getElementById("result").src = "light-lamp.png";
            } else {
                document.getElementById("result").src = "dark-lamp.png";
            }
        }
    });
}