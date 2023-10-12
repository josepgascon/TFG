<?php

require 'ConnectionSettings.php';

//user submited variables
$user_id = $_POST["user_id"];

// Check connection
if ($conn->connect_error) {
  die("Connection failed: " . $conn->connect_error);
}

$sql = "SELECT user_level_id FROM User_Level WHERE user_id = '" . $user_id . "'";;
$result = $conn->query($sql);

if ($result->num_rows > 0) {
  // output data of each row
  $rows = array();
  while($row = $result->fetch_assoc()) {
    $rows[] = $row;
  }
  echo json_encode($rows);
} else {
  echo "0";
}
$conn->close();

?>