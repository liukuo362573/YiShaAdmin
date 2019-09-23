(function ($) {
    "use strict";

    // Sticky menu 
    var $window = $(window);
    $window.on('scroll', function () {
        var scroll = $window.scrollTop();
        if (scroll < 300) {
            $(".sticky").removeClass("is-sticky");
        } else {
            if ($(".mobile-menu").is(":hidden")) {
                $(".sticky").addClass("is-sticky");
            }
        }
    });


    // slide effect dropdown
    function dropdownAnimation() {
        $('.dropdown').on('show.bs.dropdown', function (e) {
            $(this).find('.dropdown-menu').first().stop(true, true).slideDown(500);
        });

        $('.dropdown').on('hide.bs.dropdown', function (e) {
            $(this).find('.dropdown-menu').first().stop(true, true).slideUp(500);
        });
    }

    dropdownAnimation();


    // mini cart toggler
    $(".mini-cart-wrap button").on("click", function (event) {
        event.stopPropagation();
        event.preventDefault();
        $(".cart-list").slideToggle();
    });


    // responsive menu js
    jQuery('#mobile-menu').meanmenu({
        meanMenuContainer: '.mobile-menu',
        meanScreenWidth: "991"
    });


    // tooltip active js
    $('[data-toggle="tooltip"]').tooltip();


    // Hero main slider active js
    $('.hero-slider-active').slick({
        autoplay: true,
        infinite: true,
        fade: true,
        dots: false,
        arrows: true,
        prevArrow: '<button type="button" class="slick-prev"><i class="fa fa-angle-left"></i></button>',
        nextArrow: '<button type="button" class="slick-next"><i class="fa fa-angle-right"></i></button>',
        responsive: [{
            breakpoint: 768,
            settings: {
                arrows: false,
            }
        }]
    });


    // blog carousel active-2 js
    $('.blog-slider').slick({
        autoplay: true,
        infinite: true,
        fade: false,
        dots: false,
        arrows: false,
        slidesToShow: 2,
        responsive: [
            {
                breakpoint: 768,
                settings: {
                    slidesToShow: 1,
                }
            },
        ]
    });


    // brand slider active js
    var brand = $('.brand-active');
    brand.slick({
        infinite: true,
        arrows: false,
        autoplay: true,
        speed: 1000,
        pauseOnFocus: false,
        pauseOnHover: false,
        slidesToShow: 6,
        prevArrow: '<button type="button" class="slick-prev"><i class="fa fa-angle-left"></i></button>',
        nextArrow: '<button type="button" class="slick-next"><i class="fa fa-angle-right"></i></button>',
        responsive: [{
            breakpoint: 992,
            settings: {
                slidesToShow: 4,
            }
        },
        {
            breakpoint: 768,
            settings: {
                slidesToShow: 3,
                arrows: false,
            }
        },
        {
            breakpoint: 575,
            settings: {
                slidesToShow: 2,
                arrows: false,
            }
        },
        {
            breakpoint: 479,
            settings: {
                slidesToShow: 1,
                arrows: false,
            }
        },
        ]
    });


    // product slider active js
    var brandEx = $('.product-carousel-active');
    brandEx.slick({
        infinite: true,
        arrows: true,
        autoplay: true,
        speed: 1000,
        pauseOnFocus: false,
        pauseOnHover: false,
        slidesToShow: 5,
        prevArrow: '<button type="button" class="slick-prev"><i class="fa fa-angle-left"></i></button>',
        nextArrow: '<button type="button" class="slick-next"><i class="fa fa-angle-right"></i></button>',
        responsive: [{
            breakpoint: 1200,
            settings: {
                slidesToShow: 4,
            }
        },
        {
            breakpoint: 992,
            settings: {
                slidesToShow: 3,
                arrows: false,
            }
        },
        {
            breakpoint: 768,
            settings: {
                slidesToShow: 2,
                arrows: false,
            }
        },
        {
            breakpoint: 479,
            settings: {
                slidesToShow: 1,
                arrows: false,
            }
        },
        ]
    });


    // product carousel two
    $('.product-carousel-active-2').each(function () {
        var $this = $(this);
        var $row = $this.attr("data-row") ? parseInt($this.attr("data-row"), 10) : 1;
        $this.slick({
            infinite: true,
            arrows: false,
            dots: false,
            slidesToShow: 2,
            slidesToScroll: 1,
            rows: $row,
            prevArrow: '<button class="slick-prev"><i class="fa fa-angle-left"></i></button>',
            nextArrow: '<button class="slick-next"><i class="fa fa-angle-right"></i></button>',
            responsive: [
                {
                    breakpoint: 992,
                    settings: {
                        slidesToShow: 1,
                        arrows: false,
                    }
                },
                {
                    breakpoint: 480,
                    settings: {
                        slidesToShow: 1,
                        arrows: false,
                    }
                },
            ]
        });
    });


    // best sellers carousel active js
    $('.product-carousel-active-3').each(function () {
        var $this = $(this);
        var $row = $this.attr("data-row") ? parseInt($this.attr("data-row"), 10) : 1;
        $this.slick({
            infinite: true,
            arrows: false,
            dots: false,
            rows: $row,
            prevArrow: '<button class="slick-prev"><i class="fa fa-angle-left"></i></button>',
            nextArrow: '<button class="slick-next"><i class="fa fa-angle-right"></i></button>',
            responsive: [
                {
                    breakpoint: 992,
                    settings: {
                        slidesToShow: 2,
                        arrows: false,
                    }
                },
                {
                    breakpoint: 768,
                    settings: {
                        slidesToShow: 1,
                        arrows: false,
                    }
                },
            ]
        });
    });


    // product slider active js
    var deal = $('.deal-carousel-active');
    deal.slick({
        infinite: true,
        arrows: true,
        autoplay: false,
        speed: 1000,
        centerMode: true,
        centerPadding: 0,
        pauseOnFocus: false,
        pauseOnHover: false,
        slidesToShow: 3,
        prevArrow: '<button type="button" class="slick-prev"><i class="fa fa-angle-left"></i></button>',
        nextArrow: '<button type="button" class="slick-next"><i class="fa fa-angle-right"></i></button>',
        responsive: [
            {
                breakpoint: 992,
                settings: {
                    slidesToShow: 2,
                    arrows: false,
                    centerMode: false,
                }
            },
            {
                breakpoint: 576,
                settings: {
                    slidesToShow: 1,
                    arrows: false,
                }
            },
        ]
    });


    // product slider active js
    var related = $('.related-product-active');
    related.slick({
        infinite: true,
        arrows: true,
        autoplay: false,
        speed: 1000,
        slidesToShow: 4,
        prevArrow: '<button type="button" class="slick-prev"><i class="fa fa-angle-left"></i></button>',
        nextArrow: '<button type="button" class="slick-next"><i class="fa fa-angle-right"></i></button>',
        responsive: [
            {
                breakpoint: 992,
                settings: {
                    slidesToShow: 2,
                    arrows: false,
                    centerMode: false,
                }
            },
            {
                breakpoint: 576,
                settings: {
                    slidesToShow: 1,
                    arrows: false,
                }
            },
        ]
    });


    // prodct details slider active
    $('.product-large-slider').slick({
        slidesToShow: 1,
        slidesToScroll: 1,
        fade: true,
        arrows: true,
        asNavFor: '.pro-nav',
        prevArrow: '<button type="button" class="slick-prev"><i class="fa fa-angle-left"></i></button>',
        nextArrow: '<button type="button" class="slick-next"><i class="fa fa-angle-right"></i></button>',
    });


    // product details slider nav active
    $('.pro-nav').slick({
        slidesToShow: 3,
        slidesToScroll: 1,
        asNavFor: '.product-large-slider',
        centerMode: true,
        arrows: true,
        centerPadding: 0,
        focusOnSelect: true,
        prevArrow: '<button type="button" class="slick-prev"><i class="fa fa-angle-left"></i></button>',
        nextArrow: '<button type="button" class="slick-next"><i class="fa fa-angle-right"></i></button>'
    });


    // prodct details slider active
    $('.product-box-slider').slick({
        autoplay: false,
        infinite: true,
        fade: false,
        dots: false,
        arrows: true,
        prevArrow: '<button type="button" class="slick-prev"><i class="fa fa-angle-left"></i></button>',
        nextArrow: '<button type="button" class="slick-next"><i class="fa fa-angle-right"></i></button>',
        slidesToShow: 4,
        responsive: [
            {
                breakpoint: 992,
                settings: {
                    slidesToShow: 3,
                }
            },
            {
                breakpoint: 768,
                settings: {
                    slidesToShow: 2,
                }
            },
            {
                breakpoint: 480,
                settings: {
                    slidesToShow: 1,
                }
            },
        ]
    });


    // product details vertical slider active
    $('.product-large-slider2').slick({
        slidesToShow: 1,
        slidesToScroll: 1,
        fade: true,
        arrows: false,
        asNavFor: '.pro-nav2'
    });


    // product details vertical slider nav active
    $('.pro-nav2').slick({
        autoplay: true,
        slidesToShow: 4,
        slidesToScroll: 1,
        asNavFor: '.product-large-slider2',
        centerMode: true,
        arrows: false,
        vertical: true,
        centerPadding: 0,
        focusOnSelect: true,
        verticalSwiping: true
    });


    // testimonial carousel active js
    $('.testimonial-carousel-active').slick({
        autoplay: false,
        infinite: true,
        fade: false,
        dots: true,
        arrows: false,
        slidesToShow: 1
    });


    // blog gallery slider
    var gallery = $('.blog-gallery-slider');
    gallery.slick({
        arrows: true,
        autoplay: true,
        autoplaySpeed: 5000,
        pauseOnFocus: false,
        pauseOnHover: false,
        fade: true,
        dots: false,
        infinite: true,
        slidesToShow: 1,
        prevArrow: '<button type="button" class="slick-prev"><i class="fa fa-angle-left"></i></button>',
        nextArrow: '<button type="button" class="slick-next"><i class="fa fa-angle-right"></i></button>'
    });


    // sticky sidebar
    $('.sticky__sidebar').stickySidebar({
        topSpacing: 130,
        bottomSpacing: -20
    });


    // nice select active js
    $('select').niceSelect();


    // image zoom effect
    $('.img-zoom').zoom();


    // Masonary active
    $('.grid').imagesLoaded(function () {
        $('.grid').masonry({
            itemSelector: '.col-lg-6',
            columnWidth: 1
        });
    });


    // Countdown Activation
    $('[data-countdown]').each(function () {
        var $this = $(this), finalDate = $(this).data('countdown');
        $this.countdown(finalDate, function (event) {
            $this.html(event.strftime('<div class="single-countdown"><span class="single-countdown__time">%D</span><span class="single-countdown__text">Days</span></div><div class="single-countdown"><span class="single-countdown__time">%H</span><span class="single-countdown__text">Hrs</span></div><div class="single-countdown"><span class="single-countdown__time">%M</span><span class="single-countdown__text">Min</span></div><div class="single-countdown"><span class="single-countdown__time">%S</span><span class="single-countdown__text">Sec</span></div>'));
        });
    });


    // Sidebar Category
    var categoryChildren = $('.sidebar-category li .children');
    categoryChildren.slideUp();
    categoryChildren.parents('li').addClass('has-children');
    $('.sidebar-category').on('click', 'li.has-children > a', function (e) {
        if ($(this).parent().hasClass('has-children')) {
            if ($(this).siblings('ul:visible').length > 0) $(this).siblings('ul').slideUp();
            else {
                $(this).parents('li').siblings('li').find('ul:visible').slideUp();
                $(this).siblings('ul').slideDown();
            }
        }
        if ($(this).attr('href') === '#') {
            e.preventDefault();
            return false;
        }
    });


    // pricing filter
    var rangeSlider = $(".price-range"),
        amount = $("#amount"),
        minPrice = rangeSlider.data('min'),
        maxPrice = rangeSlider.data('max');
    rangeSlider.slider({
        range: true,
        min: minPrice,
        max: maxPrice,
        values: [minPrice, maxPrice],
        slide: function (event, ui) {
            amount.val("$" + ui.values[0] + " - $" + ui.values[1]);
        }
    });
    amount.val(" $" + rangeSlider.slider("values", 0) +
        " - $" + rangeSlider.slider("values", 1));


    // quantity change js
    $('.pro-qty').prepend('<span class="dec qtybtn">-</span>');
    $('.pro-qty').append('<span class="inc qtybtn">+</span>');
    $('.qtybtn').on('click', function () {
        var $button = $(this);
        var oldValue = $button.parent().find('input').val();
        if ($button.hasClass('inc')) {
            var newVal = parseFloat(oldValue) + 1;
        } else {
            // Don't allow decrementing below zero
            if (oldValue > 0) {
                newVal = parseFloat(oldValue) - 1;
            } else {
                newVal = 0;
            }
        }
        $button.parent().find('input').val(newVal);
    });


    // Checkout Page accordion
    $("#create_pwd").on("change", function () {
        $(".account-create").slideToggle("100");
    });

    $("#ship_to_different").on("change", function () {
        $(".ship-to-different").slideToggle("100");
    });


    // Payment Method Accordion
    $('input[name="paymentmethod"]').on('click', function () {
        var $value = $(this).attr('value');
        $('.payment-method-details').slideUp();
        $('[data-method="' + $value + '"]').slideDown();
    });


    // Mailchimp for dynamic newsletter
    $('#mc-form').ajaxChimp({
        language: 'en',
        callback: mailChimpResponse,
        // ADD YOUR MAILCHIMP URL BELOW HERE!
        url: 'https://company.us19.list-manage.com/subscribe/post?u=2f2631cacbe4767192d339ef2&amp;id=24db23e68a'

    });


    // mailchimp active js
    function mailChimpResponse(resp) {
        if (resp.result === 'success') {
            $('.mailchimp-success').html('' + resp.msg).fadeIn(900);
            $('.mailchimp-error').fadeOut(400);

        } else if (resp.result === 'error') {
            $('.mailchimp-error').html('' + resp.msg).fadeIn(900);
        }
    }


    // google map
    var map_id = $('#map_content');
    if (map_id.length > 0) {
        var $lat = map_id.data('lat'),
            $lng = map_id.data('lng'),
            $zoom = map_id.data('zoom'),
            $maptitle = map_id.data('maptitle'),
            $mapaddress = map_id.data('mapaddress'),
            mymap = L.map('map_content').setView([$lat, $lng], $zoom);

        L.tileLayer('https://api.tiles.mapbox.com/v4/{id}/{z}/{x}/{y}.png?access_token={accessToken}', {
            attribution: 'Map',
            maxZoom: 18,
            id: 'mapbox.streets',
            scrollWheelZoom: false,
            accessToken: 'pk.eyJ1Ijoic2hha2lsYWhtbWVlZCIsImEiOiJjamk4anF6NDgwMGd5M3BwM2c4eHU5dmIzIn0.yBLGUAB8kV1I1yGGonxzzg'
        }).addTo(mymap);

        var marker = L.marker([$lat, $lng]).addTo(mymap);
        marker.bindPopup('<b>' + $maptitle + '</b><br>' + $mapaddress).openPopup();
        mymap.scrollWheelZoom.disable();
    }

    // scroll to top
    $(window).on('scroll', function () {
        if ($(this).scrollTop() > 600) {
            $('.scroll-top').removeClass('not-visible');
        } else {
            $('.scroll-top').addClass('not-visible');
        }
    });
    $('.scroll-top').on('click', function (event) {
        $('html,body').animate({
            scrollTop: 0
        }, 1000);
    });


}(jQuery));