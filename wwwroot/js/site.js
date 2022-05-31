// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

//From Tasty / HeaderImg
function saveApi(id, path) {
  $.ajax(
    {
      type: 'POST',
      dataType: 'JSON',
      url: `/Recipes/SaveNew${path}`,
      data: { id: id },
      success:
        function (response) {
          // Generate HTML table.
          window.location.href = "/Recipes";
        },
      error:
        function (response) {
          console.log("Error: ", response);
        }
    });
}
