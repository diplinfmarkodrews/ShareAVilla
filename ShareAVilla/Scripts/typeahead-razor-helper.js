function onselected(obj, datum) {
    if (!obj || !obj.target || !datum) return;
    var newVal = datum.value.toString();
    $(obj.target).parent().next('input:hidden').attr('value', newVal);
}

function initTypeahead(obj) {
    var optionsName = $(obj).attr('data-typeahead-name');
    var optionsJsonString = $(obj).attr('data-typeahead-options');
    var options = JSON.parse(optionsJsonString);
    
    var engine = new Bloodhound({
        local: options,
        queryTokenizer: Bloodhound.tokenizers.whitespace,
        datumTokenizer: function (datum) {
            return Bloodhound.tokenizers.whitespace(datum.text);
        }
    });

    function searchWithDefaults(q, sync) {
        if (q === '') {
            sync(engine.index.all());
        } else {
            engine.search(q, sync);
        }
    }

    engine.initialize();

    $(obj).typeahead({
        hint: true,
        highlight: true,
        minLength: 0
    },
        {
            name: optionsName,
            displayKey: 'text',
            source: searchWithDefaults
        })
        .on('typeahead:selected', function (thisObj, datum) {
            onselected(thisObj, datum);
        });;
}