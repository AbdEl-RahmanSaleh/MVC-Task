
function updateTotal() {
    var price = parseFloat($("#productPrice").val()) || 0;
    var quantity = parseInt($("#productQuantity").val()) || 0;
    $("#productTotal").val((price * quantity).toFixed(2));
}


function updateOrderTotal() {
    var total = 0;
    $("#orderItemsTable tbody tr").each(function () {
        var itemTotal = parseFloat($(this).find("td:nth-child(5)").text()) || 0;
        total += itemTotal;
    });
    $("#totalAmount").text(total.toFixed(2));
}


$(document).ready(function () {
    var addedProductIds = [];

    $("#productDropdown").change(function () {
        var productId = $(this).val();
        console.log(productId);


        if (productId) {
            $.get('/Order/GetProductPrice', { productId: productId }, function (price) {
                $("#productPrice").val(price);
                updateTotal();
            });
        } else {
            $("#productPrice").val('');
            $("#productTotal").val('');
        }
    });

    $("#productQuantity").on("input", function () {
        updateTotal();
    });


    $("#addProductBtn").click(function () {
        var productId = $("#productDropdown").val();
        var productName = $("#productDropdown option:selected").text();
        var price = $("#productPrice").val();
        var quantity = $("#productQuantity").val();
        var total = $("#productTotal").val();

        if (!productId || addedProductIds.includes(productId)) {
            Swal.fire({
                icon: 'warning',
                title: 'Oops...',
                text: 'Please select a valid product that hasn\'t been added yet!',
            });
            return;
        }

        addedProductIds.push(productId);

        $.get('/Order/GetProductImage', { productId: productId }, function (image) {
            var newRow = `<tr style ="text-align: center; vertical-align: middle; data-total="${total}">
                <td><img src="/files/images/${image}" class="img-thumbnail rounded-circle" style="width:75px; height:75px; object-fit:cover;" /></td>
                <td>${productName}</td>
                <td>${price}</td>
                <td>${quantity}</td>
                <td>${total}</td>
                <td>
                    <button type="button" class="btn btn-info btn-sm edit-product" data-id="${productId}">Edit</button>
                    <button type="button" class="btn btn-danger btn-sm remove-product" data-id="${productId}">Remove</button>
                </td>
                <input type="hidden" name="orderItems[${addedProductIds.length - 1}].ProductId" value="${productId}" />
                <input type="hidden" name="orderItems[${addedProductIds.length - 1}].Quantity" value="${quantity}" />
            </tr>`;

            // Append the new row to the table
            $("#orderItemsTable tbody").append(newRow);

            // Clear form inputs for adding another product
            $("#productDropdown").val('');
            $("#productPrice").val('');
            $("#productQuantity").val(1);
            $("#productTotal").val('');

            updateOrderTotal();
        });
        });


    $(document).on("click", ".remove-product", function () {
        var productId = $(this).data("id");
        console.log(productId);
        addedProductIds = addedProductIds.filter(id => id !== productId.toString());
        $(this).closest("tr").remove();
        updateOrderTotal();
    });



    $(document).on("click", ".edit-product", function () {
        var row = $(this).closest("tr");
        var productId = $(this).data("id");
        console.log(productId);
        var price = row.find("td:nth-child(3)").text();
        var quantity = row.find("td:nth-child(4)").text();
        var total = row.find("td:nth-child(5)").text();

           
        $("#productDropdown").val(productId).change(); 
        $("#productQuantity").val(quantity);  
        $("#productPrice").val(price);  
        $("#productTotal").val(total); 

        $("#addProductBtn").hide();
        $("#saveProductBtn").show();
        $("#productDropdown").prop("disabled", true);

        $("#saveProductBtn").data("row", row);  
    });

    $("#saveProductBtn").click(function () {
        var row = $(this).data("row"); 
        var productId = $("#productDropdown").val();
        var quantity = $("#productQuantity").val();
        var price = $("#productPrice").val();
        var total = $("#productTotal").val();

        //row.find("td:nth-child(2)").text($("#productDropdown option:selected").text());
        row.find("td:nth-child(3)").text(price);  
        row.find("td:nth-child(4)").text(quantity); 
        row.find("td:nth-child(5)").text(total);


        row.find("input[name*='ProductId']").val(productId);
        row.find("input[name*='Quantity']").val(quantity);

        $("#productDropdown").prop("disabled", false);

        //row.find("input[name*='ItemTotal']").val(total);

        // Reset the form and show Add button
        $("#productDropdown").val('');
        $("#productPrice").val('');
        $("#productQuantity").val(1);
        $("#productTotal").val('');
        $("#addProductBtn").show();
        $(this).hide();

        updateOrderTotal();
    });




});