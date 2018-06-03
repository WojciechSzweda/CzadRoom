const connection = new signalR.HubConnectionBuilder()
    .withUrl("/chatHub")
    .build();


connection.on("ReceiveMessage", (user, message, roomId) => {
    const li = generateMessageLiNode(user, message)
    const msgList = document.getElementById("messagesList")
    msgList.appendChild(li);
    msgList.scrollTo(0, msgList.scrollHeight)
});

connection.on("ReceiveServerMessage", (message) => {
    const li = generateServerMessageLiNode(message)
    document.getElementById("messagesList").appendChild(li);
});

connection.on("ClientJoined", (client) => {
    if (document.getElementById(`li-${client}`) !== null)
        return
    const msg = `${client} has joined`
    const li = generateServerMessageLiNode(msg)
    document.getElementById("messagesList").appendChild(li)
    const clientLi = generateClientSidebarLi(client)
    document.getElementById(`usersInRoom`).appendChild(clientLi)
})

connection.on("ClientLeft", (client) => {
    const msg = `${client} has left`
    const li = generateServerMessageLiNode(msg)
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

document.onkeyup = (key) => {
    if (key.keyCode === 13)
        sendMessage()
}

connection.start().catch(err => console.error(err.toString()));

function sendMessage() {
    const roomID = document.getElementById("RoomID").value;
    const input = document.getElementById("messageInput")
    const message = input.value.trim();
    if (message.length > 0) {
        if (message[0] === '/')
            connection.invoke("SendCommand", message.substring(1)).catch(err => console.error(err.toString()));
        else
            connection.invoke("SendRoomMessage", roomID, message).catch(err => console.error(err.toString()));
    }
    input.value = ""
}