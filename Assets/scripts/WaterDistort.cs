using UnityEngine;
using System.Collections;

// 1) what is a model made of? vertices have Vec3[] points and Vec3[] normals; triangle is int[], a blend of 3 vert normals
// 2) MeshDebug.cs: let's practice using for() loops to output mesh data, and then visualize it as a cloud with Debug.DrawRay()
// 3) MeshWave.cs: let's use for() loops to modify each mesh vertex with a sine wave
// 4) use randomSeed array to stagger sine waves
// 5) use RecalculateNormals to get shading

// photoshop: make a tiling water texture

// LAB: let's modify this to affect a cube, but only the top portion. go.
// - make a cube in Maya, subdivide it like the cube
// - import that cube
// - add an if() statement in the for() loop that will check to see if it's the top of the cube

public class WaterDistort : MonoBehaviour {
    MeshFilter mf;

    public Vector3[] baseVertices;
    public Vector3[] normals;
    public float[] randomSeeds;
    public float waterHeight = 0.02f;

    void Start() {
        mf = GetComponent<MeshFilter>();
        baseVertices = mf.mesh.vertices;
        normals = mf.mesh.normals;

        randomSeeds = new float[mf.mesh.vertexCount];
        for ( int i = 0; i < mf.mesh.vertexCount; i++ ) {
            randomSeeds[i] = Random.Range( 0.5f, 2.5f );
        }
    }

    void Update() {
        Mesh mesh = mf.mesh;
        Vector3[] vertices = new Vector3[mf.mesh.vertexCount];
        for ( int i = 0; i < vertices.Length; i++ ) {
            vertices[i] = baseVertices[i] + normals[i] * Mathf.Sin( Time.time * randomSeeds[i] ) * waterHeight;
        }
        mesh.vertices = vertices;

        mesh.RecalculateNormals(); // don't do this at first

        //// visualize vertices at first, then visualize normals
        //for ( int i = 0; i < vertices.Length; i++ ) {
        //    Debug.DrawRay( mf.mesh.vertices[i], mf.mesh.normals[i] );
        //}
    }
}