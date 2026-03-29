#pragma warning disable 612,618
#if UNITY_6000_0_OR_NEWER
using System;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;
using UnityEngine.Rendering.RenderGraphModule;
using UnityEngine.Rendering.Universal;

namespace Edgar.Unity
{
    public class FogOfWarURPFeature : ScriptableRendererFeature
    {
        /// <summary>
        /// An injection point for the full screen pass. This is similar to the RenderPassEvent enum but limited to only supported events.
        /// </summary>
        public enum InjectionPoint
        {
            /// <summary>
            /// Inject a full screen pass before transparents are rendered.
            /// </summary>
            BeforeRenderingTransparents = RenderPassEvent.BeforeRenderingTransparents,

            /// <summary>
            /// Inject a full screen pass before post processing is rendered.
            /// </summary>
            BeforeRenderingPostProcessing = RenderPassEvent.BeforeRenderingPostProcessing,

            /// <summary>
            /// Inject a full screen pass after post processing is rendered.
            /// </summary>
            AfterRenderingPostProcessing = RenderPassEvent.AfterRenderingPostProcessing
        }

        /// <summary>
        /// Specifies at which injection point the pass will be rendered.
        /// </summary>
        public InjectionPoint injectionPoint = InjectionPoint.BeforeRenderingPostProcessing;

        /// <summary>
        /// Specifies whether the assigned material will need to use the current screen contents as an input texture.
        /// Disable this to optimize away an extra color copy pass when you know that the assigned material will only need
        /// to write on top of or hardware blend with the contents of the active color target.
        /// </summary>
        [HideInInspector]
        public bool fetchColorBuffer = true;

        /// <summary>
        /// A mask of URP textures that the assigned material will need access to. Requesting unused requirements can degrade
        /// performance unnecessarily as URP might need to run additional rendering passes to generate them.
        /// </summary>
        [HideInInspector]
        public ScriptableRenderPassInput requirements = ScriptableRenderPassInput.None;

        /// <summary>
        /// The material used to render the full screen pass (typically based on the Fullscreen Shader Graph target).
        /// </summary>
        [HideInInspector]
        public Material passMaterial;

        /// <summary>
        /// The shader pass index that should be used when rendering the assigned material.
        /// </summary>
        [HideInInspector]
        public int passIndex = 0;

        /// <summary>
        /// Specifies if the active camera's depth-stencil buffer should be bound when rendering the full screen pass.
        /// Disabling this will ensure that the material's depth and stencil commands will have no effect (this could also have a slight performance benefit).
        /// </summary>
        [HideInInspector]
        public bool bindDepthStencilAttachment = false;

        private FogOfWarRenderPass _mFogOfWarPass;

        /// <inheritdoc/>
        public override void Create()
        {
            _mFogOfWarPass = new FogOfWarRenderPass(name);
        }

        /// <inheritdoc/>
        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
            if (renderingData.cameraData.cameraType == CameraType.Preview
                || renderingData.cameraData.cameraType == CameraType.Reflection
                || renderingData.cameraData.cameraType == CameraType.SceneView
                || UniversalRenderer.IsOffscreenDepthTexture(ref renderingData.cameraData))
                return;

            if (!IsFogOfWarEnabled(renderingData))
            {
                return;
            }
        
            passMaterial = FogOfWarGrid2D.Instance.GetMaterial(renderingData.cameraData.camera, "Edgar/FogOfWarURP");
            if (passMaterial == null)
            {
                return;
            }
        
            if (passIndex < 0 || passIndex >= passMaterial.passCount)
            {
                Debug.LogWarningFormat("The full screen feature \"{0}\" will not execute - the pass index is out of bounds for the material.", name);
                return;
            }

            _mFogOfWarPass.renderPassEvent = (RenderPassEvent)injectionPoint;
            _mFogOfWarPass.ConfigureInput(requirements);
            _mFogOfWarPass.SetupMembers(passMaterial, passIndex, fetchColorBuffer, bindDepthStencilAttachment);

