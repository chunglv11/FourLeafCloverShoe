function loadProductDetails() {
    $.ajax({
        type: "GET",
        url: '/Admin/ManagerOrder/GetProductDetail',
        success: function (response) {
            if (response.success) {
                renderProductDetails(response.data);
            } else {
                alert("Failed to load product details.");
            }
        },
        error: function () {
            alert("An error occurred while loading product details.");
        }
    });
}

function renderProductDetails(data) {
    var productList = $('#productList');
    productList.empty(); // Xóa nội dung cũ
    var urlParams = new URLSearchParams(window.location.search);
    var orderId = urlParams.get('orderId');
    console.log(orderId);
    data.forEach(function (item) {
        var productHtml = `
            <div class="col-3" style="width:20%">
                <form class="formproduct" asp-controller="ManagerOrder" asp-action="AddProductDetailToOrder">
                 <input hidden name="productDetailId" value="${item.id}" />
                 <input hidden name="orderId" value="${orderId}" />
                    <div class="card product-card">
                        <div class="card-img-top">
                            <a href="">
                                <img src="${item.imageUrl}" alt="image" class="img-prod img-fluid custom-img">
                            </a>
                        </div>
                        <div class="card-body">
                            <a href="">
                                <p class="prod-content mb-0 text-muted">${item.productName} - SL: ${item.quantity}</p>
                            </a>
                            <p class="prod-content mb-0 text-muted">${item.sizeName} - ${item.colorName}</p>
                            <div class="d-flex align-items-center justify-content-between mt-2 mb-3 flex-wrap gap-1">
                                <b class="mb-0 text-truncate"><b>Giá bán: ${item.price?.toLocaleString()} VNĐ</b></b>
                            </div>
                            <div class="d-flex">
                                <div class="flex-shrink-0">
                                    <div class="d-grid">
                                        <input class="form-control form-control-sm custom-input" type="number" name="Quantity" min="0" value="1" />
                                    </div>
                                </div>
                                <div class="flex-grow-1 ms-3">
                                    <div class="d-grid">
                                        <button class="addProductButton btn-prod-card form-control-sm custom-button">Thêm</button>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </form>
            </div>
        `;
        productList.append(productHtml);
    });
}
$('.addProductButton').click(function (e) {
    e.preventDefault();
    var form = $(this).closest('form');
    var url = form.attr('action');
    var data = form.serialize();
    $.ajax({
        type: 'POST',
        url: url,
        data: data,
        success: function (response) {
            $('#message').html('<div class="alert alert-' + (response.success ? 'success' : 'danger') + '">' + response.message + '</div>');
            if (response.id != null) {
                var slElement = $('#SL_' + response.id);
                slElement.html(response.soluong);
            }
            // Tự động ẩn thông báo sau 1 giây
            setTimeout(function () {
                $('#message').html('');
            }, 1000);
        },
        error: function () {
            $('#message').html('<div class="alert alert-danger">Có lỗi xảy ra.</div>');
            // Tự động ẩn thông báo sau 1 giây
            setTimeout(function () {
                $('#message').html('');
            }, 1000);
        }
    });
});
$(document).ready(function () {
    loadProductDetails();
});