const connection = new signalR.HubConnectionBuilder()
    .withUrl('/dmHub')
    .build();

connection.start().catch(err => console.error(err.toString()))

connection.on('ReceiveMessage', (user, message, roomId) => {
    appendNewMessage(user, message)
})

connection.on('ReceiveServerMessage', (message) => {
    appendNewServerMessage(message)
})

document.getElementById('btnSendMsg').addEventListener('click', event => {
    sendMessage()
    event.preventDefault()
});

document.onkeyup = (key) => {
    if (key.keyCode === 13)
        sendMessage()
}

window.onfocus = () => {
    if (connection.connection.connectionState !== 1)
        return
    const roomID = document.getElementById('RoomID').value
    connection.invoke('Focused', roomID, true).catch(err => console.error(err.toString()))
}

window.onblur = () => {
    if (connection.connection.connectionState !== 1)
        return
    const roomID = document.getElementById('RoomID').value
    connection.invoke('Focused', roomID, false).catch(err => console.error(err.toString()))
}

connection.on('Connected', () => {
    const roomID = document.getElementById('RoomID').value
    connection.invoke('JoinRoom', roomID).catch(err => console.error(err.toString()))
    getMessages()
})

let unreadMessages = false

connection.on('Focused', (userId, isFocused) => {
    const userHeader = document.getElementById(`header-${userId}`)
    if (userHeader === null)
        return
    userHeader.style.color = isFocused ? 'green' : 'blue'
    if (unreadMessages) {
        appendNewServerMessage(`Message has been read ${formatDate(new Date())}`)
        unreadMessages = false
    }
})

connection.on('FocusedError', (err) => {
    
})

connection.on('ClientLeft', (userId) => {
    const userHeader = document.getElementById(`header-${userId}`)
    if (userHeader === null)
        return
    userHeader.style.color = 'black'
})

function sendMessage() {
    if (connection.connection.connectionState !== 1)
        return
    const roomID = document.getElementById('RoomID').value;
    const input = document.getElementById('messageInput')
    const message = input.value.trim();
    if (message.length > 0) {
        connection.invoke('SendRoomMessage', roomID, message).catch(err => console.error(err.toString()));
        unreadMessages = true
    }
    input.value = ''
}

async function getMessages() {
    const roomID = document.getElementById('RoomID').value;
    const request = fetch('./GetMessages', {
        method: 'POST',
        headers: { 'content-type': 'application/json' },
        credentials: 'same-origin',
        body: JSON.stringify({ RoomId: roomID, count: 100 })
    })
    const data = await getRequestData(request)
    if (data === null)
        return
    data.forEach(x => appendNewMessage(x.from.username, x.content, x.date))

    if (data[data.length - 1] !== undefined) {
        if (data[data.length - 1].read && data[data.length - 1].isCurrentUser) {
            appendNewServerMessage(`Message has been read ${formatDate(new Date(data[data.length - 1].readAt))}`)
        }
    }
}