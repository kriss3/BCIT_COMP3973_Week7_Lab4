$(function () {

	$("#btnGetPartiesTbl").click(getPartiesForTbl);
	//GET ALL Parties from backend API
	function getPartiesForTbl() {
		var url = 'https://localhost:44343/api/parties';
		$.ajax({
			contentType: 'application/json',
			type: 'GET',
			url: url,
			headers: { "Authorization": sessionStorage.getItem("token") },
			success: function (data) {
				var party = '';
				$('#party_table tbody').empty();
				$.each(data, function (key, value) {
					
					party += '<tr>';
					party += '<td>' + value.partyName + '</td>';
					party += '<td>' + getMyDate(value.partyDate); + '</td>';
					party += '<td>' + value.expectedNumberOfGuests + '</td>';
					party += '<td>' + value.location + '</td>';
					party += '<td>' + '<button class="btn btn-default" id="btnDel">Delete: '+value.partyId+'</button>' +'</td>';
					party += '</tr>';
					
				});
				
				$('#party_table').append(party);
			},
			error: function (xhr) {
				alert('Something went wrong, Response: ' + JSON.stringify(xhr));
				if (xhr.status === 401)
				{
					//Call modal login form and fade the backgroud
					$("#modalLoginForm").modal("toggle");
				}
			}
		});
	}
	//end

	//POST NEW Party to the API
	$("#postform").submit(function (e) {
		e.preventDefault();
		var url = "https://localhost:44343/api/parties";
		var data = {
			partyName: $("#txtPartyName").val().trim(),
			partyDate: $("#txtPartyDate").val().trim(),
			expectedNumberOfGuests: $("#txtGuestNumber").val().trim(),
			location: $("#txtLocation").val().trim()
		};

		$.ajax({
			contentType: 'application/json',
			type: 'POST',
			url: url,
			data: JSON.stringify(data),
			success: function (d) {
				alert("Saved Successfully. You can CLOSE current Window.");
				$("#postform").trigger("reset");
				$('#myModal').modal('toggle');
			},
			error: function (d) {
				alert("Error please try again: " + JSON.stringify(d));
			}
		});

	});
	//End

	//-- Private Helper Function
	function getMyDate(date) {
		var d = new Date(date);
		var day = d.getDate();
		var month = d.getMonth();
		var year = d.getFullYear();

		return day + '/' + month + '/' + year;
	}

	//Adding Delete button and call API to remove record;
	$("#party_table").on("click", "button#btnDel", function () {
		var id = $(this).text().substr(8);
		alert("About to remove Record with Id: " + id);

		//Call API
		var url = "https://localhost:44343/api/parties/";
		$.ajax({
			contentType: 'application/json',
			type: 'DELETE',
			url: url + id,
			success: function (d) {
				alert("Record with Id: " + d.partyId + " removed succesful. Refresh table.");
			},
			error: function (d) {
				alert("Error removing record: " + JSON.stringify(d));
			}
		});
	});

	//User not Authoroze to call API
	$("#btnLogin").click(login);
	function login() {
		var data = {
			Username: $("#txtUserName").val().trim(),		// "ks@ks.ks",
			Password:  $("#txtPassword").val().trim()		//P@$$w0rd"
		};
		var url = 'https://localhost:44343/login';
		$.ajax({
			contentType: 'application/json',
			type: 'POST',
			url: url,
			data: JSON.stringify(data),
			success: function (response) {
				alert('Success. Your TOKEN is and will expire in 2 min: ' + JSON.stringify(response.token));
				myToken = 'Bearer ' + response.token;
				sessionStorage.setItem("token", myToken);
				window.location.href = "https://localhost:44378/Parties.html";
				
			},
			error: function (d) {
				alert("Error fetching data. Response: " + JSON.stringify(d));
			}
		});
	}
});