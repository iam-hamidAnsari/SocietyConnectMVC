$("#sendmail").click(function () {
    var obj = {
        name: $("#name").val(),
        email: $("#email").val(),
        subject: $("#subject").val(),
        message: $("#message").val()

    };
    console.log(obj);

    $.ajax({
        url: '/Home/index',
        type: 'Post',
        contentType: 'application/x-www-form-urlencoded;charset=utf8',
        dataType: 'json',
        data: obj,

        success: function () {
            alert('Detail Send Sucessfully!!');
        },
        error: function () {
            alert('Somthing went wrong');
        }
    });

});

$(document).ready(function () {
    $('#DropDownListFlats').change(function () {
        const selectedFlatId = $(this).val();

        if (selectedFlatId) {
            $.ajax({
                url: `/Flats/GetFlatDetails?flatNo=${selectedFlatId}`, 
              /*  method: 'GET',*/
                success: function (data) {
                    console.log(data);
                    // Populate input fields with the returned data
                    $('#TextBox1').val(data.floorNo || '');
                    $('#TextBox2').val(data.blockNo || '');
                    $('#TextBox3').val(data.flatType || '');
                },
                error: function (xhr, status, error) {
                    console.error('Error fetching flat details:', error);
                    alert('Unable to fetch flat details. Please try again.');
                }
            });
        } else {
            $('#TextBox1').val('');
            $('#TextBox2').val('');
            $('#TextBox3').val('');
        }
    });
});
