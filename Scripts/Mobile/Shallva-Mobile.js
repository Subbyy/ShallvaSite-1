<<<<<<< HEAD
﻿$(function () {
    $('.main-block').each(function (index, block) {
        if (index % 2 == 0) {
            $(block).attr('style', 'background-color: brown !important');
        }  
        if (index % 3 == 0) {
            $(block).attr('style', 'background-color: peru !important');
        }
        if (index % 4 == 0) {
            $(block).attr('style', 'background-color: saddlebrown !important');
        }
    });
});
=======
﻿$(function () {

    if (!$('#welcome').length) {
        $('body').css("padding", "70px 0 70px");
    } 



    $('#menu #products > a').click(function () {
        if ($('#menu ul ul#level-0').is(':visible')) {
            $(this).siblings('ul').slideUp("slow", function () {
                //background-color: #E87655;
                $('#menu #products > a').css({
                    'background-color': '#fff',
                    'color': '#E87655'
                });
                $(this).unbind('mouseenter mouseleave');
            });
        }
        else {
            $('#menu #products > a').css({
                'background-color': '#E87655',
                'color': '#fff'
            });
            $(this).siblings('ul').slideDown("slow");
        }
    });

    $('#menu ul#level-0 li[id] > a').click(function () {
        // get all open uls
        var openUl = $(this).parent().siblings().find('ul:visible');
        // change colors of the li back to original
        openUl.parent().children('a').css({
            'background-color': '#fff',
            'color': '#E87666'
        });
        // hide all open uls
        openUl.hide();
        // show and change color the sub menu
        var subMenu = $(this).siblings('ul');
        // if hidden - show
        if (!subMenu.is(':visible')) {
            $(this).css({
                'background-color': '#E87666',
                'color': '#fff'
            });
            subMenu.slideDown("slow");
        }
        else {
            var current = $(this);
            subMenu.slideUp("slow", function () {
                current.css({
                    'background-color': '#fff',
                    'color': '#E87666'
                });
                current = null;
            });
        }
        subMenu = null;
    });

    var counter = 0;
    $('.main-block').each(function (index, block) {
        if (counter == 0) {
            $(block).attr('style', 'background-color: #FFD180 !important');
        }
        else if (counter == 1) {
            $(block).attr('style', 'background-color: #6D4C41 !important');
        }
        else if (counter == 2) {
            $(block).attr('style', 'background-color: #FFAB40 !important');
        }
        else {
            $(block).attr('style', 'background-color: #795568  !important');
        }

        counter += 1;
        if (counter > 3) {
            counter = 0;
        }
    });

    var loginErrMsg = "ההתחברות נכשלה.";
    var loginErrMsg2 = "אנא נסה שנית...";
    $('.validation-summary-errors').html(loginErrMsg + '<br />' + loginErrMsg2);
    loginErrMsg = loginErrMsg2 = null;

    $('body').scroll(function () {

    });
});
>>>>>>> origin/master
