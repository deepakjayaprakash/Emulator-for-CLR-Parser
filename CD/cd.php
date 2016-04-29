<?php

/* 1-states
2- terminals
3- parsing table
4-non terminals
5- input parsing
7- grammar
*/
 $res=shell_exec("C:/xampp/htdocs/CodeProject.Syntax.LALR/CD_grammar1/bin/Debug/CD_grammar1.exe 3");
echo $res."</br><hr>";
$split = array();
 $split=explode("For", $res);
 print_r( $split);
 $res=str_replace(" ", "</br>", $res);//("State", $res);
 	echo $res;
 	//echo $res;
 	?>	