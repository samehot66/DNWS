'use strict';

angular.module('homeList', ['ngRoute'])
  .component('homeList', {
    templateUrl: 'home/home.html',
    controller: ['$http', '$rootScope', function FollowingListController($http, $rootScope) {
      var self = this;

      self.sendTweet = function sendTweet(message) {
        const requestOptions = {
            headers: { 'X-session': $rootScope.x_session, 'Content-Type': 'application/json;charset=uft-8' }
        };
        var data ="message=" + encodeURIComponent(message);
        $http.post('http://localhost:8080/twitterapi/tweet/', data, requestOptions).then(function (response) {
          $http.get('http://localhost:8080/twitterapi/', requestOptions).then(function (response) {
            self.tweets = response.data;
            self.tweets.forEach(function iterator(value, index, collection) {
              value.Message = decodeURIComponent(value.Message);
            });
          });
        });
      }

      self.getFollowingTimeline = function getFollowingTimeline() {
        const requestOptions = {
            headers: { 'X-session': $rootScope.x_session, 'Content-Type': 'application/json;charset=uft-8'}
        };

        $http.get('http://localhost:8080/twitterapi/', requestOptions).then(function (response) {
          self.tweets = response.data;
          self.tweets.forEach(function iterator(value, index, collection) {
            value.Message = decodeURIComponent(value.Message);
          });
        });
      }

      self.getFollowingTimeline();

    }]
});