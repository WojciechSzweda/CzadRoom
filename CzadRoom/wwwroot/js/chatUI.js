function generateMessageHTML(user, message, date) {
    const li = `<li class="msg-li">
        <div class="message">
            <div class="message-client">
                <p>${user}<span class="message-date">${date === undefined ? formatDate(new Date()) : formatDate(new Date(Date.parse(date)))}</span></p>
            </div>
            <div class="message-content">
                <p>${message}</p>
            </div>
        </div>
        </li>`
    return li
}

function generateMessageLiNode(user, message, date) {
    return stringToHtmlNode(generateMessageHTML(user, message, date))
}

function generateServerMessageHTML(message) {
    const li = `<li class="msg-li">
                <div class="message-content">
                    <p>${message}</p>
                </div>
              </li>`
    return li
}

function generateServerMessageLiNode(message) {
    return stringToHtmlNode(generateServerMessageHTML(message))
}

function formatDate(date) {
    return `@${date.getHours()}:${date.getMinutes() < 10 ? '0' : ''}${date.getMinutes()} - ${('0' + date.getDate()).slice(-2)}.${('0' + date.getMonth()).slice(-2)}`
}

function stringToHtmlNode(html) {
    const template = document.createElement('template')
    template.innerHTML = html
    return template.content.firstChild
}

function generateClientSidebarLi(clientId, clientName) {
    const clientLi = document.createElement('li')
    clientLi.setAttribute('id', `li-${clientName}`)
    clientLi.setAttribute('class', 'userInSidebar')
    return clientLi
}

function generateClientSidebarLiWithFriendAdd(clientId, clientName) {
    const clientLi = document.createElement('li')
    clientLi.setAttribute('id', `li-${clientName}`)
    clientLi.setAttribute('class', 'userInSidebar')

    const button = document.createElement('button')
    button.setAttribute('data-toggle', 'modal')
    button.setAttribute('data-target', '#addFriendConfirmModal')
    button.innerText = clientName
    button.onclick = () => addFriendConfirm(clientId, clientName)
    clientLi.appendChild(button)
    return clientLi
}

function appendNewMessage(user, message, date) {
    const li = generateMessageLiNode(user, message, date)
    const msgList = document.getElementById("messagesList")
    msgList.appendChild(li);
    msgList.scrollTo(0, msgList.scrollHeight)
}

function appendNewServerMessage(message) {
    const li = generateServerMessageLiNode(message)
    const msgList = document.getElementById("messagesList")
    msgList.appendChild(li)
    msgList.scrollTo(0, msgList.scrollHeight)

}