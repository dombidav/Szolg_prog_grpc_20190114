<?php
class ID{
    public static function New($n = 8){
        $string = 'abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789';
        $result = '';
        for ($i=0; $i < $n; $i++) { 
            $result .= $string[rand(0, strlen($string))];
        }
        return $result;
    }
}