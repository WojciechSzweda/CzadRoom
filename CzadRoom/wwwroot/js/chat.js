const connection = new signalR.HubConnectionBuilder()
    .withUrl("/chatHub")
    .build();


connection.on("ReceiveMessage", (user, message, roomId) => {
    //const encodedMsg = user + " says " + message;
    //const li = document.createElement("li");
    //li.textContent = encodedMsg;
    const li = stringToHtmlNode(generateMessageHTML(user, message))
    const msgList = document.getElementById("messagesList")
    msgList.appendChild(li);
    msgList.scrollTo(0, msgList.scrollHeight)
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
    let clientLi = document.createElement('li')
    clientLi.innerText = client
    clientLi.setAttribute('id', `li-${client}`)
    //document.getElementById(`usersInRoom`).appendChild(clientLi)
})

connection.on("ClientLeft", (client) => {
    const encodedMsg = `${client} has left`
    const li = document.createElement("li")
    li.textContent = encodedMsg
    document.getElementById("messagesList").appendChild(li)
    document.getElementById(`li-${client}`).remove()
})

connection.on("Connected", () => {
    const roomID = document.getElementById("RoomID").value
    connection.invoke("JoinRoom", roomID).catch(err => console.error(err.toString()))
    console.log(`Joined room ${roomID}`)
})


document.getElementById("btnSendMsg").addEventListener("click", event => {
    sendMessage()
    event.preventDefault()
});

window.onbeforeunload = () => {
    const roomID = document.getElementById("RoomID").value;
    connection.invoke("LeaveRoom", roomID).catch(err => console.error(err.toString()));
    return
}

document.onkeyup = (key) => {
    if (key.keyCode === 13)
        sendMessage()
}

connection.start().catch(err => console.error(err.toString()));

function sendMessage() {
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
}