            _mFogOfWarPass.requiresIntermediateTexture = fetchColorBuffer;
        
            renderer.EnqueuePass(_mFogOfWarPass);
        }
    
        private static bool IsFogOfWarEnabled(in RenderingData renderingData)
        {
            // Do not execute the effect in Editor
            if (!Application.isPlaying)
            {
                return false;
            }

            // Do not execute the effect if there is no instance of the script
            if (FogOfWarGrid2D.Instance == null)
            {
                return false;
            }

            // Do not execute the effect if the FogOfWar instance is disabled
            if (!FogOfWarGrid2D.Instance.enabled)
            {
                return false;
            }

            // Do not execute the effect if the current camera does not have the FogOfWar component attached
            if (renderingData.cameraData.camera.GetComponent<FogOfWarGrid2D>() == null &&
                renderingData.cameraData.camera.GetComponent<FogOfWarAdditionalCameraGrid2D>() == null)
            {
                return false;
            }

            return true;
        }

        /// <inheritdoc/>
        protected override void Dispose(bool disposing)
        {
            _mFogOfWarPass.Dispose();
        }

        private class FogOfWarRenderPass : ScriptableRenderPass
        {
            private Material m_Material;
            private int m_PassIndex;
            private bool m_FetchActiveColor;
            private bool m_BindDepthStencilAttachment;
            private RTHandle m_CopiedColor;

            private static MaterialPropertyBlock s_SharedPropertyBlock = new MaterialPropertyBlock();

            public FogOfWarRenderPass(string passName)
            {
                profilingSampler = new ProfilingSampler(passName);
            }
        
            private static readonly int kBlitTexturePropertyId = Shader.PropertyToID("_BlitTexture");
            private static readonly int kBlitScaleBiasPropertyId = Shader.PropertyToID("_BlitScaleBias");

            public void SetupMembers(Material material, int passIndex, bool fetchActiveColor, bool bindDepthStencilAttachment)
            {
                m_Material = material;
                m_PassIndex = passIndex;
                m_FetchActiveColor = fetchActiveColor;
                m_BindDepthStencilAttachment = bindDepthStencilAttachment;
            }
        
            [Obsolete]
            public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
            {
                ResetTarget();

                if (m_FetchActiveColor)
                    ReAllocate(renderingData.cameraData.cameraTargetDescriptor);
            }

            private void ReAllocate(RenderTextureDescriptor desc)
            {
                desc.msaaSamples = 1;
                desc.depthStencilFormat = GraphicsFormat.None;
                RenderingUtils.ReAllocateHandleIfNeeded(ref m_CopiedColor, desc, name: "_FogOfWarPassColorCopy");
            }

            public void Dispose()
            {
                m_CopiedColor?.Release();
            }

            private static void ExecuteCopyColorPass(RasterCommandBuffer cmd, RTHandle sourceTexture)
            {
                Blitter.BlitTexture(cmd, sourceTexture, new Vector4(1, 1, 0, 0), 0.0f, false);
            }

            private static void ExecuteMainPass(RasterCommandBuffer cmd, RTHandle sourceTexture, Material material, int passIndex)
            {
                s_SharedPropertyBlock.Clear();
            
                if (sourceTexture != null)
                    s_SharedPropertyBlock.SetTexture(kBlitTexturePropertyId, sourceTexture);
            
                // We need to set the "_BlitScaleBias" uniform for user materials with shaders relying on core Blit.hlsl to work
                s_SharedPropertyBlock.SetVector(kBlitScaleBiasPropertyId, new Vector4(1, 1, 0, 0));

                cmd.DrawProcedural(Matrix4x4.identity, material, passIndex, MeshTopology.Triangles, 3, 1, s_SharedPropertyBlock);
            }
        
