﻿(function () {
	"use strict"
	var root = this;
	if (typeof root.GOVUK === 'undefined') {
		root.GOVUK = {};
	}	

	function getDomain() {
		var host = window.location.hostname;
		if (typeof host !== "undefined" && host !== null && host.length > 0) {
			return host;
		}
		else {
			console.log("The host name is undefined or null!");
			return null;
		}
	}

	GOVUK.cookie = function (name, value, options) {
		if (typeof value !== 'undefined') {
			if (value === false || value === null) {
				return GOVUK.setCookie(name, '', {
					days: -1
				});
			} else {
				return GOVUK.setCookie(name, value, options);
			}
		} else {
			return GOVUK.getCookie(name);
		}
	};
	GOVUK.setCookie = function (name, value, options) {
		if (typeof options === 'undefined') {
			options = {};
		}
		var currentDomain = getDomain();
		var cookieString = name + "=" + value + ";domain=" + currentDomain + "; path=/";
		if (options.days) {
			var date = new Date();
			date.setTime(date.getTime() + (options.days * 24 * 60 * 60 * 1000));
			cookieString = cookieString + "; expires=" + date.toGMTString();
		}
		if (document.location.protocol == 'https:') {
			cookieString = cookieString + "; Secure";
		}
		document.cookie = cookieString;
	};
	GOVUK.getCookie = function (name) {
		var nameEQ = name + "=";
		var cookies = document.cookie.split(';');
		for (var i = 0, len = cookies.length; i < len; i++) {
			var cookie = cookies[i];
			while (cookie.charAt(0) == ' ') {
				cookie = cookie.substring(1, cookie.length);
			}
			if (cookie.indexOf(nameEQ) === 0) {
				return decodeURIComponent(cookie.substring(nameEQ.length));
			}
		}
		return null;
	};

	GOVUK.acceptCookies = function () {
		GOVUK.cookie('iws-cookie-consent', 'yes', { days: 28 });
		document.getElementById("global-cookie-message").style.display = "none";
		document.getElementById("global-cookie-accept-message").style.display = "block";
	}

	GOVUK.rejectCookies = function () {
		GOVUK.cookie('iws-cookie-consent', 'no', { days: 28 });
		document.getElementById("global-cookie-message").style.display = "none";
		document.getElementById("global-cookie-reject-message").style.display = "block";
		GOVUK.cookie('_ga', null);
		GOVUK.cookie('_gid', null);
		GOVUK.cookie('_gat', null);
		GOVUK.cookie('_gat_govuk_shared', null);
	}
}).call(this);
(function () {
	"use strict"

	// fix for printing bug in Windows Safari
	var windowsSafari = (window.navigator.userAgent.match(/(\(Windows[\s\w\.]+\))[\/\(\s\w\.\,\)]+(Version\/[\d\.]+)\s(Safari\/[\d\.]+)/) !== null),
		style;

	if (windowsSafari) {
		// set the New Transport font to Arial for printing
		style = document.createElement('style');
		style.setAttribute('type', 'text/css');
		style.setAttribute('media', 'print');
		style.innerHTML = '@font-face { font-family: nta !important; src: local("Arial") !important; }';
		document.getElementsByTagName('head')[0].appendChild(style);
	}

	// header navigation toggle
	if (document.querySelectorAll && document.addEventListener) {
		var els = document.querySelectorAll('.js-header-toggle'),
			i, _i;
		for (i = 0, _i = els.length; i < _i; i++) {
			els[i].addEventListener('click', function (e) {
				e.preventDefault();
				var target = document.getElementById(this.getAttribute('href').substr(1)),
					targetClass = target.getAttribute('class') || '',
					sourceClass = this.getAttribute('class') || '';

				if (targetClass.indexOf('js-visible') !== -1) {
					target.setAttribute('class', targetClass.replace(/(^|\s)js-visible(\s|$)/, ''));
				} else {
					target.setAttribute('class', targetClass + " js-visible");
				}
				if (sourceClass.indexOf('js-hidden') !== -1) {
					this.setAttribute('class', sourceClass.replace(/(^|\s)js-hidden(\s|$)/, ''));
				} else {
					this.setAttribute('class', sourceClass + " js-hidden");
				}
			});
		}
	}
}).call(this);