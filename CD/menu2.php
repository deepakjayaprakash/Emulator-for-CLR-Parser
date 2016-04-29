<?php

 



 $res=shell_exec("C:/xampp/htdocs/CodeProject.Syntax.LALR/CD_grammar1/bin/Debug/CD_grammar1.exe 2");
// echo $res."</br><hr>";
$split = array();
$state = array();
$temp = array();
$split=explode("InputParsing", $res);
// print_r( $split);

 $temp=explode("end", $split[1]); 
// print_r( $temp);

 //$state=explode("State", $temp[0]);
 // print_r( $state);

  foreach ($temp as $key) {
  	echo("<tr><td>".$key."</td> </tr><br>");
  
  }

 	    
   
 
 	?>	