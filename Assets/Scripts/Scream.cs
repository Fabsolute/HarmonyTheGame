
using UnityEngine;
using System.Collections.Generic;

public class Scream : MonoBehaviour 
{
	private const float MIN_DIST = 0.01f;

	public Screamer Screamer;


	private float collisionBottom;
	private float collisionTop;
	private float collisionBottomStart;
	private float collisionTopStart;
	private Material screamMaterial;
	private PolygonCollider2D polygonCollider;
	private List<Vector3> obstaclePoints = new List<Vector3>();
	void Start () {
		
		var screamSpriteRenderer =GetComponent<SpriteRenderer>();
		if(screamSpriteRenderer!=null){
			screamMaterial = screamSpriteRenderer.material;
		}
		polygonCollider = GetComponent<PolygonCollider2D>();
	}
	
	void Update () 
	{

	}

	void OnCollisionStay2D(Collision2D collision)
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

			if (obstaclePoints.IndexOf(nearest_point) == -1)
			{
				// (new GameObject()).transform.position = nearest_point;
				obstaclePoints.Add(nearest_point);

				var center = polygonCollider.bounds.center;
				var minPoint = polygonCollider.bounds.min;
				var maxPoint = polygonCollider.bounds.max;
				switch (Screamer.Direction)
				{
					case Direction.Left:
						{
							if (nearest_point.y < center.y) // Obstacle is below 
							{
								var cutOutValueY = 1 - (center.y - nearest_point.y) / (center.y - minPoint.y);
								screamMaterial.SetFloat("_CutOutBottom", cutOutValueY);
								var cutOutValueX = (maxPoint.x - nearest_point.x) / (maxPoint.x - minPoint.x);
								screamMaterial.SetFloat("_CutOutBottomStart", cutOutValueX);
							} else // Obstacle is above
							{
								var cutOutValueY = (maxPoint.y - nearest_point.y) / (maxPoint.y - center.y);
								screamMaterial.SetFloat("_CutOutTop", cutOutValueY);
								var cutOutValueX = (maxPoint.x - nearest_point.x) / (maxPoint.x - minPoint.x);
								screamMaterial.SetFloat("_CutOutTopStart", cutOutValueX);
							}
						}
						break;
					case Direction.Right:
						{
							if (nearest_point.y < center.y) // Obstacle is below 
							{
								var cutOutValueY = 1 - (center.y - nearest_point.y) / (center.y - minPoint.y);
								screamMaterial.SetFloat("_CutOutBottom", cutOutValueY);
								var cutOutValueX = (nearest_point.x - minPoint.x) / (maxPoint.x - minPoint.x);
								screamMaterial.SetFloat("_CutOutBottomStart", cutOutValueX);
							} else // Obstacle is above
							{
								var cutOutValueY = (maxPoint.y - nearest_point.y) / (maxPoint.y - center.y);
								screamMaterial.SetFloat("_CutOutTop", cutOutValueY);
								var cutOutValueX = (nearest_point.x - minPoint.x) / (maxPoint.x - minPoint.x);
								screamMaterial.SetFloat("_CutOutTopStart", cutOutValueX);
							}
						}
						break;
					default:
					{
						if (nearest_point.y < center.y) // Obstacle is below 
						{
							var cutOutValueY = 1 - (center.y - nearest_point.y) / (center.y - minPoint.y);
							screamMaterial.SetFloat("_CutOutBottom", cutOutValueY);
							if (nearest_point.x < center.x) // Obstacle is left
							{
								var cutOutValueX = 1 - (maxPoint.x - nearest_point.x) / (maxPoint.x - minPoint.x);
							} 
						} else // Obstacle is above
						{
							var cutOutValueY = 1 - (maxPoint.y - nearest_point.y) / (maxPoint.y - center.y);
							var cutOutValueX = 1 - (maxPoint.x - nearest_point.x) / (maxPoint.x - minPoint.x);
							screamMaterial.SetFloat("_CutOutTop", cutOutValueY);
						}
					}
						break;
				}
			}
		}
	}

	public void OnPositionChanged()
	{
		obstaclePoints.Clear();
	}

	public void SetCutout(float newValue)
	{
		screamMaterial.SetFloat("_CutOut", newValue);
	}
}
