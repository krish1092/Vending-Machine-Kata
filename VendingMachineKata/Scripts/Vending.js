var coinsAdded = []; // an array of coins to keep track of what all coins have been added

var numberOfCoins = 0; // number of coins - Just for display purpose

var coinSum = 0; // sum of coins added - for checking if the user has inserted enough amount

var remainingBalance = 0; //For transactions

var products = {}; //Will be loaded after page is loaded

var listOfAcceptedCoins = {}; //Accepted coins


//Reset the transaction
function returnAllCoins() {
    document.getElementById('collection-area').innerHTML = document.getElementById('collection-area').innerHTML +', ' +'$' + coinSum;
    reset();
}

//Reset the transaction
function reset() {
    coinsAdded = [];
    coinSum = 0;
    remainingBalance = 0;
    numberOfCoins = 0;
}


//Insert Coin
function insertCoin(insertedCoin) {
    var coinSelect = document.getElementById('coin-select').selected;
   
    //Get the weight and size of the selected coin
    insertedCoin = { 
        Weight: coinSelect.options[coinSelect.selectedIndex].getAttribute('data-coin-weight') ,
        Size : coinSelect.options[coinSelect.selectedIndex].getAttribute('data-coin-size')
    };


    var validated = validateCoin(insertedCoin);
    if (validated == true) {

        $.post('Vending/GetCoinValue', { Coin: insertedCoin })
            .done(function (response) {

                //Backend validation if user plays smart by editing the javascript file
                if (CoinValue === 0)
                    updateCustomMessage('Stop editing javascript - You won\'t get through anyway!');
                else {

                    coinsAdded.push(insertedCoin);
                    numberOfCoins++;
                    coinSum = coinSum + response.CoinValue;

                    updateCustomMessage('You have inserted' + numberOfCoins + 'coins amounting to $' + coinSum);
                }
        });
        
    }
    else {
        document.getElementById('console').innerHTML('Insert Valid Coin - The ones in the list');
    }

}


//Display message in console
function updateCustomMessage(message) {
    document.getElementById('console').innerHTML = message;
}

//Calculate Balance
function vend(product) {

    var productCost = product.getAttribute('data-product-price');
    var productName = product.getAttribute('data-product-name');

    remainingBalance = coinSum - productCost;
    
    if (productAvailabilty(productName)) {


        if (remainingBalance >= 0) {

            //Ajax Call
            $.post('Vending/Complete', { Coins : coinsAdded, totalSum: coinSum, productSelected: productName })
                         .done(function (response) {
                             if (response.Validated == true && response.RemainingAmount > 0) {
                                 document.getElementById('console').innerHTML = 'I don\'t have sufficient change. please tender exact change';
                                 returnAllCoins();
                             }
                             else if (response.Validated == false){

                                 document.getElementById('console').innerHTML = 'Please tender valid coins';
                                 returnAllCoins();
                             }
                             else if (response.Validated == true && response.RemainingAmount == 0) {
                                 document.getElementById('console').innerHTML = 'Thank you for placing the order! Please collect your product';

                                 //Display to the user the product in the collect area
                                 document.getElementById('collection-area').innerHTML = document.getElementById('collection-area').innerHTML + '\n' + productName;

                                 //Deduct the existing set of products
                                 products[productName]--;
                             }

                             
                         });

        }
        else {
            updateCustomMessage('Please tender the full amount');
        }
    }
    else {
        updateCustomMessage('Please request a refill. The product is not available for selection');
    }
}


//Validate Coins - Validation happened based on weight and size
//Returns Coin value
function validateCoin(insertedCoin) {

    for (var coin in listOfAcceptedCoins) {

        if (insertedCoin.Weight == listOfAcceptedCoins[coin].Weight &&
             insertedCoin.Size == listOfAcceptedCoins[coin].Size) //Coin Value matches our list of accepted coins
            return listOfAcceptedCoins[coin].value;
    }
    return null;
}

// Refill Vending Machine
function refillVendingMachine() {
    $.post('Vending/Refill')
                     .done(function (response) {
                         document.getElementById('console').html(response);
                     });
}


// Check if product is available
function productAvailabilty(productName) {
    if (products[productName] > 0)
        return true;
    return false;
}


//Clear Collection Area 
function clearCollection() {
    document.getElementById('collection-area').innerHTML = '';
    document.getElementById('console').innerHTML = 'Insert Coin';
    reset();
}


//On document Ready - Fill the products Javascript object
(function () {
    
    var productList = document.getElementsByClassName('product');
    productList.forEach(function (product) {
        products[product.getAttribute('productName')] = product.getAttribute('product-price');
    });

    var coinList = document.getElementsByClassName('coin-select');

    for (var index = 0; index < coinList.length; index++)
    {
        listOfAcceptedCoins[coinList.options[i].text] = {
            Size: coinList.options[i].getAttribute('data-coin-size'),
            Weight: coinList.options[i].getAttribute('data-coin-weight')
        };
    }
    
})();