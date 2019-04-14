App.defineClass('Frm.Component', {
    extend: 'Frm.Object',

    createRenderer: function() {
        var config = this.getConfig();
        if (config.renderer)
            return;
        var rendererCls = 
            App.getClass(this.getClassName() + 'Renderer');
        if (App.isNullOrUndefined(rendererCls))
            rendererCls = 
                App.getClass('Frm.ComponentRenderer');
        var opt = { component: this, state: config.state };
        var renderer = new rendererCls(opt)
        if (!App.isA(renderer, 'Frm.ComponentRenderer'))
            throw Error('Renderer must inherit from "Frm.ComponentRenderer"'); 
        config.renderer = renderer;
    },
    
    render: function() {
        this.getRenderer().render();
    },
    
    refresh: function() {
        this.getRenderer().refresh();
    },
    
    show: function() {
        this.getRenderer().show();
    },
    
    hide: function() {
        this.getRenderer().hide();
    },
    
    append: function(cmp) {
        this.getRenderer().append(cmp);
    },
    
    remove: function(cmp) {
        this.getRenderer().remove(cmp);
    },
    
    getRenderer: function() {
        return this.getConfig().renderer;
    },
    
    getState: function() {
        return this.getConfig().state;
    },

    _staticCreateConfig: function(opt) {
        opt = this.callParent(arguments);
        opt = App.defaults(opt, {
            renderer: null,
            state: {}
        });
        return opt;
    }
});
