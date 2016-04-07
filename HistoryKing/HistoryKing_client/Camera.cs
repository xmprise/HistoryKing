using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading;
using System.Runtime.InteropServices;
using AForge.Video;
using AForge.Video.DirectShow; 

namespace HistoryKing_client
{
	class Camera
	{
        private IVideoSource videoSource = null;
        private Bitmap lastFrame = null;
        private string lastVideoSourceError = null;

        // image dimension
        private int width = -1;
        private int height = -1;


        // public events
        public event EventHandler NewFrame; //새로운 프레임이 들어왔을때 발생하는 이벤트 핸들러
        public event EventHandler VideoSourceError; //에러 났을때 생기는 이벤트 핸들러 

        // Last video frame
        public Bitmap LastFrame
        {
            get { return lastFrame; }
        }

        // Last video source error
        public string LastVideoSourceError
        {
            get { return lastVideoSourceError; }
        }

        // Video frame width
        public int Width
        {
            get { return width; }
        }

        // Vodeo frame height
        public int Height
        {
            get { return height; }
        }

        // Frames received from the last access to the property
        public int FramesReceived
        {
            get { return (videoSource == null) ? 0 : videoSource.FramesReceived; }
        }

        // Bytes received from the last access to the property
        public int BytesReceived
        {
            get { return (videoSource == null) ? 0 : videoSource.BytesReceived; }
        }

        // Running property
        public bool IsRunning
        {
            get { return (videoSource == null) ? false : videoSource.IsRunning; }
        }

        //내PC에 연결된 카메라 장치들 
        public FilterInfoCollection MyCameraDevices
        {
            get { return new FilterInfoCollection(FilterCategory.VideoInputDevice); }
        }



        // 생성자 
        public Camera()
        {

        }


        //카메라 장치 초기화 
        public void SetCamera(IVideoSource source)
        {
            this.videoSource = source;
            videoSource.NewFrame += new NewFrameEventHandler(video_NewFrame);
            videoSource.VideoSourceError += new VideoSourceErrorEventHandler(video_VideoSourceError);

        }


        // Start video source
        public void Start()
        {
            try
            {
                if (videoSource != null)
                {
                    videoSource.Start();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        // Siganl video source to stop
        public void SignalToStop()
        {
            if (videoSource != null)
            {
                videoSource.SignalToStop();
            }
        }

        // Wait video source for stop
        public void WaitForStop()
        {
            // lock
            Monitor.Enter(this);

            if (videoSource != null)
            {
                videoSource.WaitForStop();
            }
            // unlock
            Monitor.Exit(this);
        }

        // Abort camera
        public void Stop()
        {
            // lock
            Monitor.Enter(this);

            if (videoSource != null)
            {
                videoSource.Stop();
            }
            // unlock
            Monitor.Exit(this);
        }

        // Lock it
        public void Lock()
        {
            Monitor.Enter(this);
        }

        // Unlock it
        public void Unlock()
        {
            Monitor.Exit(this);
        }

        // On new frame
        private void video_NewFrame(object sender, NewFrameEventArgs e)
        {
            try
            {
                // lock
                Monitor.Enter(this);

                // dispose old frame
                if (lastFrame != null)
                {
                    lastFrame.Dispose();
                }

                // reset error
                lastVideoSourceError = null;
                // get new frame
                lastFrame = (Bitmap)e.Frame.Clone();


                // image dimension
                width = lastFrame.Width;
                height = lastFrame.Height;
            }
            catch (Exception)
            {
            }
            finally
            {
                // unlock
                Monitor.Exit(this);
            }

            // notify client
            if (NewFrame != null)
                NewFrame(this, new EventArgs());
        }

        // On video source error
        private void video_VideoSourceError(object sender, VideoSourceErrorEventArgs e)
        {

            // save video source error's description
            lastVideoSourceError = e.Description;

            // notify clients about the error
            if (VideoSourceError != null)
            {
                VideoSourceError(this, new EventArgs());
            }
        }

	}
}
