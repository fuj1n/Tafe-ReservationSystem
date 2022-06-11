$(() => {
    const model = window.model;
    
    const tableElements = $('[data-table-id]');
    
    function updateTable(id, change) {
        model.tables = model.tables.map(table => {
            if (table.id === id) {
                return {
                    ...table,
                    ...change
                };
            }
            return table;
        });
    }
    
    tableElements.each(function () {
        const self = $(this);
        const id = self.data('table-id');
        const table = model.tables.find(table => table.id === id);
        if(table.isAssigned) {
            self.children('[name=background]').addClass('assigned');
        }
    });
    
    tableElements.click(function () {
        const self = $(this);
        const id = self.data('table-id');
        const table = model.tables.find(table => table.id === id);
        const isAssigned = !table.isAssigned;
        updateTable(id, { isAssigned });
        self.children('[name=background]').toggleClass('assigned');
    });
    
    $('#modal-submit').on('submit', function (e) {
        const form = $(this);
        
        for(let i = 0; i < model.tables.length; i++) {
            const table = model.tables[i];
            form.append(`<input type="hidden" name="Tables[${i}].Id" value="${table.id}">`);
            form.append(`<input type="hidden" name="Tables[${i}].Name" value="${table.name}">`);
            form.append(`<input type="hidden" name="Tables[${i}].IsAssigned" value="${table.isAssigned}">`);
        }
    });
        
});