using UnityEngine;
using System.Collections;

public class Ragdoll : MonoBehaviour {

	public Rigidbody[] frozenRigidbodies;
	public TT0[] tt0;
	public FixRotation[] fixedRotations;
	public CharacterJoint[] openJointRotations;
	public GameObject[] newRigidbodies;
	public Rigidbody rootRigidbody;

	public void Work()
	{
		foreach(var r in frozenRigidbodies)
		{
			r.constraints = RigidbodyConstraints.FreezePositionZ |
				RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;
		}

		foreach(var j in openJointRotations)
		{
			var jon = j.lowTwistLimit;
			jon.limit = -45f;
			j.lowTwistLimit = jon;


			jon = j.highTwistLimit;
			jon.limit = 45f;
			j.highTwistLimit = jon;
		}

		foreach(var f in fixedRotations)
			Destroy(f);
		foreach(var t in tt0)
			Destroy(t);

		var force = transform.root.rigidbody.velocity*4000;
		StartCoroutine(AddRigidbodies(force));

		if( transform.root.rigidbody.velocity != null)
		{
			Debug.Log( transform.root.rigidbody.velocity);
			rootRigidbody.AddForce(force);
		}
	}

	IEnumerator AddRigidbodies(Vector3 force)
	{
		yield return new WaitForSeconds(Random.Range(0.3f,0.45f));

		foreach(var g in newRigidbodies)
		{
			g.transform.parent = rootRigidbody.transform.parent;
			g.AddComponent("Rigidbody");
			g.rigidbody.constraints = RigidbodyConstraints.FreezePositionZ |
				RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;
			g.AddComponent("BoxCollider");
			var forcex = force/100f;
			forcex.x *= Random.Range(0.95f,1.05f);
			forcex.y *= Random.Range(0.95f,1.05f);
			g.rigidbody.AddForce(forcex);
			yield return new WaitForSeconds(Random.Range(0.3f,0.45f));
		}

	}

	void Update()
	{
		if(Input.GetKeyDown(KeyCode.Space))
			Work();
	}
}
