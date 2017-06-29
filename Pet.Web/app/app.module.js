﻿(function () {
    'use strict';

    angular
        .module('getAPet', ['ngRoute', 'ngMaterial'])
        .factory('httpRequestInterceptor', function () {
            return {
                request: function (config) {
                    return config;
                }
            };
        })
        .config(function ($routeProvider, $httpProvider, $mdIconProvider) {
            $httpProvider.interceptors.push('httpRequestInterceptor');
            $routeProvider
                .when("/home", {
                    templateUrl: window.location.origin + "/Templates/Home",
                    controller: "HomeController",
                    controllerAs: "homeController"
                })
                .when("/mypets", {
                    templateUrl: window.location.origin + "/Templates/MyPets",
                    controller: "MyPetsController",
                    controllerAs: "myPetsController"
                })
                .when("/pet-save", {
                    templateUrl: window.location.origin + "/Templates/PetSave",
                    controller: "PetSaveController",
                    controllerAs: "petSaveController"
                })
                .when("/pet-edit/:petId", {
                    templateUrl: window.location.origin + "/Templates/PetEdit",
                    controller: "PetEditController",
                    controllerAs: "petEditController"
                })
                .when("/profile", {
                    templateUrl: window.location.origin + "/Templates/UserDetails",
                    controller: "UserDetailsController",
                    controllerAs:"userDetailsController"
                })
                .when("/userconversations", {
                    templateUrl: window.location.origin + "/Templates/UserConversations",
                    controller: "conversationsController",
                    controllerAs:"conversationsController"
                })
                .when("/userconversation/:conversationId" , {
                    templateUrl: window.location.origin + "/Templates/Conversation",
                    controller: "conversationController",
                    controllerAs: "conversationController"
                })
                .when("/petProfile", {
                    templateUrl: window.location.origin + "/Templates/PetViewProfile",
                    controller: "HomeController",
                    controllerAs: "homeController"
                })
                .otherwise({ redirectTo: "/home" });
            $mdIconProvider
                .iconSet('communication', 'img/icons/sets/communication-icons.svg', 24);
        });
})();