S : 'n' ':' A B ';' C ;
A : 'n' X | 't' X ;
X : A | '\e' ;
B : '|' A B | '\e' ;
C : S | '\e' ;