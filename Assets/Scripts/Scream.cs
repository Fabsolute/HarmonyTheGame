
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
    void Start()
    {

        var screamSpriteRenderer = GetComponent<SpriteRenderer>();
        if (screamSpriteRenderer != null)
        {
            screamMaterial = screamSpriteRenderer.material;
        }
        polygonCollider = GetComponent<PolygonCollider2D>();
    }

    void Update()
    {

    }

    void OnCollisionEnter2D(Collision2D collision){
        if(collision.collider.tag == "Player"){
            Screamer.OtherScreamers.Add(collision.collider.GetComponent<Screamer>());
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        CalculateDrawing(collision);
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        CalculateDrawing(collision);
        if(collision.collider.tag == "Player"){
            Screamer.OtherScreamers.Remove(collision.collider.GetComponent<Screamer>());
        }
    }

    void CalculateDrawing(Collision2D collision)
    {
        if (collision.collider.tag == "Box")
        {
            //For fukin right
            var box_center = collision.collider.bounds.center;
            var box_min = collision.collider.bounds.min;
            var box_max = collision.collider.bounds.max;

            var scream_center = polygonCollider.bounds.center;
            var scream_max = polygonCollider.bounds.max;
            var scream_min = polygonCollider.bounds.min;

            if (Screamer.Direction == Direction.Left)
            {
                var tempX = scream_max.x;
                scream_max = new Vector2(scream_min.x, scream_max.y);
                scream_min = new Vector2(tempX, scream_min.y);

                tempX = box_max.x;
                var symmetry_max_y = (2 * scream_center.y - box_max.y);
                var symmetry_min_y = (2 * scream_center.y - box_min.y);
                box_max = new Vector2(box_min.x, symmetry_min_y);
                box_min = new Vector2(tempX, symmetry_max_y);
            }
            else if (Screamer.Direction == Direction.Up)
            {
                scream_max = new Vector2(scream_max.y, scream_max.x);
                scream_min = new Vector2(scream_min.y, scream_min.x);
                box_min = new Vector2(box_min.y, box_min.x);
                box_max = new Vector2(box_max.y, box_max.x);
                scream_center = new Vector2(scream_center.y, scream_center.x);


                var symmetry_max_y = (2 * scream_center.y - box_max.y);
                var symmetry_min_y = (2 * scream_center.y - box_min.y);
                box_max = new Vector2(box_min.x, symmetry_min_y);
                box_min = new Vector2(box_max.x, symmetry_max_y);
            }
            else if (Screamer.Direction == Direction.Down)
            {
                scream_max = new Vector2(scream_max.y, scream_max.x);
                scream_min = new Vector2(scream_min.y, scream_min.x);
                box_min = new Vector2(box_min.y, box_min.x);
                box_max = new Vector2(box_max.y, box_max.x);
                scream_center = new Vector2(scream_center.y, scream_center.x);


                var tempX = scream_max.x;
                scream_max = new Vector2(scream_min.x, scream_max.y);
                scream_min = new Vector2(tempX, scream_min.y);
                tempX = box_max.x;
                box_max = new Vector2(box_min.x, box_max.y);
                box_min = new Vector2(tempX, box_min.y);
            }

            if (scream_center.y > box_max.y)
            {

                var cutOutValueY = 1 - (scream_center.y - box_max.y) / (scream_center.y - scream_min.y);
                var cutOutX = Mathf.Clamp01((box_min.x - scream_min.x) / (scream_max.x - scream_min.x));

                screamMaterial.SetFloat("_CutOutTopStart", cutOutX);
                screamMaterial.SetFloat("_CutOutTop", cutOutValueY);


                //kesin asagida
                // bottom_start box_min_x tir.
                // bottom box_max_y ye kadar cizilecek
            }
            else if (scream_center.y < box_min.y)
            {
                var cutOutValueY = 1 - (box_min.y - scream_center.y) / (scream_max.y - scream_center.y);
                var cutOutX = Mathf.Clamp01((box_min.x - scream_min.x) / (scream_max.x - scream_min.x));

                screamMaterial.SetFloat("_CutOutBottomStart", cutOutX);
                screamMaterial.SetFloat("_CutOutBottom", cutOutValueY);
                //kesin yukarida
                // top_start box_min_x tir.
                // top box_min_y ye kadar cizilecek
            }
            else
            {

                var cutOutX = Mathf.Clamp01((box_min.x - scream_min.x) / (scream_max.x - scream_min.x));


                screamMaterial.SetFloat("_CutOutTopStart", cutOutX);
                screamMaterial.SetFloat("_CutOutBottomStart", cutOutX);
                //box_min_x ten sonrasini cizdirme
            }


            // var nearest_point = collision.contacts[0].point;

            // var nearest_dist = Vector3.Distance(nearest_point, Screamer.transform.position);
            // foreach (var contact in collision.contacts)
            // {
            //     var current_dist = Vector3.Distance(contact.point, Screamer.transform.position);
            //     if (current_dist < nearest_dist)
            //     {
            //         nearest_dist = current_dist;
            //         nearest_point = contact.point;
            //     }
            // }

            // if (obstaclePoints.IndexOf(nearest_point) == -1)
            // {
            //     // (new GameObject()).transform.position = nearest_point;
            //     obstaclePoints.Add(nearest_point);

            //     var center = polygonCollider.bounds.center;
            //     var minPoint = polygonCollider.bounds.min;
            //     var maxPoint = polygonCollider.bounds.max;

            //     switch (Screamer.Direction)
            //     {
            //         case Direction.Left:
            //             {
            //                 if (nearest_point.y < center.y) // Obstacle is below 
            //                 {
            //                     var cutOutValueY = 1 - (center.y - nearest_point.y) / (center.y - minPoint.y);
            //                     screamMaterial.SetFloat("_CutOutBottom", cutOutValueY);
            //                     var cutOutValueX = (maxPoint.x - nearest_point.x) / (maxPoint.x - minPoint.x);
            //                     screamMaterial.SetFloat("_CutOutBottomStart", cutOutValueX);
            //                 }
            //                 else // Obstacle is above
            //                 {
            //                     var cutOutValueY = (maxPoint.y - nearest_point.y) / (maxPoint.y - center.y);
            //                     screamMaterial.SetFloat("_CutOutTop", cutOutValueY);
            //                     var cutOutValueX = (maxPoint.x - nearest_point.x) / (maxPoint.x - minPoint.x);
            //                     screamMaterial.SetFloat("_CutOutTopStart", cutOutValueX);
            //                 }
            //             }
            //             break;
            //         case Direction.Right:
            //             {
            //                 if (nearest_point.y < center.y) // Obstacle is below 
            //                 {
            //                     var cutOutValueY = 1 - (center.y - nearest_point.y) / (center.y - minPoint.y);
            //                     screamMaterial.SetFloat("_CutOutBottom", cutOutValueY);
            //                     var cutOutValueX = (nearest_point.x - minPoint.x) / (maxPoint.x - minPoint.x);
            //                     screamMaterial.SetFloat("_CutOutBottomStart", cutOutValueX);
            //                 }
            //                 else // Obstacle is above
            //                 {
            //                     var cutOutValueY = (maxPoint.y - nearest_point.y) / (maxPoint.y - center.y);
            //                     screamMaterial.SetFloat("_CutOutTop", cutOutValueY);
            //                     var cutOutValueX = (nearest_point.x - minPoint.x) / (maxPoint.x - minPoint.x);
            //                     screamMaterial.SetFloat("_CutOutTopStart", cutOutValueX);
            //                 }
            //             }
            //             break;
            //         default:
            //             {
            //                 if (nearest_point.y < center.y) // Obstacle is below 
            //                 {
            //                     var cutOutValueY = 1 - (center.y - nearest_point.y) / (center.y - minPoint.y);
            //                     screamMaterial.SetFloat("_CutOutBottom", cutOutValueY);
            //                     if (nearest_point.x < center.x) // Obstacle is left
            //                     {
            //                         var cutOutValueX = 1 - (maxPoint.x - nearest_point.x) / (maxPoint.x - minPoint.x);
            //                     }
            //                 }
            //                 else // Obstacle is above
            //                 {
            //                     var cutOutValueY = 1 - (maxPoint.y - nearest_point.y) / (maxPoint.y - center.y);
            //                     var cutOutValueX = 1 - (maxPoint.x - nearest_point.x) / (maxPoint.x - minPoint.x);
            //                     screamMaterial.SetFloat("_CutOutTop", cutOutValueY);
            //                 }
            //             }
            //             break;
            //     }
            // }
        }
    }

    // private Vector3 RotateVector(Vector3 point, Vector3 pivot, float angle)
    // {
    //     Debug.Log("Giris : " + point + " with pivot : " + pivot + " and angle is :" + angle);
    //     var diff = point - pivot;
    //     var rotated_diff = Quaternion.Euler(0, 0, angle) * diff;

    //     Debug.LogError("Cikis : " + (pivot + rotated_diff));
    //     return pivot + rotated_diff;
    // }

    public void OnPositionChanged()
    {
        obstaclePoints.Clear();
    }

    public void SetCutout(float newValue)
    {
        screamMaterial.SetFloat("_CutOut", newValue);
    }
}
