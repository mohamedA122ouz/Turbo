const dropDown = document.querySelector("#search-dropdown");
const button = document.querySelector("#search-button");
console.log("loaded");

button.onclick = ()=>{
    console.log("clicked");
    dropDown.setAttribute('data-stretching',"true");
    button.setAttribute('data-btnAnimation',"true");
}