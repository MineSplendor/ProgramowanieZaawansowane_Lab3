const uri = "https://localhost:44354/api/PersonCarRentals";
let personcarrentals = null;
function getCount(data) {
    const el = $("#counter");
    let name = "wypożyczonych samochodów";
    if (data) {
        if (data > 4) {
            name = "wypożyczonych samochodów";
        }
        else if (data > 1 && data < 5) {
            name = "wypożyczone samochody";
        }
        else if (data = 1) {
            name = "wypożyczony samochód";
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
            const tBody = $("#personcarrentals");
            $(tBody).empty();
            getCount(data.length);
            $.each(data, function (key, item) {
                const tr = $("<tr></tr>")
                    .append($("<td></td>").text(item.id))
                    .append($("<td></td>").text(item.personID))
                    .append($("<td></td>").text(item.carID))
                    .append($("<td></td>").text(item.rentalDate))
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
            personcarrentals = data;
        }
    });
}
function addItem() {
    var personID = parseInt($("#add-personid").val(), 10);
    var carID = parseInt($("#add-carid").val(), 10);
    const item = {
        personID: personID,
        carID: carID,
        rentalDate: $("#add-rentaldate").val()
    };
    $.ajax({
        type: "POST",
        accepts: "application/json",
        url: uri + '/CreatePersonCarRental',
        contentType: "application/json",
        data: JSON.stringify(item),
        error: function (jqXHR, textStatus, errorThrown) {
            alert("Something went wrong!");
        },
        success: function (result) {
            getData();
            $("#add-personid").val("");
            $("#add-carid").val("");
            $("#add-rentaldate").val("");
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
    $.each(personcarrentals, function (key, item) {
        if (item.id === id) {
            $("#edit-personid").val(item.personID);
            $("#edit-carid").val(item.carID);
            $("#edit-rentaldate").val(item.rentalDate);
            $("#edit-id").val(item.id);
        }
    });
    $("#spoiler").css({ display: "block" });
}

function updateItem() {
    var id = parseInt($("#edit-id").val(), 10);
    var personID = parseInt($("#edit-personid").val(), 10);
    var carID = parseInt($("#edit-carid").val(), 10);
    const item = {
        id: id,
        personID: personID,
        carID: carID,
        rentalDate: $("#edit-rentaldate").val()
    };
    $.ajax({
        type: "POST",
        accepts: "application/json",
        url: uri + '/UpdatePersonCarRental',
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