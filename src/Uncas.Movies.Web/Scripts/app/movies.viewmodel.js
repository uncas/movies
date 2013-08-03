window.moviesApp.moviesViewModel = (function(ko, datacontext) {
    /// <field name="movies" value="[new datacontext.movies()]"></field>
    var defaultRating = 7;
    var movies = ko.observableArray();
    var error = ko.observable();
    var optionRating = ko.observable(defaultRating);
    var optionDay = ko.observable(0);

    var search = function () {
        datacontext.getMovies(movies, error, optionRating(), optionDay());
    };

    var selectRating = function (rating) {
        optionRating(rating);
        search();
    };

    var selectDay = function (day) {
        optionDay(day);
        search();
    };

    search();

    return {
        movies: movies,
        error: error,
        optionRating: optionRating,
        selectRating: selectRating,
        optionDay: optionDay,
        selectDay: selectDay
    };

})(ko, moviesApp.datacontext);

// Initiate the Knockout bindings
ko.applyBindings(window.moviesApp.moviesViewModel);