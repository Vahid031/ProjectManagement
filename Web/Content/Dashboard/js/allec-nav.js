

/* ========================================================================preloader-overlay preloader-overlay--show
 * Allec Portfolio Menu: v 1.0.0
 * ========================================================================
 * Copyright 2015 Congruity Hub
 * http://themeforest.net/user/congruityhub
 * ======================================================================== */

var global = (function(global) {
    global.findParent = function(element, parentConstructor) {
        while (!(element instanceof parentConstructor)) {
            element = element.parentElement;
            if (element == document.body) {
                return null;
            }
        }
        return element;
    };

    //  polyfill for classList.remove and classList.add. 
    if (!("classList" in document.documentElement) && Object.defineProperty && typeof HTMLElement !== 'undefined') {
        Object.defineProperty(HTMLElement.prototype, 'classList', {
            get: function() {
                var self = this;

                function update(fn) {
                    return function(value) {
                        var classes = self.className.split(/\s+/),
                            index = classes.indexOf(value);

                        fn(classes, index, value);
                        self.className = classes.join(" ");
                    };
                }

                var ret = {
                    add: update(function(classes, index, value) {
                        ~index || classes.push(value);
                    }),

                    remove: update(function(classes, index) {
                        ~index && classes.splice(index, 1);
                    }),

                    toggle: update(function(classes, index, value) {
                        ~index ? classes.splice(index, 1) : classes.push(value);
                    }),

                    contains: function(value) {
                        return !!~self.className.split(/\s+/).indexOf(value);
                    },

                    item: function(i) {
                        return self.className.split(/\s+/)[i] || null;
                    }
                };

                Object.defineProperty(ret, 'length', {
                    get: function() {
                        return self.className.split(/\s+/).length;
                    }
                });

                return ret;
            }
        });
    }

    // polyfill for requestAnimationFrame.
    var lastTime = 0;
    var vendors = ['ms', 'moz', 'webkit', 'o'];
    for(var x = 0; x < vendors.length && !window.requestAnimationFrame; ++x) {
        window.requestAnimationFrame = window[vendors[x]+'RequestAnimationFrame'];
        window.cancelAnimationFrame = window[vendors[x]+'CancelAnimationFrame'] 
                                   || window[vendors[x]+'CancelRequestAnimationFrame'];
    }
 
    if (!window.requestAnimationFrame)
        window.requestAnimationFrame = function(callback, element) {
            var currTime = new Date().getTime();
            var timeToCall = Math.max(0, 16 - (currTime - lastTime));
            var id = window.setTimeout(function() { callback(currTime + timeToCall); }, 
              timeToCall);
            lastTime = currTime + timeToCall;
            return id;
        };
 
    if (!window.cancelAnimationFrame)
        window.cancelAnimationFrame = function(id) {
            clearTimeout(id);
        };

    // Adding class for hover states
    if ( !('ontouchstart' in window) ) {
        document.body.classList.add('hover-enable');
    }       

    return global;
}(global || {}));

// *** TAP ***
// to avoid 300 ms click delay on touch devices
// https://github.com/pukhalski/tap/

(function( window ) {
    var Tap = {};

    var utils = {};

    utils.attachEvent = function(element, eventName, callback) {
        if ('addEventListener' in window) {
            return element.addEventListener(eventName, callback, false);
        }
    };

    utils.fireFakeEvent = function(e, eventName) {
        if (document.createEvent) {
            var tap = utils.createEvent(eventName);
            tap.initialEvent = e;
            return e.target.dispatchEvent(tap);
        }
    };

    utils.createEvent = function(name) {
        if (document.createEvent) {
            var evnt = window.document.createEvent('HTMLEvents');

            evnt.initEvent(name, true, true);
            evnt.eventName = name;

            return evnt;
        }
    };

    utils.getRealEvent = function(e) {
        if (e.originalEvent && e.originalEvent.touches && e.originalEvent.touches.length) {
            return e.originalEvent.touches[0];
        } else if (e.touches && e.touches.length) {
            return e.touches[0];
        }

        return e;
    };

    var eventMatrix = [{
        // Touchable devices
        test: ('propertyIsEnumerable' in window || 'hasOwnProperty' in document) && ('ontouchstart' in window || 'ontouchstart' in document),
        events: {
            start: 'touchstart',
            move: 'touchmove',
            end: 'touchend'
        }
    }, {
        // IE10
        test: window.navigator.msPointerEnabled,
        events: {
            start: 'MSPointerDown',
            move: 'MSPointerMove',
            end: 'MSPointerUp'
        }
    }, {
        // Modern device agnostic web
        test: window.navigator.pointerEnabled,
        events: {
            start: 'pointerdown',
            move: 'pointermove',
            end: 'pointerup'
        }
    }];

    Tap.options = {
        eventName: 'tap',
        fingerMaxOffset: 11
    };

    var attachDeviceEvent, init, handlers, deviceEvents,
        coords = {};

    attachDeviceEvent = function(eventName) {
        return utils.attachEvent(document.documentElement, deviceEvents[eventName], handlers[eventName]);
    };

    handlers = {
        start: function(e) {
            e = utils.getRealEvent(e);

            coords.start = [e.pageX, e.pageY];
            coords.offset = [0, 0];
        },

        move: function(e) {
            if (!coords.start && !coords.move) {
                return false;
            }

            e = utils.getRealEvent(e);

            coords.move = [e.pageX, e.pageY];
            coords.offset = [
                Math.abs(coords.move[0] - coords.start[0]),
                Math.abs(coords.move[1] - coords.start[1])
            ];
        },

        end: function(e) {
            e = utils.getRealEvent(e);

            if (coords.offset[0] < Tap.options.fingerMaxOffset && coords.offset[1] < Tap.options.fingerMaxOffset && !utils.fireFakeEvent(e, Tap.options.eventName)) {
                // Windows Phone 8.0 trigger `click` after `pointerup` firing
                // #16 https://github.com/pukhalski/tap/issues/16
                if (window.navigator.msPointerEnabled || window.navigator.pointerEnabled) {
                    var preventDefault = function(clickEvent) {
                        clickEvent.preventDefault();
                        e.target.removeEventListener('click', preventDefault);
                    };

                    e.target.addEventListener('click', preventDefault, false);
                }

                e.preventDefault();
            }

            coords = {};
        },

        click: function(e) {
            if (!utils.fireFakeEvent(e, Tap.options.eventName)) {
                return e.preventDefault();
            }
        },

        emulatedTap: function( e ) {
            if ( coords.offset ) {
                utils.fireFakeEvent( e, Tap.options.eventName );
            }

            return e.preventDefault();
        }
    };

    init = function() {
        var i = 0;

        for (; i < eventMatrix.length; i++) {
            if (eventMatrix[i].test) {
                deviceEvents = eventMatrix[i].events;

                attachDeviceEvent('start');
                attachDeviceEvent('move');
                attachDeviceEvent('end');
                utils.attachEvent(document.documentElement, 'click', handlers['emulatedTap']);

                return false;
            }
        }

        return utils.attachEvent(document.documentElement, 'click', handlers.click);
    };

    utils.attachEvent(window, 'load', init);

    if (typeof define === 'function' && define.amd) {
        define(function() {
            init();

            return Tap;
        });
    } else {
        window.Tap = Tap;
    }

})( window );