            [Obsolete]
            public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
            {
                ref var cameraData = ref renderingData.cameraData;
                var cmd = CommandBufferPool.Get();

                using (new ProfilingScope(cmd, profilingSampler))
                {
                    RasterCommandBuffer rasterCmd = CommandBufferHelpers.GetRasterCommandBuffer(cmd);
                    if (m_FetchActiveColor)
                    {
                        CoreUtils.SetRenderTarget(cmd, m_CopiedColor);
                        ExecuteCopyColorPass(rasterCmd, cameraData.renderer.cameraColorTargetHandle);
                    }

                    if (m_BindDepthStencilAttachment)
                        CoreUtils.SetRenderTarget(cmd, cameraData.renderer.cameraColorTargetHandle, cameraData.renderer.cameraDepthTargetHandle);
                    else
                        CoreUtils.SetRenderTarget(cmd, cameraData.renderer.cameraColorTargetHandle);

                    ExecuteMainPass(rasterCmd, m_FetchActiveColor ? m_CopiedColor : null, m_Material, m_PassIndex);
                }
            
                context.ExecuteCommandBuffer(cmd);
                cmd.Clear();

                CommandBufferPool.Release(cmd);
            }

            public override void RecordRenderGraph(RenderGraph renderGraph, ContextContainer frameData)
            {
                UniversalResourceData resourcesData = frameData.Get<UniversalResourceData>();
                UniversalCameraData cameraData = frameData.Get<UniversalCameraData>();

                TextureHandle source, destination;

                Debug.Assert(resourcesData.cameraColor.IsValid());

                if (m_FetchActiveColor)
                {
                    var targetDesc = renderGraph.GetTextureDesc(resourcesData.cameraColor);
                    targetDesc.name = "_CameraColorFullScreenPass";
                    targetDesc.clearBuffer = false;

                    source = resourcesData.activeColorTexture;
                    destination = renderGraph.CreateTexture(targetDesc);
                
                    using (var builder = renderGraph.AddRasterRenderPass<CopyPassData>("Copy Color Full Screen", out var passData, profilingSampler))
                    {
                        passData.InputTexture = source;
                        builder.UseTexture(passData.InputTexture, AccessFlags.Read);

                        builder.SetRenderAttachment(destination, 0, AccessFlags.Write);

                        builder.SetRenderFunc((CopyPassData data, RasterGraphContext rgContext) =>
                        {
                            ExecuteCopyColorPass(rgContext.cmd, data.InputTexture);
                        });
                    }

                    //Swap for next pass;
                    source = destination;                
                }
                else
                {
                    source = TextureHandle.nullHandle;
                }

                destination = resourcesData.activeColorTexture;


                using (var builder = renderGraph.AddRasterRenderPass<MainPassData>(passName, out var passData, profilingSampler))
                {
                    passData.Material = m_Material;
                    passData.PassIndex = m_PassIndex;

                    passData.InputTexture = source;

                    if(passData.InputTexture.IsValid())
                        builder.UseTexture(passData.InputTexture, AccessFlags.Read);

                    bool needsColor = (input & ScriptableRenderPassInput.Color) != ScriptableRenderPassInput.None;
                    bool needsDepth = (input & ScriptableRenderPassInput.Depth) != ScriptableRenderPassInput.None;
                    bool needsMotion = (input & ScriptableRenderPassInput.Motion) != ScriptableRenderPassInput.None;
                    bool needsNormal = (input & ScriptableRenderPassInput.Normal) != ScriptableRenderPassInput.None;

                    if (needsColor)
                    {
                        Debug.Assert(resourcesData.cameraOpaqueTexture.IsValid());
                        builder.UseTexture(resourcesData.cameraOpaqueTexture);
                    }

                    if (needsDepth)
                    {
                        Debug.Assert(resourcesData.cameraDepthTexture.IsValid());
                        builder.UseTexture(resourcesData.cameraDepthTexture);
                    }

                    if (needsMotion)
                    {
                        Debug.Assert(resourcesData.motionVectorColor.IsValid());
                        builder.UseTexture(resourcesData.motionVectorColor);
                        Debug.Assert(resourcesData.motionVectorDepth.IsValid());
                        builder.UseTexture(resourcesData.motionVectorDepth);
                    }

                    if (needsNormal)
                    {
                        Debug.Assert(resourcesData.cameraNormalsTexture.IsValid());
                        builder.UseTexture(resourcesData.cameraNormalsTexture);
                    }
                
                    builder.SetRenderAttachment(destination, 0, AccessFlags.Write);

                    if (m_BindDepthStencilAttachment)
                        builder.SetRenderAttachmentDepth(resourcesData.activeDepthTexture, AccessFlags.Write);

                    builder.SetRenderFunc((MainPassData data, RasterGraphContext rgContext) =>
                    {
                        ExecuteMainPass(rgContext.cmd, data.InputTexture, data.Material, data.PassIndex);
                    });                
                }
            }

