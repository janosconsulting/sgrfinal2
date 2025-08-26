function setData (item) {

    if (getData(item) != false) {
        alert("Item already added in todo");
    } else {
        var data = getData(); // call getdata handler for getting  data from list 
        data = (data != false) ? data : [];
        data.push(item);
        data = JSON.stringify(data);

        localStorage.setItem('items', data);
    }
}

function getData (item = null){
    /*
    * localStorage.getItem(<itemname>) main method 
    * (predefined method of js) for getting item from localstorage
    */
    var data = JSON.parse(localStorage.getItem('items'));
    if (data) {

        if (item) {
            if (data.indexOf(item) != -1) {
                return data[item];
            } else {
                return '';
            }
        }
        return data;
    }
    return [];
}

function countListItem() {
    var data = getData();
    if (data) {
        return data.length;
    }
    return 0;
}


function removeData(itemId) {
    var data = getData();
    if (data) {
        var newData = data.filter((v, i) => { return i != itemId });
        newData = JSON.stringify(newData);
        localStorage.setItem('items', newData);
    } else {
        alert("no data found");
    }

} 



function showCountItems() {
    var texto = "Your cart (" + countListItem() + " Items)";
    $("#count-items").innerHTML = texto;
    
}
