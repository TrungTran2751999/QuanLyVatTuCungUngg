//lam chuc nang sticky header table va sort tung cot
{
    var $table = $('#table-data')

    function buildTable($el, cells, rows) {
        var i
        var j
        var row
        var columns = []
        var data = []

        for (i = 0; i < cells; i++) {
            columns.push({
                field: 'field' + i,
                title: 'Cell' + i,
                sortable: true
            })
        }
        for (i = 0; i < rows; i++) {
            row = {}
            for (j = 0; j < cells; j++) {
                row['field' + j] = 'Row-' + i + '-' + j
            }
            data.push(row)
        }
        console.log(data)
        var classes = $('.toolbar input:checked').next().text()

        $el.bootstrapTable('destroy').bootstrapTable({
            columns: columns,
            data: data,
            showFullscreen: true,
            search: true,
            stickyHeader: true,
            stickyHeaderOffsetLeft: parseInt($('body').css('padding-left'), 10),
            stickyHeaderOffsetRight: parseInt($('body').css('padding-right'), 10),
            theadClasses: classes
        })
    }

    $(function () {
        $('.toolbar input').change(function () {
            buildTable($table, 20, 50)
        })
        buildTable($table, 20, 50)
    })
}