            private class CopyPassData
            {
                internal TextureHandle InputTexture;
            }

            private class MainPassData
            {
                internal Material Material;
                internal int PassIndex;
                internal TextureHandle InputTexture;
            }
        }
    }
}

#elif UNITY_2022_2_OR_NEWER
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

/*
 * @Cyanilux https://github.com/Cyanilux/URP_BlitRenderFeature
*/
namespace Edgar.Unity
{
    public class FogOfWarURPFeature : ScriptableRendererFeature
    {
        internal class BlitPass : ScriptableRenderPass
        {

            private BlitSettings settings;

            private RTHandle source;
            private RTHandle destination;
            private RTHandle temp;

            //private RTHandle srcTextureId;
            private RTHandle srcTextureObject;
            private RTHandle dstTextureId;
            private RTHandle dstTextureObject;

            private string m_ProfilerTag;

            public BlitPass(RenderPassEvent renderPassEvent, BlitSettings settings, string tag)
            {
                this.renderPassEvent = renderPassEvent;
                this.settings = settings;
                m_ProfilerTag = tag;
                if (settings.srcType == Target.RenderTextureObject && settings.srcTextureObject)
                    srcTextureObject = RTHandles.Alloc(settings.srcTextureObject);
                if (settings.dstType == Target.RenderTextureObject && settings.dstTextureObject)
                    dstTextureObject = RTHandles.Alloc(settings.dstTextureObject);
            }

            public void Setup(ScriptableRenderer renderer)
            {
                if (settings.requireDepthNormals)
                    ConfigureInput(ScriptableRenderPassInput.Normal);
            }

            public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
            {
                var desc = renderingData.cameraData.cameraTargetDescriptor;
                desc.depthBufferBits = 0; // Color and depth cannot be combined in RTHandles

                //RenderingUtils.ReAllocateIfNeeded(ref temp, Vector2.one, desc, name: "_TemporaryColorTexture");
                // These resizable RTHandles seem quite glitchy when switching between game and scene view :\
                // instead,
                RenderingUtils.ReAllocateIfNeeded(ref temp, desc, name: "_TemporaryColorTexture");

                var renderer = renderingData.cameraData.renderer;
                if (settings.srcType == Target.CameraColor)
                {
                    source = renderer.cameraColorTargetHandle;
                }
                else if (settings.srcType == Target.TextureID)
                {
                    //RenderingUtils.ReAllocateIfNeeded(ref srcTextureId, Vector2.one, desc, name: settings.srcTextureId);
                    //source = srcTextureId;
                    /*
                    Doesn't seem to be a good way to get an existing target with this new RTHandle system.
                    The above would work but means I'd need fields to set the desc too, which is just messy. If they don't match completely we get a new target
                    Previously we could use a RenderTargetIdentifier... but the Blitter class doesn't have support for those in 2022.1 -_-
                    Instead, I guess we'll have to rely on the shader sampling the global textureID
                    */
                    source = temp;
                }
                else if (settings.srcType == Target.RenderTextureObject)
                {
                    source = srcTextureObject;
                }

                if (settings.dstType == Target.CameraColor)
                {
                    destination = renderer.cameraColorTargetHandle;
                }
                else if (settings.dstType == Target.TextureID)
                {
                    desc.graphicsFormat = settings.graphicsFormat;
                    RenderingUtils.ReAllocateIfNeeded(ref dstTextureId, Vector2.one, desc, name: settings.dstTextureId);
                    destination = dstTextureId;
                }
                else if (settings.dstType == Target.RenderTextureObject)
                {
                    destination = dstTextureObject;
                }
            }

