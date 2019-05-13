'use strict';

angular.module('loginPage', ['ngRoute', 'ngCookies'])
  .component('loginPage', {
    templateUrl: 'login/login.html',
    controller: ['$http','$cookies', '$window', '$rootScope', function loginPageController($http, $cookies, $window, $rootScope) {
      var self = this;
      self.cookies = $cookies;
      self.checkXSessionAndRedirect = function getSession()
      {
        $rootScope.x_session = $cookies.get('x-session');
        if($rootScope.x_session != null) {
          $window.location.href = "/";
        }
      }
      self.sendLogin = function sendLogin(username, password)
      {
        const requestOptions = {
            headers: { 'Content-Type': 'application/json;charset=uft-8' }
        };
        const data = "username=" + encodeURIComponent(username) + "&password=" + encodeURIComponent(password);
        $http.post('http://localhost:8080/twitterapi/login/', data, requestOptions).then(function (response) {
          $cookies.put('x-session', response.data.Session);
          self.checkXSessionAndRedirect();
        });
      }
      self.checkXSessionAndRedirect();
    }]
});