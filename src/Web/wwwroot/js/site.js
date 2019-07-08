// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

//Pace.js impide que signalR trabaje correctamente, esto soluciona el problema
window.paceOptions = {
    ajax: false,
    restarOnRequestAfter: false
};