            public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
            {
                if (renderingData.cameraData.cameraType == CameraType.Preview) return;
                if (renderingData.cameraData.cameraType == CameraType.SceneView) return;
                if (!IsFogOfWarEnabled(renderingData)) return;

                CommandBuffer cmd = CommandBufferPool.Get(m_ProfilerTag);
                if (settings.setInverseViewMatrix)
                {
                    cmd.SetGlobalMatrix("_InverseView", renderingData.cameraData.camera.cameraToWorldMatrix);
                }

                //Debug.Log("blit : src = " + source.name + ", dst = " + destination.name);
                if (source == destination)
                {
                    Blitter.BlitCameraTexture(cmd, source, temp, settings.blitMaterial, settings.blitMaterialPassIndex);
                    Blitter.BlitCameraTexture(cmd, temp, destination, Vector2.one);
                }
                else
                {
                    Blitter.BlitCameraTexture(cmd, source, destination, settings.blitMaterial,
                        settings.blitMaterialPassIndex);
                }

                context.ExecuteCommandBuffer(cmd);
                CommandBufferPool.Release(cmd);
            }

            public override void OnCameraCleanup(CommandBuffer cmd)
            {
                source = null;
                destination = null;
            }

            public void Dispose()
            {
                temp?.Release();
                dstTextureId?.Release();
            }
        }

        [System.Serializable]
        public class BlitSettings
        {
            public RenderPassEvent Event = RenderPassEvent.BeforeRenderingPostProcessing;

            [HideInInspector] public Material blitMaterial = null;
            [HideInInspector] public int blitMaterialPassIndex = 0;
            [HideInInspector] public bool setInverseViewMatrix = false;
            [HideInInspector] public bool requireDepthNormals = false;

            [HideInInspector] public Target srcType = Target.CameraColor;

            //public string srcTextureId = "_CameraColorTexture";
            [HideInInspector] public RenderTexture srcTextureObject;

            [HideInInspector] public Target dstType = Target.CameraColor;
            [HideInInspector] public string dstTextureId = "_BlitPassTexture";
            [HideInInspector] public RenderTexture dstTextureObject;


            [HideInInspector] public bool overrideGraphicsFormat = false;
            [HideInInspector] public UnityEngine.Experimental.Rendering.GraphicsFormat graphicsFormat;
        }

        public enum Target
        {
            CameraColor,
            TextureID,
            RenderTextureObject
        }

        public BlitSettings settings = new BlitSettings();
        private BlitPass blitPass;

        public override void Create()
        {
            var passIndex = settings.blitMaterial != null ? settings.blitMaterial.passCount - 1 : 1;
            settings.blitMaterialPassIndex = Mathf.Clamp(settings.blitMaterialPassIndex, -1, passIndex);
            blitPass = new BlitPass(settings.Event, settings, name);

            if (settings.graphicsFormat == UnityEngine.Experimental.Rendering.GraphicsFormat.None)
            {
                settings.graphicsFormat =
                    SystemInfo.GetGraphicsFormat(UnityEngine.Experimental.Rendering.DefaultFormat.LDR);
            }
        }

        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
            if (IsFogOfWarEnabled(renderingData))
            {
                settings.blitMaterial = FogOfWarGrid2D.Instance.GetMaterial(renderingData.cameraData.camera, "Edgar/FogOfWarURP");
            }

