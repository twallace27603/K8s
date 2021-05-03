// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(document).ready(function () {
    var quizLoaded = false;
    $('a[data-toggle="tab"]').on('shown.bs.tab', function (e) {
        e.target // newly activated tab
        e.relatedTarget // previous active tab
        methodName = $(e.target).attr("href").substr(1);
        operations[methodName]();
    });
    var operations = []

    operations["info"] = function () {
        $.get("/api/work/info").done(function (data) {
            var output = "<h3>Environment</h3><table>";
            for (var envKey in data.envVariables) {
                output += "<tr><td>" + envKey + "</td><td>" + data.envVariables[envKey] + "</td></tr>"
            };
            output += "</table>";
            $("#info div.content").html(output);
        });
    }
    operations["containers"] = function () {
        if (!quizLoaded) {
            try {
                $.get("/api/work/quizNames").done(function (data) {
                    var items = "";
                    $(data).each(function (index, quiz) {
                        items += "<option value='" + quiz.quizID + "'>" + quiz.quizName + "</option>";
                    });
                    $("#quizList").html(items);
                    quizLoaded = true;
                });

                $("#quizList").change(function () {
                    var id = $(this).val();
                    $.get("/api/work/quiz/" + id).done(function (data) {
                        var output = "<div>" + data.quizName + "</div><ul>";
                        $.each(data.topics, function (index, topic) {
                            output += "<li><ul>" + topic.topicName;
                            $.each(topic.questions, function (i, question) {
                                output += "<li>" + question.questionText + "</li>";
                            });
                            output += "</ul></li>";
                        });
                        output += "</ul>";

                        $("#quiz").html(output);
                    });
                });


            } catch (ex) {

            }
        }
    }
    operations["monitoring"] = function () {

    }
    $("#load").click(function (e) {
        e.preventDefault();
        $.get("/api/work/load").done(function (data) {
            $("#quiz").text("Uploaded " + data + " quizes.");
        });


    });

    $("#sendRequests").click(function () {
        var result = "";
        var iterations = $("#requests").val();
        var payloadSize = 100;
        var generateErrors = $('#errors').is(':checked');
        var delay = $('#delay').is(':checked');
        var errors = {};
        var messageCount = 0;
        var byteCount = 0;
        $("#results").html("");
        $("#displayErrors").html("");

        var workers = [];
        for (var lcv = 0; lcv < iterations; lcv++) {
            var w = new Worker("js/worker.js");
            w.onmessage = function (message) {
                if (message.data.success) {
                    messageCount++;
                    byteCount += message.data.payload;
                    $("#results").html("<p>Received " + messageCount + " messages with " + byteCount + " total bytes.</p>");
                } else {
                    if (errors[message.data.payload]) {
                        errors[message.data.payload]++;
                    } else {
                        errors[message.data.payload] = 1;
                    }
                    output = "Errors:<br />"
                    for (var key in errors) {
                        output += "<br />" + key + ": " + errors[key];
                    }
                    $("#displayErrors").html(output);
                }
            }
            w.postMessage({
                payloadSize: payloadSize,
                generateErrors: generateErrors,
                delay:delay
            });

            /*
            $.get("/api/work/resultpayload/" + payloadSize + "/" + generateErrors + "/" + delay).done(function (data) {
                //result +="<li>Received " + data.length + " bytes at " + Date() + "</li>";
                messageCount++;
                byteCount += data.length;
                $("#results").html("<p>Received " + messageCount + " messages with " + byteCount + " total bytes.</p>");
            }).fail(function (jqXhr, failText, ex) {
                if (errors[jqXhr.status]) {
                    errors[jqXhr.status]++;
                } else {
                    errors[jqXhr.status] = 1;
                }
                output = "Errors:<br />"
                for (var key in errors) {
                    output += "<br />" + key + ": " + errors[key];
                }
                $("#displayErrors").html(output);
            });
            */
        }

    });

    $("#loadProcessor").click(function () {
        var seconds = $("#seconds").val();
        if (!seconds || seconds < 60) seconds = 60;
        $("#results").html("Starting load process");
        $("#displayErrors").html("");
        
        $.get("/api/work/processor/" + seconds).done(function (data) {
            $("#results").html(data);
        }).fail(function (jqXhr, failText, ex) {
            $("#results").html(failText);
        });
    });

    operations["info"]();

});
