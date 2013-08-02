window.moviesApp.moviesViewModel = (function(ko, datacontext) {
    /// <field name="movies" value="[new datacontext.movies()]"></field>
    var defaultRating = 7;
    var movies = ko.observableArray();
    var error = ko.observable();
    var optionRating = ko.observable(defaultRating);

    var search = function () {
        datacontext.getMovies(movies, error, optionRating());
    };

    var chooseRating = function (rating) {
        optionRating(rating);
        search();
    };

    search();

    return {
        movies: movies,
        error: error,
        optionRating: optionRating,
        chooseRating: chooseRating
    };

})(ko, moviesApp.datacontext);

// Initiate the Knockout bindings
ko.applyBindings(window.moviesApp.moviesViewModel);