var page = 0,
    inCallback = false,
    hasReachedEndOfInfiniteScroll = false;

var scrollHandler = function () {
    if (hasReachedEndOfInfiniteScroll == false &&
            ($(window).scrollTop() == $(document).height() - $(window).height())) {
        loadMoreToInfiniteScrollTable(moreRowsUrl);
    }
}

function loadMoreToInfiniteScrollTable(loadMoreRowsUrl) {
    if (page > -1 && !inCallback) {
        inCallback = true;
        page++;
        $("div#loading").show();
        $.ajax({
            type: 'GET',
            url: loadMoreRowsUrl,
            data: "pageIndex=" + page,
            success: function (data, textstatus) {
                if (data != '') {
                    $("#newFeedsList").append(data);
                }
                else {
                    page = -1;
                }

                inCallback = false;
                $("div#loading").hide();
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
            }
        });
    }
}

function showNoMoreRecords() {
    hasReachedEndOfInfiniteScroll = true;
}

function clearInput(inputId) {
    $('#' + inputId).val('');
}

function comment(inputId) {
    $('#' + inputId).focus();
}

var itemToDelete;
$(document).on('click', '.close', function () {
    itemToDelete = $(this).data("id");
})

$("#deleteComment").click(function () {
    $.post("/Home/DeleteComment", { commentId: itemToDelete }, function (data) {
        if (data) {
            $("#commentBox" + itemToDelete).remove();
        }
    })
})

$(function () {
    $('body').on('click', '.modal-link', function (e) {
        e.preventDefault();
        $(this).attr('data-target', '#modal-container');
        $(this).attr('data-toggle', 'modal');
    });

    $('body').on('click', '.modal-close-btn', function () {
        $('#modal-container').modal('hide');
    });

    $('#modal-container').on('hidden.bs.modal', function () {
        $(this).removeData('bs.modal');
    });
});

$('*[data-poload]').hover(function () {
    var e = $(this);
    e.off('hover');
    $.get(e.data('poload'), function (d) {
        e.popover({

            content: d,
            html: true,
            container: 'body'
        }).popover('show');
    });
});

$('body').on('click', function (e) {
    //only buttons
    if ($(e.target).data('toggle') !== 'popover'
        && $(e.target).parents('.popover.in').length === 0) {
        $('*[data-poload]').popover('hide');
    }
});

$(document).ready($(function () {
    var states = new Bloodhound({
        datumTokenizer: Bloodhound.tokenizers.whitespace,
        queryTokenizer: Bloodhound.tokenizers.whitespace,
        // `states` is an array of state names defined in "The Basics"
        local: states
    });

    $('.typeahead').typeahead({
        name: 'name',
        local: ['yasser', 'shyam', 'sujesh', 'siddhesh', 'vaibhav']
    });
}));