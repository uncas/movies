window.moviesApp.moviesViewModel = (function(ko, datacontext) {
    /// <field name="movies" value="[new datacontext.movies()]"></field>
    var movies = ko.observableArray();
    var error = ko.observable();
    var search = function() {
        datacontext.getMovies(movies, error); // load movies
    };
    var optionRating = 5;
    search();

    return {
        movies: movies,
        error: error,
        search: search,
        optionRating: optionRating
    };

})(ko, moviesApp.datacontext);

// Initiate the Knockout bindings
ko.applyBindings(window.moviesApp.moviesViewModel);