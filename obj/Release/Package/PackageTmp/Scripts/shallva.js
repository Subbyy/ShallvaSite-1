

$(document).ready(function () {
    $('#to-top').click(function () {
        $("html, body").stop().animate({ scrollTop: 0 }, '500', 'swing');
    });

    initCategoriesMenu();
    
    $('#products > ul > li').mouseenter(function () {
        var list = $(this).find('ul');
        var itemMove = 11;
        list.css('top', (-1 * (list.find('li').length - 1) * itemMove) + 'px');
        list = itemMove = null;
    });

});

function initCategoriesMenu() {
    var mainTop = $('#main-content').css('padding-top');
    mainTop = parseFloat(mainTop.replace('px', ''));

    var footerTop = $('footer').position().top;
    var fix = 50;
    var height = footerTop - mainTop - fix;

    var itemHeight = 22;
    var fixedHeight = 0;
    var itemsInHeight = 0;

    // calculate max-height
    while (fixedHeight + itemHeight < height) {
        fixedHeight += itemHeight;
        itemsInHeight++;
    }

    //$('#menu ul ul').css('max-height', fixedHeight + 'px');

    var totalItems = $('#menu > ul > li > ul > li').length;

    if (totalItems > itemHeight) {
        var itemsWidth = $('#products > ul').css('width');
        itemsWidth = parseFloat(itemsWidth.replace('px', '')) * 2;
        $('#products > ul').css('width', itemsWidth)
    }

    mainTop = footerTop = fix = height = fixedHeight = itemHeight = totalItems = null;
}