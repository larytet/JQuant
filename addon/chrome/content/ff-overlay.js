jquant.onFirefoxLoad = function(event) {
  document.getElementById("contentAreaContextMenu")
          .addEventListener("popupshowing", function (e){ jquant.showFirefoxContextMenu(e); }, false);
};

jquant.showFirefoxContextMenu = function(event) {
  // show or hide the menuitem based on what the context menu is on
  document.getElementById("context-jquant").hidden = gContextMenu.onImage;
};

window.addEventListener("load", jquant.onFirefoxLoad, false);
