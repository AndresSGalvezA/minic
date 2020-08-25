<p align="right">
2020-08-27
</p>
<p align="left">
Compiladores Secc.01 </br> Universidad Rafael Landívar
</p>

<p align="left">
Sergio Lara [1044418] </br> Andres Galvez [1024718]
</p>

<h1 align= "center"><b>Mini C#  👨‍💻</b></h1>

<p align="center">
Mini Compilador de C# </br> 
</p>

<h3 align = "left">Analizador Léxico</h3>
<img align="right" alt="GIF" src="https://i.pinimg.com/originals/e4/26/70/e426702edf874b181aced1e2fa5c6cde.gif" />
<p align="left">
Tomamos la lógica de ir evaluando línea por línea el archivo de entrada.  Para poder identificar el formato de cada línea y separar esta en los diferentes tipos de tokens o errores que puede producir, utilizamos Expresiones Regulares (ER) en la librería Regex.
</br> </br>
Primero analizamos la línea y verificamos que no se encuentra vacía, si no lo está, pasa a ser una cadena de caracteres la cual evaluamos en nuestra ER; gracias a Regex, esta es capaz de separar y devolvernos fragmentos que encajan en la ER, pero al haber tantas variantes, solía confundir algunos tokens o errores por otros; por ejemplo, una cadena de texto (string), supongamos "hola ", cuando era evaluada, la ER nos devolvía <"hola>  y <"> como token o errores por separado, por lo que dentro de la ER formamos grupos que tienen una jerarquía, para que ciertas partes de la ER tengan una mayor prioridad de hacer "match" antes que otras.
</br> </br>
Jerarquia de nuestra ER y su porción de expresión regular
</br> </br>
1) Strings (""((\w)|(\s)|(\p{P})|(\p{S}))+"") </br> </br>
2) Comentarios de una sola linea (//((\w)|(\s)|(\p{P})|(\p{S}))+) </br></br>
3) Numeros </br>
 3.1) Exponenciales ([0-9]+[.][0-9]*(E|e)[+]?[0-9]+) </br>
 3.2) Hexadecimales (0(x|X)([0-9]|[a-fA-F])+) </br>
 3.3) Flotantes ([0-9]+[.][0-9]*) </br>
 3.4) Enteros ([0-9]+) </br> </br>
4) Error string incompleto (""((\w)|(\s)|(\p{P})|(\p{S}))+) </br> </br>
5) Identificadores/Reservadas ([a-zA-Z]((\w)|[_])*)) </br> </br>
6) Operadores </br>
 6.1) De agrupación juntos [()]|[{}]|[\[\]] --> Se quito de la ER, la solución se describirá en Analizar() </br>
 6.2) De comparación (<=|>=|==|!=|&&|[||]) </br>
 6.3) Matemáticos (Son un solo caracter) --> Se reconocen como error 8) y luego se identifican como Operador en getTypeToken(string) </br> </br>
7) Error '*/'   ([*][/]) </br> </br>
8) Caracteres de error ((\p{P}){1}|(\p{S}){1})
</br> </br>
A pesar de que con Regex somos capaces de clasificar y dividr por sub-grupos, aun existen tres tokens/errores que no han sido identificados: los comentarios multilínea, si una cadena es identificador o palabra reservada y los operadores de agrupación cuando vienen uno tras otro. </br>
Como función principal utilizamos Analizar(), la cual recorre el archivo línea por línea y cuando encuentra un '/*' ignora la línea actual y las siguientes hasta encontrar un '*/', si no es encontrado estos dos caractes de esa manera cuando Analizar() llega al final del archiva, esta guarda la posión donde se encuentra el '/*' y emite un error EOF de un comentario.</br> </br>

Si la cadena no es un comentario multilínea, entonces analizamos la cadena entrante y utilizamos el metodo Regex.Match(string) para encontrar cada sub-cadena que cumple con la ER; si el "match" es un operador de agrupación de apertura '(', '[' o '{', guardamos en una variable temporal los datos de este caracter (index y caracter) para luego si el siguiente match es un operador de agrupación de cierre se concatenen y sean analizados como un único token. </br> </br>
Luego, en la cadena a analizar, necesitamos saber qué tipo de token genera esta, el tipo de token se obtiene con el método getTypeToken(string), el cual devuelve el número al que pertenece la cadena según la jeraquía indicada anteriormente. </br>

Si el resultado de getTypeToken(string) indica que es un número, se obtiene su valor y se procede a generar el token. </br>
Si el resultado es un Identificador/Reservada se analiza si la cadena es una palabra reservada o un identificador, para luego ser generado su respectivo token. </br>
Si no es ninguno de estos dos se manda a generar el token o error con el caracter que se esta analizando en ese momento.
</p>


