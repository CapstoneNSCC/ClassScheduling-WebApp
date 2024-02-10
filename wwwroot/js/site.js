// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
//login code functionality


let usernameLength = 0;
let passwordLength = 0;
let loginBtn = document.getElementById("loginBtn");

function onLoginUsernameChange(e) {
  usernameLength = e;
  checkLoginDisabled();
}

function onLoginPasswordChange(e) {
  passwordLength = e;
  checkLoginDisabled();
}

function checkLoginDisabled() {
  if (usernameLength > 0 && passwordLength > 0) {
    loginBtn.disabled = false;
  } else {
    loginBtn.disabled = true;
  }
}