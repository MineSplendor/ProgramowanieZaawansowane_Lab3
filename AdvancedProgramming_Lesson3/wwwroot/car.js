const uri = "https://localhost:44354/api/Cars";
let cars = null;
function getCount(data) {
    const el = $("#counter");
    let name = "samochodów";
    if (data) {
        if (data > 4) {
            name = "samochodów";
        }
        else if (data > 1 && data < 5) {
            name = "samochody";
        }
        else if (data = 1) {
            name = "samochód";
        }
        el.text(data + " " + name);
    } else {
        el.text("Brak " + name);
    }
}
$(document).ready(function () {
    getData();
});
function getData() {
    $.ajax({
        type: "GET",
        url: uri,
        cache: false,
        success: function (data) {
            const tBody = $("#cars");
            $(tBody).empty();
            getCount(data.length);
            $.each(data, function (key, item) {
                const tr = $("<tr></tr>")
                    .append($("<td></td>").text(item.id))
                    .append($("<td></td>").text(item.make))
                    .append($("<td></td>").text(item.model))
                    .append($("<td></td>").text(item.year))
                    .append($("<td></td>").text(item.color))
                    .append(
                        $("<td></td>").append(
                            $("<button>Edytuj</button>").on("click", function () {
                                editItem(item.id);
                            })
                        )
                    )
                    .append(
                        $("<td></td>").append(
                            $("<button>Usuń</button>").on("click", function () {
                                deleteItem(item.id);
                            })
                        )
                    );
                tr.appendTo(tBody);
            });
            cars = data;
        }
    });
}
function addItem() {
    var year = parseInt($("#add-year").val(), 10);
    const item = {
        make: $("#add-make").val(),
        model: $("#add-model").val(),
        year: year,
        color: $("#add-color").val()
    };
    $.ajax({
        type: "POST",
        accepts: "application/json",
        url: uri + '/CreateCar',
        contentType: "application/json",
        data: JSON.stringify(item),
        error: function (jqXHR, textStatus, errorThrown) {
            alert("Something went wrong!");
        },
        success: function (result) {
            getData();
            $("#add-make").val("");
            $("#add-model").val("");
            $("#add-year").val("");
            $("#add-color").val("");
        }
    });
}

function deleteItem(id) {
    $.ajax({
        url: uri + "/" + id,
        type: "DELETE",
        success: function (result) {
            getData();
        }
    });
}

function editItem(id) {
    $.each(cars, function (key, item) {
        if (item.id === id) {
            $("#edit-make").val(item.make);
            $("#edit-model").val(item.model);
            $("#edit-year").val(item.year);
            $("#edit-color").val(item.color);
            $("#edit-id").val(item.id);
        }
    });
    $("#spoiler").css({ display: "block" });
}

function updateItem() {
    var id = parseInt($("#edit-id").val(), 10);
    var year = parseInt($("#edit-year").val(), 10);
    const item = {
        id: id,
        make: $("#edit-make").val(),
        model: $("#edit-model").val(),
        year: year,
        color: $("#edit-color").val()
    };
    $.ajax({
        type: "POST",
        accepts: "application/json",
        url: uri + '/UpdateCar',
        contentType: "application/json",
        data: JSON.stringify(item),
        error: function (jqXHR, textStatus, errorThrown) {
            alert("Something went wrong!");
        },
        success: function (result) {
            getData();
            closeInput();
        }
    });
}

function closeInput() {
    $("#spoiler").css({ display: "none" });
}