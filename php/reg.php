<?php
require_once('lib/config.php');
$key = '';
if(!isset($error))
    $error='';
if(!empty($_POST)){
    $sql = new SQL();
    $sql->execute('INSERT INTO `user` (`usr`, `psw`, `token`) VALUES (?, ?, ?)', $_POST['username'], password_hash($_POST['password'], PASSWORD_BCRYPT), uniqid('', true));
    $error = 'User added';
}
?>
<html>
   
   <head>
      <title>Register Page</title>
      
      <style type = "text/css">
         body {
            font-family:Arial, Helvetica, sans-serif;
            font-size:14px;
         }
         label {
            font-weight:bold;
            width:100px;
            font-size:14px;
         }
         .box {
            border:#666666 solid 1px;
         }
      </style>
      
   </head>
   
   <body bgcolor = "#FFFFFF">
	
      <div align = "center">
         <div style = "width:300px; border: solid 1px #333333; " align = "left">
            <div style = "background-color:#333333; color:#FFFFFF; padding:3px;"><b>Regisztráció</b></div>
				
            <div style = "margin:30px">
               
               <form action = "" method = "post">
                  <label>Felhasználó:</label><br><input type = "text" name = "username" class = "box"/><br /><br />
                  <label>Jelszó:</label><br><input type = "password" name = "password" class = "box" /><br/><br />
                  <input type = "submit" value = " Elküld "/><br />
               </form>
               
               <div style = "font-size:11px; color:#cc0000; margin-top:10px"><?php echo $error; ?></div>
					
            </div>
				
         </div>
			
      </div>

   </body>
</html>