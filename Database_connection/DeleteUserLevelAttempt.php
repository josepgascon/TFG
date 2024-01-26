<?php

require 'ConnectionSettings.php';

//variables submited by user
$user_id = $_POST["user_id"];
$level_id = $_POST["level_id"];

// Check connection
if ($conn->connect_error) {
  die("Connection failed: " . $conn->connect_error);
}

$sql = "DELETE FROM User_Level WHERE user_id='" . $user_id . "' AND level_id='" . $level_id . "'";
$result = $conn->query($sql);

if ($conn->query($sql) === TRUE) {
    echo " User Level Attempt deleted successfully";
  } else {
    echo "Error: " . $sql . "<br>" . $conn->error;
  }
$conn->close();

?>