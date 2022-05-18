using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayGrappleRope : MonoBehaviour {
    private MeshFilter mf;
    private MeshRenderer mr;
    private Transform player;
    [SerializeField] private Material material;

    private float width = 0.5f;

    private void Start() {
        mf = this.gameObject.AddComponent<MeshFilter>();
        mr = this.gameObject.AddComponent<MeshRenderer>();
        mr.sharedMaterial = material;
        player = FindObjectOfType<GrapplingHook>().gameObject.transform;
        this.gameObject.transform.eulerAngles -= this.gameObject.transform.parent.eulerAngles;

        Mesh mesh = new Mesh();

        Vector3[] vertices = new Vector3[4]
        {
            new Vector3(width / 2, 0, 0),
            new Vector3(-width / 2, 0, 0),
            new Vector3(-this.gameObject.transform.parent.position.x + player.position.x + width / 2, -this.gameObject.transform.parent.position.y + player.position.y, 0),
            new Vector3(-this.gameObject.transform.parent.position.x + player.position.x - width / 2, -this.gameObject.transform.parent.position.y + player.position.y, 0)
        };
        mesh.vertices = vertices;

        int[] tris = new int[6]
        {
            0, 2, 1,
            2, 3, 1
        };
        mesh.triangles = tris;

        Vector3[] normals = new Vector3[4]
        {
            -Vector3.forward,
            -Vector3.forward,
            -Vector3.forward,
            -Vector3.forward
        };
        mesh.normals = normals;

        Vector2[] uv = new Vector2[4]
        {
            new Vector2(0, 0),
            new Vector2(1, 0),
            new Vector2(0, 1),
            new Vector2(1, 1)
        };
        mesh.uv = uv;

        mf.mesh = mesh;
    }

    private void Update() {
        Vector3[] vertices = new Vector3[4]
        {
            new Vector3(width / 2, 0, 0),
            new Vector3(-width / 2, 0, 0),
            new Vector3(-this.gameObject.transform.parent.position.x + player.position.x + width / 2, -this.gameObject.transform.parent.position.y + player.position.y, 0),
            new Vector3(-this.gameObject.transform.parent.position.x + player.position.x - width / 2, -this.gameObject.transform.parent.position.y + player.position.y, 0)
        };
        mf.mesh.vertices = vertices;
    }
}
