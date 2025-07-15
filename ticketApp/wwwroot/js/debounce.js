let timer = 0;
function debounce(callback, delayTime) {
    return () => {
        clearTimeout(timer);
        timer = setTimeout(callback, delayTime);
    }
}