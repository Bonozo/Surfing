using UnityEngine;
using System.Collections;

public class GUI_Mobis : MonoBehaviour
{
	
	public GameObject[] mobis;
	public Material[] mobiMats;
	public int selectedMobi = 0;
	
	void Start(){
		MobiSwap();
	}
	
	// Subscribe to events
	void OnEnable(){
		EasyTouch.On_SimpleTap += On_SimpleTap;
	}

	void OnDisable(){
		UnsubscribeEvent();
	}
	
	void OnDestroy(){
		UnsubscribeEvent();
	}
	
	void UnsubscribeEvent(){
		EasyTouch.On_SimpleTap -= On_SimpleTap;	
	}
	
	// Simple tap
	private void On_SimpleTap( Gesture gesture){
		
		// Verification that the action on the object
		if (gesture.pickObject.name == "MobiLeft"){
			if(selectedMobi <= 0){
				selectedMobi = mobis.Length-1;	
			} else {
				selectedMobi = selectedMobi - 1;
			}
			MobiSwap();
		} else if (gesture.pickObject.name == "MobiRight"){
			if(selectedMobi >= mobis.Length-1){
				selectedMobi = 0;	
			} else {
				selectedMobi = selectedMobi + 1;
			}
			MobiSwap();
		}
	}
	
	private void MobiSwap(){
		GameManager.m_chosenMobi = (GameManager.ChosenMobi)(selectedMobi);
		for(int i = 0; i < mobis.Length; i++){
			mobis[i].renderer.material = mobiMats[selectedMobi];
		}
	}

	
//	//m_swipeIndex
//	// Use this for initialization
//	public override void Start ()
//	{
//		base.Start();
//	}
//	
//	// Update is called once per frame
//	public override void Update()
//	{
////		base.Update();
////		if(mobiString !=
//		GameManager.m_chosenMobi = (GameManager.ChosenMobi)(m_swipeIndex % m_guiPlanes.Length);
//		
//		
////		GameManager.m_chosenMobi = (GameManager.ChosenMobi)(m_swipeIndex % m_guiPlanes.Length);
////		Debug.Log(GameManager.m_chosenSled);
//	}
}
