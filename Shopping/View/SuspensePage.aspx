<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SuspensePage.aspx.cs" Inherits="Shopping.View.SuspensePage" %>
<!DOCTYPE html>
<html lang="en">
<head>
	<meta charset="utf-8">
	<meta http-equiv="X-UA-Compatible" content="IE=edge">
	<meta name="viewport" content="width=device-width, initial-scale=1">
	<title>Website Notice</title>

	<!-- Latest compiled and minified CSS -->
	<link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.2/css/bootstrap.min.css">

	<!-- Optional theme -->
	<link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.2/css/bootstrap-theme.min.css">

	<!-- HTML5 shim and Respond.js for IE8 support of HTML5 elements and media queries -->
	<!-- WARNING: Respond.js doesn't work if you view the page via file:// -->
	<!--[if lt IE 9]>
	<script src="https://oss.maxcdn.com/html5shiv/3.7.2/html5shiv.min.js"></script>
	<script src="https://oss.maxcdn.com/respond/1.4.2/respond.min.js"></script>
	<![endif]-->
	<style>
		html, body {
			height: 100%;
			margin: 0;
			overflow: hidden;
		}
		body {
			padding-top: 0;
			padding-bottom: 0;
			display: flex;
			justify-content: center;
			align-items: center;
			background-color: #f8f8f8;
		}
		.container {
			width: 100%;
			max-width: 800px;
			text-align: center;
		}
		.jumbotron {
			background-color: #fff;
			padding: 40px;
			border-radius: 8px;
			box-shadow: 0 4px 12px rgba(0,0,0,0.1);
		}
		.jumbotron h1 {
			font-size: 3.5em;
			color: #d9534f;
			margin-bottom: 20px;
		}
		.jumbotron p {
			font-size: 1.5em;
			color: #555;
		}
	</style>
</head>
<body role="document">
<form id="form1" runat="server">
<div class="container theme-showcase" role="main">
	<div class="jumbotron">
		<h1>This account has been banned</h1>
		<p>We apologize for the inconvenience. This account has been banned due to a violation of our terms of service or policies.</p>
		<p>If you believe this is an error or require further assistance, please contact our support team immediately.</p>
	</div>
</div>  

<!-- jQuery (necessary for Bootstrap's JavaScript plugins) -->
<script src="https://ajax.googleapis.com/ajax/libs/jquery/1.11.2/jquery.min.js"></script>
<!-- Include all compiled plugins (below), or include individual files as needed -->
<!-- Latest compiled and minified JavaScript -->
<script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.2/js/bootstrap.min.js"></script>
</form>
</body>
</html>
