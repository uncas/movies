window.moviesApp = window.moviesApp || {};

window.moviesApp.datacontext = (function() {

    var datacontext = {
        getMovies: getMovies
    };

    return datacontext;

    function getMovies(moviesObservable, errorObservable, rating) {
        return ajaxRequest("get", moviesUrl(rating))
            .done(getSucceeded)
            .fail(getFailed);

        function getSucceeded(data) {
            var mappedMovies = $.map(data, function(list) { return new createMovie(list); });
            moviesObservable(mappedMovies);
        }

        function getFailed() {
            errorObservable("Error retrieving movies.");
        }
    }

    function createMovie(data) {
        return new datacontext.Movie(data); // movie is injected by movies.model.js
    }

    // Private

    function ajaxRequest(type, url, data, dataType) { // Ajax helper
        var options = {
            dataType: dataType || "json",
            contentType: "application/json",
            cache: false,
            type: type,
            data: data ? data.toJson() : null
        };
        var antiForgeryToken = $("#antiForgeryToken").val();
        if (antiForgeryToken) {
            options.headers = {
                'RequestVerificationToken': antiForgeryToken
            };
        }
        return $.ajax(url, options);
    }

    // routes

    function moviesUrl(rating) { return "/api/movie/?rating=" + rating; }

})();