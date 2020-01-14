<?php
/**
 * User: Dombi Tibor (HL5U4V)
 * Date: 2019. 10. 17.
 * Time: 9:02
 *
*	class SQL:
*		Use:
*			$sql = new SQL();
*			print $sql->execute("SELECT * FROM user WHERE id=?", 2)."<br>";
*			var_dump($sql->result);
*
*		Construct:
*			new SQL([database.php], [PDO options], [db object, if present(out)], [stmt object, if present(out)])
*
*		Properties:
*			$sql->sql 		: Last query {string}
*			$sql->variables : Variables passed by the last query {indexed array}
*			$sql->data      : Relative location of config.php {string}
*			$sql->con       : PDO connection object {PDO}
*			$sql->options   : PDO options {PDO options array}
*			$sql->stmt      : PDO statement object {PDO::statement}
*			$sql->result    : Result array {associative array (Field => Value)}
*			$sql->count     : Result count {int}
*			$sql->conError  : Connection error log {PDO::Error}
*			$sql->stmtError : Statement error log {PDO::Statement::Error}
*
*		Return:
*			false : on error (see conError above) {bool}
*			true  : on success {bool}
*
*		Functions:
*			$sql->execute(string sql,[variables]);
*			Parameters:
*				sql       : query to execute, use ? for placeholder {string}
*				variables : variables for placeholders {arguments}
*			Return:
*				false  : on error (see stmtError) {bool}
*				result : on success {array}
*/

class SQL
{
    public $sql = '';
    public $variables = [];

    public $data = 'config.php';
    public $con;
    public $options = null;
    public $stmt;

    public $result = [];
    public $count = 0;
    public $conError = [];
    public $stmtError = [];


    /**
     * SQL constructor.
     * @param string $data
     * @param array $options
     * @param mysqli $con
     * @param mysqli_stmt $stmt
     */
    public function __construct($data = 'config.php', $options = [PDO::ATTR_EMULATE_PREPARES => false, PDO::ATTR_ERRMODE => PDO::ERRMODE_EXCEPTION], &$con = null, &$stmt = null)
    {

        $this->options = $options;
        $this->stmt = $stmt;
        require_once 'str.php';
        if (!(STR::ends_With($data, ".php"))) $data = $data . ".php";
        $this->data = $data;
        if (!(defined('DBH'))) require_once($data);
        if ($con == null) {
            $this->con = new PDO('mysql:host=' . DBH . ';dbname=' . DBN, DBU, DBP, $options);
            if ($this->con === false) {
                $this->conError = $this->con->errorInfo();
                return false;
            }
        } else {
            $this->con = &$con;
            if ($this->con === false) {
                $this->conError = $this->con->errorInfo();
                return false;
            }
        }
        return true;
    }

    /**
     * Execute SQL command (transaction)
     * @param $sql
     * @return array|bool
     */
    public function execute($sql)
    {
        $this->stmtError = [];
        $this->count = 0;
        $this->result = [];
        $this->sql = $sql;
        $this->stmt = $this->con->prepare($sql);
        if (func_num_args() > 1) {
            for ($i = 1; $i < func_num_args(); $i++) {
                $this->stmt->bindValue($i, func_get_arg($i), PDO::PARAM_STR);
                array_push($this->variables, func_get_arg($i));
            }
        }
        $this->stmt->execute();
        if ($this->stmt === false) {
            $this->stmtError = $this->stmt->errorInfo();
            return false;
        }
        $this->result = $this->stmt->fetchAll(PDO::FETCH_ASSOC);
        $this->count = count($this->result);
        if ($this->count === 0) $this->stmtError = "Zero rows";
        return $this->result;
    }
}
