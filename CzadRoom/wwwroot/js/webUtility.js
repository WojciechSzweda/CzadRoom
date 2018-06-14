async function getRequestData(request) {
    const [responseError, response] = await resolve(request)
    if (responseError === null) {
        const [dataErr, data] = await resolve(response.json())
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