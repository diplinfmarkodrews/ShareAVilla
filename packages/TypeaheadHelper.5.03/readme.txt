1. Add typeahead input in a razor view:

@Html.TypeaheadFor(model=>model.Country, Model.Countries)

In the same way as @Html.DropDownListFor(...), with the second parameter as IEnumerable<SelectListItem>.

2. Include the assets
Place Scripts\typeahead-razor-helper.js after Scripts\typeahead.bundle.js (which includes BloodHound). 

3. Initiate 
Bind the JavaScript to the input by

initTypeahead($('.typeahead'));

