using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sea : MonoBehaviour
{
    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private float squareSize;

    private void Start()
    {
        MakeMesh();
    }

    private void MakeMesh()
    {
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        Mesh mesh = new Mesh();
        int[] triangles = new int[width * height * 6];
        Vector3[] vertices = new Vector3[width * height * 4];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                vertices[(x + y * width) * 4]     = new Vector3((-(width / 2) + x    ) * squareSize, 0, (-(height / 2) + y    ) * squareSize);
                vertices[(x + y * width) * 4 + 1] = new Vector3((-(width / 2) + x    ) * squareSize, 0, (-(height / 2) + y + 1) * squareSize);
                vertices[(x + y * width) * 4 + 2] = new Vector3((-(width / 2) + x + 1) * squareSize, 0, (-(height / 2) + y + 1) * squareSize);
                vertices[(x + y * width) * 4 + 3] = new Vector3((-(width / 2) + x + 1) * squareSize, 0, (-(height / 2) + y    ) * squareSize);

                triangles[(x + y * width) * 6]     = (x + y * width) * 4;
                triangles[(x + y * width) * 6 + 1] = (x + y * width) * 4 + 1;
                triangles[(x + y * width) * 6 + 2] = (x + y * width) * 4 + 2;

                triangles[(x + y * width) * 6 + 3] = (x + y * width) * 4 + 2;
                triangles[(x + y * width) * 6 + 4] = (x + y * width) * 4 + 3;
                triangles[(x + y * width) * 6 + 5] = (x + y * width) * 4;
            }
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        meshFilter.mesh = mesh;
    }
}