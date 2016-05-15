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

Dropzone.options.photo = { // The camelized version of the ID of the form element

    // The configuration we've talked about above
    //paramName: "inputFiles",
    autoDiscover: false,
    autoProcessQueue: false,
    uploadMultiple: true,
    parallelUploads: 100,
    maxFiles: 100,
    dictDefaultMessage: "Bạn có thể kéo ảnh hoặc click để chọn",
    previewsContainer: "#photo > .modal-body",

    // The setting up of the dropzone
    init: function () {
        var myDropzone = this;

        // First change the button to actually tell Dropzone to process the queue.
        this.element.querySelector("button[type=submit]").addEventListener("click", function (e) {
            // Make sure that the form isn't actually being sent.
            e.preventDefault();
            e.stopPropagation();
            myDropzone.processQueue();
        });

        // Listen to the sendingmultiple event. In this case, it's the sendingmultiple event instead
        // of the sending event because uploadMultiple is set to true.
        this.on("sendingmultiple", function () {
            // Gets triggered when the form is actually being sent.
            // Hide the success button or the complete form.
        });
        this.on("successmultiple", function (files, response) {
            // Gets triggered when the files have successfully been sent.
            // Redirect user or notify of success.
        });
        this.on("errormultiple", function (files, response) {
            // Gets triggered when there was an error sending the files.
            // Maybe show form again, and notify user of error
        });
    }

}

//var numbers = new Bloodhound({
//    datumTokenizer: function (d) { return Bloodhound.tokenizers.whitespace(d.num); },
//    queryTokenizer: Bloodhound.tokenizers.whitespace,
//    local: [
//    { num: 'one' },
//    { num: 'two' },
//    { num: 'three' },
//    { num: 'four' },
//    { num: 'five' },
//    { num: 'six' },
//    { num: 'seven' },
//    { num: 'eight' },
//    { num: 'nine' },
//    { num: 'ten' }
//    ]
//});

//// initialize the bloodhound suggestion engine
//numbers.initialize();

//// instantiate the typeahead UI
//$('#bloodhound .typeahead').typeahead(null, {
//    displayKey: 'num',
//    source: numbers.ttAdapter()
//});



$(function () {
    var usernames = new Bloodhound({
        datumTokenizer: function (datum) {
            return Bloodhound.tokenizers.whitespace(datum.tokens);
        },
        queryTokenizer: Bloodhound.tokenizers.whitespace,
        limit: 5,
        remote: {
            url: "/home/autocomplete/",
            replace: function (url, query) {
                return url + "?searchTerm=" + query;
            },
            filter: function (usernames) {
                return $.map(usernames, function (data) {
                    return {
                        id: data.UserId,
                        symbol: data.Avartar,
                        name: data.FullName,
                        username: data.UserName
                    }
                });
            }
        }
    });
    usernames.initialize();
    $('#bloodhound .typeahead').typeahead(null, {
        name: 'usernames',
        displayKey: 'name',
        minLength: 1, // send AJAX request only after user type in at least X characters
        source: usernames.ttAdapter(),
        templates: {
            suggestion: function (data) {
                return '<p>' + '<img src="' + data.symbol + '" class="img-responsive user-avatar" alt="Avatar" />' + data.name + '</p>';
            }
        }
    }).on('typeahead:selected', function (obj, stock) {
        window.location.href = "/profile/detail/" + stock.username;
    });
});