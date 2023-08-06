function convertCanvasToImage() {
    var image = new Image();
    image.src = canvas.toDataURL('image.png');
    var drawingDataInput = document.getElementById('drawingData');
    drawingDataInput.value = image.src;
    //var downloadLink = document.createElement('a');
    //downloadLink.href = image.src;
    //downloadLink.download = 'drawing.png';
    //downloadLink.click();
}