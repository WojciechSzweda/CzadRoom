﻿const connection = new signalR.HubConnectionBuilder()
    .withUrl("/chatHub")
    .build();


connection.on("ReceiveMessage", (user, message, roomId) => {
    const encodedMsg = user + " says " + message;
    const li = document.createElement("li");
    li.textContent = encodedMsg;
    document.getElementById("messagesList").appendChild(li);
});

connection.on("ReceiveServerMessage", (message) => {
    const encodedMsg = "server says " + message;
    const li = document.createElement("li");
    li.textContent = encodedMsg;
    document.getElementById("messagesList").appendChild(li);
});

connection.on("ClientJoined", (client) => {
    const encodedMsg = `${client} has joined`
    const li = document.createElement("li")
    li.textContent = encodedMsg
    document.getElementById("messagesList").appendChild(li)
})

connection.on("ClientLeft", (client) => {
    const encodedMsg = `${client} has left`
    const li = document.createElement("li")
    li.textContent = encodedMsg
    document.getElementById("messagesList").appendChild(li)
})

connection.on("Connected", () => {
    const roomID = document.getElementById("RoomID").value
    connection.invoke("JoinRoom", roomID).catch(err => console.error(err.toString()))
    console.log(`Joined room ${roomID}`)
})


document.getElementById("sendButton").addEventListener("click", event => {
    const roomID = document.getElementById("RoomID").value;
    const input = document.getElementById("messageInput")
    const message = input.value;
    if (message.length > 0) {
        if (message[0] === '/')
            connection.invoke("SendCommand", message.substring(1)).catch(err => console.error(err.toString()));
        else
            connection.invoke("SendRoomMessage", roomID, message).catch(err => console.error(err.toString()));
    }
    input.value = ""
    event.preventDefault();
});

window.onbeforeunload = () => {
    const roomID = document.getElementById("RoomID").value;
    connection.invoke("LeaveRoom", roomID).catch(err => console.error(err.toString()));
    return
}

document.onkeyup = (key) => {
    if (key.keyCode === 13)
        document.getElementById("sendButton").click()
}

connection.start().catch(err => console.error(err.toString()));