global = (function(global) {

    // *** NAVIGATION ***
    var _body = document.body;
    var overlay;

    var Navigation = function(HTMLElement) {
        //  Variables
        Object.defineProperties(this, {
            //  Menu items wrapper
            _wrapper: {
                value: HTMLElement
            }
        });

        overlay = document.createElement('div');
        overlay.classList.add('nav-overlay');
        overlay.classList.add('elem-hide');
        overlay.addEventListener('click', this.close.bind(this));

        overlay.addEventListener('animationend', function() {
            if (_body.classList.contains('nav-close')) {
                overlay.classList.add('elem-hide');
            }

        });
        overlay.addEventListener('webkitAnimationEnd', function() {
            if (_body.classList.contains('nav-close')) {
                overlay.classList.add('elem-hide');
            }
        });

        _body.appendChild(overlay);

    };

    Object.defineProperties(Navigation.prototype, {
        open: {
            value: function() {

                overlay.classList.remove('elem-hide');
                _body.classList.remove('nav-close');
                _body.classList.add('nav-open');

                this._wrapper.classList.add('open');
            },
            enumerable: true
        },
        close: {
            value: function() {

                _body.classList.remove('nav-open');
                _body.classList.add('nav-close');

                this._wrapper.classList.remove('open');
            },
            enumerable: true
        },
        showSub: {
            value: function(target) {

                var activeLink = target;

                if (!activeLink.nextElementSibling) {
                    if (activeLink.target === '_blank') {
                        window.open(activeLink, '_blank');
                    } else {
                        window.location.href = activeLink;
                    }
                    return false;
                }

                var mainParent = global.findParent(activeLink, HTMLUListElement);

                var subListChildren = activeLink.nextElementSibling.children;
                var timeMaxAnimation = new TimelineMax();

                for (var i = 0, max = mainParent.children.length; i < max; i++) {
                    mainParent.children[i].firstElementChild.classList.add('elem-hide');
                }

                activeLink.nextElementSibling.classList.remove('elem-hide');
                activeLink.nextElementSibling.classList.add('elem-show');

                timeMaxAnimation.staggerTo(subListChildren, 0.6, {
                    opacity: 1,
                    ease: Power3.easeInOut
                });

                return true;

            },
            enumerable: true
        },
        hideSub: {
            value: function() {

                var navSubsShow = this._wrapper.querySelectorAll('.elem-show');
                var lastSubsShow = navSubsShow[navSubsShow.length - 1];

                if (!lastSubsShow) {
                    return 0;
                }

                var mainParent = lastSubsShow.parentElement.parentElement;

                for (var i = 0, max = mainParent.children.length; i < max; i++) {
                    mainParent.children[i].firstElementChild.classList.remove('elem-hide');
                }

                lastSubsShow.classList.remove('elem-show');
                lastSubsShow.classList.add('elem-hide');

                for (i = 0, max = lastSubsShow.children.length; i < max; i++) {
                    lastSubsShow.children[i].setAttribute('style', 'opacity: 0');
                }

            },
            enumerable: true
        }
    });

    // *** SCROLLBAR ***
    var frameRequested;
    var autoScrollFrame;
    var newThumbPosition = 0, newOverviewPosition = 0;
    var scrollHideDelay = 500;
    var thumbPosition = 0, overviewPosition = 0;
    var velocity, amplitude, frame = 0, timestamp, ticker, target;

    var Scrollbar = function (HTMLElement, options) {
        //thumb is handle of scrollbar, also known as scrollbox

        Object.defineProperties(this, {
            _element: {
                value: HTMLElement
            },
            _viewPort: {
                value: document.createElement('div')
            },
            _overview: {
                value: document.createElement('div')
            },
            _scrollbarTrack: {
                value: document.createElement('div')
            },
            _thumb: {
                value: document.createElement('div')
            },
            // multiplicator for touch movement (number)
            touchScrollSpeed: {
                value: options.touchScrollSpeed || 1
            },
            //multiplicator for mouse wheel movement (number)
            mouseWheelSpeed: {
                value: options.mouseWheelSpeed || 0.5,
                writable: true
            },
            // if customThumbHeight is defined, thumb height is fixed. If customThumbHeight is undefined, thumb height is calculated depending on other sizes (number)
            customThumbHeight: {
                value: options.customThumbHeight || undefined
            },
            // if true parent element cannot be scrolled while mouse/touch is on the 'scrollable' element (bool)
            preventDefaultScroll: {
                value: options.preventDefaultScroll ||  false
            },
            // if true sizes of scrollbar are recalculated when window is resized (bool)
            _updateOnWindowResize: {
                value: options.updateOnWindowResize || true
            },
            // _measurements will contain all necessary information about sizes and maximum positions
            _measurements: {
                value: {}
            },
            _enabled: {
                value: false,
                writable: true
            }
        });

        //Classes are added for proper work of CSS styles
        this._element.classList.add('scrollable');
        this._viewPort.classList.add('viewport');
        this._overview.classList.add('overview');
        this._scrollbarTrack.classList.add('scrollbar-track');
        this._thumb.classList.add('thumb');

        var fragment = document.createDocumentFragment();
        fragment.appendChild(this._scrollbarTrack);
        this._scrollbarTrack.appendChild(this._thumb);
        fragment.appendChild(this._viewPort);
        this._viewPort.appendChild(this._overview);

        var children = this._element.children;
        while (children.length) {
            this._overview.appendChild(children[0]);
        }
        this._element.appendChild(fragment);

        this._checkOverflow();
        this._calculateHeights();
        if (!this._enabled) {
            this._scrollbarTrack.classList.add('display-none');
        }

        this._setScrollPosition();

        this._initScrollingByThumbMousemove();
        this._initMouseWheelScrolling();
        if ('ontouchstart' in window) {
            this._initTouchScrolling();
            this._initScrollingByThumbTouchmove();
        }
        if ('onpointerdown' in window) {
            this._initPointerScrolling();
        }

        if (this._updateOnWindowResize) {
            this._initWindowResize();
        }
    };

    Object.defineProperties(Scrollbar.prototype, {

        _setScrollPosition: {
            value: function() {                            
                if (transformProperty === "msTransform") {
                    this._thumb.style[transformProperty] = 'translateY(' + newThumbPosition + 'px)';
                    this._overview.style[transformProperty] = 'translateY(' + (-newOverviewPosition) + 'px)';
                } else {
                   this._thumb.style[transformProperty] = 'translateY(' + newThumbPosition + 'px) translateZ(0)';
                    this._overview.style[transformProperty] = 'translateY(' + (-newOverviewPosition) + 'px) translateZ(0)'; 
                }
                
                thumbPosition = parseInt(this._thumb.style[transformProperty].slice(11), 10); // translateY( - 11 characters
                
                overviewPosition = -parseInt(this._overview.style[transformProperty].slice(11), 10);
                frameRequested = false;
            }
        },

        //calculates all sizes of scrollbar
        _calculateHeights: {
            value: function() {

                this._measurements.ratio = this._measurements.viewPortHeight / this._measurements.overviewHeight;
                this._measurements.thumbHeight = this._calculateThumbHeight();
                this._measurements.maxThumbPosition = this._calculateMaxThumbPosition();
                this._measurements.maxOverviewPosition = this._calculateMaxOverviewPosition();                
                this._thumb.style.height = this._measurements.thumbHeight + 'px';
                if (this._measurements.scrollPercent === undefined) {
                    this._measurements.scrollPercent = 0;
                }
            },
        },

        _checkOverflow: {
            value: function() {
                this._measurements.viewPortHeight = this._element.clientHeight - parseInt(getComputedStyle(this._element).paddingTop, 10);
                this._measurements.overviewHeight = this._overview.scrollHeight;
                this._enabled = (this._measurements.overviewHeight > this._measurements.viewPortHeight);
            }
        },

        resize: {
            value: function () {
                cancelAnimationFrame(autoScrollFrame);
                this._checkOverflow();
                if (this._enabled) {
                    this._calculateHeights();
                    this._scrollbarTrack.classList.remove('display-none');
                } else {                    
                    this._scrollbarTrack.classList.add('display-none');
                }
                this.scrollOverviewTo(0);
            },
            enumerable: true
        },

        _calculateThumbHeight: {
            value: function() {
                var height = this.fixedThumbHeight ? this.fixedThumbHeight : (this._measurements.ratio * this._measurements.viewPortHeight);
                return height;
            }
        },

        _calculateMaxThumbPosition: {
            value: function() {
                return Math.max(0, this._measurements.viewPortHeight - this._measurements.thumbHeight);
            }
        },

        _calculateMaxOverviewPosition: {
            value: function() {
                return Math.max(0, this._measurements.overviewHeight - this._measurements.viewPortHeight);
            }
        },

        _initScrollingByThumbMousemove: {
            value: function() {
                var _this = this;
                var isScrollingByThumbMousemove = false;
                var timer;
                var scrollByThumbMousemove = function(e) {
                    _this._scrollByThumbMousemove(e);
                };

                this._thumb.addEventListener('mousedown', function(e) {
                    if (_this._enabled) {
                        _this._startScrollingByThumbMousemove(e);
                        document.addEventListener('mousemove', scrollByThumbMousemove);
                        isScrollingByThumbMousemove = true;
                        _this._thumb.classList.add('scroll-active');
                        if (timer) {
                            clearTimeout(timer);
                        }
                    }
                });

                document.addEventListener('mouseup', function() {
                    if (isScrollingByThumbMousemove) {
                        isScrollingByThumbMousemove = false;
                        document.removeEventListener('mousemove', scrollByThumbMousemove);
                        timer = setTimeout(function() {
                            _this._thumb.classList.remove('scroll-active');
                        }, scrollHideDelay);
                        
                    }
                });

                this._thumb.addEventListener('click', function(e) {
                    e.stopPropagation();
                });
            }
        },

        _startScrollingByThumbMousemove: {
            value: function(e) {
                e.preventDefault(); // prevent selecting text
                this.lastY = e.pageY;
            }
        },

        _scrollByThumbMousemove: {
            value: function(e) {
                var delta = e.pageY - this.lastY;
                this._scrollTumbBy(delta);
                this.lastY = e.pageY;
            }
        },

        _initScrollingByThumbTouchmove: {
            value: function() {
                var _this = this;
                var isScrollingByThumbTouchmove = false;
                var timer;
                var scrollByThumbTouchmove = function(e) {
                    _this._scrollByThumbTouchmove(e);
                };

                this._thumb.addEventListener('touchstart', function(e) {
                    if (_this._enabled) {
                        _this._startScrollingByThumbTouchmove(e);
                        document.addEventListener('touchmove', scrollByThumbTouchmove);
                        isScrollingByThumbTouchmove = true;
                        _this._thumb.classList.add('scroll-active');
                        if (timer) {
                            clearTimeout(timer);
                        }
                    }
                });

                document.addEventListener('touchend', function() {
                    if (isScrollingByThumbTouchmove) {
                        isScrollingByThumbTouchmove = false;
                        document.removeEventListener('touchmove', scrollByThumbTouchmove);
                        timer = setTimeout(function() {
                            _this._thumb.classList.remove('scroll-active');
                        }, scrollHideDelay);
                    }
                });
            }
        },

        _startScrollingByThumbTouchmove: {
            value: function(e) {
                e.preventDefault(); // prevent selecting text
                this.lastY = e.changedTouches[0].pageY;
            }
        },

        _scrollByThumbTouchmove: {
            value: function(e) {
                var delta = e.changedTouches[0].pageY - this.lastY;
                this._scrollTumbBy(delta);
                this.lastY = e.changedTouches[0].pageY;
            }
        },

        _scrollTumbBy: {
            value: function(delta) {
                
                thumbPosition += delta;
                thumbPosition = positionOrMax(thumbPosition, this._measurements.maxThumbPosition);
                var oldScrollPercent = this._measurements.scrollPercent;
                this._measurements.scrollPercent = thumbPosition / this._measurements.maxThumbPosition;
                
                newThumbPosition = thumbPosition;
                newOverviewPosition = this._measurements.scrollPercent * this._measurements.maxOverviewPosition;
                if (!frameRequested) {
                    requestAnimationFrame(this._setScrollPosition.bind(this));
                    frameRequested = true;
                }
                return true;
            }
        },

        _initMouseWheelScrolling: {
            value: function() {
                var _this = this;
                var timer;
                if ('onwheel' in document) {
                    this._element.addEventListener('wheel', function(e) {
                        if (_this._enabled) {
                            var scrolled = _this._mouseWheelScroll(e.deltaY);
                            _this._stopEventConditionally(e, scrolled);
                            if (e.deltaMode == 1) {
                                _this.mouseWheelSpeed = 8;
                            }
                            _this._thumb.classList.add('scroll-active');
                            if (timer) {
                                clearTimeout(timer);
                            }
                            timer = setTimeout(function() {
                                _this._thumb.classList.remove('scroll-active');
                            }, scrollHideDelay);
                        }                        
                    });
                } else if ('onmousewheel' in document) {
                    this._element.addEventListener('mousewheel', function(e) {
                        if (_this._enabled) {
                            _this.mouseWheelSpeed = 0.15;
                            var scrolled = _this._mouseWheelScroll(-e.wheelDelta);
                            _this._stopEventConditionally(e, scrolled);
                            _this._thumb.classList.add('scroll-active');
                            if (timer) {
                                clearTimeout(timer);
                            }
                            timer = setTimeout(function() {
                                _this._thumb.classList.remove('scroll-active');
                            }, scrollHideDelay);
                        }

                    });
                }
            }
        },

        _mouseWheelScroll: {
            value: function(deltaY) {
                var delta = deltaY * this.mouseWheelSpeed;
                if (delta !== 0)
                    return this._scrollOverviewBy(delta);
            }
        },

        _scrollOverviewBy: {
            value: function(delta) {
                overviewPosition += delta;
                return this.scrollOverviewTo(overviewPosition);
            }
        },

        scrollOverviewTo: {
            value: function(overviewPositionNew) {
                overviewPositionNew = positionOrMax(overviewPositionNew, this._measurements.maxOverviewPosition);
                var oldScrollPercent = this._measurements.scrollPercent;
                this._measurements.scrollPercent = overviewPositionNew / this._measurements.maxOverviewPosition;
                if (oldScrollPercent != this._measurements.scrollPercent) {
                    var thumbPosition = this._measurements.scrollPercent * this._measurements.maxThumbPosition;
                    newThumbPosition = thumbPosition;
                    newOverviewPosition = overviewPositionNew;
                    if (!frameRequested) {
                        requestAnimationFrame(this._setScrollPosition.bind(this));
                        frameRequested = true;
                    }
                    return true;
                }
                else {
                    overviewPosition = positionOrMax(overviewPosition, this._measurements.maxOverviewPosition);
                    return false;
                }
            },
            enumerable: true
        },

        //if we scroll 'scrollable' element, parent is not scrolled
        //if there is nothing to scroll more in 'scrollable' element, parent is scrolled
        _stopEventConditionally: {
            value: function(e, condition) {
                if (condition || this.preventDefaultScroll) {
                    e.preventDefault();
                    e.stopPropagation();
                }
            }
        },

        _initTouchScrolling: {
            value: function() {
                var _this = this;
                var timer;
                this._overview.addEventListener('touchstart', function(e) {
                    if (_this._enabled) {
                        _this._startTouchScrolling(e);
                        _this._thumb.classList.add('scroll-active');
                        if (timer) {
                            clearTimeout(timer);
                        }
                    }
                });
                this._overview.addEventListener('touchmove', function(e) {
                    _this._touchScroll(e);
                });
                this._overview.addEventListener('touchend', function(e) {
                    _this._stopTouchScrolling(e);
                    timer = setTimeout(function() {
                        _this._thumb.classList.remove('scroll-active');
                    }, scrollHideDelay);
                });
            }
        },

        _startTouchScrolling: {
            value: function(e) {
                if (e.changedTouches) {
                    this.lastY = e.changedTouches[0].pageY;
                    this._touchScrolling = true;

                    velocity = amplitude = 0;
                    frame = newOverviewPosition;
                    timestamp = Date.now();
                    clearInterval(ticker);
                    ticker = setInterval(track, 48);

                    // e.stopPropagation();
                }
            }
        },

        _touchScroll: {
            value: function(e) {
                if (this._touchScrolling && e.changedTouches) {
                    var delta = (this.lastY - e.changedTouches[0].pageY) * this.touchScrollSpeed;                    
                    if (delta !== 0) {
                        var scrolled = this._scrollOverviewBy(delta);
                        if (scrolled) {
                            this.lastY = e.changedTouches[0].pageY;
                        }
                        // this._stopEventConditionally(e, scrolled);
                    }                    
                }
            }
        },

        _stopTouchScrolling: {
            value: function(e) {
                if (this._touchScrolling) {
                    clearInterval(ticker);
                    if (velocity > 10 || velocity < -10) {
                        amplitude = 0.8 * velocity;
                        target = Math.round(newOverviewPosition + amplitude);
                        timestamp = Date.now();
                        autoScrollFrame = requestAnimationFrame(this._autoScroll.bind(this));
                    }
                }
                
                this._touchScrolling = false;
                //e.stopPropagation();
            }
        },

        _autoScroll: {
            value: function () {
                var elapsed, delta, timeConstant = 325;
                if (amplitude) {
                    elapsed = Date.now() - timestamp;
                    delta = -amplitude * Math.exp(-elapsed / timeConstant);
                    if (delta > 0.5 || delta < -0.5) {
                        this.scrollOverviewTo(target + delta);
                        autoScrollFrame = requestAnimationFrame(this._autoScroll.bind(this));
                    } else {
                        this.scrollOverviewTo(target);
                    }
                }
            }
        },

        _initPointerScrolling: {
            value: function() {
                var _this = this;
                var pointerScroll = function(e) {
                    _this._pointerScroll(e);
                };
                this._overview.addEventListener('pointerdown', function(e) {
                    if (_this._enabled)
                    {
                        _this._startPointerScrolling(e);
                        document.addEventListener('pointermove', pointerScroll);
                        _this._thumb.classList.add('scroll-active');
                        if (timer) {
                            clearTimeout(timer);
                        }
                    }                       
                });             
                this._overview.addEventListener('pointerup', function(e) {
                    _this._stopPointerScrolling(e);
                    document.removeEventListener('pointermove', pointerScroll);
                    timer = setTimeout(function() {
                        _this._thumb.classList.remove('scroll-active');
                    });
                });
            }
        },

        _startPointerScrolling: {
            value: function(e) {
                this.lastY = e.pageY;
                this._pointerScrolling = true;                
                e.stopPropagation();
                e.preventDefault();
            }
        },

        _pointerScroll: {
            value: function(e) {
                if (this._pointerScrolling) {
                    var delta = (this.lastY - e.pageY) * this.touchScrollSpeed;
                    var scrolled = this._scrollOverviewBy(delta);
                    if (scrolled) {
                        this.lastY = e.pageY;
                    }
                }
            }
        },

        _stopPointerScrolling: {
            value: function(e) {
                this._pointerScrolling = false;
                e.stopPropagation();
            }
        },

        _initWindowResize: {
            value: function() {
                var _this = this;
                this.windowResize = function() {
                    _this._checkOverflow();
                    if (_this._enabled) {
                        _this._calculateHeights();
                        _this._keepScrollPosition();
                        _this._scrollbarTrack.classList.remove('display-none');
                    } else {
                        _this._calculateHeights();
                        _this._scrollbarTrack.classList.add('display-none');
                        _this.scrollOverviewTo(0);
                    }
                };
                window.addEventListener('resize', this.windowResize);
            }
        },

        _keepScrollPosition: {
            value: function() {
                var thumbPosition = this._measurements.scrollPercent * this._measurements.maxThumbPosition;
                var overviewPosition = this._measurements.scrollPercent * this._measurements.maxOverviewPosition;
                newOverviewPosition = overviewPosition;
                newThumbPosition = thumbPosition;
                if (!frameRequested) {
                        requestAnimationFrame(this._setScrollPosition.bind(this));
                        frameRequested = true;
                }
            }
        }
    });

    function track() {
        var now, elapsed, delta, v;

        now = Date.now();
        elapsed = now - timestamp;
        timestamp = now;
        delta = newOverviewPosition - frame;
        frame = newOverviewPosition;

        v = 1000 * delta / (1 + elapsed);
        velocity = 0.8 * v + 0.2 * velocity;
    }

    function positionOrMax(p, max) {
        if (p < 0) {
            return 0;
        } else {
            if (p > max)
            {
                return max;                
            } else {
                return p;
            }                
        }
    }

    // polyfill transform
    var transform = ["transform", "msTransform", "webkitTransform", "mozTransform", "oTransform"];   

    function getSupportedPropertyName(properties) {
        for (var i = 0; i < properties.length; i++) {
            if (typeof document.body.style[properties[i]] != "undefined") {
                return properties[i];
            }
        }
        return null;
    } 

    var transformProperty = getSupportedPropertyName(transform);

    global.createScrollbar = function(HTMLElement, options) {
        return new Scrollbar(HTMLElement, options);
    };

    global.createNavigation = function(HTMLElement) {
        return new Navigation(HTMLElement);
    };

    return global;
}(global || {}));



