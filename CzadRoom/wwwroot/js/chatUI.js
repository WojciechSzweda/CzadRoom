function generateMessageHTML(user, message, date) {
    let li = `<li class="msg-li">
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
    let li = `<li class="msg-li">
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
    let template = document.createElement('template')
    template.innerHTML = html
    return template.content.firstChild
}

function generateClientSidebarLi(client) {
    let clientLi = document.createElement('li')
    clientLi.innerText = client
    clientLi.setAttribute('id', `li-${client}`)
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
    document.getElementById("messagesList").appendChild(li)
}