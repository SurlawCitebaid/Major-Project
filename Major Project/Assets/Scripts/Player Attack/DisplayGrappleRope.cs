using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class DisplayGrappleRope : MonoBehaviour {
    private SpriteShapeController ssc;
    private Transform player;

    [SerializeField] private float width = 0.1875f;

    private void Awake() {
        ssc = this.gameObject.GetComponent<SpriteShapeController>();
        player = FindObjectOfType<GrapplingHook>().gameObject.transform;
    }

    private void Start() {
        this.gameObject.transform.eulerAngles -= this.gameObject.transform.parent.eulerAngles;
    }

    private void LateUpdate() {
        float angle = 90 - (Mathf.Rad2Deg * Mathf.Atan2(this.gameObject.transform.parent.position.y - player.position.y, this.gameObject.transform.parent.position.x - player.position.x));
        float xLen = (Mathf.Cos(angle * Mathf.Deg2Rad) * (width / 2));
        float yLen = (Mathf.Sin(angle * Mathf.Deg2Rad) * (width / 2));
        ssc.spline.SetPosition(0, new Vector3(xLen, -yLen));
        ssc.spline.SetPosition(1, new Vector3(-xLen, yLen));
        ssc.spline.SetPosition(2, new Vector3(-this.gameObject.transform.parent.localPosition.x + player.position.x - xLen, player.position.y - this.gameObject.transform.parent.localPosition.y + yLen));
        ssc.spline.SetPosition(3, new Vector3(-this.gameObject.transform.parent.localPosition.x + player.position.x + xLen, player.position.y - this.gameObject.transform.parent.localPosition.y - yLen));
    }
}
