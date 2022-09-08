$(document).ready(function () {
    function onScanSuccess(decodedText, decodedResult) {
        console.log(`Scan result: ${decodedText}`, decodedResult);
        loadPartialView(`/Books/SearchLending/${decodedText}`);
        html5QrcodeScanner.clear();
    }

    function loadPartialView(loadPath) {
        $("#button_area").text('読み込み中…。');
        $("#button_area").load(loadPath, function (response, status, xhr) {
            if (status === 'error') {
                $('#message').text('エラー (status : ' + xhr.status + ', statusText : ' + xhr.statusText + ')');
            } else {
                $('#message').text('正常 (status : ' + xhr.status + ', statusText : ' + xhr.statusText + ')');
            }
        });
    }

    var html5QrcodeScanner = new Html5QrcodeScanner(
        "reader", { fps: 10, qrbox: 250 });
    html5QrcodeScanner.render(onScanSuccess);

    $("#search").click(function () {
        console.log($("#BarcodeId").val());
        var id = $("#BarcodeId").val();
        loadPartialView(`/Books/SearchLending/${id}`);
    });
});