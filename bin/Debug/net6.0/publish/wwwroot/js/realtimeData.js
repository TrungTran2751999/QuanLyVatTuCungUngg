const connection = new signalR.HubConnectionBuilder()
.withUrl("/chathub")
.build();

connection.on("ReiceiveData", (datas) => {
    $("#data-table tbody").html("");
    datas = JSON.parse(datas);
    let totalRow = "";
    for(let i=0; i<datas.length; i++){
        let row = `<tr>
                      <td>${datas[i]["idSystem"]||datas[datas.length-2]["idSystem"]+1}</td>
                      <td>${datas[i]["name"]||datas[i]["Name"]}</td>
                      <td>${datas[i]["age"]||datas[i]["Age"]}</td>
                      <td>${datas[i]["email"]||datas[i]["Email"]}</td>
                   </tr>`;
        totalRow += row;
    }
    $("#data-table tbody").append(totalRow);
});

connection.onclose((error) => {
    connection.start()
        .then(function() {
        console.log("Connected to SignalR hub");
    })
        .catch(function() {
        console.error(error);
    });
});

connection.start()
.then(function() {
    console.log("Connected to SignalR hub");
})
.catch(function() {
    console.error(error);
});
function updateRealtime(data){
    connection.invoke("UpdateStudent",data)
        .catch(error => {
            console.error(error);
    });
}