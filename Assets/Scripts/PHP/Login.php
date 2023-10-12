<?php

require 'ConnectionSettings.php';

//variables submited by user
$loginUser = $_POST["loginUser"];
$loginPass = $_POST["loginPass"];

// Check connection
if ($conn->connect_error) {
  die("Connection failed: " . $conn->connect_error);
}

$sql = "SELECT password, user_id FROM User WHERE username = '" . $loginUser . "'";
$result = $conn->query($sql);

if ($result->num_rows > 0) {
  // output data of each row
  while($row = $result->fetch_assoc()) {
    if($row["password"] == $loginPass){
        echo $row["user_id"];
    }
    else {
        echo "Wrong password";
    }
  }
} else {
  echo "Username does not exist";
}
$conn->close();

?>