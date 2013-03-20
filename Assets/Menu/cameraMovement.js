var speed : int = 5;
var atEnd : boolean = true;
var right : int = 90;
var left : int = 0;

function Start () {

}

function Update () {
if (atEnd)
transform.position.x += speed * Time.deltaTime;
else
transform.position.x -= speed * Time.deltaTime;

if (transform.position.x < left)
atEnd = true;
if (transform.position.x > right)
atEnd = false;
}