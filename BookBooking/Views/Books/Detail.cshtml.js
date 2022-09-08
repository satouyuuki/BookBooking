console.log("detail js");

$(document).ready(function () {
    // ボタンを押したときに読み込む
    $("#ShowBarCode").click(function () {
        var bookHistoryId = $(this).data('bookhistoryid');
        // 存在するアクションを指定
        loadPartialView(`/Books/Detail/${bookHistoryId}/ShowBarCode`);
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