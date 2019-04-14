App.defineClass('Frm.ComponentRenderer', {
    extend: 'Frm.Object',
    
    _generateEl: function(state) {
        return App.createElement('div');
    },
    
    _refreshEl: function(el, state) {
        el.style.display = state.isVisible ? '' : 'none';
    },

    render: function() {
        if (this.isRendered())
            return;
        var config = this.getConfig();
        var state = this.getState();
        config.el = this._generateEl(state);
        this._refreshEl(config.el, state);
        config.isRendered = true;
    },
    
    refresh: function() {
        this._refreshEl(this.getEl(), this.getState());
    },
    
    show: function() {
        var state = this.getState();
        state.isVisible = true;
        this._refreshEl(this.getEl(), state);
    },
    
    hide: function() {
        var state = this.getState();
        state.isVisible = false;
        this._refreshEl(this.getEl(), state);
    },
    
    append: function(cmp) {
        if (App.isNullOrUndefined(cmp))
            return;
        cmp.createRenderer();
        cmp.render();
        this.getEl().append(cmp.getRenderer().getEl());
    },
    
    remove: function(cmp) {
        if (App.isNullOrUndefined(cmp))
            return;
        var renderer = 
            cmp.getRenderer();
        if (App.isNullOrUndefined(renderer) || !renderer.isRendered())
            return;
        this.getEl().remove(renderer.getEl());
    },
    
    getComponent: function() {
        return this.getConfig().component;
    },
    
    getState: function() {
        return this.getConfig().state;
    },

    isRendered: function() {
        return this.getConfig().isRendered;
    },
    
    getEl: function() {
        return this.getConfig().el;
    },
    
    isVisible: function() {
        return this.getState().isVisible;
    },

    _staticCreateConfig: function(opt) {
        opt = this.callParent(arguments);
        opt = App.defaults(opt, {
            component: null,
            state: {},
            isRendered: false,
            el: null
        });
        opt.state = App.defaults(opt.state, {
            isVisible: true
        });
        return opt;
    }
});
