define('common', ['jquery'], {
    findElementByCid: function (e, cid) {
        if (typeof e == 'undefined')
            throw new TypeError("e cannot be null");

        if (typeof cid != 'string')
            throw new TypeError("cid must be type of string");

        return e.find('[data-cid=' + cid + ']');
    }
});