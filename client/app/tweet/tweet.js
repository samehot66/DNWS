'use strict';

angular.module('tweetList', ['ngRoute'])
  .component('tweetList', {
    templateUrl: 'tweet/tweet.html',
    controller: ['$http', '$rootScope', function TweetListController($http, $rootScope) {
      var self = this;

      const requestOptions = {
          headers: { 'X-session': $rootScope.x_session, 'Content-Type': 'application/json;charset=uft-8'}
      };

      $http.get('http://localhost:8080/twitterapi/tweet/', requestOptions).then(function (response) {
        self.tweets = response.data;
        self.tweets.forEach(function iterator(value, index, collection) {
          value.Message = decodeURIComponent(value.Message);
        });
      });
    }]
});