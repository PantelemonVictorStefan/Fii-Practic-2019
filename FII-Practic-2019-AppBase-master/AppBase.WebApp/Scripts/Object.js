App.defineClass('Frm.Object', {
    _config: {},

    ctor: function(opt) {
        this._config = opt;
    },

    getClassName: function() {
        return this._className;
    },

    getConfig: function() {
        return this._config;
    },

    callParent: function(args) {
        var prev = arguments.callee.caller.$previous;
        if (App.isFn(prev))
            return prev.apply(this, args);
    },

    _staticCreateConfig: function (opt) {
        return opt;
    }
});
