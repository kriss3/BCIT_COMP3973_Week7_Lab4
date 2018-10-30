$(function () {
	var getPatients = function () {
		var url = "https://localhost:44343/api/patients/";
		$.get(url).always(showResponse);
		return false;
	};

	var showResponse = function (object) {
		$("#preOutput").text(JSON.stringify(object, null, 4));
	};

	$("#btnGetPatients").click(getPatients);

	var getAlert = function myfunction() {
		$("#myForm").text("Response from my js script");
	};

	$("#btnGetAlert").click(getAlert);

	//asdf
	$("#postform").submit(function (e) {
		e.preventDefault();
		var url = "https://localhost:44343/api/patients";
		var data = {
			name: $("#txtPatientName").val().trim(),
			ailments: [
				{ name: $("#txtAilmentName1").val().trim()}
				//{ name: $("#txtAilmentName2").val().trim()}
			],
			medications: [
				{ name: $("#txtMedication1").val().trim(), doses: $("#txtDose1").val().trim() }
				//{ name: $("#txtMedication2").val().trim(), doses: $("#txtDose2").val().trim() }
			]
		};
		
		$.ajax({
			contentType: 'application/json',
			type: 'POST',
			url: url,
			data: JSON.stringify(data),
			success: function (d) {
				alert("Saved Successfully");
				$("#postResult").text("Record created ok. Id=" + d.patientId);
				$("#postform").trigger("reset");
			},
			error: function (d) {
				alert("Error please try again: " + JSON.stringify(d));
			}
		});
		
	});
	//asdf
});