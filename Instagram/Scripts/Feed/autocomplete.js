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