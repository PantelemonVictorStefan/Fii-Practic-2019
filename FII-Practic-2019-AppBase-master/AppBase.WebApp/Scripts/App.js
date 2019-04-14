(function(base) {
    base.App = base.App || {
        _classDefinitions: {},
        _objectClsName: 'Frm.Object',

        defineClass: function(name, body) {
            if (this._classDefinitions[name])
                throw Error('Class "' + name + '" is already defined');
            this._classDefinitions[name] = body;
            if (name != this._objectClsName && App.isEmptyString(body.extend))
                body.extend = this._objectClsName;
        },

        createClass: function(name, opt) {
            var cls = App.getClass(name);
            if (App.isNullOrUndefined(cls))
                throw Error('Class "' + name + '" cannot be found');
            return new cls(opt);
        },

        init: function() {
            this._initClass(this._objectClsName);
            while (Object.keys(this._classDefinitions).length) {
                App.forEach(this._classDefinitions, 
                    function(name, body) {
                        var cls = App.getClass(body.extend);
                        if (cls)
                            this._initClass(name);
                    }, this);
            }
        },

        isFn: function(obj) {
            return typeof(obj) == 'function';
        },

        forEach: function(obj, fn, scope) {
            if (App.isFn(fn))
                for (var param in obj)
                    if (fn.call(scope || base, param, obj[param]) == true)
                        break;
        },

        isEmptyString: function(str) {
            return typeof(str) != 'string' || str.length == 0;
        },

        isNullOrUndefined: function(obj) {
            return obj == undefined || obj == null;
        },

        getClass: function(name) {
            var clsBase = base;
            var namespace = name.split('.');
            var clsName = namespace[namespace.length - 1];
            if (namespace.length > 1)
                App.forEach(namespace,
                    function(idx, name) {
                        if (idx == namespace.length - 1)
                            return true;
                        clsBase = clsBase[name];
                    });
            return clsBase[clsName];
        },

        _initClass: function(name) {
            var clsDef = this._classDefinitions[name];
            var cls = function(opt) {
                this._className = name;
                if (App.isFn(this.ctor))
                    this.ctor(this._staticCreateConfig(opt));
            };
            if (App.isEmptyString(clsDef.extend) || name == this._objectClsName)
                cls.prototype = new (function() {})();
            else {
                var baseCls = App.getClass(clsDef.extend);
                if (App.isNullOrUndefined(baseCls))
                    throw Error('Base class "' + clsDef.extend + '" cannot be found');
                var f = function() {};
                f.prototype = baseCls.prototype;
                cls.prototype = new f();
                cls.superPrototype = baseCls.prototype;
            }
            App.forEach(clsDef,
                function(memberName, member) {
                    var proto = cls.prototype;
                    if (App.isFn(proto[memberName]) && App.isFn(member)) {
                        var previousFn = proto[memberName];
                        proto[memberName] = member;
                        proto[memberName].$previous = previousFn;
                    } else
                        proto[memberName] = member;
                });
            var clsBase = base;
            var namespace = name.split('.');
            var clsName = namespace[namespace.length - 1];
            if (namespace.length > 1)
                App.forEach(namespace,
                    function(idx, name) {
                        if (idx == namespace.length - 1)
                            return true;
                        clsBase = (clsBase[name] = clsBase[name] || {});
                    });
            clsBase[clsName] = cls;
            if (clsDef.singleton === true)
                clsBase[clsName] = new clsBase[clsName]();
            delete this._classDefinitions[name];
        },

        defaults: function(opt, dic) {
            opt = opt || {};
            App.forEach(dic, function(name, val) {
                if (App.isNullOrUndefined(opt[name]))
                    opt[name] = val;
            });
            return opt;
        },

        override: function(opt, dic) {
            opt = opt || {};
            App.forEach(dic, function(name, val) {
                opt[name] = val;
            });
            return opt;
        },
        
        isA: function(inst, cls) {
            return inst instanceof (typeof(cls) == 'string' ? App.getClass(cls) : cls);
        },
        
        createElement: function(tag) {
            return document.createElement(tag);
        }
    };
})(window);
