// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(`#openKrogerSearch`).click(function () {

  let searchTerm = $('#exampleDataList').val();
  $(`#searchIngredientResults`).html(`<div class="spinner-border text-primary" role="status">
      <span class="visually-hidden">Loading...</span>
    </div>`);
  callAjax(searchTerm);
});


const retrieveResults = (event, id, name) => {
  event.preventDefault();
  $(`#pagination`).addClass('d-none');
  $(`#searchIngredientResults`).html(`<div class="spinner-border text-primary" role="status">
      <span class="visually-hidden">Loading...</span>
    </div>`);
  let page = parseInt(event.target.innerText);
  callAjax(name, page);
}

function callAjax(searchTerm, page) {
  if (!page) {
    page = 1;
  }
  $.ajax(
    {
      type: 'GET',
      dataType: 'JSON',
      url: '/Recipes/GetProductListings',
      data: { searchTerm: `${searchTerm}`, page: `${page}` },
      success:
        function (response) {
          let jsonOut = JSON.parse(response);
          console.log(response);
          printResults(searchTerm, jsonOut.data.sort(function (a, b) {
            return a.productId - b.productId;
          }));
          $(`#searchIngredientButton-${searchTerm}`).hide();
          $(`#pagination`).removeClass('d-none');
          generatePagination(0, jsonOut.meta.pagination.total, page, searchTerm);
        },
      error:
        function (response) {
          console.log("Error: ", response);
          $(`#searchIngredientButton-${ingredientId}`).show();
          $(`#pagination`).addClass('d-none');
        }
    });
}
const generatePagination = (id, total, page = 1, searchTerm) => {
  $(`#paginationUL`).html('');

  console.log("here :", id, total, page, searchTerm);
  outputHtml = `<li class="page-item">
              <a class="page-link" href="#" onclick='retrieveResults(event, ${id}, "${searchTerm}")' aria-label="Previous">
                <span aria-hidden="true">&laquo;</span>
              </a>
            </li>`;
  let pageStart = page;
  if (pageStart <= 3) {
    pageStart = 1;
  } else {
    pageStart = page - 2;
  }
  console.log('pageStart: ', pageStart)
  for (let x = 1; x <= 5 && x < Math.ceil(total / 6); x++) {
    console.log(x);

    outputHtml += `<li class="page-item ${(pageStart === page) ? "active" : ""}">
              <a class="page-link" href="#" onclick='retrieveResults(event, ${id}, "${searchTerm}")'>${pageStart}</span></a>
            </li>`;
    pageStart++;
  }
  outputHtml += `
            <li class="page-item">
              <a class="page-link" href="#" onclick='retrieveResults(event, ${id}, "${searchTerm}")' aria-label="Next">
                <span aria-hidden="true">&raquo;</span>
              </a>
            </li>`;

  $(`#paginationUL`).html(outputHtml);
  $(`#pagination`).removeClass('d-none');
}

function printResults(ingredientId, jsonResult) {
  console.log("left side: ", jsonResult);
  let htmlString = `<div class="row">`;
  jsonResult.forEach((item) => {
    htmlString += `
                <div class="col-md-6">
                  <div class="border rounded p-3 pt-1 m-3 shadow bg-white">
                    <div class="row">
                      <div class="col-md-4 ps-0" style="max-height:9em;overflow:hidden;">`;
    let imgUrl;
    item.images.forEach((imgCategory) => {
      if (imgCategory.featured) {
        imgCategory.sizes.forEach((image) => {
          if (image.size === 'medium') {
            imgUrl = image.url;
          }
        });
      }
    });
    htmlString += `<img src="${imgUrl}" alt="Photo of ${item.description}" class="rounded" style="max-height: 100%; width: auto !important;" id='${item.upc}-imgUrl'>
                      </div>
                      <div class="col-md-8">
                        <div class="text-end">
                          <i class="fas fa-map-marker-alt mb-2"></i>`;
    let aisle = '';
    item.aisleLocations.forEach((aisleLoc) => {
      if (aisleLoc.description.toLowerCase().includes("aisle")) {
        aisle = aisleLoc.description.toLowerCase();
      }
    });
    if (aisle === '' && item.aisleLocations.length > 0) {
      aisle = (item.aisleLocations[0].description) ? item.aisleLocations[0].description : "";
    }
    htmlString += `&nbsp;<span id='${item.upc}-aisle'>${aisle.toLowerCase()}</span><div>
                        <span id='${item.upc}-name'>${item.description}</span>
                        <p class="text-primary fs-5 fw-light mb-0 mt-3">`;
    let itemPrice;
    if (item.items[0].price && item.items[0].price.promo === 0) {
      itemPrice = `$<span id='${item.upc}-price'>${item.items[0].price.regular.toFixed(2)}</span>`;
    } else if (item.items[0].price && item.items[0].price.promo > 0) {
      itemPrice = `<strike class="me-2">$${item.items[0].price.regular.toFixed(2)}</strike> <span style="background-color: #fed241;" class="text-dark">$<span id='${item.upc}-price'>${item.items[0].price.promo.toFixed(2)}</span></span>`;
    } else {
      itemPrice = '$-.--';
    }

    htmlString += `${itemPrice} <span class="text-dark fs-6">/ <span id='${item.upc}-size'>${item.items[0].size}</span></span></p>
                  <button type="button" class="btn btn-primary btn-sm" id="${item.upc}-addToCartButton" onClick="handleAddToPantryClick('${item.upc}', '${item.categories[0]}')">
                    Add to Pantry
                  </button>
                      </div>
                    </div>
                  </div>
                </div>
              </div>
            </div>
                `;
  });

  $(`#searchIngredientResults`).html(`</div>${htmlString}`);

}

function handleAddToPantryClick(upc, category) {
  let jsonOutput = {};

  jsonOutput.KrogerUPC = upc;
  jsonOutput.KrogerCategory = category;
  jsonOutput.KrogerAisle = $(`#${upc}-aisle`).text();
  jsonOutput.KrogerCost = $(`#${upc}-price`).text();
  jsonOutput.KrogerItemName = $(`#${upc}-name`).text();
  jsonOutput.KrogerItemSize = $(`#${upc}-size`).text();
  jsonOutput.KrogerImgLink = $(`#${upc}-imgUrl`).attr('src');
  console.log(jsonOutput);
  $.ajax(
    {
      type: 'POST',
      dataType: 'JSON',
      url: '/Pantry/SaveToPantry',
      data: { jsonPost: JSON.stringify(jsonOutput) },
      success:
        function (response) {
          // Generate HTML table.
          alert("success");
        },
      error:
        function (response) {
          console.log("Error: ", response);
        }
    });

}
