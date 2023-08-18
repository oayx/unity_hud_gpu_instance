
//=======================================================
// 作者：hannibal
// 描述：合并mesh
//=======================================================
using System.Collections.Generic;
using UnityEngine;

namespace YX
{
    public class HUDMeshBuild : MonoBehaviour
    {
        [Header("名称")]
        public float NameWidth = 1f;
        public float NameHeight = 0.5f;
        public float NameYOffset = 0f;

        [Header("进度条")]
        public float ProgressWidth = 1f;
        public float ProgressHeight = 0.5f;
        public float ProgressYOffset = 1f;

        private List<Vector3> _vertices = new List<Vector3>();
        private List<Color> _colors = new List<Color>();
        private List<Vector2> _uvs = new List<Vector2>();
        private List<int> _triangles = new List<int>();

        public Mesh BuildMesh()
        {
            _vertices.Clear();
            _colors.Clear();
            _uvs.Clear();
            _triangles.Clear();
            {
                InitProgress();
                InitName();
            }
            Mesh mesh = new Mesh();
            mesh.vertices = _vertices.ToArray();
            mesh.colors = _colors.ToArray();
            mesh.uv = _uvs.ToArray();
            mesh.SetTriangles(_triangles, 0);
            return mesh;
        }
        private void InitProgress()
        {
            int indexOffset = _vertices.Count;
            Vector3 pos = Vector3.up * ProgressYOffset;

            _vertices.Add(pos + new Vector3(0, -ProgressHeight * 0.5f, 0));
            _vertices.Add(pos + new Vector3(ProgressWidth * 0.5f, -ProgressHeight * 0.5f, 0));
            _vertices.Add(pos + new Vector3(0, ProgressHeight * 0.5f, 0));
            _vertices.Add(pos + new Vector3(ProgressWidth * 0.5f, ProgressHeight * 0.5f, 0));

            _uvs.Add(new Vector2(0f, 0f));
            _uvs.Add(new Vector2(1f, 0f));
            _uvs.Add(new Vector2(0f, 1f));
            _uvs.Add(new Vector2(1f, 1f));

            //通过顶点颜色的alpha识别是进度条还是文字
            _colors.Add(Color.red + new Color(0, 0, 0, 0.1f));
            _colors.Add(Color.red + new Color(0, 0, 0, 0.1f));
            _colors.Add(Color.red + new Color(0, 0, 0, 0.1f));
            _colors.Add(Color.red + new Color(0, 0, 0, 0.1f));

            {
                _triangles.Add(indexOffset);
                _triangles.Add(indexOffset + 2);
                _triangles.Add(indexOffset + 1);

                _triangles.Add(indexOffset + 1);
                _triangles.Add(indexOffset + 2);
                _triangles.Add(indexOffset + 3);
            }
        }

        private void InitName()
        {
            int indexOffset = _vertices.Count;
            Vector3 pos = Vector3.up * NameYOffset;

            _vertices.Add(pos + new Vector3(0, -NameHeight * 0.5f, 0));
            _vertices.Add(pos + new Vector3(NameWidth * 0.5f, -NameHeight * 0.5f, 0));
            _vertices.Add(pos + new Vector3(0, NameHeight * 0.5f, 0));
            _vertices.Add(pos + new Vector3(NameWidth * 0.5f, NameHeight * 0.5f, 0));

            _uvs.Add(new Vector2(0f, 0f));
            _uvs.Add(new Vector2(1f, 0f));
            _uvs.Add(new Vector2(0f, 1f));
            _uvs.Add(new Vector2(1f, 1f));

            _colors.Add(Color.blue);
            _colors.Add(Color.blue);
            _colors.Add(Color.blue);
            _colors.Add(Color.blue);

            {
                _triangles.Add(indexOffset);
                _triangles.Add(indexOffset + 2);
                _triangles.Add(indexOffset + 1);

                _triangles.Add(indexOffset + 1);
                _triangles.Add(indexOffset + 2);
                _triangles.Add(indexOffset + 3);
            }
        }
    }
}