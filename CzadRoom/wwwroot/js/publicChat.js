const connection = new signalR.HubConnectionBuilder()
    .withUrl("/publicHub")
    .build()

let username

connection.on("ReceiveMessage", (user, message, roomId) => {
    appendNewMessage(user, message)
})

connection.on("ReceiveServerMessage", (message) => {
    appendNewServerMessage(message)
})

connection.on("ClientJoined", (clientName) => {
    console.log(clientName)
    if (document.getElementById(`li-${clientName}`) !== null)
        return
    const msg = `${clientName} has joined`
    appendNewServerMessage(msg)
    const clientLi = generateClientSidebarLi(clientName, clientName)
    document.getElementById(`usersInRoom`).appendChild(clientLi)
})

connection.on("ClientLeft", (client) => {
    const msg = `${client} has left`
    appendNewServerMessage(msg)
    document.getElementById(`li-${client}`).remove()
})

connection.on("Connected", async () => {
    const roomID = document.getElementById("RoomID").value
    username = await getUsername()
    console.log(username)
    connection.invoke("JoinRoom", roomID, username).catch(err => console.error(err.toString()))
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

async function getUsername() {
    const request = fetch('./GetUsername', {
        method: 'POST',
        credentials: 'same-origin'
    })
    const data = await getRequestData(request)
    if (data === null)
        return
    console.log(data)
    return data
}

