window.moviesApp.moviesViewModel = (function(ko, datacontext) {
    /// <field name="movies" value="[new datacontext.movies()]"></field>
    var movies = ko.observableArray();
    var error = ko.observable();

    datacontext.getMovies(movies, error); // load movies

    return {
        movies: movies,
        error: error
    };

})(ko, moviesApp.datacontext);

// Initiate the Knockout bindings
ko.applyBindings(window.moviesApp.moviesViewModel);