async function addFriend(friendId) {
    const url = `http://${window.location.host}/Community/AddFriend`
    const response = await fetch(url, {
        method: 'POST',
        headers: { 'content-type': 'application/json' },
        credentials: 'same-origin',
        body: JSON.stringify({ID: friendId})
    })
    console.log(await response.status)
}

function addFriendConfirm(friendId, friendUserName) {
    document.getElementById('addFriendModalUsername').innerText = friendUserName
    document.getElementById('addFriendModalBtn').onclick = () => addFriend(friendId)
}
