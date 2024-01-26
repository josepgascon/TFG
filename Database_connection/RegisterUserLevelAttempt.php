<?php

require 'ConnectionSettings.php';

//variables submited by user
$user_id = $_POST["user_id"];
$level_id = $_POST["level_id"];
$attempts = $_POST["attempts"];
$average_score = $_POST["average_score"];
$max_score = $_POST["max_score"];
$perfectly_completed = $_POST["perfectly_completed"];

// Check connection
if ($conn->connect_error) {
  die("Connection failed: " . $conn->connect_error);
}

$sql = "INSERT INTO User_Level(user_level_id, user_id, level_id, attempts, average_score, max_score, perfectly_completed) 
VALUES (NULL, '" . $user_id . "','" . $level_id . "','" . $attempts . "','" . $average_score . "','" . $max_score . "','" . $perfectly_completed . "')";
$result = $conn->query($sql);

if ($conn->query($sql) === TRUE) {
    echo "New User Level Attempt created successfully";
  } else {
    echo "Error: " . $sql . "<br>" . $conn->error;
  }
$conn->close();

?>
