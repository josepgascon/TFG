<?php

require 'ConnectionSettings.php';

//variables submited by user
$user_level_id = $_POST["user_level_id"];

// Check connection
if ($conn->connect_error) {
  die("Connection failed: " . $conn->connect_error);
}

$sql = "SELECT level_id, attempts, average_score, max_score, perfectly_completed FROM User_Level WHERE user_level_id = '" . $user_level_id . "'";
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