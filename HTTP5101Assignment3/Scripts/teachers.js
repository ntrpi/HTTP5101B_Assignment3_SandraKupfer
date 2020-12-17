// This code borrows heavily from
// https://github.com/christinebittle/BlogProject_7/blob/master/BlogProject/Scripts/authors.js

class Teacher {
	constructor(teacherFName, teacherLName, employeeNumber, hireDate, salary)
	{
		this.teacherFName = teacherFName;
		this.teacherLName = teacherLName;
		this.employeeNumber = employeeNumber;
		this.hireDate = hireDate;
		this.salary = salary;
	}

	getPropertyError()
	{
		if (!this.teacherFName || this.teacherFName.length === 0) {
			return "first name";

		} else if (!this.teacherLName || this.teacherLName.length === 0) {
			return "last name";

		} else if (!this.employeeNumber || employeeNumber.Length == 0) {
			return "employee number";

		} else if (!hireDate) {
			return "hire date";
		}
		return null;
	}

	getData(id) {
		var teacherData = {
			"teacherFname": this.teacherFname,
			"teacherLname": this.teacherLname,
			"employeeNumber": this.employeeNumber,
			"hireDate": this.hireDate,
			"salary": this.salary,
		};
		if (id) {
			teacherData.teacherId = id;
        }
		return teacherData;
    }
}

function getTeacherFromDom() {
	var teacherFname = document.getElementById('firstName').value;
	var teacherLname = document.getElementById('lastName').value;
	var employeeNumber = document.getElementById('employeeNumber').value;
	var hireDate = document.getElementById('hireDate').value;
	var salary = document.getElementById('salary').value;

	var teacher = new Teacher(teacherFname, teacherLname, employeeNumber, hireDate, salary);
	return teacher;
}

function validateTeacher(teacher) {

	var errorBox = document.getElementById("errorBox");

	var propertyError = teacher.getPropertyError();
	var isValid;
	var errorMsg;
	var display;
	if (propertyError != null) {
		isValid = false;
		errorMsg = "Error: Unable to add new teacher: "
			+ missingProperty
			+ " is missing or invalid.";
		display = "block";
	} else {
		isValid = true;
		errorMsg = "";
		display = "none";
	}
	//errorBox.innerHTML = errorMsg;
	//errorBox.style.display = display;
	return isValid;
}

function sendRequest(teacherData, functionName) {

	// Send a post request to the data controller to do something with the teacher.
	var URL = "http://localhost:58996/api/TeacherData/" + functionName + "/";
	var rq = new XMLHttpRequest();
	rq.open("POST", URL, true);
	rq.setRequestHeader("Content-Type", "application/json");
	rq.onreadystatechange = function () {
		//ready state should be 4 AND status should be 200
		if (rq.readyState == 4 && rq.status == 200) {
			//request is successful and the request is finished
			//nothing to render, the method returns nothing.
		}
	}
	//POST information sent through the .send() method
	rq.send(JSON.stringify(teacherData));
}

function addTeacher()
{
	var teacher = getTeacherFromDom();
	var isValid = validateTeacher(teacher);
	if (!isValid) {
		return;
	}

	sendRequest(teacher.getData(), "addTeacher");
}

function UpdateTeacher(teacherId) {

	var teacher = getTeacherFromDom();
	if (!validateTeacher()) {
		return;
	}

	sendRequest(teacher.getData(teacherId), "updateTeacher");
}
