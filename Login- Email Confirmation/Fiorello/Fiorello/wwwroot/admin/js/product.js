$(document).ready(function () {
    $(document).on("click", "#delete", function (e) {
        e.preventDefault();

        let productId = $(this).attr("data-id");

        let removedElem = $(this).parent().parent();

        let data = { id: productId };

        $.ajax({
            url: '/admin/Product/DeleteProductImage',
            type: 'POST',
            data: data, 
            success: function (response) {
                $(removedElem).remove();
            }
        });

    })
})