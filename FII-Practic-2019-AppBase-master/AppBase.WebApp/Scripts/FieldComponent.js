App.defineClass('Frm.FieldComponent', {
    extend: 'Frm.Component',
    
    render: function() {
        this.callParent(arguments);
        this.refresh();
    },
    
    refresh: function() {
        this.callParent(arguments);
        this.getRenderer().markAsValid(this.getConfig().isValid = this.validate());
    },
    
    getValue: function() {
        return this.getConfig().value;
    },
    
    setValue: function(val) {
        this.getConfig().value = val;
        this.refresh();
    },
    
    getLabelText: function() {
        return this.getConfig().labelText;
    },
    
    setLabelText: function(labelText) {
        this.getConfig().labelText = labelText;
        this.refresh();
    },
    
    getIsMandatory: function() {
        return this.getConfig().isMandatory;
    },
    
    setIsMandatory: function(val) {
        this.getConfig().isMandatory = val;
        this.refresh();
    },
    
    validate: function() {
        return !this.getIsMandatory() || !App.isNullOrUndefined(this.getValue());
    },
    
    isValid: function() {
        return this.getConfig().isValid;
    },

    _staticCreateConfig: function(opt) {
        opt = this.callParent(arguments);
        opt = App.defaults(opt, {
            value: null,
            labelText: '',
            isMandatory: false,
            isValid: true
        });
        return opt;
    }
});
