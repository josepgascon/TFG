<?php

require 'ConnectionSettings.php';

//variables submited by user
$loginUser = $_POST["loginUser"];
$loginPass = $_POST["loginPass"];

// Check connection
if ($conn->connect_error) {
  die("Connection failed: " . $conn->connect_error);
}

$sql = "SELECT username FROM User WHERE username = '" . $loginUser . "'";
$result = $conn->query($sql);

if ($result->num_rows > 0) {
    //tell user username already taken
    echo "Username is already taken";
}
else {
  //echo "Creating user...";
  //insert username and password to DB
  $sql2 = "INSERT INTO User (username, password) VALUES ('" . $loginUser . "' , '" . $loginPass . "')";
}
if ($conn->query($sql2) === TRUE) {
    //echo "New record created successfully";
    
    $sql3 = "SELECT user_id FROM User WHERE username = '" . $loginUser . "'";
    $result = $conn->query($sql);
    
    if ($result->num_rows > 0) {
      // output data of each row
      while($row = $result->fetch_assoc()) {
         echo $row["user_id"];
      }
    }
  } else {
    echo "Error: " . $sql2 . "<br>" . $conn->error;
  }
$conn->close();

?>