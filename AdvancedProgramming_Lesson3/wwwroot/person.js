const uri = "https://localhost:44354/api/People";
let people = null;
function getCount(data) {
    const el = $("#counter");
    let name = "osób";
    if (data) {
        if (data > 4) {
            name = "osób";
        }
        else if (data > 1 && data < 5) {
            name = "osoby";
        }
        else if (data = 1) {
            name = "osoba";
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
            const tBody = $("#people");
            $(tBody).empty();
            getCount(data.length);
            $.each(data, function (key, item) {
                const tr = $("<tr></tr>")
                    .append($("<td></td>").text(item.id))
                    .append($("<td></td>").text(item.firstName))
                    .append($("<td></td>").text(item.lastName))
                    .append($("<td></td>").text(item.age))
                    .append($("<td></td>").text(item.address))
                    .append($("<td></td>").text(item.email))
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
            people = data;
        }
    });
}
function addItem() {
    var age = parseInt($("#add-age").val(), 10);
    const item = {
        firstName: $("#add-firstname").val(),
        lastName: $("#add-lastname").val(),
        age: age,
        address: $("#add-address").val(),
        email: $("#add-email").val()
    };
    $.ajax({
        type: "POST",
        accepts: "application/json",
        url: uri + '/CreatePerson',
        contentType: "application/json",
        data: JSON.stringify(item),
        error: function (jqXHR, textStatus, errorThrown) {
            alert("Something went wrong!");
        },
        success: function (result) {
            getData();
            $("#add-firstname").val("");
            $("#add-lastname").val("");
            $("#add-age").val("");
            $("#add-address").val("");
            $("#add-email").val("");
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
    $.each(people, function (key, item) {
        if (item.id === id) {
            $("#edit-firstname").val(item.firstName);
            $("#edit-lastname").val(item.lastName);
            $("#edit-age").val(item.age);
            $("#edit-address").val(item.address);
            $("#edit-email").val(item.email);
            $("#edit-id").val(item.id);
        }
    });
    $("#spoiler").css({ display: "block" });
}

function updateItem() {
    var id = parseInt($("#edit-id").val(), 10);
    var age = parseInt($("#edit-age").val(), 10);
    const item = {
        id: id,
        firstName: $("#edit-firstname").val(),
        lastName: $("#edit-lastname").val(),
        age: age,
        address: $("#edit-address").val(),
        email: $("#edit-email").val()
    };
    $.ajax({
        type: "POST",
        accepts: "application/json",
        url: uri + '/UpdatePerson',
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