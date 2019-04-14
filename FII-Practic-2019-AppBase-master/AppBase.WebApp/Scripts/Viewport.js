App.defineClass('Frm.Viewport', {
    extend: 'Frm.Object',
    singleton: true,
    component: null,
    
    setComponent: function(cmp) {
        if (App.isNullOrUndefined(cmp))
            return;
        cmp.createRenderer();
        cmp.render();
        this.component = cmp;
        document.body.append(this.component.getRenderer().getEl());
    },
    
    clear: function() {
        document.body.remove(this.component.getRenderer().getEl());
    }
});
