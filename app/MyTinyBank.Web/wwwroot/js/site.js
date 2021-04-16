
$('.js-card-pay').on('click', (event) => {

    $('.js-alert-success').addClass('collapse in');
    $('.js-alert-failure').addClass('collapse in');

    document.getElementById("cardNumber").disabled = true;
    document.getElementById("expirationMonth").disabled = true;
    document.getElementById("expirationYear").disabled = true;
    document.getElementById("amount").disabled = true;
    document.getElementById("payButton").disabled = true;

    let cardNumber = $('.js-card-number').val();
    let expirationMonth = $('.js-expiration-month').val();
    let expirationYear = $('.js-expiration-year').val();
    let amount = $('.js-amount').val();


    
    let data = JSON.stringify(
        {
            cardNumber: cardNumber,
            expirationMonth: expirationMonth,
            expirationYear: expirationYear,
            amount: amount
        });


    // ajax call
    let result = $.ajax(
        {
            url: '/card/checkout',
            method: 'PUT',
            contentType: 'application/json',
            data: data
        }).done(response => {
            if (response.codeResult == 200) {
                $('.js-alert-success').removeClass('collapse in');
            }
            else {
                $('.js-errorMsg').val(response.errorText);
                $('.js-alert-failure').removeClass('collapse in');
            }
      
        }).fail(failure => {
            alert('failure');


        }).always(() => {

            document.getElementById("cardNumber").disabled = false;
            document.getElementById("expirationMonth").disabled = false;
            document.getElementById("expirationYear").disabled = false;
            document.getElementById("amount").disabled = false;
            document.getElementById("payButton").disabled = false;            
        });    

});


//Dimitri apo do kai kato de ta koitas einai dika mou practise test...



//$('.js-add-existing-card-save').on('click', (event) => {

//    let cardNumber = $('.js-card-number').val();
//    let customerId = $('.js-customer-id').val();
//    let accountId = $('.js-account_number').val();

//    let data = JSON.stringify(
//        {
//            cardNumber: cardNumber,
//            customerId: customerId,
//            accountId: accountId
//        });


//    // ajax call
//    let result = $.ajax(
//        {
//            url: `/card/${customerId}/AddExistingCardFormSave`,
//            method: 'PUT',
//            contentType: 'application/json',
//            data: data
//        }).done(response => {
//            if (response != null) {
//                alert('success');
//            }
//            else {
//                alert('success but failure!');
//            }
//        }).fail(failure => {
//            alert('failure');
//        });

//});



//$('.js-add-card-save').on('click', (event) => {

//    let customerId = $('.js-customer-id').val();
//    let accountId = $('.js-account_number').val();

//    let data = JSON.stringify(
//        {
//            customerId: customerId,
//            accountId: accountId,
//            cardType: null
//        });

//    // ajax call
//    let result = $.ajax(
//        {
//            url: `/card/${customerId}/AddCardFormSave`,
//            method: 'PUT',
//            contentType: 'application/json',
//            data: data
//        }).done(response => {
//            if (response != null) {
//                $('.js-card-number').val(response.cardNumber);
//                $('.js-expiration-date').value(response.Expiration);//den paizei afto.
//                alert('success');
//            }
//            else {
//                alert('failure!');
//            }
//        }).fail(failure => {
//            alert('failure');
//        });

//});


//$('.js-account-card').on('click', (event) => {

//    let customerId = $('.js-customer-id').val();

//    // ajax call
//    let result = $.ajax(
//        {
//            url: `/customer/accCardTwo/${customerId}`,
//            method: 'GET'//,
//            //contentType: 'application/json',
//            //data: data
//        }).done(response => {

//            $('.tableAccCard thead').empty();
//            $('.tableAccCard tbody').empty();

//            $('.tableAccCard thead').append(
//                `
//                <tr>
//                    <th style="text-align:center; vertical-align:middle">
//                    AccountId
//                    </th>
//                    <th style="text-align:center; vertical-align:middle">
//                    CardNumber
//                    </th>
//                </tr>
//                `);


//            for (let i = 0; i < response.length; i++) {
//                $('.tableAccCard tbody').append(
//                    `
//                <tr>
//                    <td align="center" style="vertical-align: middle">
//                        ${response[i].account}
//                    </td>
//                    <td align="center" style="vertical-align: middle">
//                        ${response[i].card}
//                    </td>
//                </tr>
//                `);
//            }

//        }).fail(failure => {
//            alert('failure');
//        });

//});



//$('.band-comments-div a').on('click', (event) => {

//    let accountId = $(event.currentTarget).attr('name');

//    // ajax call
//    let result = $.ajax(
//        {
//            url: `/account/${accountId}`,
//            method: 'GET'//,
//            //contentType: 'application/json',
//            //data: data
//        }).done(response => {

