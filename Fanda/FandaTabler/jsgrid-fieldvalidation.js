// jsgrid - field - validation:

validate: {
    validator: function (value, item) {
        if (value == undefined || value == null || value == "")
            return false;
        var gridData = $("#jsGrid").jsGrid("option", "data");
        var editRow = $("#jsGrid").jsGrid("option", "_editingRow");
        var editItem = undefined;
        if (editRow != null)
            editItem = editRow.data("JSGridItem");
        //console.log("editingItem", editItem, item);
        //debugger;
        for (i = 0; i < gridData.length; i++) {
            if (editItem == undefined) {
                if (value.toLowerCase() == gridData[i].name.toLowerCase())
                    return false;
            }
            else {
                if (editItem.id == gridData[i].id)
                    continue;
                if (value.toLowerCase() == gridData[i].name.toLowerCase())
                    return false;
            }
        }
        //clearError();
        return true;
    },
    message: function (value, item) {
        if (value == undefined || value == null || value == "")
            return "is required";
        else
            return "'" + value + "' already exists";
    },
    param: undefined
}