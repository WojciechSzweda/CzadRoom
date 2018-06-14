const connection = new signalR.HubConnectionBuilder()
    .withUrl("/chatHub")
    .build()


connection.on("ReceiveMessage", (user, message, roomId) => {
    appendNewMessage(user,message)
})

connection.on("ReceiveServerMessage", (message) => {
    appendNewServerMessage(message)
})

connection.on("ClientJoined", (clientId, clientName) => {
    if (document.getElementById(`li-${clientName}`) !== null)
        return
    const msg = `${clientName} has joined`
    appendNewServerMessage(msg)
    const clientLi = generateClientSidebarLiWithFriendAdd(clientId, clientName)
    document.getElementById(`usersInRoom`).appendChild(clientLi)
})

connection.on("ClientLeft", (client) => {
    const msg = `${client} has left`
    appendNewServerMessage(msg)
    document.getElementById(`li-${client}`).remove()
})

connection.on("Connected", () => {
    const roomID = document.getElementById("RoomID").value
    connection.invoke("JoinRoom", roomID).catch(err => console.error(err.toString()))
    getMessages()
})


document.getElementById("btnSendMsg").addEventListener("click", event => {
    sendMessage()
    event.preventDefault()
});

document.onkeyup = (key) => {
    if (key.keyCode === 13)
        sendMessage()
}

connection.start().catch(err => console.error(err.toString()))

function sendMessage() {
    if (connection.connection.connectionState !== 1)
        return
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

async function getMessages() {
    const roomID = document.getElementById("RoomID").value;
    const request = fetch('./GetMessages', {
        method: 'POST',
        headers: { 'content-type': 'application/json' },
        credentials: 'same-origin',
        body: JSON.stringify({ roomID, count: 10 })
    })
    const data = await getRequestData(request)
    if (data === null)
        return
    data.forEach(x => appendNewMessage(x.username, x.content, x.date))
}

