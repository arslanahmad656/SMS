function htmlbodyHeightUpdate() {
    var height3 = $(window).height();
    var height1 = $('.nav').height() + 50;
    height2 = $('.container-main').height();
    if (height2 > height3) {
        $('html').height(Math.max(height1, height3, height2) + 10);
        $('body').height(Math.max(height1, height3, height2) + 10);
    } else
    {
        $('html').height(Math.max(height1, height3, height2));
        $('body').height(Math.max(height1, height3, height2));
    }

}
$(document).ready(function () {
    htmlbodyHeightUpdate();
    $(window).resize(function () {
        htmlbodyHeightUpdate();
    });
    $(window).scroll(function () {
        height2 = $('.container-main').height();
        htmlbodyHeightUpdate();
    });
});

$(document).ready(function(){
    $('#myTable').dataTable();
});

function calc_total(){
    var sum = 0;
    $('.input-amount').each(function(){
        sum += parseFloat($(this).text());
    });
    $(".preview-total").text(sum);    
}
$(document).on('click', '.input-remove-row', function(){ 
    var tr = $(this).closest('tr');
    tr.fadeOut(200, function(){
        tr.remove();
        calc_total()
    });
});

$(function(){
    $('.preview-add-button').click(function(){
        var form_data = {};
        form_data["studentname"] = $('.payment-form input[name="studentname"]').val();
        form_data["fathername"] = $('.payment-form input[name="fathername"]').val();
        form_data["class"] = $('.payment-form #class option:selected').text();
        form_data["section"] = $('.payment-form #section option:selected').text();
        form_data["amount"] = parseFloat($('.payment-form input[name="amount"]').val()).toFixed(2);
        form_data["discount"] = parseFloat($('.payment-form input[name="discount"]').val()).toFixed(2);
        form_data["status"] = $('.payment-form #status option:selected').text();
        form_data["date"] = $('.payment-form input[name="date"]').val();
        form_data["remove-row"] = '<i class="material-icons" style="cursor: pointer;">&#xe15b;</i>';
        var row = $('<tr></tr>');
        $.each(form_data, function( type, value ) {
            $('<td class="input-'+type+'"></td>').html(value).appendTo(row);
        });
        $('.preview-table').append(row); 
        calc_total();
    });  
});