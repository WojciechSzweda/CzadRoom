const connection = new signalR.HubConnectionBuilder()
    .withUrl("/dmHub")
    .build();

connection.start().then(console.log("started connection")).catch(err => console.error(err.toString()))

connection.on("ReceiveMessage", (user, message, roomId) => {
    appendNewMessage(user, message)
})

connection.on("ReceiveServerMessage", (message) => {
    appendNewServerMessage(message)
})

connection.on("ReadConfirm", (username, date) => {

})

document.getElementById("btnSendMsg").addEventListener("click", event => {
    sendMessage()
    event.preventDefault()
});

document.onkeyup = (key) => {
    if (key.keyCode === 13)
        sendMessage()
}

connection.on("Connected", () => {
    const roomID = document.getElementById("RoomID").value
    connection.invoke("JoinRoom", roomID).catch(err => console.error(err.toString()))
    getMessages()
    console.log(`Joined room ${roomID}`)
})

function sendMessage() {
    const roomID = document.getElementById("RoomID").value;
    const input = document.getElementById("messageInput")
    const message = input.value.trim();
    if (message.length > 0) {
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
        body: JSON.stringify({ RoomId: roomID, count: 10 })
    })
    const data = await getRequestData(request)
    if (data === null)
        return
    console.log(data)
    data.forEach(x => appendNewMessage(x.from.username, x.content, x.date))

    if (data[data.length - 1] !== undefined) {
        if (data[data.length - 1].read && data[data.length - 1].isCurrentUser) {
            appendNewServerMessage(`Message has been read ${formatDate(new Date(data[data.length - 1].readAt))}`)
        }
    }
}

async function getRequestData(request) {
    let [responseError, response] = await resolve(request)
    if (responseError === null) {
        let [dataErr, data] = await resolve(response.json())
        if (dataErr === null) {
            return data
        }
    }
    return null
}

function resolve(promise) {
    return promise.then(data => {
        return [null, data];
    }).catch(err => [err]);
}
