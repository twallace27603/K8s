onmessage = function (message) {
    fetch("/api/work/resultpayload/" + message.data.payloadSize + "/" + message.data.generateErrors + "/" + message.data.delay)
         .then(response => {
            if (response.ok) {
                byteCount = 100;
                postMessage({
                    success: true,
                    payload: byteCount
                });
            } else {
                postMessage({
                    success:false,
                    payload:response.status
                })
                       
            }
        }
    );
    /*.fail(function (jqXhr, failText, ex) {
        postMessage({
            success:false,
            payload:jqXhr.status
        })
     });*/
}