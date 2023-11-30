    const connection = new signalR.HubConnectionBuilder()
    .withUrl("/chathub")
    .build();

    connection.on("ReceiveMessage", (user, message) => {
        console.log(user + ": " + message);
    });

    connection.onclose((error) => {
        alert("MAT KET NOT")
    });

    connection.start()
    .then(function() {
        console.log("Connected to SignalR hub");
    })
    .catch(function() {
        console.error(error);
    });
    document.getElementById("sendButton").addEventListener("click", event => {
        event.preventDefault();
    
        const user = document.getElementById("userInput").value;
        const message = document.getElementById("messageInput").value;
    
        connection.invoke("SendMessage", user, message)
            .catch(error => {
                console.error(error);
            });
    
        document.getElementById("messageInput").value = "";
    });