
using UnityEngine;

public class Scream : MonoBehaviour {

	public Screamer Screamer;


	private float collisionBottom;
	private float collisionTop;
	private float collisionBottomStart;
	private float collisionTopStart;
	private Material screamMaterial;
	private PolygonCollider2D polygonCollider;
	void Start () {
		
		var screamSpriteRenderer =GetComponent<SpriteRenderer>();
		if(screamSpriteRenderer!=null){
			screamMaterial = screamSpriteRenderer.material;
		}
		polygonCollider = GetComponent<PolygonCollider2D>();
	}
	
	void Update () {
		if(screamMaterial!=null){
			Debug.Log("ok");
		}
	}

	void OnCollisionStay(Collision collision)
	{
		if(collision.collider.tag == "Box")
		{
			var nearest_point = collision.contacts[0].point;
			var nearest_dist = Vector3.Distance(nearest_point,Screamer.transform.position);
			foreach(var contact in collision.contacts){
				var current_dist = Vector3.Distance(contact.point,Screamer.transform.position);
				if(current_dist<nearest_dist){
					nearest_dist = current_dist;
					nearest_point = contact.point;
				}
			}

			 var length = polygonCollider.bounds.extents * 2;
			 var min= polygonCollider.bounds.min;
			 var max = polygonCollider.bounds.max;
		}
	}
}
