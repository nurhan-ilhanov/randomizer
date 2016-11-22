// Write your Javascript code.
$(document).ready(function () {
    $('.getRandom').on('click', function () {
        var itemId = $(this).attr('data-id');

        $.ajax({
            url: '/ElementLists/DrawItem/',
            type: 'POST',
            data: { id: itemId },
            success: function (result) {
                $('.draw-result').html(result.ans);
                $('#getRandomItem').modal('show');
            }
        });
    });

    $('.getRandoms').on('click', function () {
        var itemId = $(this).attr('data-id');
        $('#getRandomItems').modal('show');

        $('#getRandomItems').on('shown.bs.modal', function () {
            $('#elements-number').focus().select();
        });

        $('button#submit').click(function () {
            var numberOfElements = $('input[name="numberOfElements"]').val();

            if (numberOfElements != null) {
                $.ajax({
                    url: '/ElementLists/DrawItems/',
                    type: 'POST',
                    data: { id: itemId, count: numberOfElements },
                    success: function (result) {
                        $('.draw-result').html(result.ans);
                        $('#getRandomItem').modal('show');
                    }
                });
            }
        });
    });
});
