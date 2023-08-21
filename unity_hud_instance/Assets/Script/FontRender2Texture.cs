
//=======================================================
// 作者：hannibal
// 描述：文字渲染到贴图
//=======================================================
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.UI;

namespace YX
{
    [RequireComponent(typeof(Text))]
    public class FontRender2Texture : MonoBehaviour
    {
        [SerializeField]
        private Camera _uiCamera;
        [SerializeField]
        private int _cellWidth = 100;
        [SerializeField]
        private int _cellHeight = 50;

        private int _slice = 0;
        [SerializeField]
        private int _textureDepth = 100;
        public Texture2DArray TextureArray { get; private set; }

        private Text _text;
        private RenderTexture _renderTexture;

        private void Awake()
        {
            _text = GetComponent<Text>();

            TextureArray = new Texture2DArray(_cellWidth, _cellHeight, _textureDepth, GraphicsFormat.R8G8B8A8_UNorm, TextureCreationFlags.None);
            TextureArray.name  = "FontTextureArray";

            _renderTexture = new RenderTexture(_cellWidth, _cellHeight, 0, GraphicsFormat.R8G8B8A8_UNorm);
            _renderTexture.name = "FontRenderTexture";
            _renderTexture.useMipMap = false;

            _uiCamera.enabled = false;
            _uiCamera.targetTexture = _renderTexture;
        }

        public int Draw(string txt)
        {
            _text.text = txt;
            
            _uiCamera.enabled = true;
            {
                RenderTexture.active = _renderTexture;
                _uiCamera.Render();
                RenderTexture.active = null;
            }
            _uiCamera.enabled = false;

            Graphics.CopyTexture(_renderTexture, 0, 0, 0, 0, _cellWidth, _cellHeight, TextureArray, _slice, 0, 0, 0);
            _slice++;
            return _slice - 1;
        }

        public void Reset()
        {
            _slice = 0;
        }
    }
}