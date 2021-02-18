const API_PRODUCT_URL = "/api/productapi";
const API_SHOPPING_CART_URL = "/api/shoppingcartapi";

async function AddToFavorite(id)
{
    let data = { "ProductID": id };
    const response = await fetch(API_PRODUCT_URL + "/addtofavorite",
        {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(data)
        }
    )


    let res = await response.json();
    if (ErrorHandler(res)) {
        if (res.sucess) {

            let doc = document.querySelector("[data-fav-id='" + id + "']");
            let c = doc.style.color === "red" ? "black" : "red";
            let c2 = doc.style.color === "red" ? "red" : "black";
            doc.removeAttribute("onmouseout");
            doc.removeAttribute("onmouseover");
            doc.style.color = c;
            doc.setAttribute("onmouseover", "this.style.color = '" + c +"'");
            doc.setAttribute("onmouseout", "this.style.color = '"+c2+"'");
        }
    }
}

async function PromoCode() {
    let code = $("#code").val();
    let data = { "code": code };
    const response = await fetch(API_SHOPPING_CART_URL + "/applypromocode",
        {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(data)
        }
    )

    let res = await response.json();
    if (ErrorHandler(res)) {
        if (res.sucess) {

            let resJson = JSON.parse(res.message);

            var discountSpan = document.getElementById("discountSpan");
            var promoCodeButton = document.getElementById("promoCodeButton");
            var codeInput = document.getElementById("code");
            promoCodeButton.innerHTML = '<button class="ml-2" onclick="ClearCode();">Wyczyść</button>';
            var HTML =
                '<div class="row">' +
                '<div class="col">CENA PRZED ZNIŻKĄ</div>' +
                '<div class="col text-right" id="summary-before-discount-price">' + resJson.summaryBeforeDiscountPrice + '</div>' +
                '</div>' +

                '<div class="row">' +
                '<div class="col" id="summary-discount-percent">ZNIŻKA (' + resJson.summaryDiscountPercent + '%)</div>' +
                '<div class="col text-right" id="summary-discount-price">-' + resJson.summaryDiscountPrice + '</div>' +
                '</div>';
            discountSpan.innerHTML = HTML;

            codeInput.value = code.toUpperCase();

            let summaryTotalPrice = document.getElementById("summary-total-price");
            summaryTotalPrice.innerText = resJson.summaryTotalPrice;
            
        }
    }
}

async function ClearCode() {

    const response = await fetch(API_SHOPPING_CART_URL + "/clearpromocode",
        {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            }
        }
    )

    let res = await response.json();
    if (ErrorHandler(res)) {
        if (res.sucess) {

            let resJson = JSON.parse(res.message);

            var codeInput = document.getElementById("code");
            codeInput.value = "";

            var discountSpan = document.getElementById("discountSpan");
            discountSpan.innerHTML = "";

            var promoCodeButton = document.getElementById("promoCodeButton");
            promoCodeButton.innerHTML = "";

            let summaryTotalPrice = document.getElementById("summary-total-price");
            summaryTotalPrice.innerText = resJson.summaryTotalPrice;

        }
    }
}

async function AddToCart(id, amount) {
    let data = { "ProductID": id, "Amount": amount };
    const response = await fetch(API_PRODUCT_URL + "/addtocart",
        {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(data)
        });

    if (ErrorHandler(await response.json())) {
        const response2 = await fetch(API_PRODUCT_URL + "/getshoppingcartcount");
        let res = await response2.json();
        if (res.sucess)
            $("#basket").text(res.message);
    }


}
async function BuyNow(id, amount) {
    let data = { "ProductID": id, "Amount": amount };
    const response = await fetch(API_PRODUCT_URL + "/buynow",
        {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(data)
        });

    ErrorHandler(await response.json());
}

async function DeleteShoppingCartItem(id) {
    let data = { "ProductID": id };
    const response = await fetch(API_SHOPPING_CART_URL + "/deleteshoppingcartitem",
        {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(data)
        });

    let res = await response.json();
    if (ErrorHandler(res)) {
        if (res.sucess)
        {
            let resJson = JSON.parse(res.message);
            ChangeShoppingCart(resJson, id);
        }
    }
}

