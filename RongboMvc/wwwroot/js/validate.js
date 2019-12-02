; (function ($, layui) {
    var layer = layui.layer;
    var toasting = false;
    $.validator.unobtrusive.options = {
        onkeyup: false,
        onfocusout: false,
        focusInvalid: false,
        errorPlacement: function (error) {
            if (toasting)
                return;
            var message = $.trim(error.text());
            if (message) {
                toasting = true;
                layer.msg(message, { icon: 5 }, function () {
                    toasting = false;
                });
            }
        },
        invalidHandler: function () {
        },
        success: function () {
        }
    };

    $.validator.setDefaults($.validator.unobtrusive.options);
})(jQuery,layui)