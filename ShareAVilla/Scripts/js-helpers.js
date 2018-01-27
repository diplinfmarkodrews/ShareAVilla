
function post_to_url(path, params, method) {
    method = method || 'POST';

    var form = document.createElement('form');

    // Move the submit function to another variable
    // so that it doesn't get overwritten.
    form._submit_function_ = form.submit;

    form.setAttribute('method', method);
    form.setAttribute('action', path);
    form.setAttribute('target', '_blank');

    for (var key in params) {
        var hiddenField = document.createElement('input');
        hiddenField.setAttribute('type', 'hidden');
        hiddenField.setAttribute('name', key);
        hiddenField.setAttribute('value', params[key]);

        form.appendChild(hiddenField);
    }

    document.body.appendChild(form);
    form._submit_function_(); // Call the renamed function.
}