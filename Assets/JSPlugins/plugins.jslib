mergeInto(LibraryManager.library, {
    GetWebBackgroundColor: function() {
        var returnStr = window.getComputedStyle(document.body).getPropertyValue('--game-background-color');
        var bufferSize = lengthBytesUTF8(returnStr) + 1;
        var buffer = _malloc(bufferSize);
        stringToUTF8(returnStr, buffer, bufferSize);
        return buffer;
    },
});