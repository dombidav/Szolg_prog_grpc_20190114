<?php
/**
 *  * User: Dombi Tibor HL5U4V
 * Date: 2019. 10. 17.
 * Time: 9:38
 */

/*
 * Procedural programming is stupid. Change my mind
 * Usage:
 * Instance:
 *      echo STR('Some string')->remove(' string');
 *        => 'Some'
 * Or static:
 *      echo STR::remove_SubStr('Some string', ' string');
 *        => 'Some'
 */
class STR{

    public $str = '';

    /**
     * STR constructor.
     * @param $object
     */
    public function __construct($object)
    {
        try{
            $this->str = (string) $object;
        }catch (Exception $e){
            $this->str='';
        }
    }

    public function startsWith($needle){
        return self::starts_With($this->str, $needle);
    }

    public function endsWith($needle){
        return self::ends_With($this->str, $needle);
    }

    public function isRegex($string){
        return self::is_Regex($string);
    }

    public function remove($string){
        return self::remove_Substr($this->str, $string);
    }

    public function toLower(){
        return self::to_Lower($this->str);
    }

    public function toUpper(){
        return self::to_Upper($this->str);
    }

    public function StartWithCapital(){
        return self::to_Upper_Start($this->str);
    }

    public function __toString()
    {
        return $this->str;
    }

    public static function starts_With($haystack, $needle)
    {
        $length = strlen($needle);
        return (substr($haystack, 0, $length) === $needle);
    }

    public static function ends_With($haystack, $needle)
    {
        $len = strlen($needle);
        if ($len == 0) {
            return true;
        }
        return (substr($haystack, -$len) === $needle);
    }

    /**
     * @param $e string
     * @param $OUT string message output
     * @return bool
     */
    public static function is_Regex($e, &$OUT = null)
    {
        ini_set('track_errors', 'on');
        $php_errormsg = '';
        @preg_match($e, '');
        $OUT = $php_errormsg;
        return $php_errormsg ? false : true;
    }

    /**
     * @param $s string
     * @return string
     */
    public static function to_Lower($s)
    {
        return strtolower($s);
    }

    public static function to_Upper($arg)
    {
        return strtoupper($arg);
    }

    public static function to_Upper_Start($s)
    {
        return ucfirst($s);
    }

    /**
     * @param $from
     * @param $string
     * @return mixed
     */
    public static function remove_Substr($from, $string)
    {
        return str_replace($string, '', $from);
    }

}
function str($object){
    return new STR($object);
}