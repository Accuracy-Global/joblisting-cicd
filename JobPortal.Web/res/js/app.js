

$(window).scroll(function () {
    var top = $(window).scrollTop();
    if (top >= 300) {
        $('nav').addClass('secondary');
        $('nav').addClass('fixed-top');
    }
    else
        if ($("nav").hasClass('secondary')) {
            $("nav").removeClass('secondary');
            $('nav').removeClass('fixed-top');

        }
});     
  $(function() {
    $(".toggle").on("click", function() {
        if ($(".item").hasClass("active")) {
            $(".item").removeClass("active");
        } else {
            $(".item").addClass("active");
        }
    });
});
$(document).ready(function(){
  $('.counter').counterUp({
    delay: 10,
    time: 1200
  });
});

// Navbar
jQuery(document).ready(function ($) {
    "use strict";
  
    var navclone = function () {
      $(".js-clone-nav").each(function () {
        var $this = $(this);
        $this.clone().attr("class", "clone-view").appendTo(".mobile-view-body");
      });
  
      $("body").on("click", ".js-toggle", function (e) {
        var $this = $(this);
        e.preventDefault();
  
        if ($("body").hasClass("off-view")) {
          $("body").removeClass("off-view");
        } else {
          $("body").addClass("off-view");
        }
      });
  
      $(document).mouseup(function (e) {
        var container = $(".mobile-view");
        if (!container.is(e.target) && container.has(e.target).length === 0) {
          if ($("body").hasClass("off-view")) {
            $("body").removeClass("off-view");
          }
        }
      });
  
      $(window).resize(function () {
        var $this = $(this),
          w = $this.width();
        if (w > 768) {
          if ($("body").hasClass("off-view")) {
            $("body").removeClass("off-view");
          }
        }
      });
    };
    navclone();
  });
  
