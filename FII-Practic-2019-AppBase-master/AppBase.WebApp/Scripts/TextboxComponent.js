App.defineClass('Frm.TextboxComponent', {
    extend: 'Frm.FieldComponent',
    
    getMaxLength: function() {
        return this.getConfig().maxLength;
    },
    
    validate: function() {
        if (this.getIsMandatory())
            if (App.isNullOrUndefined(this.getValue()) || App.isEmptyString(this.getValue()))
                return false;
        if (!App.isNullOrUndefined(this.getMaxLength()) && this.getValue().length > this.getMaxLength())
            return false;       
        return true;
    },

    _staticCreateConfig: function(opt) {
        opt = this.callParent(arguments);
        opt = App.defaults(opt, {
            maxLength: 120
        });
        return opt;
    }
});
