using System.Collections.Generic;
using UnityEngine;

namespace YX
{
    public class InstanceInfo
    {
        public int ValidCount = 0;
        public Matrix4x4[] Matrices = new Matrix4x4[1023];
        public MaterialPropertyBlock Blocks = new MaterialPropertyBlock();

        public void Reset()
        {
            ValidCount = 0;
            Blocks.Clear();
        }
    }

    public class DrawInstanceManager : MonoBehaviour
    {
        [SerializeField]
        private DrawInstance[] _drawInstances;

        [SerializeField]
        private Material _instanceMat;
        [SerializeField]
        private FontRender2Texture _font2Texture;

        private HUDMeshBuild _meshBuild;
        private Mesh _instanceMesh;

        /// <summary>
        /// 多线程更新耗时
        /// </summary>
        private System.Diagnostics.Stopwatch _updateStopwatch = new System.Diagnostics.Stopwatch();
        public long UpdateTimeConsume { get; private set; } = 0;

        public static DrawInstanceManager Instance;
        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            _meshBuild = GetComponent<HUDMeshBuild>();
            for (int i = 0; i < _drawInstances.Length; ++i)
                _drawInstances[i].Startup();

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

        }
        private void LateUpdate()
        {
            _updateStopwatch.Restart();
            {
                for (int i = 0; i < _drawInstances.Length; ++i)
                    _drawInstances[i].Update();
                for (int i = 0; i < _drawInstances.Length; ++i)
                    _drawInstances[i].Draw(_instanceMesh, _instanceMat);
            }
            _updateStopwatch.Stop();
            UpdateTimeConsume = _updateStopwatch.ElapsedMilliseconds;
        }
    }

    [System.Serializable]
    public class DrawInstance
    {
        /// <summary>
        /// 容量，1可以容纳511个单位
        /// </summary>
        public int Capacity = 1;

        /// <summary>
        /// 绘制信息
        /// </summary>
        private int _currBatchIndex = 0;
        /// <summary>
        /// 收集到的实例信息
        /// </summary>
        private List<InstanceInfo> _instanceInfo = null;

        public void Startup()
        {
            _instanceInfo = new List<InstanceInfo>(Capacity);
            for (int i = 0; i < _instanceInfo.Capacity; ++i)
            {
                InstanceInfo drawMesh = new InstanceInfo();
                _instanceInfo.Add(drawMesh);
            }
        }

        private int _tmpIndexMesh = 0;//临时变量
        private InstanceInfo _tmpInsInfo;//临时变量
        private Vector4[] _parms = new Vector4[511];
        private void PreUpdate()
        {
            _tmpIndexMesh = 0;
            _currBatchIndex = 0;
            _tmpInsInfo = _instanceInfo[_currBatchIndex];
            _tmpInsInfo.Reset();

        }
        public void Update()
        {
            PreUpdate();
            var actors = ActorManager.Instance.AllActors;
            foreach (var actor in actors)
            {
                if (_tmpIndexMesh >= 511)
                {
                    _tmpInsInfo.Blocks.SetVectorArray("_Parms", _parms);

                    //以下还原
                    ++_currBatchIndex;
                    _tmpInsInfo = _instanceInfo[_currBatchIndex];
                    _tmpInsInfo.Reset();

                    _tmpIndexMesh = 0;
                }
                _tmpInsInfo.ValidCount++;
                _tmpInsInfo.Matrices[_tmpIndexMesh] = actor.Matrix;
                _parms[_tmpIndexMesh].x = actor.Progress;
                _parms[_tmpIndexMesh].y = actor.NameIndex;
                _tmpIndexMesh++;
            }
            EndUpdate();
        }
        private void EndUpdate()
        {
            if (_tmpIndexMesh > 0)
            {
                ++_currBatchIndex;
                _tmpInsInfo.Blocks.Clear();
                _tmpInsInfo.Blocks.SetVectorArray("_Parms", _parms);
            }
        }
        public void Draw(Mesh mesh, Material material)
        {
            for (int i = 0; i < _currBatchIndex; ++i)
            {
                InstanceInfo instanceInfo = _instanceInfo[i];
                Graphics.DrawMeshInstanced(mesh, 0, material, instanceInfo.Matrices, instanceInfo.ValidCount, instanceInfo.Blocks, UnityEngine.Rendering.ShadowCastingMode.Off, false);
            }
        }
    }
}