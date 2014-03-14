using UnityEngine;
using System.Collections;

public class BBLogout : MonoBehaviour {

	public UILabel label;

	void OnClick()
	{
		StartCoroutine (Post ());
	}

	IEnumerator Post()
	{
		label.text = "Logging";

		yield return StartCoroutine(MobiFacebook.Instance.Login());
		if(MobiFacebook.Instance.Result==false)
		{
			label.text = "Login Falied";
		}
		else
		{
			label.text = "Reout";
			yield return StartCoroutine(MobiFacebook.Instance.ReutorizeWithPublishPermission());
			if(MobiFacebook.Instance.Result==false)
			{
				label.text = "Reout falied";
			}
			else
			{
				label.text = "posting";
				yield return StartCoroutine(MobiFacebook.Instance.PostOnWall(12));
				if(MobiFacebook.Instance.Result==false)
				{
					label.text = "Post failed";
				}
				else
				{
					label.text = "Posted!";
				}
			}
		}
	}
}
