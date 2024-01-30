// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.
function startAutoLogout() {
    // setup timeout that will occur when session has expired (20 minutes = 60000 * 20 milliseconds)
    window.setTimeout(() => document.location = "/Login", 450000);
    // setup timeout that will occur when session about to expire to warn user (18 minutes = 60000 * 18 milliseconds)
    window.setTimeout(() => document.getElementById("lblExpire").innerHTML = "WARNING : Session is about to expire!", 300000);
    context.Session.SetString("auth", "false");
    context.Session.SetString("user", _username);
}
// Write your JavaScript code.
