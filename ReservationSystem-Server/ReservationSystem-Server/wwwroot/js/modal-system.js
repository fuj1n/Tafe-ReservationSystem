function openError(message) {
    $('#modal-body').html(`<p class="text-danger">Error: ${message}</p>`);
    $('#modal-title').text('Error');
    $('.modal-dialog').removeClass().addClass('modal-dialog modal-dialog-scrollable');
    $('#modal').modal('show');
}

function openModal(url) {
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
            _openModal(modal, url);
            modal.off('hidden.bs.modal');
        });
    } else {
        _openModal(modal, url);
    }
}

function _openModal(modal, url, retries = 0) {
    const isUrlAbsolute = new RegExp('^(?:[a-z]+:)?//', 'i');
    
    const submitBtn = $('#btn-save');
    submitBtn.off('click');
    submitBtn.addClass('d-none');
    
    if(isUrlAbsolute.test(url)) {
        openError('Cross-origin urls are not permitted');
        return;
    }
    
    fetch(url, { mode: 'same-origin'})
        .then(response => {
            if(!response.ok) {
                throw new Error(`${response.status} ${response.statusText}`);
            }
            
            return response.text()
        }).then(html => {
            $('#modal-body').html(html);
            
            $('#modal-title').text($('#modal-name').text());

            const modalSize = `modal-${$('#modal-size').text()}`;
            $('.modal-dialog').removeClass()
                .addClass('modal-dialog modal-dialog-scrollable')
                .addClass(modalSize);

            const action = $('#modal-action');
            const submittable = $('#modal-submit');
            const hasAction = action.length > 0 || submittable.length > 0;

            if(action.length > 0) {
                submitBtn.on('click', () => {
                    action.trigger('click');
                });
            }
            if(submittable.length > 0) {
                submitBtn.on('click', () => {
                    submittable.submit();
                });
            }

            if (hasAction) {
                submitBtn.removeClass('d-none');
            }

            autoBindModals();
            modal.modal('show');
        }).catch(error => {
            if(retries < 3) {
                _openModal(modal, url, retries + 1);
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

function bindModal(selector, url) {
    bindModalToElement($(selector), url);
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
    
    const searchParams = new URLSearchParams(window.location.search);
    if(searchParams.has('modal')) {
        openModal(searchParams.get('modal'));
    }
});