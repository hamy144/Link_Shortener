function PostLink(urlToShorten) {
    $.post("/?urlToShorten=" + urlToShorten)
        .done(function (data, status) {
            if (status == "success") {
                anchor = $("#link");
                anchor.attr("href", data);
                anchor.html(data);
                $(".d-none").removeClass("d-none");
                $("#copy").val(data);
            }
            else {
                alert(status);
            }
        });
};

function CopyLink() {
    var $temp = $("<input>");
    $("body").append($temp);
    $temp.val($("#link").attr('href')).select();
    document.execCommand("copy");
    $temp.remove();
}