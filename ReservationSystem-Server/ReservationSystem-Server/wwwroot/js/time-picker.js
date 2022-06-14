$(() => {
    $('#edit-time').click(() => {
        $('#time-display').addClass('visually-hidden');
        $('#time-select').removeClass('visually-hidden');
    });

    $('.select-time-slot').click(function () {
        $('#time-display').removeClass('visually-hidden');
        $('#time-select').addClass('visually-hidden');

        $('#time-display input').val($(this).data('display'));
        $('#time-picker-val').val($(this).data('time'));
    });
});