            if (settings.blitMaterial == null)
            {
                return;
            }

            renderer.EnqueuePass(blitPass);
        }

        public override void SetupRenderPasses(ScriptableRenderer renderer, in RenderingData renderingData)
        {
            blitPass.Setup(renderer);
        }

        protected override void Dispose(bool disposing)
        {
            blitPass.Dispose();
        }

        private static bool IsFogOfWarEnabled(in RenderingData renderingData)
        {
            // Do not execute the effect in Editor
            if (!Application.isPlaying)
            {
                return false;
            }

            // Do not execute the effect if there is no instance of the script
            if (FogOfWarGrid2D.Instance == null)
            {
                return false;
            }

            // Do not execute the effect if the FogOfWar instance is disabled
            if (!FogOfWarGrid2D.Instance.enabled)
            {
                return false;
            }

            // Do not execute the effect if the current camera does not have the FogOfWar component attached
            if (renderingData.cameraData.camera.GetComponent<FogOfWarGrid2D>() == null &&
                renderingData.cameraData.camera.GetComponent<FogOfWarAdditionalCameraGrid2D>() == null)
            {
                return false;
            }

            return true;
        }
    }
}
#else
/*
 * Adapted for the Fog of War feature in SRP
 * ------------------------------------------------------------------------------------------------------------------------
 * Blit Renderer Feature                                                https://github.com/Cyanilux/URP_BlitRenderFeature
 * ------------------------------------------------------------------------------------------------------------------------
 * Based on the Blit from the UniversalRenderingExamples
 * https://github.com/Unity-Technologies/UniversalRenderingExamples/tree/master/Assets/Scripts/Runtime/RenderPasses
 * 
 * Extended to allow for :
 * - Specific access to selecting a source and destination (via current camera's color / texture id / render texture object
 * - Automatic switching to using _AfterPostProcessTexture for After Rendering event, in order to correctly handle the blit after post processing is applied
 * - Setting a _InverseView matrix (cameraToWorldMatrix), for shaders that might need it to handle calculations from screen space to world.
 *     e.g. reconstruct world pos from depth : https://twitter.com/Cyanilux/status/1269353975058501636 
 * ------------------------------------------------------------------------------------------------------------------------
 * @Cyanilux
*/
using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Edgar.Unity
{
    /// <summary>
    /// Scriptable renderer feature that has to be enabled to make the Fog of War work in URP.
    /// </summary>
    public class FogOfWarURPFeature : ScriptableRendererFeature
    {

        internal class BlitPass : ScriptableRenderPass
        {

            public Material blitMaterial = null;
            public FilterMode filterMode { get; set; }

            private BlitSettings settings;

            private RenderTargetIdentifier source { get; set; }
            private RenderTargetIdentifier destination { get; set; }

            RenderTargetHandle m_TemporaryColorTexture;
            RenderTargetHandle m_DestinationTexture;
            string m_ProfilerTag;

            public BlitPass(RenderPassEvent renderPassEvent, BlitSettings settings, string tag)
            {
                this.renderPassEvent = renderPassEvent;
                this.settings = settings;
                blitMaterial = settings.blitMaterial;
                m_ProfilerTag = tag;
                m_TemporaryColorTexture.Init("_TemporaryColorTexture");
                if (settings.dstType == Target.TextureID)
                {
                    m_DestinationTexture.Init(settings.dstTextureId);
                }
            }

            public void Setup(RenderTargetIdentifier source, RenderTargetIdentifier destination)
            {
                this.source = source;
                this.destination = destination;
            }

            public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
            {
                CommandBuffer cmd = CommandBufferPool.Get(m_ProfilerTag);

                RenderTextureDescriptor opaqueDesc = renderingData.cameraData.cameraTargetDescriptor;
                opaqueDesc.depthBufferBits = 0;

                if (settings.setInverseViewMatrix)
                {
                    Shader.SetGlobalMatrix("_InverseView", renderingData.cameraData.camera.cameraToWorldMatrix);
                }

                if (settings.dstType == Target.TextureID)
                {
                    cmd.GetTemporaryRT(m_DestinationTexture.id, opaqueDesc, filterMode);
                }
                
                //Debug.Log($"src = {source},     dst = {destination} ");
                // Can't read and write to same color target, use a TemporaryRT
                if (source == destination || (settings.srcType == settings.dstType && settings.srcType == Target.CameraColor))
                {
                    cmd.GetTemporaryRT(m_TemporaryColorTexture.id, opaqueDesc, filterMode);
                    Blit(cmd, source, m_TemporaryColorTexture.Identifier(), blitMaterial, settings.blitMaterialPassIndex);
                    Blit(cmd, m_TemporaryColorTexture.Identifier(), destination);
                }
                else
                {
                    Blit(cmd, source, destination, blitMaterial, settings.blitMaterialPassIndex);
                }

                context.ExecuteCommandBuffer(cmd);
                CommandBufferPool.Release(cmd);
            }

            public override void FrameCleanup(CommandBuffer cmd)
            {
                if (settings.dstType == Target.TextureID)
                {
                    cmd.ReleaseTemporaryRT(m_DestinationTexture.id);
                }
                if (source == destination || (settings.srcType == settings.dstType && settings.srcType == Target.CameraColor))
                {
                    cmd.ReleaseTemporaryRT(m_TemporaryColorTexture.id);
                }
            }
        }

        [System.Serializable]
        internal class BlitSettings
        {

            public RenderPassEvent Event = RenderPassEvent.BeforeRenderingPostProcessing;

            [HideInInspector]
            public Material blitMaterial = null;

            [HideInInspector]
            public int blitMaterialPassIndex = 0;

            [HideInInspector]
            public bool setInverseViewMatrix = false;

            [HideInInspector]
            public Target srcType = Target.CameraColor;

            [HideInInspector]
            public string srcTextureId = "_CameraColorTexture";

            #pragma warning disable 0649

            [HideInInspector]
            public RenderTexture srcTextureObject;

            #pragma warning restore 0649

            [HideInInspector]
            public Target dstType = Target.CameraColor;

            [HideInInspector]
            public string dstTextureId = "_BlitPassTexture";

            #pragma warning disable 0649

            [HideInInspector]
            public RenderTexture dstTextureObject;

            #pragma warning restore 0649
        }

        internal enum Target
        {
            CameraColor,
            TextureID,
            RenderTextureObject
        }

        [SerializeField]
        internal BlitSettings settings = new BlitSettings();

        BlitPass blitPass;

        private RenderTargetIdentifier srcIdentifier, dstIdentifier;

        public override void Create()
        {
            var passIndex = settings.blitMaterial != null ? settings.blitMaterial.passCount - 1 : 1;
            settings.blitMaterialPassIndex = Mathf.Clamp(settings.blitMaterialPassIndex, -1, passIndex);
            blitPass = new BlitPass(settings.Event, settings, name);

            if (settings.Event == RenderPassEvent.AfterRenderingPostProcessing)
            {
                Debug.LogWarning("Note that the \"After Rendering Post Processing\"'s Color target doesn't seem to work? (or might work, but doesn't contain the post processing) :( -- Use \"After Rendering\" instead!");
            }

            UpdateSrcIdentifier();
            UpdateDstIdentifier();
        }

        private void UpdateSrcIdentifier()
        {
            srcIdentifier = UpdateIdentifier(settings.srcType, settings.srcTextureId, settings.srcTextureObject);
        }

        private void UpdateDstIdentifier()
        {
            dstIdentifier = UpdateIdentifier(settings.dstType, settings.dstTextureId, settings.dstTextureObject);
        }

        private RenderTargetIdentifier UpdateIdentifier(Target type, string s, RenderTexture obj)
        {
            if (type == Target.RenderTextureObject)
            {
                return obj;
            }
            else if (type == Target.TextureID)
            {
                //RenderTargetHandle m_RTHandle = new RenderTargetHandle();
                //m_RTHandle.Init(s);
                //return m_RTHandle.Identifier();
                return s;
            }
            return new RenderTargetIdentifier();
        }

        private Material material;

        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
            if (IsFogOfWarEnabled(renderingData))
            {
                material = FogOfWarGrid2D.Instance.GetMaterial(renderingData.cameraData.camera);

                if (material == null)
                {
                    return;
                }
                
                #if !EDGAR_URP_13_OR_NEWER
                ApplyFogOfWar(renderer, renderingData);
                #endif
                
                renderer.EnqueuePass(blitPass);
            }
        }
        
        #if EDGAR_URP_13_OR_NEWER
        public override void SetupRenderPasses(ScriptableRenderer renderer, in RenderingData renderingData)
        {
            if (IsFogOfWarEnabled(renderingData))
            {
                ApplyFogOfWar(renderer, renderingData);
            }
        }
        #endif

        private bool IsFogOfWarEnabled(in RenderingData renderingData)
        {
            // Do not execute the effect in Editor
            if (!Application.isPlaying)
            {
                return false;
            }

            // Do not execute the effect if there is no instance of the script
            if (FogOfWarGrid2D.Instance == null)
            {
                return false;
            }

            // Do not execute the effect if the FogOfWar instance is disabled
            if (!FogOfWarGrid2D.Instance.enabled)
            {
                return false;
            }

            // Do not execute the effect if the current camera does not have the FogOfWar component attached
            if (renderingData.cameraData.camera.GetComponent<FogOfWarGrid2D>() == null && renderingData.cameraData.camera.GetComponent<FogOfWarAdditionalCameraGrid2D>() == null)
            {
                return false;
            }

            return true;
        }

        private void ApplyFogOfWar(ScriptableRenderer renderer, in RenderingData renderingData)
        {
            if (material == null)
            {
                return;
            }
            
            blitPass.blitMaterial = material;

            if (settings.Event == RenderPassEvent.AfterRenderingPostProcessing)
            {
            }
            // Comment for LWRP
            else if (settings.Event == RenderPassEvent.AfterRendering && renderingData.postProcessingEnabled)
            {
                // If event is AfterRendering, and src/dst is using CameraColor, switch to _AfterPostProcessTexture instead.
                if (settings.srcType == Target.CameraColor)
                {
                    settings.srcType = Target.TextureID;
                    settings.srcTextureId = "_AfterPostProcessTexture";
                    UpdateSrcIdentifier();
                }
                if (settings.dstType == Target.CameraColor)
                {
                    settings.dstType = Target.TextureID;
                    settings.dstTextureId = "_AfterPostProcessTexture";
                    UpdateDstIdentifier();
                }
            }
            else
            {
                // If src/dst is using _AfterPostProcessTexture, switch back to CameraColor
                if (settings.srcType == Target.TextureID && settings.srcTextureId == "_AfterPostProcessTexture")
                {
                    settings.srcType = Target.CameraColor;
                    settings.srcTextureId = "";
                    UpdateSrcIdentifier();
                }
                if (settings.dstType == Target.TextureID && settings.dstTextureId == "_AfterPostProcessTexture")
                {
                    settings.dstType = Target.CameraColor;
                    settings.dstTextureId = "";
                    UpdateDstIdentifier();
                }
            }

            var src = (settings.srcType == Target.CameraColor) ? renderer.cameraColorTarget : srcIdentifier;
            var dest = (settings.dstType == Target.CameraColor) ? renderer.cameraColorTarget : dstIdentifier;

            blitPass.Setup(src, dest);
        }
    }
}
#endif