async function ChangeAmountOfProduct(id, amount)
{
    let data = { "ProductID": id,"Amount": amount };
    const response = await fetch(API_SHOPPING_CART_URL + "/changeamountofproduct",
        {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(data)
        });

    let res = await response.json();
    if (ErrorHandler(res)) {
        if (res.sucess)
        {
            let resJson = JSON.parse(res.message);
            ChangeShoppingCart(resJson, id);
        }
    }
}

function ErrorHandler(res)
{
    let errMessage = document.querySelector("[class='errorMessage']");
    errMessage.innerText = "";
    errMessage.parentElement.style.display = "none";

    if (!res.sucess)
    {
        errMessage.innerText = res.message;
        errMessage.parentElement.style.display = "";
        return false;
    }

    errMessage.innerText = "";
    errMessage.parentElement.style.display = "none";
    return true;
}


async function ChangeShoppingCart(resJson, id) {
    if (resJson.deleted) {
        let doc = document.querySelector("[data-id='" + id + "']");
        let docSummary = document.querySelector("[data-summary-id='" + id + "']");

        doc.parentElement.removeChild(doc);
        docSummary.parentElement.removeChild(docSummary);
    } else {
        let doc2 = document.querySelector("[data-count='" + id + "']");
        let doc2SummaryAmount = document.querySelector("[data-summary-amount='" + id + "']");
        let doc2SummaryPrice = document.querySelector("[data-summary-price='" + id + "']");
        let doc2Price = document.querySelector("[data-price='" + id + "']");
        doc2.innerText = resJson.count;
        doc2SummaryAmount.innerText = resJson.count;
        doc2SummaryPrice.innerText = resJson.summaryPrice;
        doc2Price.innerText = resJson.summaryPrice;
    }


    let summaryTotalPrice = document.getElementById("summary-total-price");
    summaryTotalPrice.innerText = resJson.summaryTotalPrice;

    if (resJson.summaryBeforeDiscountPrice != undefined)
    { 
        let summaryBeforeDiscountPrice = document.getElementById("summary-before-discount-price");
        summaryBeforeDiscountPrice.innerText = resJson.summaryBeforeDiscountPrice;

        let summaryDiscountPercent = document.getElementById("summary-discount-percent");
        summaryDiscountPercent.innerText =  "ZNIŻKA ("+ resJson.summaryDiscountPercent+"%)";

        let summaryDiscountPrice = document.getElementById("summary-discount-price");
        summaryDiscountPrice.innerText = "-" + resJson.summaryDiscountPrice;
    }

    const response2 = await fetch(API_PRODUCT_URL + "/getshoppingcartcount");
    let res2 = await response2.json();
    if (res2.sucess) {
        $("#basket").text(res2.message);
        $("#product-id").text(res2.message);
    }
}

async function Pay(id)
{
    const response = await fetch(API_SHOPPING_CART_URL + "/pay",
        {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            }
        });

    let res = await response.json();
    if (ErrorHandler(res)) {
        if (res.sucess) {

            alert(res.message)
            window.location.href = "/ShoppingHistory/" + id;
        }
    }
}

$(function () {
    $("#shippingSelect").on("change", async function (e) {
        let selected = $("#shippingSelect option:selected");
        let id = selected.data("shipping-id");
        let data = { "ID": id };
        const response = await fetch(API_SHOPPING_CART_URL + "/changeshippingoption",
            {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(data)
            });

        let res = await response.json();
        if (ErrorHandler(res)) {
            if (res.sucess) {

                let resJson = JSON.parse(res.message);

                if (resJson.summaryBeforeDiscountPrice != undefined) {
                    let summaryBeforeDiscountPrice = document.getElementById("summary-before-discount-price");
                    summaryBeforeDiscountPrice.innerText = resJson.summaryBeforeDiscountPrice;

                    let summaryDiscountPercent = document.getElementById("summary-discount-percent");
                    summaryDiscountPercent.innerText = "ZNIŻKA (" + resJson.summaryDiscountPercent + "%)";

                    let summaryDiscountPrice = document.getElementById("summary-discount-price");
                    summaryDiscountPrice.innerText = "-" + resJson.summaryDiscountPrice;
                }

                let summaryTotalPrice = document.getElementById("summary-total-price");
                summaryTotalPrice.innerText = resJson.summaryTotalPrice;

            }
        }
    });
})