! function(t) {
    if ("object" == typeof exports && "undefined" != typeof module) module.exports = t();
    else if ("function" == typeof define && define.amd) define([], t);
    else {
        var e;
        e = "undefined" != typeof window ? window : "undefined" != typeof global ? global : "undefined" != typeof self ? self : this, e.StickyState = t()
    }
}(function() {
    var t;
    return function e(t, s, i) {
        function o(n, l) {
            if (!s[n]) {
                if (!t[n]) {
                    var a = "function" == typeof require && require;
                    if (!l && a) return a(n, !0);
                    if (r) return r(n, !0);
                    var h = new Error("Cannot find module '" + n + "'");
                    throw h.code = "MODULE_NOT_FOUND", h
                }
                var c = s[n] = {
                    exports: {}
                };
                t[n][0].call(c.exports, function(e) {
                    var s = t[n][1][e];
                    return o(s ? s : e)
                }, c, c.exports, e, t, s, i)
            }
            return s[n].exports
        }
        for (var r = "function" == typeof require && require, n = 0; n < i.length; n++) o(i[n]);
        return o
    }({
        1: [function(t, e, s) {
            function i() {
                return window.scrollY || window.pageYOffset || 0
            }

            function o() {
                return Math.max(document.body.scrollHeight, document.body.offsetHeight, document.documentElement.clientHeight, document.documentElement.scrollHeight, document.documentElement.offsetHeight)
            }

            function r(t, e) {
                var s = t.getBoundingClientRect(),
                    o = s.top + i(),
                    r = e || s.height;
                return {
                    top: o,
                    bottom: o + r,
                    height: r,
                    width: s.width
                }
            }

            function n(t, e) {
                var s = h({}, t);
                return s.top -= e.top, s.bottom = s.top + t.height, s
            }

            function l(t) {
                var e = {
                    top: null,
                    bottom: null
                };
                for (var s in e) {
                    var i = parseInt(window.getComputedStyle(t)[s]);
                    i = isNaN(i) ? null : i, e[s] = i
                }
                return e
            }

            function a(t) {
                var e = t.previousElementSibling;
                return e && "script" === e.tagName.toLocaleLowerCase() && (e = a(e)), e
            }
            var h = t("object-assign"),
                c = t("fastscroll"),
                d = {
                    featureTested: !1
                },
                p = {
                    disabled: !1,
                    className: "sticky",
                    stateClassName: "is-sticky",
                    fixedClass: "sticky-fixed",
                    wrapperClass: "sticky-wrap",
                    absoluteClass: "is-absolute"
                },
                u = function(t, e) {
                    if (!t) throw new Error("StickyState needs a DomElement");
                    this.el = t, this.options = h({}, p, e), this.setState({
                        sticky: !1,
                        absolute: !1,
                        fixedOffset: "",
                        offsetHeight: 0,
                        bounds: {
                            top: null,
                            bottom: null,
                            height: null,
                            width: null
                        },
                        restrict: {
                            top: null,
                            bottom: null,
                            height: null,
                            width: null
                        },
                        style: {
                            top: null,
                            bottom: null
                        },
                        disabled: this.options.disabled
                    }, !0), this.scrollTarget = "auto" !== window.getComputedStyle(this.el.parentNode).overflow ? window : this.el.parentNode, this.hasOwnScrollTarget = this.scrollTarget !== window, this.hasOwnScrollTarget && (this.updateFixedOffset = this.updateFixedOffset.bind(this)), this.firstRender = !0, this.resizeHandler = null, this.fastScroll = null, this.wrapper = null, this.render = this.render.bind(this), this.addSrollHandler(), this.addResizeHandler(), this.render()
                };
            u.prototype.setState = function(t, e) {
                this.lastState = this.state || t, this.state = h({}, this.state, t), e !== !0 && this.render()
            }, u.prototype.getBoundingClientRect = function() {
                return this.el.getBoundingClientRect()
            }, u.prototype.getBounds = function(t) {
                var e = this.getBoundingClientRect(),
                    s = o();
                if (t !== !0 && null !== this.state.bounds.height && this.state.offsetHeight === s && e.height === this.state.bounds.height) return {
                    offsetHeight: s,
                    style: this.state.style,
                    bounds: this.state.bounds,
                    restrict: this.state.restrict
                };
                var i, h, c = l(this.el),
                    d = this.wrapper || this.el,
                    p = 0;
                if (this.canSticky()) {
                    var u = a(d);
                    p = 0, u ? (p = parseInt(window.getComputedStyle(u)["margin-bottom"]), p = p || 0, i = r(u), this.hasOwnScrollTarget && (i = n(i, r(this.scrollTarget)), p += this.fastScroll.scrollY), i.top = i.bottom + p) : (u = d.parentNode, p = parseInt(window.getComputedStyle(u)["padding-top"]), p = p || 0, i = r(u), this.hasOwnScrollTarget && (i = n(i, r(this.scrollTarget)), p += this.fastScroll.scrollY), i.top = i.top + p), this.hasOwnScrollTarget && (h = r(this.scrollTarget), h.top = 0, h.height = this.scrollTarget.scrollHeight || h.height, h.bottom = h.height), i.height = d.clientHeight, i.width = d.clientWidth, i.bottom = i.top + i.height
                } else if (i = r(d, e.height), this.hasOwnScrollTarget) {
                    var f = r(this.scrollTarget);
                    p = this.fastScroll.scrollY, i = n(i, f), h = f, h.top = 0, h.height = this.scrollTarget.scrollHeight || h.height, h.bottom = h.height
                }
                return h = h || r(d.parentNode), {
                    offsetHeight: s,
                    style: c,
                    bounds: i,
                    restrict: h
                }
            }, u.prototype.updateBounds = function(t) {
                t = t === !0, this.setState(this.getBounds(), t)
            }, u.prototype.updateFixedOffset = function() {
                this.lastState.fixedOffset = this.state.fixedOffset, this.state.sticky ? this.state.fixedOffset = this.scrollTarget.getBoundingClientRect().top + "px" : this.state.fixedOffset = "", this.lastState.fixedOffset !== this.state.fixedOffset && this.render()
            }, u.prototype.canSticky = function() {
                return u["native"]()
            }, u.prototype.addSrollHandler = function() {
                if (!this.fastScroll) {
                    var t = c.hasScrollTarget(this.scrollTarget);
                    this.fastScroll = c.getInstance(this.scrollTarget), this.onScroll = this.onScroll.bind(this), this.fastScroll.on("scroll:start", this.onScroll), this.fastScroll.on("scroll:progress", this.onScroll), this.fastScroll.on("scroll:stop", this.onScroll), t && this.fastScroll.scrollY > 0 && this.fastScroll.trigger("scroll:progress")
                }
            }, u.prototype.removeSrollHandler = function() {
                this.fastScroll && (this.fastScroll.off("scroll:start", this.onScroll), this.fastScroll.off("scroll:progress", this.onScroll), this.fastScroll.off("scroll:stop", this.onScroll), this.fastScroll.destroy(), this.fastScroll = null)
            }, u.prototype.addResizeHandler = function() {
                this.resizeHandler || (this.resizeHandler = this.onResize.bind(this), window.addEventListener("resize", this.resizeHandler, !1), window.addEventListener("orientationchange", this.resizeHandler, !1))
            }, u.prototype.removeResizeHandler = function() {
                this.resizeHandler && (window.removeEventListener("resize", this.resizeHandler), window.removeEventListener("orientationchange", this.resizeHandler), this.resizeHandler = null)
            }, u.prototype.onScroll = function(t) {
                this.updateStickyState(!1), this.hasOwnScrollTarget && !this.canSticky() && (this.updateFixedOffset(), this.state.sticky && !this.hasWindowScrollListener ? (this.hasWindowScrollListener = !0, c.getInstance(window).on("scroll:progress", this.updateFixedOffset)) : !this.state.sticky && this.hasWindowScrollListener && (this.hasWindowScrollListener = !1, c.getInstance(window).off("scroll:progress", this.updateFixedOffset)))
            }, u.prototype.onResize = function(t) {
                this.updateBounds(!0), this.updateStickyState(!1)
            }, u.prototype.getStickyState = function() {
                if (this.state.disabled) return {
                    sticky: !1,
                    absolute: !1
                };
                var t = this.fastScroll.scrollY,
                    e = this.state.style.top,
                    s = this.state.style.bottom,
                    i = this.state.sticky,
                    o = this.state.absolute;
                if (null !== e) {
                    var r = this.state.restrict.bottom - this.state.bounds.height - e;
                    e = this.state.bounds.top - e, this.state.sticky === !1 && t >= e && r >= t ? (i = !0, o = !1) : this.state.sticky && (e > t || t > r) && (i = !1, o = t > r)
                } else if (null !== s) {
                    t += window.innerHeight;
                    var n = this.state.restrict.top + this.state.bounds.height - s;
                    s = this.state.bounds.bottom + s, this.state.sticky === !1 && s >= t && t >= n ? (i = !0, o = !1) : this.state.sticky && (t > s || n > t) && (i = !1, o = n >= t)
                }
                return {
                    sticky: i,
                    absolute: o
                }
            }, u.prototype.updateStickyState = function(t) {
                var e = this.getStickyState();
                e.sticky === this.state.sticky && e.absolute === this.state.absolute || (t = t === !0, e = h(e, this.getBounds()), this.setState(e, t))
            }, u.prototype.render = function() {
                var t = this.el.className;
                if (this.firstRender) {
                    if (this.firstRender = !1, !this.canSticky()) {
                        this.wrapper = document.createElement("div"), this.wrapper.className = this.options.wrapperClass;
                        var e = this.el.parentNode;
                        e && e.insertBefore(this.wrapper, this.el), this.wrapper.appendChild(this.el), t += " " + this.options.fixedClass
                    }
                    this.updateBounds(!0), this.updateStickyState(!0)
                }
                if (!this.canSticky()) {
                    var s = this.state.disabled || null === this.state.bounds.height || !this.state.sticky && !this.state.absolute ? "auto" : this.state.bounds.height + "px";
                    this.wrapper.style.height = s, this.state.absolute !== this.lastState.absolute && (this.wrapper.style.position = this.state.absolute ? "relative" : "", t = -1 === t.indexOf(this.options.absoluteClass) && this.state.absolute ? t + (" " + this.options.absoluteClass) : t.split(" " + this.options.absoluteClass).join(""), this.el.style.marginTop = this.state.absolute && null !== this.state.style.top ? this.state.restrict.height - (this.state.bounds.height + this.state.style.top) + (this.state.restrict.top - this.state.bounds.top) + "px" : "", this.el.style.marginBottom = this.state.absolute && null !== this.state.style.bottom ? this.state.restrict.height - (this.state.bounds.height + this.state.style.bottom) + (this.state.restrict.bottom - this.state.bounds.bottom) + "px" : ""), this.hasOwnScrollTarget && !this.state.absolute && this.lastState.fixedOffset !== this.state.fixedOffset && (this.el.style.marginTop = this.state.fixedOffset)
                }
                var i = t.indexOf(this.options.stateClassName) > -1;
                return this.state.sticky && !i ? t += " " + this.options.stateClassName : !this.state.sticky && i && (t = t.split(" " + this.options.stateClassName).join("")), this.el.className !== t && (this.el.className = t), this.el
            }, u["native"] = function() {
                if (d.featureTested) return d.canSticky;
                if ("undefined" != typeof window) {
                    if (d.featureTested = !0, window.Modernizr && window.Modernizr.hasOwnProperty("csspositionsticky")) return d.canSticky = window.Modernizr.csspositionsticky;
                    var t = document.createElement("div");
                    document.documentElement.appendChild(t);
                    var e = ["sticky", "-webkit-sticky", "-moz-sticky", "-ms-sticky", "-o-sticky"];
                    d.canSticky = !1;
                    for (var s = 0; s < e.length && (t.style.position = e[s], d.canSticky = !!window.getComputedStyle(t).position.match("sticky"), !d.canSticky); s++);
                    document.documentElement.removeChild(t)
                }
                return d.canSticky
            }, u.apply = function(t) {
                if (t)
                    if (t.length)
                        for (var e = 0; e < t.length; e++) new u(t[e]);
                    else new u(t)
            }, e.exports = u
        }, {
            fastscroll: 4,
            "object-assign": 5
        }],
        2: [function(e, s, i) {
            ! function(e) {
                "use strict";
                var i = function(t, e) {
                    var s = [].slice.call(arguments, 2),
                        i = function() {
                            return e.apply(t, s)
                        };
                    return i
                };
                "undefined" != typeof s && s.exports ? s.exports = i : "undefined" != typeof t ? t(function() {
                    return i
                }) : e.delegate = i
            }(this)
        }, {}],
        3: [function(t, e, s) {
            "use strict";

            function i(t) {
                for (var e in t)
                    if (t.hasOwnProperty(e)) return !1;
                return !0
            }
            var o = {},
                r = function() {
                    this._eventMap = {}, this._destroyed = !1
                };
            r.getInstance = function(t) {
                if (!t) throw new Error("key must be");
                return o[t] || (o[t] = new r)
            }, r.prototype.addListener = function(t, e) {
                var s = this.getListener(t);
                return s ? -1 === s.indexOf(e) ? (s.push(e), !0) : !1 : (this._eventMap[t] = [e], !0)
            }, r.prototype.addListenerOnce = function(t, e) {
                var s = this,
                    i = function() {
                        return s.removeListener(t, i), e.apply(this, arguments)
                    };
                return this.addListener(t, i)
            }, r.prototype.removeListener = function(t, e) {
                if ("undefined" == typeof e) return this.removeAllListener(t);
                var s = this.getListener(t);
                if (s) {
                    var i = s.indexOf(e);
                    if (i > -1) return s = s.splice(i, 1), s.length || delete this._eventMap[t], !0
                }
                return !1
            }, r.prototype.removeAllListener = function(t) {
                var e = this.getListener(t);
                return e ? (this._eventMap[t].length = 0, delete this._eventMap[t], !0) : !1
            }, r.prototype.hasListener = function(t) {
                return null !== this.getListener(t)
            }, r.prototype.hasListeners = function() {
                return null !== this._eventMap && void 0 !== this._eventMap && !i(this._eventMap)
            }, r.prototype.dispatch = function(t, e) {
                var s = this.getListener(t);
                if (s) {
                    e = e || {}, e.type = t, e.target = e.target || this;
                    for (var i = -1; ++i < s.length;) s[i](e);
                    return !0
                }
                return !1
            }, r.prototype.getListener = function(t) {
                var e = this._eventMap ? this._eventMap[t] : null;
                return e || null
            }, r.prototype.destroy = function() {
                if (this._eventMap) {
                    for (var t in this._eventMap) this.removeAllListener(t);
                    this._eventMap = null
                }
                this._destroyed = !0
            }, r.prototype.on = r.prototype.bind = r.prototype.addEventListener = r.prototype.addListener, r.prototype.off = r.prototype.unbind = r.prototype.removeEventListener = r.prototype.removeListener, r.prototype.once = r.prototype.one = r.prototype.addListenerOnce, r.prototype.trigger = r.prototype.dispatchEvent = r.prototype.dispatch, e.exports = r
        }, {}],
        4: [function(t, e, s) {
            "use strict";
            var i = t("delegatejs"),
                o = t("eventdispatcher"),
                r = {},
                n = function(t, e) {
                    return t = t || window, n.hasScrollTarget(t) ? n.getInstance(t) : (r[t] = this, this.options = e || {}, this.options.hasOwnProperty("animationFrame") || (this.options.animationFrame = !0), "function" != typeof window.requestAnimationFrame && (this.options.animationFrame = !1), this.scrollTarget = t, void this.init())
                };
            n.___instanceMap = r, n.getInstance = function(t, e) {
                return t = t || window, r[t] || new n(t, e)
            }, n.hasInstance = function(t) {
                return void 0 !== r[t]
            }, n.hasScrollTarget = n.hasInstance, n.clearInstance = function(t) {
                t = t || window, n.hasInstance(t) && (n.getInstance(t).destroy(), delete r[t])
            }, n.UP = "up", n.DOWN = "down", n.NONE = "none", n.LEFT = "left", n.RIGHT = "right", n.prototype = {
                destroyed: !1,
                scrollY: 0,
                scrollX: 0,
                lastScrollY: 0,
                lastScrollX: 0,
                timeout: 0,
                speedY: 0,
                speedX: 0,
                stopFrames: 5,
                currentStopFrames: 0,
                firstRender: !0,
                animationFrame: !0,
                lastEvent: {
                    type: null,
                    scrollY: 0,
                    scrollX: 0
                },
                scrolling: !1,
                init: function() {
                    this.dispatcher = new o, this.updateScrollPosition = this.scrollTarget === window ? i(this, this.updateWindowScrollPosition) : i(this, this.updateElementScrollPosition), this.updateScrollPosition(), this.trigger = this.dispatchEvent, this.lastEvent.scrollY = this.scrollY, this.lastEvent.scrollX = this.scrollX, this.onScroll = i(this, this.onScroll), this.onNextFrame = i(this, this.onNextFrame), this.scrollTarget.addEventListener ? (this.scrollTarget.addEventListener("mousewheel", this.onScroll, !1), this.scrollTarget.addEventListener("scroll", this.onScroll, !1)) : this.scrollTarget.attachEvent && (this.scrollTarget.attachEvent("onmousewheel", this.onScroll), this.scrollTarget.attachEvent("scroll", this.onScroll))
                },
                destroy: function() {
                    this.destroyed || (this.cancelNextFrame(), this.scrollTarget.addEventListener ? (this.scrollTarget.removeEventListener("mousewheel", this.onScroll), this.scrollTarget.removeEventListener("scroll", this.onScroll)) : this.scrollTarget.attachEvent && (this.scrollTarget.detachEvent("onmousewheel", this.onScroll), this.scrollTarget.detachEvent("scroll", this.onScroll)), this.dispatcher.off(), this.dispatcher = null, this.onScroll = null, this.updateScrollPosition = null, this.onNextFrame = null, this.scrollTarget = null, this.destroyed = !0)
                },
                getAttributes: function() {
                    return {
                        scrollY: this.scrollY,
                        scrollX: this.scrollX,
                        speedY: this.speedY,
                        speedX: this.speedX,
                        angle: 0,
                        directionY: 0 === this.speedY ? n.NONE : this.speedY > 0 ? n.UP : n.DOWN,
                        directionX: 0 === this.speedX ? n.NONE : this.speedX > 0 ? n.RIGHT : n.LEFT
                    }
                },
                updateWindowScrollPosition: function() {
                    this.scrollY = window.scrollY || window.pageYOffset || 0, this.scrollX = window.scrollX || window.pageXOffset || 0
                },
                updateElementScrollPosition: function() {
                    this.scrollY = this.scrollTarget.scrollTop, this.scrollX = this.scrollTarget.scrollLeft
                },
                onScroll: function() {
                    if (this.currentStopFrames = 0, this.firstRender && (this.firstRender = !1, this.scrollY > 1)) return this.updateScrollPosition(), void this.dispatchEvent("scroll:progress");
                    if (this.scrolling || (this.scrolling = !0, this.dispatchEvent("scroll:start"), this.options.animationFrame && (this.nextFrameID = requestAnimationFrame(this.onNextFrame))), !this.options.animationFrame) {
                        clearTimeout(this.timeout), this.onNextFrame();
                        var t = this;
                        this.timeout = setTimeout(function() {
                            t.onScrollStop()
                        }, 100)
                    }
                },
                onNextFrame: function() {
                    return this.updateScrollPosition(), this.speedY = this.lastScrollY - this.scrollY, this.speedX = this.lastScrollX - this.scrollX, this.lastScrollY = this.scrollY, this.lastScrollX = this.scrollX, this.options.animationFrame && this.scrolling && 0 === this.speedY && this.currentStopFrames++ > this.stopFrames ? void this.onScrollStop() : (this.dispatchEvent("scroll:progress"), void(this.options.animationFrame && (this.nextFrameID = requestAnimationFrame(this.onNextFrame))))
                },
                onScrollStop: function() {
                    this.scrolling = !1, this.options.animationFrame && (this.cancelNextFrame(), this.currentStopFrames = 0), this.dispatchEvent("scroll:stop")
                },
                cancelNextFrame: function() {
                    cancelAnimationFrame(this.nextFrameID)
                },
                dispatchEvent: function(t, e) {
                    e = e || this.getAttributes(), this.lastEvent.type === t && this.lastEvent.scrollY === e.scrollY && this.lastEvent.scrollX === e.scrollX || (this.lastEvent = {
                        type: t,
                        scrollY: e.scrollY,
                        scrollX: e.scrollX
                    }, e.target = this.scrollTarget, this.dispatcher.dispatch(t, e))
                },
                on: function(t, e) {
                    return this.dispatcher.addListener(t, e)
                },
                off: function(t, e) {
                    return this.dispatcher.removeListener(t, e)
                }
            }, e.exports = n
        }, {
            delegatejs: 2,
            eventdispatcher: 3
        }],
        5: [function(t, e, s) {
            "use strict";

            function i(t) {
                if (null === t || void 0 === t) throw new TypeError("Object.assign cannot be called with null or undefined");
                return Object(t)
            }
            var o = Object.prototype.hasOwnProperty,
                r = Object.prototype.propertyIsEnumerable;
            e.exports = Object.assign || function(t, e) {
                for (var s, n, l = i(t), a = 1; a < arguments.length; a++) {
                    s = Object(arguments[a]);
                    for (var h in s) o.call(s, h) && (l[h] = s[h]);
                    if (Object.getOwnPropertySymbols) {
                        n = Object.getOwnPropertySymbols(s);
                        for (var c = 0; c < n.length; c++) r.call(s, n[c]) && (l[n[c]] = s[n[c]])
                    }
                }
                return l
            }
        }, {}]
    }, {}, [1])(1)
});
$(".carousel").carousel(), $(window).on("load resize", function() {
    var s = $(".middle-block").height(),
        t = ($("#left").height(), $("#right").height(), $(".sidebar.sticky").width());
    $(this).width() > 991 ? ($(".sidebar").css("width", t + "px"), $("#left").css("height", s + "px"), $("#right").css("height", s + "px")) : ($("#left").css("height", "auto"), $("#right").css("height", "auto"));
    var i = {
        disabled: !1,
        className: "sticky",
        stateClassName: "is-sticky",
        fixedClass: "sticky-fixed",
        wrapperClass: "sticky-wrap",
        absoluteClass: "is-absolute"
    };
    StickyState.apply(document.querySelectorAll(".sticky"), i)
});