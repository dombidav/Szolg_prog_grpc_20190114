<?php
require_once('lib/config.php');
$key = '';
if(!isset($error))
    $error='';
if(!empty($_POST)){
    $sql = new SQL();
    $result = $sql->execute('SELECT * FROM `user` WHERE `usr`=?', $_POST['username']);
    if($sql->count > 0){
        if(password_verify($_POST['password'], $result[0]['psw'])){
            $key = $result[0]['token'];
        }else{
            $error = "Jelszó nem jó";
        }
    }
    else{
        $error = "Felhasználó nem található";
    }
}
?>

<html>
   <head>
      <title>Login</title>
      
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
   
   <body>
      <div align = "center">
         <div style = "width:300px; border: solid 1px #333333; " align = "left">
            <div style = "background-color:#333333; color:#FFFFFF; padding:3px;"><b>Bejelentkezés</b></div>
				
            <div style = "margin:30px">
               <?php if(empty($_POST)) : ?>
               <form action = "" method = "post">
                  <label>Felhasználó:</label><br><input type = "text" name = "username" class = "box"/><br /><br />
                  <label>Jelszó:</label><br><input type = "password" name = "password" class = "box" /><br/><br />
                  <input type = "submit" value = " Submit "/><br />
               </form> 
               <a href="reg.php">Regisztráció</a>
               <?php else : ?>
                Token: <?=$key?>
               <?php endif; ?>
               <div style = "font-size:11px; color:#cc0000; margin-top:10px"><?=$error; ?></div>
            </div>
         </div>
      </div>
   </body>
</html>