mergeInto(LibraryManager.library, {
    GetWebBackgroundColor: function() {
        var returnStr = window.getComputedStyle(document.body).backgroundColor;
        var bufferSize = lengthBytesUTF8(returnStr) + 1;
        var buffer = _malloc(bufferSize);
        stringToUTF8(returnStr, buffer, bufferSize);
        return buffer;
    },
});