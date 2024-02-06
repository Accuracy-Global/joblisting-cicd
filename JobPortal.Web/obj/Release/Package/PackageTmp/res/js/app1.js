$(window).scroll(function () {
    var top = $(window).scrollTop();
    if (top >= 300) {
        $("nav").addClass("secondary");
        $("nav").addClass("fixed-top");
    } else if ($("nav").hasClass("secondary")) {
        $("nav").removeClass("secondary");
        $("nav").removeClass("fixed-top");
    }
});

window.onload = function () {
    var form = document.getElementById("contact-form");

    form.addEventListener("submit", function (e) {
        e.preventDefault();

        var fn = document.getElementById("fname").value;
        var ln = document.getElementById("lname").value;
        var mail = document.getElementById("email").value;
        var ph = document.getElementById("Phone").value;
        var msg = document.getElementById("message").value;
        fetch("https://joblistingmobile.azurewebsites.net/get_in_touch", {
            method: "POST",
            body: JSON.stringify({
                FirstName: fn,
                LastName: ln,
                Email: mail,
                phone: ph,
                Message: msg,
            }),
            headers: {
                "Content-type": "application/json; charset=UTF-8",
            },
        })
            .then((response) => response.json())
            .then((json) => console.log(json));

        this.reset();
        alert("ThankYou For Reaching Us...Our Team Will Contact You Soon!!")

    });


    // var form2 = document.getElementById("registration-form");
    // form2.addEventListener("submit", function (e) {
    //   e.preventDefault();

    //   var firstname = document.getElementById("fstname").value;
    //   var lastname = document.getElementById("lstname").value;
    //   var email = document.getElementById("email2").value;
    //   var telephone = document.getElementById("tel").value;
    //   var com = document.getElementById("company").value;
    //   var info = document.getElementByname("flexRadioDefault").value;
    //   fetch("http://localhost:3000/register", {
    //     method: "POST",
    //     body: JSON.stringify({
    //       FName: firstname,
    //       LName: lastname,
    //       mail: email,
    //       Contact: telephone,
    //       Company: com,
    //       How_Did_you_hear: info,
    //     }),
    //     headers: {
    //       "Content-type": "application/json; charset=UTF-8",
    //     },
    //   }).then((data) => console.log(data));
    // });
    // alert("registration form submitted !!!!");
}

function submitForm() {
    $(function () {
        $('#registration-form').on('submit', function (e) {

            e.preventDefault();
            $.ajax({
                url: 'https://joblistingmobile.azurewebsites.net/web_register',
                type: 'POST',
                data: $('#registration-form').serialize(),
                success: function (data) {
                    alert('Form submitted Successfully. ')
                    // Here Form Will be Reload
                    location.reload();
                },
                error: function () {
                    //Error Message
                    alert("Some Error Occurred !! Please Try Again !!!!")
                },
                complete: function () {
                    $('#registration-form').each(function () {
                        //Here form fields will be cleared.
                        this.reset();

                    });
                }

            });
        });
    });
}

