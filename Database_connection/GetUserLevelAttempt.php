<?php

require 'ConnectionSettings.php';

//variables submited by user
$user_id = $_POST["user_id"];
$level_id = $_POST["level_id"];

// Check connection
if ($conn->connect_error) {
  die("Connection failed: " . $conn->connect_error);
}

$sql = "SELECT user_level_id, attempts, average_score, max_score, perfectly_completed FROM User_Level WHERE user_id = '" . $user_id . "' AND level_id = '" . $level_id . "'";
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