/* =======================================================================
 * jQuery.Huipreview.js v3.0.1 图片视频预览
 * http://www.h-ui.net/
 * Created & Modified by guojunhui
 * Date modified 2017.07.26
 *
 * Copyright 2017 北京颖杰联创科技有限公司 All rights reserved.
 * Licensed under MIT license.
 * http://opensource.org/licenses/MIT
 * ========================================================================*/
!function ($) {
    $.fn.Huipreview = function (options) {
        var defaults = {
            type: "image",
            className: "active",
            bigImgWidth: 300,
            top: 0,
        }
        var options = $.extend(defaults, options);
        this.each(function () {
            var self = $(this);
            var timer;
            self.hover(
                function () {
                    clearTimeout(timer);
                    timer = setTimeout(function () {
                        $("#preview-wraper").remove();
                        var img = self.find("img");
                        if (img.length == 0) {
                            img = self.parent().find(".up-img");
                        }
                        var smallImg = img.attr('src');
                        var bigImg = img.attr('src');
                        var bigImgW = img.attr('width');
                        var bigImgH = img.attr('height');
                        var winW = $(window).width();
                        var winW5 = winW / 2;
                        var imgT = self.parent().offset().top - options.top;
                        var imgL = self.parent().offset().left;
                        var imgW = self.parent().width();
                        var imgH = self.parent().height();
                        var ww = (imgL + imgW / 2);
                        var tooltipLeft = "auto", tooltipRight = "auto";
                        if (ww < winW5) {
                            tooltipLeft = (imgW + imgL) + "px";
                        } else {
                            tooltipRight = (winW - imgL) + "px";
                        }

                        self.addClass(options.className);
                        if (bigImg == '') {
                            return false;
                        } else {
                            var tooltip_keleyi_com =
                                '<div id="preview-wraper" style="position: absolute;z-index:999;width:' + options.bigImgWidth + 'px;height:auto;top:' + imgT + 'px;right:' + tooltipRight + ';left:' + tooltipLeft + '">';
                            if (options.type == "video") {
                                tooltip_keleyi_com +=
                                    '<video id="banner-video" width="100%" autoplay loop>' +
                                    '<source type="video/mp4" src="' + bigImg + '" />' +
                                    '<object width="100%" type="http://lib.h-ui.net/flashmediaelement.swf">' +
                                    '<param name="movie" value="http://lib.h-ui.net/flashmediaelement.swf" />' +
                                    '<param name="flashvars" value="' + midimg + '" />' +
                                    '</object>' +
                                    '</video>';
                            } else {
                                tooltip_keleyi_com += '<img src="' + smallImg + '" width="' + options.bigImgWidth + '">';
                            }

                            tooltip_keleyi_com += '</div>';

                            $("body").append(tooltip_keleyi_com);
                            if (options.type == "image") {
                                /*图片预加载*/
                                var image = new Image();
                                image.src = bigImg;
                                /*创建一个Image对象*/
                                image.onload = function () {
                                    $('#preview-wraper').find("img").attr("src", bigImg).css("width", options.bigImgWidth);
                                };
                            }
                        }
                    }, 10);
                },
                function () {
                    clearTimeout(timer);
                    self.removeClass(options.className);
                    $("#preview-wraper").remove();
                }
            );
        });
    }
}(window.jQuery);