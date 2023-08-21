
//=======================================================
// 作者：hannibal
// 描述：hud instance
//=======================================================
using System.Collections.Generic;
using UnityEngine;

namespace YX
{
    /// <summary>
    /// 使用Instanced批量渲染对象
    /// </summary>
    [RequireComponent(typeof(HUDMeshBuild))]
    public class HUDInstanced : MonoBehaviour
    {
        [SerializeField]
        private Material _instanceMat;
        [SerializeField]
        private FontRender2Texture _font2Texture;
        
        private HUDMeshBuild _meshBuild;
        private Mesh _instanceMesh;

        private Matrix4x4[] _matrices;
        private MaterialPropertyBlock _block;

        private void Awake()
        {
            _meshBuild = GetComponent<HUDMeshBuild>();
        }

        private void Start()
        {
            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();
            {
                _instanceMesh = _meshBuild.BuildMesh();
                for (int i = 0; i < 500; ++i)
                {
                    _font2Texture.Draw("Test:" + i);
                }
            }
            stopwatch.Stop();
            Debug.LogFormat("初始化字体耗时:{0}ms", stopwatch.ElapsedMilliseconds);

            _instanceMat.SetTexture("_FontTex", _font2Texture.TextureArray);

            BuildMatrixAndBlock();
        }
        void BuildMatrixAndBlock()
        {
            _block = new MaterialPropertyBlock();
            _matrices = new Matrix4x4[500];
            Vector4[] parms = new Vector4[500];
            for (var i = 0; i < 32; i++)
            {
                for (var j = 0; j < 32; j++)
                {
                    var ind = i * 32 + j;
                    if(ind >= 500) break;
                    _matrices[ind] = Matrix4x4.TRS(new Vector3((i - 8) * 2, j - 16, 0), Quaternion.identity, Vector3.one);
                    parms[ind].x = ind / 500f;
                    parms[ind].y = ind;
                }
            }
            _block.SetVectorArray("_Parms", parms);
        }
        void Update()
        {
            Graphics.DrawMeshInstanced(_instanceMesh, 0, _instanceMat, _matrices, 500, _block, UnityEngine.Rendering.ShadowCastingMode.Off, false);
        }
    }
}
