﻿function openModal(url) {
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
    const submitBtn = $('#btn-save');
    submitBtn.off('click');
    submitBtn.addClass('d-none');
    
    fetch(url)
        .then(response => {
            if(!response.ok) {
                throw response;
            }
            
            return response.text()
        }).then(html => {
            $('#modal-body').html(html);
            
            $('#modal-title').text($('#modal-name').text());

            const modalSize = `modal-${$('#modal-size').text()}`;
            $('.modal-dialog').removeClass().addClass('modal-dialog modal-dialog-scrollable').addClass(modalSize);

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
                $('#modal-body').text(`Error: ${error.status} ${error.statusText}`);
                $('#modal-title').text('Error');
                $('.modal-dialog').removeClass().addClass('modal-dialog modal-dialog-scrollable');
                modal.modal('show');
            }
        });
}

function bindModal(selector, url) {
    $(selector).on('click', function () {
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

        item.on('click', function () {
            openModal(url);
        });
    });
}

$(() => {
    autoBindModals();
    
    const searchParams = new URLSearchParams(window.location.search);
    if(searchParams.has('modal')) {
        openModal(searchParams.get('modal'));
    }
});