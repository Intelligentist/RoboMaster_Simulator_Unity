using UnityEngine;
using System;
using System.Reflection;
using System.Collections;

namespace MK.Glow
{
    [ExecuteInEditMode]
    [ImageEffectAllowedInSceneView]
    [RequireComponent(typeof(Camera))]
    public class MKGlowFree : MonoBehaviour
    {
        public enum MKGlowType
        {
            Selective = 0,
            Fullscreen = 1,
        }
        #region Get/Set
        private GameObject GlowCameraObject
        {
            get
            {
                if (!glowCameraObject)
                {
                    glowCameraObject = new GameObject("glowCameraObject");
                    glowCameraObject.hideFlags = HideFlags.HideAndDontSave;
                    glowCameraObject.AddComponent<Camera>();
                    GlowCamera.orthographic = false;
                    GlowCamera.enabled = false;
                    GlowCamera.renderingPath = RenderingPath.VertexLit;
					GlowCamera.hideFlags = HideFlags.HideAndDontSave;
                }
                return glowCameraObject;
            }
        }
        private Camera GlowCamera
        {
            get
            {
                if (glowCamera == null)
                {
                    glowCamera = GlowCameraObject.GetComponent<Camera>();
                }
                return glowCamera;
            }
        }
        public LayerMask GlowLayer
        {
            get { return glowLayer; }
            set { glowLayer = value; }
        }
        public MKGlowType GlowType
        {
            get { return glowType; }
            set { glowType = value; }
        }
        public Color GlowTint
        {
            get { return glowTint; }
            set { glowTint = value; }
        }
        public int Samples
        {
            get { return samples; }
            set { samples = value; }
        }
        public int BlurIterations
        {
            get { return blurIterations; }
            set
            {
                blurIterations = Mathf.Clamp(value, 0, 10);
            }
        }
        public float GlowIntensity
        {
            get { return glowIntensity; }
            set { glowIntensity = value; }
        }
        public float BlurSpread
        {
            get { return blurSpread; }
            set { blurSpread = value; }
        }

        private Material BlurMaterial
        {
            get
            {
                if (blurMaterial == null)
                {
                    blurMaterial = new Material(blurShader);
                    blurMaterial.hideFlags = HideFlags.HideAndDontSave;
                }
                return blurMaterial;
            }
        }

        private Material CompositeMaterial
        {
            get
            {
                if (compositeMaterial == null)
                {
                    compositeMaterial = new Material(compositeShader);
                    compositeMaterial.hideFlags = HideFlags.HideAndDontSave;
                }
                return compositeMaterial;
            }
        }
        #endregion


        #region Constants
        private static float[] gaussFilter = new float[11]
		{
			0.402f,0.623f,0.877f,1.120f,1.297f,1.362f,1.297f,1.120f,0.877f,0.623f,0.402f,
        };
        #endregion


        #region shaders
        [SerializeField]
        private Shader blurShader;
        [SerializeField]
        private Shader compositeShader;
        [SerializeField]
        private Shader glowRenderShader;
        #endregion


        #region privates
        private Material compositeMaterial;
        private Material blurMaterial;

        private Camera glowCamera;
        private GameObject glowCameraObject;
        private RenderTexture glowTexture;

        [SerializeField]
        [Tooltip("recommend: -1")]
        private LayerMask glowLayer = -1;

        [SerializeField]
        private Camera renderCamera;

        [SerializeField]
        [Tooltip("Selective = to specifically bring objects to glow, Fullscreen = complete screen glows")]
        private MKGlowType glowType = MKGlowType.Selective;
        [SerializeField]
        [Tooltip("The glows coloration")]
        private Color glowTint = new Color(1, 1, 1, 0);
        [SerializeField]
        [Tooltip("Width of the glow effect")]
        private float blurSpread = 0.25f;
        [SerializeField]
        [Tooltip("Number of used blurs")]
        private int blurIterations = 7;
        [SerializeField]
        [Tooltip("The global luminous intensity")]
        private float glowIntensity = 0.55f;
        [SerializeField]
        [Tooltip("Downsampling steps of the blur")]
        private int samples = 3;
        #endregion

        private void Main()
        {
            if (glowRenderShader == null)
            {
                enabled = false;
                Debug.LogWarning("Failed to load MKGlow Render Shader");
                return;
            }
            if (compositeShader == null)
            {
                enabled = false;
                Debug.LogWarning("Failed to load MKGlow Composite Shader");
                return;
            }

            if (blurShader == null)
            {
                enabled = false;
                Debug.LogWarning("Failed to load MKGlow Blur Shader");
                return;
            }

            if (renderCamera == null)
            {
                enabled = false;
                Debug.LogWarning("Failed to load render camera");
                return;
            }

            if(!SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.Default) || !SystemInfo.supportsImageEffects)
            {
                enabled = false;
                Debug.LogWarning("Glow not supported by platform");
                return;
            }
        }
        
        private void Reset()
        {
            glowLayer = -1;
            SetupShaders();
            if (renderCamera == null)
                renderCamera = GetComponent<Camera>();
        }
        
        private void OnEnable()
        {
            SetupShaders();
            if (renderCamera == null)
                renderCamera = GetComponent<Camera>();
        }

        private void SetupShaders()
        {
            if (!blurShader)
                blurShader = Shader.Find("Hidden/MKGlowBlur");

            if (!compositeShader)
                compositeShader = Shader.Find("Hidden/MKGlowComposite");

            if (!glowRenderShader)
                glowRenderShader = Shader.Find("Hidden/MKGlowRender");
        }

