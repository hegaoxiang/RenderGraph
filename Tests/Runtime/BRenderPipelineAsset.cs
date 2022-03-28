using UnityEngine;
using UnityEngine.Rendering;

namespace Colorful.RenderGraph.Test
{
    [CreateAssetMenu(menuName = "BRenderPipeline/BRenderPieplineAsset")]
    public class BRenderPipelineAsset : RenderPipelineAsset
    {
        public RenderGraph renderGraph;
        public Texture SkyBoxTex;
        protected override RenderPipeline CreatePipeline()
        {
            
            return new BRenderPipeline(this);
        }
    }

    public class BRenderPipeline : RenderPipeline
    {
        RenderGraph mRenderGraph;

        BRenderPipelineAsset mAsset;
        public BRenderPipeline(BRenderPipelineAsset bRenderPipelineAsset)
        {
            mRenderGraph = bRenderPipelineAsset.renderGraph;
            mAsset = bRenderPipelineAsset;
        }
        protected override void Dispose(bool disposing)
        {

            base.Dispose(disposing);
            if(mRenderGraph)
                mRenderGraph.Cleanup();
            mRenderGraph = null;
        }
        protected override void Render(ScriptableRenderContext context, Camera[] cameras)
        {
            if (mRenderGraph == null)
                return;
            BeginFrameRendering(context,cameras);
            foreach (var camera in cameras)
            {
                BeginCameraRendering(context,camera);

                mRenderGraph.Excute(context,camera);
                context.Submit();
                EndCameraRendering(context,camera);
            }
            mRenderGraph.EndFrame();
            EndFrameRendering(context,cameras);
        }
    }
}
