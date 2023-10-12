<?php

require 'ConnectionSettings.php';

// Check connection
if ($conn->connect_error) {
  die("Connection failed: " . $conn->connect_error);
}

$sql = "SELECT user_id, username, password FROM User";
$result = $conn->query($sql);

if ($result->num_rows > 0) {
  // output data of each row
  while($row = $result->fetch_assoc()) {
    echo "id: " . $row["user_id"]. " - username: " . $row["username"]. " " . $row["password"]. "<br>";
  }
} else {
  echo "0 results";
}
$conn->close();

?>