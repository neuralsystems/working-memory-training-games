using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasketGame_RainParticleSystemBehavior : MonoBehaviour {


//	public ParticleSystem Bubble_Rain;
//	public ParticleSystem Burst_bubble;
//	List<ParticleCollisionEvent> Collision_Events;
	// Use this for initialization
	void Start () {
//		Collision_Events = new List<ParticleCollisionEvent> ();
////		var width_percentage = .5f, height_percentage
//		transform.position = Camera.main.GetComponent<BasketGame_SceneVariables>().GetPointOnScreen(.5f,1.1f);
	}

//	void OnParticleCollision(GameObject other){
//		Debug.Log ("Collided");
//		ParticlePhysicsExtensions.GetCollisionEvents (Bubble_Rain, other, Collision_Events);
//		for (int i = 0; i < Collision_Events.Count; i++) {
//			EmitAtLocation (Collision_Events[i]);
//		}
//	}
//
//	void EmitAtLocation(ParticleCollisionEvent particleCollisionEvent){
//		Burst_bubble.transform.position = particleCollisionEvent.intersection;
//		Burst_bubble.transform.rotation = Quaternion.LookRotation (particleCollisionEvent.normal);
//		Burst_bubble.Emit (1);
////		Destroy (particleCollisionEvent);
//	}
	// Update is called once per frame
	void Update () {
//		if (Input.GetButton ("Fire1")) {
//			Bubble_Rain.Emit (1);
//		}
	}
}
