/**
 * jQuery.share - social media sharing plugin
 * ---
 * @author Carol Skelly (http://in1.com)
 * @version 1.0
 * @license MIT license (http://opensource.org/licenses/mit-license.php)
 * ---
 */

;(function ( $, window, undefined ) {
    
    var document = window.document;

    $.fn.share = function(method) {

        var methods = {

            init : function(options) {
                this.share.settings = $.extend({}, this.share.defaults, options);
                var settings = this.share.settings,
                    networks = this.share.settings.networks,
                    theme = this.share.settings.theme,
                    orientation = this.share.settings.orientation,
                    affix = this.share.settings.affix,
                    margin = this.share.settings.margin,
                    pageTitle = this.share.settings.title||$(document).attr('title'),
                    pageUrl = this.share.settings.urlToShare||$(location).attr('href'),
                    pageDesc = "";
                
                $.each($(document).find('meta[name="description"]'),function(idx,item){
                    pageDesc = $(item).attr("content");
        		});
                
                // each instance of this plugin
                return this.each(function () {
                    var $element = $(this),
                        id=$element.attr("id"),
                        u=encodeURIComponent(pageUrl),
                        t = encodeURIComponent(pageTitle),
                        d=pageDesc!=null? pageDesc.substring(0,250) : "",
                        href;

                    // append HTML for each network button
                   
                    var sochtm = "<ul class=\"soc\" style=\"padding-left: 0px;\">";
                    for (var item in networks) {
                        item = networks[item];
                        href = helpers.networkDefs[item].url;
                        cls = helpers.networkDefs[item].cls;
                        sv = helpers.networkDefs[item].s;
                        href = href.replace('|u|',u).replace('|t|',t).replace('|d|',d)
                                   .replace('|140|', t.substring(0, 130));
                        
                        sochtm += "<li><a href='" + href + "' title='Share this page on " + item +
                            "' class='pop " + cls + "'>" + sv + "</a></li>";
                    }
                    sochtm += "</ul>";

                    $(sochtm).appendTo($element);
                    
                    // customize css
                    $("#"+id+".share-"+theme).css('margin',margin);
                    
                    if (orientation != "horizontal"){
                        $("#"+id+" a.share-"+theme).css('display','block');
                    }
                    else {
                        $("#"+id+" a.share-"+theme).css('display','inline-block');
                    }
                    
                    if (typeof affix != "undefined"){
                        $element.addClass('share-affix');
                        if (affix.indexOf('right')!=-1){
                            $element.css('left','auto');
                            $element.css('right','0px');
                            if (affix.indexOf('center')!=-1){
                                $element.css('top','40%');
                            }
                        }
                        else if (affix.indexOf('left center')!=-1){
                            $element.css('top','40%');
                        }
                        
                        if (affix.indexOf('bottom')!=-1){
                            $element.css('bottom','0px');
                            $element.css('top','auto');
                            if (affix.indexOf('center')!=-1){
                                $element.css('left','40%');
                            }
                        }
                    }
                    
                    // bind click
                    $('.pop').click(function(){
                        window.open($(this).attr('href'),'t','toolbar=0,resizable=1,status=0,width=640,height=528');
                        return false;
                    });
                    
                    
                });// end plugin instance
            
            }        
        }

        var helpers = {
            networkDefs: {
                facebook: { url: 'http://www.facebook.com/share.php?u=|u|', cls: "icon-26", s: "<svg viewBox=\"0 0 512 512\"><path d=\"M211.9 197.4h-36.7v59.9h36.7V433.1h70.5V256.5h49.2l5.2-59.1h-54.4c0 0 0-22.1 0-33.7 0-13.9 2.8-19.5 16.3-19.5 10.9 0 38.2 0 38.2 0V82.9c0 0-40.2 0-48.8 0 -52.5 0-76.1 23.1-76.1 67.3C211.9 188.8 211.9 197.4 211.9 197.4z\" /></svg>" },
                //http://twitter.com/home?status=jQuery%20Share%20Social%20Media%20Plugin%20-%20Share%20to%20multiple%20social%20networks%20from%20a%20single%20form%20http://plugins.in1.com/share/demo
                    twitter: { url: 'https://twitter.com/share?url=|u|&text=|140|', cls: "icon-10", s: "<svg viewBox=\"0 0 512 512\"><path d=\"M419.6 168.6c-11.7 5.2-24.2 8.7-37.4 10.2 13.4-8.1 23.8-20.8 28.6-36 -12.6 7.5-26.5 12.9-41.3 15.8 -11.9-12.6-28.8-20.6-47.5-20.6 -42 0-72.9 39.2-63.4 79.9 -54.1-2.7-102.1-28.6-134.2-68 -17 29.2-8.8 67.5 20.1 86.9 -10.7-0.3-20.7-3.3-29.5-8.1 -0.7 30.2 20.9 58.4 52.2 64.6 -9.2 2.5-19.2 3.1-29.4 1.1 8.3 25.9 32.3 44.7 60.8 45.2 -27.4 21.4-61.8 31-96.4 27 28.8 18.5 63 29.2 99.8 29.2 120.8 0 189.1-102.1 185-193.6C399.9 193.1 410.9 181.7 419.6 168.6z\" /></svg>" },
                    linkedin: { url: 'http://www.linkedin.com/shareArticle?mini=true&url=|u|&title=|t|&summary=|d|&source=in1.com', cls: "icon-17", s: "<svg viewBox=\"0 0 512 512\"><path d=\"M186.4 142.4c0 19-15.3 34.5-34.2 34.5 -18.9 0-34.2-15.4-34.2-34.5 0-19 15.3-34.5 34.2-34.5C171.1 107.9 186.4 123.4 186.4 142.4zM181.4 201.3h-57.8V388.1h57.8V201.3zM273.8 201.3h-55.4V388.1h55.4c0 0 0-69.3 0-98 0-26.3 12.1-41.9 35.2-41.9 21.3 0 31.5 15 31.5 41.9 0 26.9 0 98 0 98h57.5c0 0 0-68.2 0-118.3 0-50-28.3-74.2-68-74.2 -39.6 0-56.3 30.9-56.3 30.9v-25.2H273.8z\" /></svg>" },
                //in1: { url: 'http://www.in1.com/cast?u=|u|', w: '490', h: '529', svg: "" },
                //tumblr: { url: 'http://www.tumblr.com/share?v=3&u=|u|', svg: "" },
                //digg: { url: 'http://digg.com/submit?url=|u|&title=|t|', svg: "" },
                    googleplus: { url: 'https://plusone.google.com/_/+1/confirm?hl=en&url=|u|', cls: "icon-14", s: "<svg viewBox=\"0 0 512 512\"><path d=\"M179.7 237.6L179.7 284.2 256.7 284.2C253.6 304.2 233.4 342.9 179.7 342.9 133.4 342.9 95.6 304.4 95.6 257 95.6 209.6 133.4 171.1 179.7 171.1 206.1 171.1 223.7 182.4 233.8 192.1L270.6 156.6C247 134.4 216.4 121 179.7 121 104.7 121 44 181.8 44 257 44 332.2 104.7 393 179.7 393 258 393 310 337.8 310 260.1 310 251.2 309 244.4 307.9 237.6L179.7 237.6 179.7 237.6ZM468 236.7L429.3 236.7 429.3 198 390.7 198 390.7 236.7 352 236.7 352 275.3 390.7 275.3 390.7 314 429.3 314 429.3 275.3 468 275.3\" /></svg>" },
                //reddit: { url: 'http://reddit.com/submit?url=|u|', svg: "" },
                //pinterest: { url: 'http://pinterest.com/pin/create/button/?url=|u|&media=&description=|d|', svg: "" },
                //posterous: { url: 'http://posterous.com/share?linkto=|u|&title=|t|', svg: "" },
                //stumbleupon: { url: 'http://www.stumbleupon.com/submit?url=|u|&title=|t|', svg: "" },
                //email: { url: 'mailto:?subject=|t|', svg: "" }
            }
        }
     
        if (methods[method]) {
            return methods[method].apply(this, Array.prototype.slice.call(arguments, 1));
        } else if (typeof method === 'object' || !method) {
            return methods.init.apply(this, arguments);
        } else {
            $.error( 'Method "' +  method + '" does not exist in social plugin');
        }

    }

    $.fn.share.defaults = {
        networks: ['facebook','twitter','linkedin'],
        theme: 'icon', // use round icons sprite
        autoShow: true,
        margin: '3px',
        orientation: 'horizontal',
        useIn1: false
    }

    $.fn.share.settings = {}
        
})(jQuery, window);
