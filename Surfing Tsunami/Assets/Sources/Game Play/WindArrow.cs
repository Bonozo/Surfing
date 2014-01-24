using UnityEngine;
using System.Collections;

public class WindArrow : MonoBehaviour {
	
	public GameObject gui;
	
	public tk2dTextMesh textSection;
	public tk2dSprite sprite;
	
	void OnEnable()
	{
		gui.SetActive(false);
	}
	
	public void ShowArrow(WindState wind)
	{
		StopAllCoroutines();
		StartCoroutine(ShowArrowThread(wind));
		
		int section = LevelInfo.State.Section;
		
		// Object Speed * (1.0 + ([Section #] * 0.1) )
		LevelInfo.State.MaxForce = 30f*(1.0f+section*0.1f);
		
		textSection.text = "SECTION " + section + "\n\nCONDITIONS' " + SectionText(section) + "%";
	}

	string SectionText(int section)
	{
		switch(section)
		{
		case 1: return "VERY POOR";
		case 2: return "POOR";
		case 3: return 	"POOR TO FAIR";
		case 4: return "FAIR";
		case 5: return 	"FAIR TO GOOD";
		case 6: return "GOOD";
		case 7: return "VERY GOOD";
		case 8: return "GOOD TO EPIC";
		default: return "EPIC";
		}
	}
	
	public void HideArrow()
	{
		StopAllCoroutines();
		gui.SetActive(false);
	}
	
	private IEnumerator ShowArrowThread(WindState wind)
	{
		Color col = new Color(1f,1f,1f,0f);

		switch(wind)
		{
		case WindState.Down:
			sprite.transform.rotation = Quaternion.Euler(0,0,-90);
			break;
		case WindState.Left:
			sprite.transform.rotation = Quaternion.Euler(0,0,-180f);
			break;
		case WindState.Right:
			sprite.transform.rotation = Quaternion.Euler(0,0,0f);
			break;
		case WindState.DownLeft:
			sprite.transform.rotation = Quaternion.Euler(0,0,-135f);
			break;
		case WindState.DownRight:
			sprite.transform.rotation = Quaternion.Euler(0,0,-45f);
			break;
		}	
		
		var pos = LevelInfo.Camera.Center+new Vector3(0f,-20f,0); pos.z=LevelInfo.Environments.depthWindArrow;
		sprite.transform.position = pos;
		sprite.color = col;
		textSection.color = col;
		gui.SetActive(true);
		
		while( col.a < 1)
		{
			col.a += Time.deltaTime;
			sprite.color = col;
			textSection.color = col;
			textSection.Commit();
			yield return null;
		}	
		
		float wait = 2f;
		while( wait > 0f )
		{
			wait-=Time.deltaTime;
			sprite.transform.Translate( 0.1f*LevelInfo.Camera.Height*Time.deltaTime,0,0);
			yield return null;
		}
		
		gui.SetActive(false);
		
	}
}
