﻿(function(ko, datacontext) {
    datacontext.Movie = movie;

    function movie(data) {
        var self = this;
        data = data || {};

        self.movieId = data.movieId;
        self.title = data.title;
        self.imdbUrl = data.imdbUrl;
        self.showUrl = data.showUrl;

        self.toJson = function() { return ko.toJSON(self); };
    }

})(ko, moviesApp.datacontext);