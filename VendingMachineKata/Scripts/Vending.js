var coinInserted = false; // A boolean to know if there is a coin inserted for the current transaction

var coinsAdded = []; // an array of coins to keep track of what all coins have been added

var numberOfCoins = 0; // number of coins - Just for display purpose

var coinSum = 0; // sum of coins added - for checking if the user has inserted enough amount

var remainingBalance = 0; //For transactions

//Accepted coins
var listOfAcceptedCoins = {
    quarter: { weight: 25, size: 3, value: 0.25 },
    dime: { weight: 10, size: 1, value: 0.10 },
    nickel: { weight: 5, size: 2, value: 0.05 }
}

//Update display based on the current state of transaction
function updateDisplay() {

    if (coinInserted)
        document.getElementById('coin-display').innerHTML = 'You have inserted' + numberOfCoins + 'coins amounting to $' + coinSum;
    else
        document.getElementById('coin-display').innerHTML = 'Please Insert Coins';

}

//Reset the transaction
function returnAllCoins() {

    coinsAdded = [];
    coinInserted = false;
    coinSum = 0;
    remainingBalance = 0;
    numberOfCoins = 0;

}

//Insert Coin
function insertCoin(insertedCoin) {
    var insertedCoinValue = validateCoin(insertedCoin);
    if (insertedCoinValue != null) {

        coinsAdded.push(insertedCoin);

        numberOfCoins++;

        coinInserted = true;

        coinSum = coinSum + insertedCoin;
    }
    else {
        alert('Insert Valid Coin - The ones in the list');
    }

}


//Update Display For Vending

function vendingDisplay(remainingBalance) {

    if (remainingBalance === 0)
        updateCustomMessage('remaining-balance', 'You have tendered the exact change. Thank you!');
    if (remainingBalance > 0)
        updateCustomMessage('remaining-balance', 'Please collect a return of $' + remainingBalance + 'Thank you!');
    if (remainingBalance < 0)
        updateCustomMessage('remaining-balance', 'You need to pay $' + remainingBalance + 'more');

}


function updateCustomMessage(id, message) {
    document.getElementById(id).innerHTML = message;
}

//Calculate Balance
function vend() {
    var productCost = document.getElementById('product').value;
    remainingBalance = coinSum - productCost;
    vendingDisplay(remainingBalance);

    //Ajax Call

    if (remainingBalance >= 0) {
            $.post('', { insertedCoins: coinsAdded, totalSum : coinSum })
                     .done(function (response) {
                        $('#console').html(response);

    });

    }
    else {
        updateCustomMessage('', 'Please tender the full amount');
    }
}


//Validate Coins - Validation happened based on weight and size
//Returns Coin value
function validateCoin(insertedCoin) {

    for (var coin in listOfAcceptedCoins) {

        if (insertedCoin.weight == listOfAcceptedCoins[coin].weight &&
             insertedCoin.size == listOfAcceptedCoins[coin].size) //Coin Value matches our list of accepted coins
            return listOfAcceptedCoins[coin].value;
    }
    return null;
}

// Refill Vending Machine
function refillVendingMachine() {
    $.post('', { insertedCoins: coinsAdded, totalSum: coinSum })
                     .done(function (response) {
                         $('#console').html(response);

                     });
}