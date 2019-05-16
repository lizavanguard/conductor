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
            mesh = CreateMockMesh();
            meshFilter.sharedMesh = mesh;
        }

        Mesh CreateMockMesh()
        {
            var tempMesh = new Mesh();

            int vertexCount = (divide.x + 1) * (divide.y + 1);

            // vertex
            var heightMap = new float[vertexCount];
            var vertices = new Vector3[vertexCount];

            Vector2 tileSize = size;
            tileSize.x = tileSize.x / (float)(divide.x);
            tileSize.y = tileSize.y / (float)(divide.y);

            float verticalSCale = 4.0f;
            float horizontalScale = 10.0f;
            for (int i = 0; i < heightMap.Length; i++)
            {
                int xIndex = i % (divide.x + 1);
                int zIndex = i / (divide.x + 1);

                Vector3 position = new Vector3(tileSize.x * (float)xIndex, 0.0f, tileSize.y * (float)zIndex);
                position.y = Mathf.PerlinNoise(position.x / horizontalScale, position.z / horizontalScale) * verticalSCale;
                heightMap[i] = position.y;
                vertices[i] = position;
            }

            tempMesh.SetVertices(new List<Vector3>(vertices));

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

            tempMesh.SetTriangles(indices, 0);

            // normal
            var addedCounts = new int[vertexCount];
            var normals = new Vector3[vertexCount];
            for (int i = 0; i < tileCount * 2; i++)
            {
                int i0 = indices[i * 3];
                int i2 = indices[i * 3 + 1];
                int i1 = indices[i * 3 + 2];

                var p0 = vertices[i0];
                var p1 = vertices[i1];
                var p2 = vertices[i2];

                var v1 = p1 - p0;
                var v2 = p2 - p0;

                var normal = Vector3.Cross(v1, v2).normalized;

                // 必要ないはずだけど一応ね
                if (normal.y < 0.0f)
                {
                    normal = -normal;
                }

                addedCounts[i0]++;
                addedCounts[i1]++;
                addedCounts[i2]++;
                normals[i0] += normal;
                normals[i1] += normal;
                normals[i2] += normal;
            }

            for (int i = 0; i < vertexCount; i++)
            {
                normals[i] /= (float)addedCounts[i];
            }

            tempMesh.SetNormals(new List<Vector3>(normals));

            // texcoord

            tempMesh.vertices = vertices;

            return tempMesh;
        }
    }
}
