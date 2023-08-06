// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
var canvas = document.getElementById("sketchpad");
var context = canvas.getContext("2d");

var isDrawing = false;
var lastX = 0;
var lastY = 0;

canvas.addEventListener("mousedown", function (e) {
    isDrawing = true;
    lastX = e.clientX - canvas.offsetLeft;
    lastY = e.clientY;
    console.log('MouseDown', lastX, lastY);
});

canvas.addEventListener("mousemove", function (e) {
    if (isDrawing) {
        var currentX = e.clientX - canvas.offsetLeft;
        var currentY = e.clientY - canvas.offsetTop;

        context.beginPath();
        context.moveTo(lastX, lastY);
        context.lineTo(currentX, currentY);
        context.strokeStyle = "black";
        context.lineWidth = 5;
        context.stroke();

        lastX = currentX;
        lastY = currentY;
    }
    console.log('MouseDown', lastX, lastY);
});

canvas.addEventListener("mouseup", function () {
    isDrawing = false;
});