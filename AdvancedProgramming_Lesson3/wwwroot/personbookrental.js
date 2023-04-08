const uri = "https://localhost:44354/api/PersonBookRentals";
let personbookrentals = null;
function getCount(data) {
    const el = $("#counter");
    let name = "wypożyczonych książek";
    if (data) {
        if (data > 4) {
            name = "wypożyczonych książek";
        }
        else if (data > 1 && data < 5) {
            name = "wypożyczone książki";
        }
        else if (data = 1) {
            name = "wypożyczona książka";
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
            const tBody = $("#personbookrentals");
            $(tBody).empty();
            getCount(data.length);
            $.each(data, function (key, item) {
                const tr = $("<tr></tr>")
                    .append($("<td></td>").text(item.id))
                    .append($("<td></td>").text(item.personID))
                    .append($("<td></td>").text(item.bookID))
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
            personbookrentals = data;
        }
    });
}
function addItem() {
    var personID = parseInt($("#add-personid").val(), 10);
    var bookID = parseInt($("#add-bookid").val(), 10);
    const item = {
        personID: personID,
        bookID: bookID,
        rentalDate: $("#add-rentaldate").val()
    };
    $.ajax({
        type: "POST",
        accepts: "application/json",
        url: uri + '/CreatePersonBookRental',
        contentType: "application/json",
        data: JSON.stringify(item),
        error: function (jqXHR, textStatus, errorThrown) {
            alert("Something went wrong!");
        },
        success: function (result) {
            getData();
            $("#add-personid").val("");
            $("#add-bookid").val("");
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
    $.each(personbookrentals, function (key, item) {
        if (item.id === id) {
            $("#edit-personid").val(item.personID);
            $("#edit-bookid").val(item.bookID);
            $("#edit-rentaldate").val(item.rentalDate);
            $("#edit-id").val(item.id);
        }
    });
    $("#spoiler").css({ display: "block" });
}

function updateItem() {
    var id = parseInt($("#edit-id").val(), 10);
    var personID = parseInt($("#edit-personid").val(), 10);
    var bookID = parseInt($("#edit-bookid").val(), 10);
    const item = {
        id: id,
        personID: personID,
        bookID: bookID,
        rentalDate: $("#edit-rentaldate").val()
    };
    $.ajax({
        type: "POST",
        accepts: "application/json",
        url: uri + '/UpdatePersonBookRental',
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