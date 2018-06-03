function generateMessageHTML(user, message) {
    let li = `<li class="msg-li">
        <div class="message">
            <div class="message-client">
                <p>${user}<span class="message-date">${formatDate(new Date())}</span></p>
            </div>
            <div class="message-content">
                <p>${message}</p>
            </div>
        </div>
        </li>`
    return li
}

function generateMessageLiNode(user, message) {
    return stringToHtmlNode(generateMessageHTML(user,message))
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
    return `${date.getHours()}:${date.getMinutes() < 10 ? '0' : ''}${date.getMinutes()}`
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