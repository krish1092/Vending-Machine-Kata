var coinsAdded = []; // an array of coins to keep track of what all coins have been added

var numberOfCoins = 0; // number of coins - Just for display purpose

var coinSum = 0; // sum of coins added - for checking if the user has inserted enough amount

var remainingBalance = 0; //For transactions

var products = {}; //Will be loaded after page is loaded

var listOfAcceptedCoins = {}; //Accepted coins


/*Note: The application has been developed with plain javascript without use of specific libraries 
    Most of the DOM changes can be handled better with a javascript library like Knockout, Moustache, etc. But the requirements do not specify a library as such.

*/


//Insert Coin
function insertCoin(insertedCoin) {
    var coinSelect = document.getElementById('coin-select');
   
    //Get the weight and size of the selected coin
    insertedCoin = { 
        Weight: coinSelect.options[coinSelect.selectedIndex].getAttribute('data-coin-weight') ,
        Size: coinSelect.options[coinSelect.selectedIndex].getAttribute('data-coin-size'),
        Name: coinSelect.options[coinSelect.selectedIndex].text
    };


    var validated = validateCoin(insertedCoin);
    insertedCoin = JSON.stringify(insertedCoin);

    if (validated == true) {

        $.get('GetCoinValue', { CoinAsJSONString : insertedCoin })
            .done(function (response) {

                //Backend validation if user plays smart by editing the javascript file
                if (response.CoinValue === 0)
                    updateCustomMessage('Stop editing javascript - You won\'t get through anyway!');
                else {

                    coinsAdded.push(insertedCoin); //Add the coin to the list of coins already there
                    numberOfCoins++; 
                    coinSum = coinSum + response.CoinValue; //Total sum of the coins added

                    updateCustomMessage('You have inserted ' + numberOfCoins + ' coins amounting to $' + coinSum);
                }
        });
        
    }
    else {
        document.getElementById('console').innerHTML = 'Insert Valid Coin - The ones in the list';
    }

}



//Validate Coins - Validation happened based on weight and size
function validateCoin(insertedCoin) {

    for (var coin in listOfAcceptedCoins) {

        if (insertedCoin.Weight == listOfAcceptedCoins[coin].Weight &&
             insertedCoin.Size == listOfAcceptedCoins[coin].Size) //Coin Value matches our list of accepted coins
            return true;
    }
    return false;
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

    productAvailabilty(productName);
    
}


//finish vending callback function
function finishVending(productCount, productName) {
    if (productCount > 0) {

        if (remainingBalance >= 0) {

            //Ajax Call
            $.post('CompleteVending', {FinishVendingString : JSON.stringify({ Coins: coinsAdded, RemainingAmount: remainingBalance, ProductName: productName })})
                         .done(function (response) {
                             if (response.CoinsValidated == true && response.RemainingAmount > 0) { //No sufficient change
                                 document.getElementById('console').innerHTML = 'I don\'t have sufficient change. please tender exact change';
                                 returnAllCoins();
                             }
                             else if (response.CoinsValidated == false) { // Coin Validation Fails

                                 updateCustomMessage('Please tender valid coins');
                                 returnAllCoins();
                             }
                             else if (response.ProductValidated == false) {
                                 updateCustomMessage('Please choose a valid product');
                                 returnAllCoins();
                             }
                             else if (response.CoinsValidated == true && response.RemainingAmount == 0 && response.ProductValidated == true) {
                                 updateCustomMessage('Thank you for placing the order! Please collect your product');

                                 //Display to the user the product in the collect area
                                 document.getElementById('collection-area').innerHTML = document.getElementById('collection-area').innerHTML + '\n' + productName;

                                 //Return the amount
                                 if (response.AmountToBeReturned > 0)
                                     document.getElementById('collection-area').innerHTML = document.getElementById('collection-area').innerHTML + ', ' + response.AmountToBeReturned;

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


// Check if product is available
function productAvailabilty(productName) {

    $.get('GetProductCount', { ProductName: productName })
        .done(function(response){
            finishVending(response.ProductCount, productName);
        });
                     
    
}



//Reset the transaction
function returnAllCoins() {
    document.getElementById('collection-area').innerHTML = document.getElementById('collection-area').innerHTML + ', ' + '$' + coinSum;
    reset();
}

//Reset the transaction
function reset() {
    coinsAdded = [];
    coinSum = 0;
    remainingBalance = 0;
    numberOfCoins = 0;
}


// Refill Vending Machine
function refillVendingMachine() {
    $.get('RefillVendingMachine')
                     .done(function (response) {
                         document.getElementById('console').html(response);
                     });
}


//Clear Collection Area 
function clearCollection() {
    document.getElementById('collection-area').innerHTML = '';
    document.getElementById('console').innerHTML = 'Insert Coin';
    reset();
}


//On document Ready - Fill the products & acceptedCoins Javascript object
(function () {
    
    var productList = document.getElementsByClassName('product');

    for (var index = 0; index < productList.length; index++) {
        products[productList[index].getAttribute('data-product-name')] = productList[index].getAttribute('data-product-price');
    }
    
    var coinList = document.getElementById('coin-select');

    for (index = 0; index < coinList.length; index++)
    {
        listOfAcceptedCoins[coinList.options[index].text] = {
            Size: coinList.options[index].getAttribute('data-coin-size'),
            Weight: coinList.options[index].getAttribute('data-coin-weight')
        };
    }
    
})();