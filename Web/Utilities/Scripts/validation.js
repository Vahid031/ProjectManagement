function HandleValidation() {

    $(".numeric").each(function () {

        $(this).bind("keyup", function () {
            var _this = $(this);
            _this.val(_this.val().replace(/\D/g, ''));
            //_this.val(_this.val().replace(/[^0-9]+\./, ''));
        });
    });

    $(".persian").each(function () {

        $(this).bind("keyup", function () {
            var _this = $(this);
            _this.val(_this.val().replace(/[^\u0600-\u06FF_ ]*$/, ''));
        });
    });

    $(".persian_numerical").each(function () {

        $(this).bind("keyup", function () {
            var _this = $(this);
            _this.val(_this.val().replace(/[^\u0600-\u06FF_ 0-9]*$/, ''));
        });
    });

}
