$(function () {

	$("#btnGetPartiesTbl").click(getPartiesForTbl);

	//GET ALL Parties from backend API
	function getPartiesForTbl() {
		var url = 'https://localhost:44343/api/parties';
		$.ajax({
			contentType: 'application/json',
			type: 'GET',
			url: url,
			success: function (data) {
				var party = '';
				$('#party_table tbody').empty();
				$.each(data, function (key, value) {

					
					
					party += '<tr>';
					party += '<td>' + value.partyName + '</td>';
					party += '<td>' + getMyDate(value.partyDate); + '</td>';
					party += '<td>' + value.expectedNumberOfGuests + '</td>';
					party += '<td>' + value.location + '</td>';
					party += '<td>' + '<button class="btn btn-default" id="btnDel">Delete ('+value.partyId+')</button>' +'</td>';
					party += '</tr>';
					
				});
				
				$('#party_table').append(party);
			},
			error: function (data) {
				alert('Something went wrong, Responsoe: ' + JSON.stringify(data));
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


});