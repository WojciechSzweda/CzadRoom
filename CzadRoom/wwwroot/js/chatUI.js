//function generateMessageHTML(user, message) {
//    let main = document.createElement('li')
//    main.setAttribute('class', 'message')
//    let userDiv = document.createElement('div')
//    userDiv.setAttribute('class', 'col-xs-2')
//    userDiv.textContent = user
//    let msgDiv = document.createElement('div')
//    msgDiv.setAttribute('class', 'col-xs-10')
//    msgDiv.textContent = message

//    main.appendChild(userDiv)
//    main.appendChild(msgDiv)

//    return main
//}

function generateMessageHTML(user, message) {
    let li = `<li style="width:100%">
        <div class="msj macro">
        <div class="text text-l">
        <p>${user} <small>${formatDate(new Date())}</small></p>
        <p>${message}</p>
        </div>
        </div>
        </li>`
    return li
}

function formatDate(date) {
    return `${date.getHours()}:${date.getMinutes() < 10 ? '0' : ''}${date.getMinutes()}`
}

function stringToHtmlNode(html) {
    let template = document.createElement('template')
    template.innerHTML = html
    return template.content.firstChild
}