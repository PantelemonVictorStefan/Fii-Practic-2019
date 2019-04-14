App.defineClass('Frm.TextboxComponentRenderer', {
    extend: 'Frm.FieldComponentRenderer',
    
    _generateEl: function(state) {
        var el = this.callParent(arguments);
        var config = this.getConfig();
        config.input = App.createElement('input');
        config.input.type = 'text';
        config.input.onblur = function() {
            config.component.setValue(config.input.value);
        };
        el.append(config.input);
        return el;
    },
    
    _refreshEl: function(el, state) {
        this.callParent(arguments);
        var config = this.getConfig();
        config.input.value = config.component.getValue();
    },

    _staticCreateConfig: function(opt) {
        opt = this.callParent(arguments);
        opt = App.defaults(opt, {
            input: null
        });
        return opt;
    }
});