(function() {

    var navListWrapper = document.getElementById('nav-list-wrapper');
    //  Navigation container
    var navContainer = document.body.querySelector('.nav');
    //  Navigation list
    var navList = navContainer.querySelector('.nav-list');
    // Main navigation toggle
    var openButton = navContainer.querySelector('.nav__btn-open');

    var menu = global.createNavigation(navListWrapper);
    var container = document.querySelector('.nav-list-container');
    var scroll = global.createScrollbar(container, {
        preventDefaultScroll: true,
        updateOnWindowResize: true
    });

    var navTitle = document.querySelector('.nav__title');    
    var closeButton = navContainer.querySelector('.nav__btn-close');
    var menuLevel = 1;
    var navTitleText = navTitle.textContent;
    var navTitleSubText = 'Back to previous';

    var breadcrumbs = document.createElement('div');
    breadcrumbs.classList.add('nav__breadcrumbs');
    navTitle.parentElement.insertBefore(breadcrumbs, navTitle.nextElementSibling);
    breadcrumbs.addEventListener('tap', function(e) {
        var target = e.target;
        if (target instanceof HTMLSpanElement) {
            while (target !== breadcrumbs.lastElementChild && breadcrumbs.lastElementChild) {
                goToPreviousMenu();
            }
        }
        e.stopPropagation();
    });

    var animBlockPrimary = document.createElement('div');
    var animBlockSecondary = document.createElement('div');
    animBlockPrimary.classList.add('nav-list__block-anim--primary');
    animBlockSecondary.classList.add('nav-list__block-anim--secondary');
    navList.parentElement.appendChild(animBlockSecondary);
    navList.parentElement.appendChild(animBlockPrimary);

    // Event listeners for user interaction with menu
    navListWrapper.addEventListener('tap', menuItemClick);
    navTitle.addEventListener('tap', goToPreviousMenu);
    openButton.addEventListener('tap', openNavigation);
    closeButton.addEventListener('tap', closeNavigation);

    // Element which content will be replaced with loaded page
    var sectionAjax = document.querySelector('[data-insert]');

    // Elements for preloader
    var preloaderOverlay = document.createElement('div');
    var preloaderDot = document.createElement('div');
    var preloaderContainer = document.createElement('div');
    var preloaderBtnBack = document.createElement('a');
    var preloaderBtnDirect = document.createElement('a');
    //preloaderOverlay.classList.add('elem-hide', 'preloader-overlay');
    //preloaderDot.classList.add('preloader-dot');
    //preloaderContainer.classList.add('preloader-container');
    //preloaderBtnBack.classList.add('elem-hide', 'btn-preloader');
    //preloaderBtnDirect.classList.add('elem-hide', 'btn-preloader');
    //sectionAjax.parentElement.insertBefore(preloaderOverlay, sectionAjax);
    //preloaderOverlay.appendChild(preloaderContainer);
    //preloaderContainer.appendChild(preloaderDot);
    //preloaderContainer.appendChild(preloaderBtnBack);
    //preloaderContainer.appendChild(preloaderBtnDirect);
    //preloaderBtnBack.textContent = 'Back';
    //preloaderBtnDirect.textContent = 'Direct';

    // Setting display none for preloader after it has faded out
    //preloaderOverlay.addEventListener('animationend', function() {
    //    if (preloaderOverlay.classList.contains('preloader-overlay--hide')) {
    //        preloaderOverlay.classList.add('elem-hide');
    //        preloaderBtnBack.classList.add('elem-hide');
    //        preloaderBtnDirect.classList.add('elem-hide');
    //    }
    //});

    // Checking keyframe bug for opacity animation
    var checkKeyframeBug = document.createElement('div');
    var hasKeyframebug = false;
    checkKeyframeBug.classList.add('checkKeyframeBug');
    document.body.appendChild(checkKeyframeBug);
    checkKeyframeBug.classList.add('ajax-section--show');
    checkKeyframeBug.addEventListener('animationend', function() {
        if (getComputedStyle(checkKeyframeBug).opacity == 0) {
            hasKeyframebug = true;
        }
    });

    function menuItemClick(e) {
        var targetAnchor = global.findParent(e.target, HTMLAnchorElement);

         e.preventDefault();

        if (targetAnchor) {

            // Loading new page if clicked link contains data-menu attribute
            var newPage = targetAnchor.getAttribute("data-menu");
            if (newPage) {
                if (sectionAjax instanceof HTMLIFrameElement) {
                    sectionAjax.src = newPage;
                    return;
                }
                loadNewPage(newPage, targetAnchor);
                menu.close();
                //Enable scrolling
                window.removeEventListener('keydow', preventKeyboardScrolling);                
                document.body.removeEventListener('touchmove', prevent);                      
                document.body.removeEventListener('wheel', prevent);                
                document.body.removeEventListener('mousewheel', prevent);

                return;
            }

            // Showing submenu if clicked link has it
            goToSubmenu(e, targetAnchor);

        }
    }

    function goToSubmenu(e, targetAnchor) {
        if (targetAnchor.classList.contains('nav-list__link--primary') && window.innerWidth > 768) {
            var tweenMaxAnimation = new TimelineMax();
            var sparkles, targetAnchorNextElemHeight;
            var timeSparklesAnimation = 0.8;
            var animBlockPositionOffsetX = e.initialEvent.clientX - (window.innerWidth - container.offsetWidth) - animBlockPrimary.offsetWidth / 2;
            var animBlockPositionOffsetY = e.initialEvent.clientY - (navListWrapper.offsetHeight - container.offsetHeight) - animBlockPrimary.offsetHeight / 2;

            targetAnchor.nextElementSibling.classList.add('elem-show');
            targetAnchorNextElemHeight = targetAnchor.nextElementSibling.offsetHeight;
            targetAnchor.nextElementSibling.classList.remove('elem-show');

            //  Class which change primary link hover
            targetAnchor.classList.add('block-animation');

            sparkles = targetAnchor.querySelector('.sparkles');
            navTitle.classList.add('nav__title--back');

            if (!sparkles) {
                sparkles = document.createElement('div');
                var timeSparklesAnimation = 0;
            }

            //  Primary links (tiles) animation
            tweenMaxAnimation.set(targetAnchor, {
                zIndex: 5,
                backgroundColor: "#f3f7f7"
            }).to(sparkles, timeSparklesAnimation, {
                opacity: 1,
                scale: 1
            }).set(animBlockSecondary, {
                x: animBlockPositionOffsetX,
                y: animBlockPositionOffsetY,
                opacity: 1
            }).set(animBlockPrimary, {
                x: animBlockPositionOffsetX,
                y: animBlockPositionOffsetY,
                opacity: 1
            }).to(sparkles, 1, {
                opacity: 0,
                y: "+=5"
            }).to(animBlockPrimary, 0.8, {
                x: 0,
                y: 0,
                height: targetAnchorNextElemHeight,
                width: "100%",
                force3D: true,
                backgroundColor: "#f3f7f7",
                ease: Power3.easeInOut
            }, "-=0.8").to(targetAnchor, 0.4, {
                opacity: 0
            }, "-=0.8").to(animBlockSecondary, 0.6, {
                x: 0,
                y: 0,
                width: "100%",
                height: targetAnchorNextElemHeight,
                force3D: true,
                ease: Power3.easeInOut
            }, "-=0.3").to(animBlockSecondary, 0.2, {
                height: "100%",
                onComplete: function() {

                    navTitle.classList.add('nav__title--back');
                    navTitle.textContent = navTitleSubText;
                    targetAnchor.classList.remove('block-animation');
                    if ( menu.showSub(targetAnchor) ) {
                        menuLevel++;
                        var breadcrumb = document.createElement('span');
                        breadcrumb.textContent = 'start ';
                        breadcrumbs.appendChild(breadcrumb);
                        breadcrumb = document.createElement('span');
                        breadcrumb.textContent = targetAnchor.textContent;
                        breadcrumbs.appendChild(breadcrumb);
                    }
                    
                    tweenMaxAnimation.set(animBlockPrimary, {
                        width: 20,
                        height: 20,
                        opacity: 0
                    }).to(animBlockSecondary, 0.01, {
                        width: 5,
                        height: 5,
                        opacity: 0
                    }).set(targetAnchor, {
                        backgroundColor: "#e0f5f7",
                        zIndex: 1,
                        opacity: 1
                    }).set(sparkles, {
                        opacity: 0,
                        scale: 0.2,
                        y: "-=5",
                        onComplete: function() {
                            scroll.resize();
                        }
                    });
                }
            }, "-=0.4");

        } else {
            navTitle.classList.add('nav__title--back');
            navTitle.textContent = navTitleSubText;
            if ( menu.showSub(targetAnchor) ) {
                menuLevel++;
                scroll.resize();
                var breadcrumb = document.createElement('span');
                breadcrumb.textContent = targetAnchor.textContent;
                breadcrumbs.appendChild(breadcrumb);
            }            
        }

        
    }

    // AJAX loading
    function loadNewPage(newPage, targetAnchor) {
        var request = new XMLHttpRequest();
        request.open('GET', newPage, true);

        var ajaxTimeLine = new TimelineLite();
        var overlayTimeLine = new TimelineLite();

        // Timer to show buttons if loading takes more than 3 seconds
        var timerForBtns = setTimeout(function() {
            preloaderBtnBack.classList.remove('elem-hide');
            preloaderBtnDirect.classList.remove('elem-hide');
            preloaderBtnBack.addEventListener('click', back);
            preloaderBtnDirect.addEventListener('click', direct);
        }, 3000);

        //var back = function() {
        //    request.removeEventListener('load', addLoadedPage);
        //    sectionAjax.classList.remove('ajax-section--hide');
        //    sectionAjax.classList.add('ajax-section--show');

        //    preloaderOverlay.classList.add('preloader-overlay--hide');
        //    preloaderOverlay.classList.remove('preloader-overlay--show');

        //    document.body.style.overflowY = 'auto';
        //};

        //var direct = function() {
        //    request.removeEventListener('load', addLoadedPage);
        //    preloaderOverlay.classList.remove('preloader-overlay--show');
        //    preloaderOverlay.classList.add('elem-hide');
        //    window.location.href = targetAnchor.href;
        //};

        //if (!hasKeyframebug) {
        //    preloaderOverlay.classList.remove('elem-hide');
        //    preloaderOverlay.classList.remove('preloader-overlay--hide');
        //    preloaderOverlay.classList.add('preloader-overlay--show');
        //    sectionAjax.classList.add('ajax-section--hide');
        //    sectionAjax.classList.remove('ajax-section--show');
        //}


        document.body.style.overflowY = 'hidden';

        var dataHiddenClasses = sectionAjax.getAttribute('data-hidden-classes');
        //if (dataHiddenClasses) {
        //    var classesToHide = dataHiddenClasses.split(' ');
        //}

        var addLoadedPage = function() {
            if (request.status >= 200 && request.status < 400) {
                // Success!
                var resp = request.responseText;
                clearTimeout(timerForBtns);
                preloaderBtnBack.removeEventListener('click', back);
                preloaderBtnDirect.removeEventListener('click', direct);

                var bodyBegin = resp.indexOf('<body>');
                var bodyEnd = resp.indexOf('</body>');

                if (bodyBegin != -1 && bodyEnd != -1) {
                    resp = resp.slice(bodyBegin + 6, bodyEnd);
                }

                var fragment = document.createElement('div');
                fragment.innerHTML = resp;

                // Check if received fragment has element with 'data-load' attribute
                var dataLoad = fragment.querySelector('[data-load]');
                var functionsOnLoad = '';
                if (dataLoad) {
                    fragment = dataLoad;
                    // Check if 'data-load' attribute has list of functions to invoke after adding received fragment
                    if (fragment.getAttribute('data-load')) {
                        functionsOnLoad = fragment.getAttribute('data-load').split(' ');
                    }
                }

                // Hide elements with classes
                if (classesToHide) {
                    for (var i = classesToHide.length - 1; i >= 0; i--) {
                        var elemToHide = fragment.querySelectorAll('.' + classesToHide[i]);
                        for (var j = elemToHide.length - 1; j >= 0; j--) {
                            elemToHide[j].style.display = 'none';
                        }
                    }
                }

                sectionAjax.innerHTML = fragment.innerHTML;

                if (!hasKeyframebug) {

                    sectionAjax.classList.remove('ajax-section--hide');
                    sectionAjax.classList.add('ajax-section--show');

                    preloaderOverlay.classList.remove('preloader-overlay--show');
                    preloaderOverlay.classList.add('preloader-overlay--hide');
                }

                document.body.style.overflowY = '';
                window.scrollTo(0, 0);

                // Invoking functions from 'data-load' attribute
                if (Array.isArray(functionsOnLoad)) {
                    for (var i = functionsOnLoad.length - 1; i >= 0; i--) {
                        if (typeof window[functionsOnLoad[i]] == 'function') {
                            window[functionsOnLoad[i]]();
                        }
                    }
                }
            } else {
                // We reached our target server, but it returned an error
                window.location.href = targetAnchor.href;
            }
        }
        request.addEventListener('load', addLoadedPage);

        request.onerror = function() {
            // There was a connection error of some sort
            window.location.href = targetAnchor.href;
        };

        request.send();
    }

    function goToPreviousMenu() {
        menu.hideSub();
        scroll.resize();

        if (menuLevel > 1) {
            menuLevel--;
        }

        if (menuLevel == 1) {
            navTitle.classList.remove('nav__title--back');
            navTitle.textContent = navTitleText;
        }

        if (breadcrumbs.children.length !== 0) {
            breadcrumbs.removeChild(breadcrumbs.lastElementChild);
            if (breadcrumbs.children.length == 1){
                breadcrumbs.removeChild(breadcrumbs.lastElementChild);
            }                
        }
        
    }

    function openNavigation(e) {
        goToPreviousMenu();
        menu.open();
        e.preventDefault();

        //Disable scrolling
        window.addEventListener('keydow', preventKeyboardScrolling);
        if ('ontouchmove' in window) {
            document.body.addEventListener('touchmove', prevent);
        }
        if ('onwheel' in window) {
            document.body.addEventListener('wheel', prevent);
        } else {
            document.body.addEventListener('mousewheel', prevent);
        }
        
    }

    function closeNavigation(e) {
        menu.close();
        e.preventDefault();

        //Enable scrolling
        window.removeEventListener('keydow', preventKeyboardScrolling);        
        document.body.removeEventListener('touchmove', prevent);              
        document.body.removeEventListener('wheel', prevent);        
        document.body.removeEventListener('mousewheel', prevent);
        
    }

    function prevent(e) {
        e.preventDefault();
    }

    function preventKeyboardScrolling(e) {
        // space, page up, page down and arrow keys:
        if([32, 33, 34, 37, 38, 39, 40].indexOf(e.keyCode) > -1) {
            e.preventDefault();
        }
    }
}());

