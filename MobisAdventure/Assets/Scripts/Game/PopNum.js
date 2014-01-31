#pragma strict

public var moveTime : float = 4.0;

function Start () {
	//move up and fade out
	iTween.MoveBy(gameObject, iTween.Hash("amount", Vector3(0,5,0), "time", moveTime, 
	"easetype", "easeInQuad",
	"oncomplete", "End"));
//	iTween.ColorTo(gameObject, iTween.Hash("a", 0, "time", moveTime,
//	"oncomplete", "End"));
}

function End () {
	Destroy(gameObject);
}