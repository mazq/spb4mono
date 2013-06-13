
(function ($) {

    var tabId = 0,
	listId = 0;

    $.widget("ui.spbtabs", {
        options: {
            add: null,
            ajaxOptions: null,
            cache: false,
            cookie: null, // e.g. { expires: 7, path: '/', domain: 'jquery.com', secure: true }
            collapsible: false,
            disable: null,
            disabled: [],
            enable: null,
            event: 'click',
            fx: null, // e.g. { height: 'toggle', opacity: 'toggle', duration: 200 }
            idPrefix: 'spb-tabs-',
            load: null,
            panelTemplate: '<div></div>',
            remove: null,
            select: null,
            show: null,
            spinner: '<em>Loading&#8230;</em>',
            tabTemplate: '<li><a href="#{href}"><span>#{label}</span></a></li>'
        },
        _create: function () {
            this._tabify(true);
        },

        _tabId: function (a) {
            return a.title && a.title.replace(/\s/g, '_').replace(/[^A-Za-z0-9\-_:\.]/g, '') ||
			this.options.idPrefix + (++tabId);
        },

        _sanitizeSelector: function (hash) {
            return hash.replace(/:/g, '\\:'); // we need this because an id may contain a ":"
        },

        _cookie: function () {
            var cookie = this.cookie || (this.cookie = this.options.cookie.name || 'spb-tabs-' + (++listId));
            return $.cookie.apply(null, [cookie].concat($.makeArray(arguments)));
        },

        _ui: function (tab, panel) {
            return {
                tab: tab,
                panel: panel,
                index: this.anchors.index(tab)
            };
        },

        _cleanup: function () {
            // restore all former loading tabs labels
            this.lis.filter('.ui-state-processing').removeClass('ui-state-processing')
				.find('span:data(label.spbtabs)')
				.each(function () {
				    var el = $(this);
				    el.html(el.data('label.spbtabs')).removeData('label.spbtabs');
				});
        },

        _tabify: function (init) {

            this.list = this.element.find('ol,ul').eq(0);
            this.lis = $('li:has(a[href])', this.list);
            this.anchors = this.lis.map(function () { return $('a', this)[0]; });
            this.panels = $([]);

            var self = this, o = this.options;

            var fragmentId = /^#.+/; // Safari 2 reports '#' for an empty hash
            this.anchors.each(function (i, a) {
                var href = $(a).attr('href');

                // For dynamically created HTML that contains a hash as href IE < 8 expands
                // such href to the full page url with hash and then misinterprets tab as ajax.
                // Same consideration applies for an added tab with a fragment identifier
                // since a[href=#fragment-identifier] does unexpectedly not match.
                // Thus normalize href attribute...
                var hrefBase = href.split('#')[0], baseEl;
                if (hrefBase && (hrefBase === location.toString().split('#')[0] ||
					(baseEl = $('base')[0]) && hrefBase === baseEl.href)) {
                    href = a.hash;
                    a.href = href;
                }

                // inline tab
                if (fragmentId.test(href)) {
                    self.panels = self.panels.add(self._sanitizeSelector(href));
                }

                // remote tab
                else if (href != '#') { // prevent loading the page itself if href is just "#"
                    $.data(a, 'href.spbtabs', href); // required for restore on destroy

                    // TODO until #3808 is fixed strip fragment identifier from url
                    // (IE fails to load from such url)
                    $.data(a, 'load.spbtabs', href.replace(/#.*$/, '')); // mutable data

                    var id = self._tabId(a);
                    a.href = '#' + id;
                    var $panel = $('#' + id);
                    if (!$panel.length) {
                        $panel = $("<div id=\"" + id + "\"></div>").addClass('tn-tabs-panel')
						.insertAfter(self.panels[i - 1] || self.list);
                        $panel.data('destroy.spbtabs', true);
                    }
                    self.panels = self.panels.add($panel);
                }

                // invalid tab href
                else {
                    o.disabled.push(i);
                }
            });

            // initialization from scratch
            if (init) {
                // attach necessary classes for styling
                this.element.addClass('tn-tabs');
                this.list.addClass('tn-tabs-nav tn-border-gray tn-border-bottom tn-helper-clearfix');
                this.lis.addClass('tn-widget-content tn-border-gray tn-border-trl');
                this.panels.addClass('tn-tabs-panel');

                // Selected tab
                // use "selected" option or try to retrieve:
                // 1. from fragment identifier in url
                // 2. from cookie
                // 3. from selected class attribute on <li>
                if (o.selected === undefined) {
                    if (location.hash) {
                        this.anchors.each(function (i, a) {
                            if (a.hash == location.hash) {
                                o.selected = i;
                                return false; // break
                            }
                        });
                    }
                    if (typeof o.selected != 'number' && o.cookie) {
                        o.selected = parseInt(self._cookie(), 10);
                    }
                    if (typeof o.selected != 'number' && this.lis.filter('.tn-tabs-selected').length) {
                        o.selected = this.lis.index(this.lis.filter('.tn-tabs-selected'));
                    }
                    o.selected = o.selected || (this.lis.length ? 0 : -1);
                }
                else if (o.selected === null) { // usage of null is deprecated, TODO remove in next release
                    o.selected = -1;
                }

                // sanity check - default to first tab...
                o.selected = ((o.selected >= 0 && this.anchors[o.selected]) || o.selected < 0) ? o.selected : 0;

                // Take disabling tabs via class attribute from HTML
                // into account and update option properly.
                // A selected tab cannot become disabled.
                o.disabled = $.unique(o.disabled.concat(
				$.map(this.lis.filter('.ui-state-disabled'),
					function (n, i) { return self.lis.index(n); })
			)).sort();

                if ($.inArray(o.selected, o.disabled) != -1) {
                    o.disabled.splice($.inArray(o.selected, o.disabled), 1);
                }

                // highlight selected tab
                this.panels.hide();
                this.lis.removeClass('tn-tabs-selected');
                if (o.selected >= 0 && this.anchors.length) { // check for length avoids error when initializing empty list
                    this.panels.eq(o.selected).show();
                    this.lis.eq(o.selected).addClass('tn-tabs-selected');

                    // seems to be expected behavior that the show callback is fired
                    self.element.queue("spbtabs", function () {
                        self._trigger('show', null, self._ui(self.anchors[o.selected], self.panels[o.selected]));
                    });

                    this.load(o.selected);
                }

                // clean up to avoid memory leaks in certain versions of IE 6
                $(window).bind('unload', function () {
                    self.lis.add(self.anchors).unbind('.spbtabs');
                    self.lis = self.anchors = self.panels = null;
                });

            }
            // update selected after add/remove
            else {
                o.selected = this.lis.index(this.lis.filter('.tn-tabs-selected'));
            }

            // set or update cookie after init and add/remove respectively
            if (o.cookie) {
                this._cookie(o.selected, o.cookie);
            }

            // disable tabs
            for (var i = 0, li; (li = this.lis[i]); i++) {
                $(li)[$.inArray(i, o.disabled) != -1 &&
				!$(li).hasClass('tn-tabs-selected') ? 'addClass' : 'removeClass']('ui-state-disabled');
            }

            // reset cache if switching from cached to not cached
            if (o.cache === false) {
                this.anchors.removeData('cache.spbtabs');
            }

            // remove all handlers before, tabify may run on existing tabs after add or option change
            this.lis.add(this.anchors).unbind('.spbtabs');

            // set up animations
            var hideFx, showFx;
            if (o.fx) {
                if ($.isArray(o.fx)) {
                    hideFx = o.fx[0];
                    showFx = o.fx[1];
                }
                else {
                    hideFx = showFx = o.fx;
                }
            }

            // Reset certain styles left over from animation
            // and prevent IE's ClearType bug...
            function resetStyle($el, fx) {
                $el.css({ display: '' });
                if (!$.support.opacity && fx.opacity) {
                    $el[0].style.removeAttribute('filter');
                }
            }

            // Show a tab...
            var showTab = showFx ?
			function (clicked, $show) {
			    $(clicked).closest('li').addClass('tn-tabs-selected');
			    $show.hide().show() // avoid flicker that way
					.animate(showFx, showFx.duration || 'normal', function () {
					    resetStyle($show, showFx);
					    self._trigger('show', null, self._ui(clicked, $show[0]));
					});
			} :
			function (clicked, $show) {
			    $(clicked).closest('li').addClass('tn-tabs-selected');
			    $show.show();
			    self._trigger('show', null, self._ui(clicked, $show[0]));
			};

            // Hide a tab, $show is optional...
            var hideTab = hideFx ?
			function (clicked, $hide) {
			    $hide.animate(hideFx, hideFx.duration || 'normal', function () {
			        self.lis.removeClass('tn-tabs-selected');
			        $hide.hide();
			        resetStyle($hide, hideFx);
			        self.element.dequeue("spbtabs");
			    });
			} :
			function (clicked, $hide, $show) {
			    self.lis.removeClass('tn-tabs-selected');
			    $hide.hide();
			    self.element.dequeue("spbtabs");
			};

            // attach tab event handler, unbind to avoid duplicates from former tabifying...
            this.anchors.bind(o.event + '.spbtabs', function () {
                var el = this, $li = $(this).closest('li'), $hide = self.panels.filter(':not(.ui-tabs-hide)'),
					$show = $(self._sanitizeSelector(this.hash));

                // If tab is already selected and not collapsible or tab disabled or
                // or is already loading or click callback returns false stop here.
                // Check if click handler returns false last so that it is not executed
                // for a disabled or loading tab!
                if (($li.hasClass('tn-tabs-selected') && !o.collapsible) ||
				$li.hasClass('ui-state-disabled') ||
				$li.hasClass('ui-state-processing') ||
				self._trigger('select', null, self._ui(this, $show[0])) === false) {
                    this.blur();
                    return false;
                }

                o.selected = self.anchors.index(this);

                self.abort();

                // if tab may be closed
                if (o.collapsible) {
                    if ($li.hasClass('tn-tabs-selected')) {
                        o.selected = -1;

                        if (o.cookie) {
                            self._cookie(o.selected, o.cookie);
                        }

                        self.element.queue("spbtabs", function () {
                            hideTab(el, $hide);
                        }).dequeue("spbtabs");

                        this.blur();
                        return false;
                    }
                    else if (!$hide.length) {
                        if (o.cookie) {
                            self._cookie(o.selected, o.cookie);
                        }

                        self.element.queue("spbtabs", function () {
                            showTab(el, $show);
                        });

                        self.load(self.anchors.index(this)); // TODO make passing in node possible, see also http://dev.jqueryui.com/ticket/3171

                        this.blur();
                        return false;
                    }
                }

                if (o.cookie) {
                    self._cookie(o.selected, o.cookie);
                }

                // show new tab
                if ($show.length) {
                    if ($hide.length) {
                        self.element.queue("spbtabs", function () {
                            hideTab(el, $hide);
                        });
                    }
                    self.element.queue("spbtabs", function () {
                        showTab(el, $show);
                    });

                    self.load(self.anchors.index(this));
                }
                else {
                    throw 'jQuery UI Tabs: Mismatching fragment identifier.';
                }

                // Prevent IE from keeping other link focussed when using the back button
                // and remove dotted border from clicked link. This is controlled via CSS
                // in modern browsers; blur() removes focus from address bar in Firefox
                // which can become a usability and annoying problem with tabs('rotate').
                if ($.browser.msie) {
                    this.blur();
                }

            });

            // disable click in any case
            this.anchors.bind('click.spbtabs', function () { return false; });
        },
        select: function (index) {
            if (typeof index == 'string') {
                index = this.anchors.index(this.anchors.filter('[href$=' + index + ']'));
            }
            else if (index === null) { // usage of null is deprecated, TODO remove in next release
                index = -1;
            }
            if (index == -1 && this.options.collapsible) {
                index = this.options.selected;
            }

            this.anchors.eq(index).trigger(this.options.event + '.spbtabs');
            return this;
        },

        load: function (index) {
            var self = this, o = this.options, a = this.anchors.eq(index)[0], url = $.data(a, 'load.spbtabs');

            this.abort();

            // not remote or from cache
            if (!url || this.element.queue("spbtabs").length !== 0 && $.data(a, 'cache.spbtabs')) {
                this.element.dequeue("spbtabs");
                return;
            }

            // load remote from here on
            this.lis.eq(index).addClass('ui-state-processing');

            if (o.spinner) {
                var span = $('span', a);
                span.data('label.spbtabs', span.html()).html(o.spinner);
            }

            this.xhr = $.ajax($.extend({}, o.ajaxOptions, {
                url: url,
                success: function (r, s) {
                    $(self._sanitizeSelector(a.hash)).html(r);

                    // take care of tab labels
                    self._cleanup();

                    if (o.cache) {
                        $.data(a, 'cache.spbtabs', true); // if loaded once do not load them again
                    }

                    // callbacks
                    self._trigger('load', null, self._ui(self.anchors[index], self.panels[index]));
                    try {
                        o.ajaxOptions.success(r, s);
                    }
                    catch (e) { }
                },
                error: function (xhr, s, e) {
                    // take care of tab labels
                    self._cleanup();

                    // callbacks
                    self._trigger('load', null, self._ui(self.anchors[index], self.panels[index]));
                    try {
                        // Passing index avoid a race condition when this method is
                        // called after the user has selected another tab.
                        // Pass the anchor that initiated this request allows
                        // loadError to manipulate the tab content panel via $(a.hash)
                        o.ajaxOptions.error(xhr, s, index, a);
                    }
                    catch (e) { }
                }
            }));

            // last, so that load event is fired before show...
            self.element.dequeue("spbtabs");

            return this;
        },

        abort: function () {
            // stop possibly running animations
            this.element.queue([]);
            this.panels.stop(false, true);

            // "spbtabs" queue must not contain more than two elements,
            // which are the callbacks for the latest clicked tab...
            this.element.queue("spbtabs", this.element.queue("spbtabs").splice(-2, 2));

            // terminate pending requests from other tabs
            if (this.xhr) {
                this.xhr.abort();
                delete this.xhr;
            }

            // take care of tab labels
            this._cleanup();
            return this;
        },

        url: function (index, url) {
            this.anchors.eq(index).removeData('cache.spbtabs').data('load.spbtabs', url);
            return this;
        },

        length: function () {
            return this.anchors.length;
        }
    });


})(jQuery);