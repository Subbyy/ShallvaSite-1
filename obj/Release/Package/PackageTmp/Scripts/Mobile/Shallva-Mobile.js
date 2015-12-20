$(function () {
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
