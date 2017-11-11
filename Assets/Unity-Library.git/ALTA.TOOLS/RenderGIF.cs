
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ThreadPriority = System.Threading.ThreadPriority;
using Alta.Gif;
namespace Alta.Tools
{

    public class RenderGIF : MonoBehaviour
    {
        public ThreadPriority WorkerPriority = ThreadPriority.BelowNormal;

        /// <summary>
        /// Called by each worker thread every time a frame is processed during the save process.
        /// The first parameter holds the worker ID and the second one a value in range [0;1] for
        /// the actual progress. This callback is probably not thread-safe, use at your own risks.
        /// </summary>
        public Action<int, float> OnFileSaveProgress;

        /// <summary>
        /// Called once a gif file has been saved. The first parameter will hold the worker ID and
        /// the second one the absolute file path.
        /// </summary>
        public Action<int, string> OnFileSaved;

        public void Render(List<GifFrame> _frames, string _filepath, int _framePerSecond = 30)
        {
            Resources.UnloadUnusedAssets();
            GifEncoder encoder = new GifEncoder();

            encoder.SetDelay(Mathf.RoundToInt(1000f / _framePerSecond));
            //StartCoroutine(encoder.Run(_frames,filepath, OnFileSaveProgress));

            Worker worker = new Worker(WorkerPriority)
            {
                m_Encoder = encoder,
                m_Frames = _frames,
                m_FilePath = _filepath,
                m_OnFileSaved = OnFileSaved,
                m_OnFileSaveProgress = OnFileSaveProgress
            };
            worker.Start();
        }

        public void Render(string _filepath, int _framePerSecond, params Texture2D[] texs)
        {
            List<GifFrame> _frames = new List<GifFrame>();
            foreach (var item in texs)
            {
                _frames.Add(new GifFrame() { Data = item.GetPixels32(), Height = item.height, Width = item.width });
            }
            Render(_frames, _filepath, _framePerSecond);
        }
    }
}
