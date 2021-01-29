// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(document).ready(function () {
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
    $("#load").click(function (e) {         
        e.preventDefault();
        $.get("/api/work/load").done(function (data) {
            $("#quiz").text("Uploaded " + data + " quizes."); 
        });


    });
});