        private void OnDisable()
        {
            if (compositeMaterial)
            {
                DestroyImmediate(compositeMaterial);
            }
            if (blurMaterial)
            {
                DestroyImmediate(blurMaterial);
            }

            if (glowCamera)
                DestroyImmediate(GlowCamera);

            if (glowCameraObject)
                DestroyImmediate(GlowCameraObject);

            if (glowTexture)
            {
                RenderTexture.ReleaseTemporary(glowTexture);
                DestroyImmediate(glowTexture);
            }
        }

        private void SetupGlowCamera()
        {
            GlowCamera.CopyFrom(GetComponent<Camera>());
            GlowCamera.clearFlags = CameraClearFlags.SolidColor;
            GlowCamera.rect = new Rect(0, 0, 1, 1);
            GlowCamera.backgroundColor = new Color(0, 0, 0, 0);
            GlowCamera.cullingMask = glowLayer;
            GlowCamera.targetTexture = glowTexture;
            if (GlowCamera.actualRenderingPath != RenderingPath.VertexLit)
                GlowCamera.renderingPath = RenderingPath.VertexLit;
        }

        private void OnPreRender()
        {
            if (!gameObject.activeSelf || !enabled)
                return;

            if (glowTexture != null)
            {
                RenderTexture.ReleaseTemporary(glowTexture);
                glowTexture = null;
            }

            if (GlowType == MKGlowType.Selective)
            {
                glowTexture = RenderTexture.GetTemporary((int)((GetComponent<Camera>().pixelWidth) / samples), (int)((GetComponent<Camera>().pixelHeight) / samples), 16, RenderTextureFormat.Default);
                SetupGlowCamera();
                GlowCamera.RenderWithShader(glowRenderShader, "RenderType");
            }
            else
            {
                if (GlowCamera)
                    DestroyImmediate(GlowCamera);
                if (GlowCameraObject)
                    DestroyImmediate(GlowCameraObject);
            }

            BlurMaterial.color = new Color(glowTint.r, glowTint.g, glowTint.b, glowIntensity);
        }

        protected virtual void OnRenderImage(RenderTexture src, RenderTexture dest)
        {
            if (!gameObject.activeSelf || !enabled)
                return;

            if (GlowType == MKGlowType.Selective)
            {
                PerformSelectiveGlow(src, dest);
            }
            else if (GlowType == MKGlowType.Fullscreen)
            {
                PerformFullScreenGlow(src, dest);
            }
        }

        private void PerformBlur(RenderTexture src, RenderTexture dest)
        {
            float off = BlurSpread;
            BlurMaterial.SetTexture("_MainTex", src);
            BlurMaterial.SetFloat("_Shift", off);
            Graphics.Blit(src, dest, BlurMaterial);
        }

        private void PerformBlur(RenderTexture src, RenderTexture dest, int iteration)
        {
            float offset =  iteration * BlurSpread;
            offset *= gaussFilter[iteration];

            BlurMaterial.SetTexture("_MainTex", src);
            BlurMaterial.SetFloat("_Shift", offset);
            Graphics.Blit(src, dest, BlurMaterial);
        }

        private void PerformGlow(RenderTexture glowBuffer, RenderTexture dest, RenderTexture src)
        {
            CompositeMaterial.SetTexture("_GlowTex", glowBuffer);
            Graphics.Blit(src, dest, CompositeMaterial);
        }

        protected void PerformSelectiveGlow(RenderTexture source, RenderTexture dest)
        {
            Vector2 TextureSize;
            TextureSize.x = source.width / Samples;
            TextureSize.y = source.height / Samples;

            RenderTexture glowBuffer = RenderTexture.GetTemporary((int)TextureSize.x, (int)TextureSize.y, 0, RenderTextureFormat.Default);

            PerformBlur(glowTexture, glowBuffer);

            for (int i = 0; i < BlurIterations; i++)
            {
                RenderTexture glowBufferSecond = RenderTexture.GetTemporary((int)TextureSize.x, (int)TextureSize.y, 0, RenderTextureFormat.Default);
                PerformBlur(glowBuffer, glowBufferSecond, i);
                RenderTexture.ReleaseTemporary(glowBuffer);
                glowBuffer = glowBufferSecond;
            }
            
            PerformGlow(glowBuffer, dest, source);

            RenderTexture.ReleaseTemporary(glowBuffer);

            if (glowTexture != null)
            {
                RenderTexture.ReleaseTemporary(glowTexture);
                glowTexture = null;
            }
        }

        protected void PerformFullScreenGlow(RenderTexture source, RenderTexture destination)
        {
            Vector2 TextureSize;
            TextureSize.x = source.width / Samples;
            TextureSize.y = source.height / Samples;
            RenderTexture glowBuffer = RenderTexture.GetTemporary((int)TextureSize.x, (int)TextureSize.y, 0, RenderTextureFormat.Default);

            PerformBlur(source, glowBuffer);

            for (int i = 0; i < BlurIterations; i++)
            {
                RenderTexture glowBufferSecond = RenderTexture.GetTemporary((int)TextureSize.x, (int)TextureSize.y, 0, RenderTextureFormat.Default);
                PerformBlur(glowBuffer, glowBufferSecond, i);
                RenderTexture.ReleaseTemporary(glowBuffer);
                glowBuffer = glowBufferSecond;
            }
            Graphics.Blit(source, destination);

            PerformGlow(glowBuffer, destination, source);

            RenderTexture.ReleaseTemporary(glowBuffer);
        }
    }
}