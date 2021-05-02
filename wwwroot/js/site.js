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
        alert("info");
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
        alert("monitoring");
    }
    /*
    $.get("/api/work/quizNames").done(function (data) {
        var items = "";
        $(data).each(function (index, quiz) { 
            items += "<option value='" + quiz.quizID + "'>" + quiz.quizName + "</option>";
        });
        $("#quizList").html(items);
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
    */
    $("#load").click(function (e) {
        e.preventDefault();
        $.get("/api/work/load").done(function (data) {
            $("#quiz").text("Uploaded " + data + " quizes.");
        });


    });

});
