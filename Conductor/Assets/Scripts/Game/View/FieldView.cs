using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Conductor.Game.View
{
    public class FieldView : MonoBehaviour
    {
        [SerializeField] MeshRenderer renderer;
        [SerializeField] MeshFilter meshFilter;
        [SerializeField] Vector2Int divide;
        [SerializeField] Vector2 size;

        Mesh mesh;
        float[] heightMap;

        private void Awake()
        {

        }

        Mesh CreateMockMesh()
        {
            var mesh = new Mesh();

            int vertexCount = (divide.x + 1) * (divide.y + 1);

            // vertex
            var heightMap = new float[vertexCount];
            var vertices = new Vector3[vertexCount];

            Vector2 tileSize = size;
            tileSize.x = tileSize.x / (float)(divide.x);
            tileSize.y = tileSize.y / (float)(divide.y);

            for (int i = 0; i < heightMap.Length; i++)
            {
                int xIndex = i % (divide.x + 1);
                int zIndex = i / (divide.x + 1);

                Vector3 position = new Vector3(tileSize.x * (float)xIndex, 0.0f, tileSize.y * (float)zIndex);
                position.y = Mathf.PerlinNoise(position.x, position.z);
                heightMap[i] = position.y;
                vertices[i] = position;
            }

            // index N型で
            int tileCount = divide.x * divide.y;
            var indices = new int[tileCount * 6];
            for (int i = 0; i < tileCount; i++)
            {
                int xIndex = i % divide.x;
                int zIndex = i / divide.x;

                int xVertexOffset = xIndex;
                int zVertexOffset = zIndex * (divide.x + 1);
                int vertexOffset = xVertexOffset + zVertexOffset;

                indices[i * 6 + 0] = vertexOffset + 0;
                indices[i * 6 + 1] = vertexOffset + divide.x + 1;
                indices[i * 6 + 2] = vertexOffset + 1;
                indices[i * 6 + 3] = vertexOffset + 1;
                indices[i * 6 + 4] = vertexOffset + divide.x + 1;
                indices[i * 6 + 5] = vertexOffset + divide.x + 2;
            }

            mesh.SetTriangles(indices, 0);

            // normal どうせモックだし正確な法線じゃなくてもよくね
            var addedCounts = new int[vertexCount];
            var normals = new Vector3[vertexCount];
            全三角形でループ、三角形法線を算出して足していって最後に割る

            // texcoord

            mesh.vertices = vertices;

            return mesh;
        }
    }
}
