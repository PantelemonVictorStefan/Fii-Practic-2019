App.defineClass('Frm.FieldComponentRenderer', {
    extend: 'Frm.ComponentRenderer',
    
    _generateEl: function(state) {
        var config = this.getConfig();       
        var el = App.createElement('div');
        el.classList.add('field');
        config.label = App.createElement('label');
        el.append(config.label);
        return el;
    },
    
    _refreshEl: function(el, state) {
        this.callParent(arguments);
        var config = this.getConfig();
        var cmpConfig = config.component.getConfig();
        var labelText = cmpConfig.labelText;
        if (cmpConfig.isMandatory)
            labelText += '*';
        config.label.innerText = labelText;
    },
    
    markAsValid: function(isValid) {
        var classList = this.getEl().classList;
        classList.remove('invalid');
        if (!isValid)
            classList.add('invalid');
    },
    
    _staticCreateConfig: function(opt) {
        opt = this.callParent(arguments);
        opt = App.defaults(opt, {
            label: null
        });
        return opt;
    }
});
