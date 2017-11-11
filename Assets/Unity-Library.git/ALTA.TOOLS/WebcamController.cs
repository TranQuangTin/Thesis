using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Alta.Tools
{
    public class WebcamController : MonoBehaviour
    {
        private WebCamTexture webcam;
        public bool stopOnDisable = true;

        public virtual WebCamTexture OpenWebcam(string name, int width, int height)
        {
            webcam = new WebCamTexture(name, width, height);
            webcam.Play();
            return webcam;
        }

        public virtual void Pause()
        {
            this.webcam.Pause();
        }

        public virtual void Resume()
        {
            this.webcam.Play();
        }

        public virtual WebCamTexture OpenWebcam(string name)
        {
            webcam = new WebCamTexture(name);
            webcam.Play();
            return webcam;
        }

        public virtual WebCamTexture OpenWebcam(int index)
        {
            webcam = new WebCamTexture(WebCamTexture.devices[index].name);
            webcam.Play();
            return webcam;
        }

        public virtual WebCamTexture OpenWebcam(int index, int width, int height)
        {
            webcam = new WebCamTexture(WebCamTexture.devices[index].name,width,height);
            webcam.Play();
            return webcam;
        }


        public virtual void Stop()
        {
            if (webcam == null)
                return;
            webcam.Stop();
        }

        public virtual Texture2D TakePhoto(int x, int y, int width, int height)
        {
            if (webcam == null)
            {
                throw new WebcamException("Chua bat webcam");
            }
            if (x + width > webcam.width)
                throw new WebcamException(string.Format("Chieu rong cua hinh Khong du de chup tu {0} den {1}", x, x + width));
            if (y + height > webcam.height)
                throw new WebcamException(string.Format("Chieu cao cua hinh Khong du de chup tu {0} den {1}", y, y + height));

            Texture2D tex = new Texture2D(width, height);

            tex.SetPixels(webcam.GetPixels(x, y, width, height));
            tex.Apply();
            return tex;
        }

        public virtual Texture2D TakePhoto(int width, int height)
        {
            return TakePhoto(0, 0, width, height);
        }

        public virtual Texture2D TakePhoto()
        {
            return TakePhoto(0, 0, webcam.width, webcam.height);
        }

        public virtual void OnDisable()
        {
            if(stopOnDisable)
                this.Stop();
        }

    }


    public class WebcamException : System.Exception
    {
        public WebcamException()
                : base() { }

        public WebcamException(string message)
                : base(message) { }

        public WebcamException(string format, params object[] args)
                : base(string.Format(format, args)) { }

        public WebcamException(string message, System.Exception innerException)
                : base(message, innerException) { }

        public WebcamException(string format, System.Exception innerException, params object[] args)
                : base(string.Format(format, args), innerException) { }
    }

}