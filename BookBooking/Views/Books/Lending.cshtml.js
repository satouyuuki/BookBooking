$(document).ready(function () {
    // ボタンを押したときに読み込む
    $("#search").click(function () {
        console.log($("#BarcodeId").val());
        var id = $("#BarcodeId").val();
        // 存在するアクションを指定
        loadPartialView(`/Books/SearchLending/${id}`);
    });
    // 指定したパスの partial view を読み込む

    function loadPartialView(loadPath) {
        $("#button_area").text('読み込み中…。');
        $("#button_area").load(loadPath, function (response, status, xhr) {
            // status === 'error' ならエラーと判断

            if (status === 'error') {
                $('#message').text('エラー (status : ' + xhr.status + ', statusText : ' + xhr.statusText + ')');
            } else {
                $('#message').text('正常 (status : ' + xhr.status + ', statusText : ' + xhr.statusText + ')');
            }
        });
    }
});