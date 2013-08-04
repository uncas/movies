(function(ko, datacontext) {
    datacontext.Movie = movie;

    function movie(data) {
        var self = this;
        data = data || {};

        self.movieId = data.movieId;
        self.title = data.title;
        self.imdbUrl = data.imdbUrl;
        self.showUrl = data.showUrl;
        self.cinemaUrl = data.cinemaUrl;
        self.imdbRating = "IMDB: " + data.imdbRating;
        self.showTime = data.showTime;
        self.showLocation = data.showLocation;
        self.movieUrl = data.movieUrl;
        self.director = "Director: " + data.director;
        self.rated = "Rated: " + data.rated;
        self.genre = "Genre: " + data.genre;

        self.toJson = function() { return ko.toJSON(self); };
    }

})(ko, moviesApp.datacontext);