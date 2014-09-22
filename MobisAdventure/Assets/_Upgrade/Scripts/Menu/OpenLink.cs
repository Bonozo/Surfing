/* --------------------------------
 * 	©Bonozo LLC, http://bonozo.com 
 * --------------------------------*/

using UnityEngine;
using System.Collections;

public class OpenLink : MonoBehaviour {

	public string url;

	void OnClick()
	{
		Application.OpenURL (url);
	}
}
