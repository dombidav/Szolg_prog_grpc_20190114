<?php
define('LIB_PATH', 'lib/' );
define('DBH', '127.0.0.1');         
define('DBN', 'book_store');    
define('DBU', 'root');              
define('DBP', '');                  
require_once("str.php");
foreach (scandir(dirname(__FILE__)) as $filename) {
    $path = dirname(__FILE__) . '\\' . $filename;
    if (is_file($path) && (STR($path)->endsWith(".php")))
        require_once($path);
}