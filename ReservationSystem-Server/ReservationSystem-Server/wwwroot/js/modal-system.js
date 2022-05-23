function setTitle(title) {
    $('#modal-title').text(title);
}

function setBody(html) {
    $('#modal-body').html(html);
}

function setButton(text, className = 'btn btn-primary') {
    const submitBtn = $('#btn-save');
    const isHidden = submitBtn.hasClass('d-none');
    
    submitBtn.removeClass();
    submitBtn.addClass(className);
    
    if(isHidden) {
        submitBtn.addClass('d-none');
    }
    
    submitBtn.text(text);
}

function setShowButton(show) {
    const submitBtn = $('#btn-save');
    if(show) {
        submitBtn.removeClass('d-none');
    } else {
        submitBtn.addClass('d-none');
    }
}

function displayModal() {
    $('#modal').modal('show');
}

function hideModal() {
    // Dirty way to eliminate race condition with modal opening and closing too quickly
    const modal = $('#modal');
    modal.modal('hide');
    modal.on('shown.bs.modal', function () {
        modal.modal('hide');
        modal.off('shown.bs.modal');
    });
}

function setModalSize(sizeClass = '') {
    $('.modal-dialog').removeClass()
        .addClass('modal-dialog modal-dialog-scrollable')
        .addClass(sizeClass);
}

function submitForm(form) {
    if(!form || !form.length) {
        openError('No form provided');
        return;
    }
    
    const url = form.attr('action');
    let method = form.attr('method').toUpperCase();
    if(method.trim() === '') {
        method = 'GET';
    }
    
    const data = new FormData(form[0]);
    
    openModal(url, method, data);
}

function openError(message) {
    setBody(`<p class="text-danger">Error: ${message}</p>`);
    setTitle('Error');
    setModalSize();
    displayModal();
}

function openModal(url, method = 'GET', data = null) {
    if(!url) {
        console.error("No url provided");
        return;
    }
    
    const modal = $('#modal');
    
    // this song and dance is done to still display the animation when switching between modals
    // the hidden.bs.modal event is not triggered when the modal is not shown
    if(modal.hasClass('show')) {
        modal.modal('hide');
        modal.on('hidden.bs.modal', function () {
            _openModal(modal, url, method, data);
            modal.off('hidden.bs.modal');
        });
    } else {
        _openModal(modal, url, method, data);
    }
}

function _openModal(modal, url, method = 'GET', data = null, retries = 0) {
    const isUrlAbsolute = new RegExp('^(?:[a-z]+:)?//', 'i');
    
    setShowButton(false);
    
    if(isUrlAbsolute.test(url)) {
        openError('Cross-origin urls are not permitted');
        return;
    }
    
    setTitle('Loading...');
    setBody($('#modal-loader').html()); // Open the loader
    setModalSize();
    displayModal();
    
    if(method === 'GET' && data) {
        const params = new URLSearchParams();
        Object.keys(data).forEach(key => {
            params.append(key, data[key]);
        });
        url += '?' + params.toString();
    }
    
    fetch(url, { mode: 'same-origin', method: method, body: method !== "GET" ? data : null})
        .then(response => {
            if(!response.ok) {
                throw new Error(`${response.status} ${response.statusText}`);
            }
            
            return response.text()
        }).then(html => {
            setBody(html);
            
            if($('modal-refresh-on-close').length > 0) {
                modal.on('hidden.bs.modal', function () {
                    location.reload();
                });
            }
            
            if($('modal-close').length > 0) {
                hideModal();
                return;
            }
            
            setTitle($('#modal-name').text());

            const modalSize = `modal-${$('#modal-size').text()}`;
            setModalSize(modalSize);

            const action = $('#modal-action');
            const submittable = $('#modal-submit');
            const hasAction = action.length > 0 || submittable.length > 0;
            
            const submitBtn = $('#btn-save');
            
            if(action.length > 0) {
                submitBtn.on('click', () => {
                    action.trigger('click');
                });
            }
            if(submittable.length > 0) {
                submitBtn.on('click', () => {
                    submitForm(submittable);
                });
            }

            if (hasAction) {
                const btnText = $('#modal-button-text');
                const btnClass = $('#modal-button-class');
                
                setButton(btnText.length > 0 ? btnText.text() : "Save", btnClass.length > 0 ? btnClass.text() : "btn btn-primary");
                setShowButton(true);
            }

            autoBindModals();
        }).catch(error => {
            if(retries < 3) {
                _openModal(modal, url, method, data, retries + 1);
            } else {
                openError(`${error.message}`);
            }
        });
}

function bindModalToElement(item, url) {
    item.on('click', function () {
        openModal(url.replace('{id}', $(this).data('id')));
    });
}

function autoBindModals() {
    const modals = $("a.modal-for");
    modals.each(function () {
        const item = $(this);
        
        const url = item.attr('href');
        item.removeAttr('href');
        
        item.removeClass('modal-for');

        bindModalToElement(item, url);
    });
}

$(() => {
    autoBindModals();
    //
    // const searchParams = new URLSearchParams(window.location.search);
    // if(searchParams.has('modal')) {
    //     openModal(searchParams.get('modal'));
    // }
});