//            $('.js-hidden-acc').val(response.accountObj.accountId);
//            $('.js-account-dtls tbody').empty();
//            $('.js-account-dtls tbody').append(
//                `<tr id="${response.accountObj.accountId}">
//                    <td>
//                        ${response.accountObj.accountId}
//                    </td>                        
//                    <td>
//                        ${response.accountObj.balance}
//                    </td>
//                    <td>
//                        <select id="cmbStateAcc"> 
//                            <option value="0">Undefined</option>
//                            <option value="1">Active</option>
//                            <option value="2">Inactive</option>
//                        </select>                        
//                    </td>
//                </tr>`);
//        }).fail(failure => {

//        });
//});



//$('.js-save-change-acc-state').on('click', (event) => {

//    let state = document.getElementById("cmbStateAcc").value;
//    let account = $('.js-hidden-acc').val();

//    let data = JSON.stringify(
//        {
//            state: state,
//            account: account
//        });


//    let result = $.ajax(
//        {
//            url: '/account/changeStateAcc',
//            method: 'PUT',
//            contentType: 'application/json',
//            data: data
//        }).done(response => {
//            alert("Successed update the state of account.");
//        }).fail(failure => {
//            alert("Failed update the state of account.");
//        });

//});



//$('.js-add-customer').on('click', (event) => {

//    let lastName = $('.js-last-name').val();
//    let firstName = $('.js-first-name').val();
//    let countryCode = $('.js-country-code').val();
//    let vatNumber = $('.js-vat-number').val();
//    let address = $('.js-address').val();
//    let dateOfBirth = $('.js-date-of-birth').val();
//    let email = $('.js-email').val();
//    let phone = $('.js-phone').val();
//    let custType = $('.js-customer-type').val();


//    let data = JSON.stringify(
//        {
//            lastName: lastName,
//            firstName: firstName,
//            countryCode: countryCode,
//            vatNumber: vatNumber,
//            address: address,
//            dateOfBirth: dateOfBirth,
//            email: email,
//            phone: phone,
//            custType: parseInt(custType)
//        });


//    let result = $.ajax(
//        {
//            url: '/customer/addCustomerSave',
//            method: 'PUT',
//            contentType: 'application/json',
//            data: data
//        }).done(response => {
//            $('.js-alert-success').removeClass('collapse in');
//        }).fail(failure => {
//            $('.js-alert-failure').removeClass('collapse in');
//        });

//});



//$('.js-add-account-save').on('click', (event) => {

//    let Currency = $('.js-currency').val();
//    let Description = $('.js-description').val();
//    let CustomerId = $('.js-customer-id').val();

//    let data = JSON.stringify(
//        {
//            Currency: Currency,
//            Description: Description,
//            CustomerId: CustomerId
//        });

//    // ajax call
//    let result = $.ajax(
//        {
//            url: '/account/addAccountSave',
//            method: 'PUT',
//            contentType: 'application/json',
//            data: data
//        }).done(response => {
//            $('.js-alert-success').removeClass('collapse in');
//        }).fail(failure => {
//            $('.js-alert-failure').removeClass('collapse in');
//        });
//});



//$('.js-update-customer').on('click', (event) => {

//    let customerId = $('.js-customer-id').val();
//    let lastName = $('.js-last-name').val();
//    let firstName = $('.js-first-name').val();
//    let countryCode = $('.js-country-code').val();
//    let vatNumber = $('.js-vat-number').val();
//    let address = $('.js-address').val();
//    let dateOfBirth = $('.js-date-of-birth').val();
//    let email = $('.js-email').val();
//    let phone = $('.js-phone').val();
//    let custType = $('.js-customer-type').val();


//    let data = JSON.stringify(
//        {
//            lastName: lastName,
//            firstName: firstName,
//            countryCode: countryCode,
//            vatNumber: vatNumber,
//            address: address,
//            dateOfBirth: dateOfBirth,
//            email: email,
//            phone: phone,
//            custType: parseInt(custType)
//        });

//    // ajax call
//    let result = $.ajax(
//        {
//            url: `/customer/${customerId}`,
//            method: 'PUT',
//            contentType: 'application/json',
//            data: data
//        }).done(response => {
//            $('.js-alert-success').removeClass('collapse in');
//        }).fail(failure => {
//            $('.js-alert-failure').removeClass('collapse in');
//        });
//});




//$('.js-search-customer').on('click', (event) => {

//    let vatNumber = $('.js-search-vat-number').val();
//    let firstName = $('.js-search-first-name').val();
//    let lastName = $('.js-search-last-name').val();

//    let data =
//    {
//        vatNumber: vatNumber,
//        firstName: firstName,
//        lastName: lastName
//    };

//    // ajax call
//    let result = $.ajax(
//        {
//            url: '/customer/search',
//            method: 'GET',
//            //contentType: 'application/json',
//            data: data
//        }).done(response => {
//            $('.js-customers-list tbody').empty();
//            for (let i = 0; i < response.length; i++) {
//                $('.js-customers-list tbody').append(
//                    `<tr id="${response[i].customerId}">
//                        <td>
//                            <a href="/customer/${response[i].customerId}">${response[i].vatNumber}</a>
//                        </td>
//                        <td>
//                            ${response[i].firstname}
//                        </td>
//                        <td>
//                            ${response[i].lastname}
//                        </td>
//                    </tr>`);
//            }

//        }).fail(failure => {
//        });

//});


