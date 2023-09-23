using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using CodeMonkey;

public class RadarPulse : MonoBehaviour {

    [SerializeField] private Transform pfRadarPing;
    [SerializeField] private LayerMask radarLayerMask;

    private Transform pulseTransform;
    private float range;
    private float rangeMax;
    private float rangeSpeed;
    private float fadeRange;
    private SpriteRenderer pulseSpriteRenderer;
    private Color pulseColor;
    private List<Collider2D> alreadyPingedColliderList;

    private void Awake() {
        pulseTransform = transform.Find("Pulse");
        pulseSpriteRenderer = pulseTransform.GetComponent<SpriteRenderer>();
        pulseColor = pulseSpriteRenderer.color;
        rangeMax = 400f;
      //  fadeRange = 1f;
        rangeSpeed = 80f;
        alreadyPingedColliderList = new List<Collider2D>();
    }

    private void Update() {
        range += rangeSpeed * Time.deltaTime;
        if (range > rangeMax) {
            range = 0f;
            alreadyPingedColliderList.Clear();
        }
        pulseTransform.localScale = new Vector3(range, range);

        RaycastHit2D[] raycastHit2DArray = Physics2D.CircleCastAll(transform.position, range / 2f, Vector2.zero);
        foreach (RaycastHit2D raycastHit2D in raycastHit2DArray) {

            if (raycastHit2D.collider != null) {
                // Hit something
                if (!alreadyPingedColliderList.Contains(raycastHit2D.collider)) {
                    alreadyPingedColliderList.Add(raycastHit2D.collider);
                    if (raycastHit2D.collider.gameObject.tag == "Mine")
                    {
                        // Hit a mine
                        Instantiate(pfRadarPing, raycastHit2D.point, Quaternion.identity);
                    }
                }
            }
        }
    }

}
