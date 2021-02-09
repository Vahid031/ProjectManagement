(function() {
    try {
      new CustomEvent("IE has CustomEvent, but doesn't support constructor");
    } catch (e) {

      window.CustomEvent = function(event, params) {
        var evt;
        params = params || {
          bubbles: false,
          cancelable: false,
          detail: undefined
        };
        evt = document.createEvent("CustomEvent");
        evt.initCustomEvent(event, params.bubbles, params.cancelable, params.detail);
        return evt;
      };

      CustomEvent.prototype = Object.create(window.Event.prototype);
    }
}());

(function() {
    // Open menu using button
    var btnMenuOpen = document.querySelector('.section__btn');
    var openButton = document.querySelector('.nav__btn-open');

    if (!btnMenuOpen || !openButton) {
        return;
    }
    
    btnMenuOpen.addEventListener('tap', function(){
        openButton.classList.add('nav__btn-open--hover');
        var click = new CustomEvent('tap');
        setTimeout(function(){
            openButton.dispatchEvent(click);
        }, 300);

        setTimeout(function(){
            openButton.classList.remove('nav__btn-open--hover');
        }, 450);
    });

}());

// Typing effect
typingWord();
function typingWord() {
    var elementWithWord = document.querySelector('[data-type-word]');

    if (!elementWithWord) {
        return;
    }

    var word = elementWithWord.textContent;
    var wordArray = word.split('');
    var i = 0;
    var randomTime;

    elementWithWord.innerHTML = '';

    setTimeout( output, 500 );

    function output(){
        if ( i < wordArray.length )   {
            elementWithWord.innerHTML += wordArray[i];
            randomTime = Math.round( ( Math.random() * 150 ) + 50 );
            setTimeout( output, randomTime );
            i++;

            if ( i === wordArray.length - 1) {
                setTimeout( function() {

                    elementWithWord.nextElementSibling.classList.add('slide-up');
                }, 500);
            }
        }
    }
}

