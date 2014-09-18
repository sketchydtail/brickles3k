using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.Kinect;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Brickles3k
{
    class KinectManager
    {
        

        public Texture2D colourVideo;
        public string connectedStatus = "Not connected";
        public Texture2D depthVideo;
        public KinectSensor kinect;
        public bool kinected;
        public Skeleton skeleton;
        public Skeleton[] skeletonData;
        private Game _game;

        public KinectManager()
        {

        }

        public void KinectSensors_StatusChanged(object sender, StatusChangedEventArgs e)
        {
            if (kinect == e.Sensor)
            {
                if (e.Status == KinectStatus.Disconnected ||
                    e.Status == KinectStatus.NotPowered)
                {
                    kinect = null;
                    DiscoverKinectSensor();
                }
            }
        }

        public void DiscoverKinectSensor()
        {
            foreach (KinectSensor sensor in KinectSensor.KinectSensors)
            {
                if (sensor.Status == KinectStatus.Connected)
                {
                    // Found one, set our sensor to this
                    kinect = sensor;
                    break;
                }
            }

            if (kinect == null)
            {
                connectedStatus = "No Kinect sensor found";
                return;
            }

            // You can use the kinectSensor.Status to check for status
            // and give the user some kind of feedback
            switch (kinect.Status)
            {
                case KinectStatus.Connected:
                    {
                        connectedStatus = "Status: Connected";
                        break;
                    }
                case KinectStatus.Disconnected:
                    {
                        connectedStatus = "Status: Disconnected";
                        break;
                    }
                case KinectStatus.NotPowered:
                    {
                        connectedStatus = "Status: Connect the power";
                        break;
                    }
                default:
                    {
                        connectedStatus = "Status: Error";
                        break;
                    }
            }

            // Init the found and connected device
            if (kinect.Status == KinectStatus.Connected)
            {
                InitializeKinect();
            }
        }

        public bool InitializeKinect()
        {
            // Color stream
            //kinect.ColorStream.CameraSettings.PowerLineFrequency = PowerLineFrequency.FiftyHertz;   //reduce 50hz flicker
            kinect.ColorStream.Enable(ColorImageFormat.RgbResolution640x480Fps30);
            kinect.DepthStream.Enable(DepthImageFormat.Resolution320x240Fps30);
            kinect.AllFramesReady += kinect_AllFramesReady;


            colourVideo = new Texture2D(Game1.game.graphics.GraphicsDevice, kinect.ColorStream.FrameWidth,
                kinect.ColorStream.FrameHeight);
            depthVideo = new Texture2D(Game1.game.GraphicsDevice, kinect.DepthStream.FrameWidth,
                kinect.DepthStream.FrameHeight);


            // Skeleton Stream
            kinect.SkeletonStream.TrackingMode = SkeletonTrackingMode.Seated;
            kinect.SkeletonStream.Enable(new TransformSmoothParameters
            {
                Smoothing = 0.2f,
                Correction = 0.6f,
                Prediction = 0.4f,
                JitterRadius = 0.05f,
                MaxDeviationRadius = 0.04f
            });
            //kinect.SkeletonFrameReady += new EventHandler<SkeletonFrameReadyEventArgs>(kinect_AllFramesReady);

            try
            {
                kinect.Start();
                kinected = true;
            }
            catch
            {
                //connectedStatus = "Unable to start the Kinect Sensor";
                return false;
            }
            return true;
        }

        public void kinect_AllFramesReady(object sender, AllFramesReadyEventArgs imageFrames)
        {
            //
            // Color Frame 
            //

            //Get raw image
            ColorImageFrame colorVideoFrame = imageFrames.OpenColorImageFrame();

            if (colorVideoFrame != null)
            {
                //Create array for pixel data and copy it from the image frame
                var pixelData = new Byte[colorVideoFrame.PixelDataLength];
                colorVideoFrame.CopyPixelDataTo(pixelData);

                //Convert RGBA to BGRA
                var bgraPixelData = new Byte[colorVideoFrame.PixelDataLength];
                for (int i = 0; i < pixelData.Length; i += 4)
                {
                    bgraPixelData[i] = pixelData[i + 2];
                    bgraPixelData[i + 1] = pixelData[i + 1];
                    bgraPixelData[i + 2] = pixelData[i];
                    bgraPixelData[i + 3] = 255; //The video comes with 0 alpha so it is transparent
                }

                // Create a texture and assign the realigned pixels
                colourVideo = new Texture2D(Game1.game.graphics.GraphicsDevice, colorVideoFrame.Width, colorVideoFrame.Height);
                colourVideo.SetData(bgraPixelData);
            }

            //
            // Depth Frame
            //
            DepthImageFrame depthVideoFrame = imageFrames.OpenDepthImageFrame();

            if (depthVideoFrame != null)
            {
                Debug.WriteLineIf(Game1.game.debugging, "Frame");
                //Create array for pixel data and copy it from the image frame
                var pixelData = new short[depthVideoFrame.PixelDataLength];
                depthVideoFrame.CopyPixelDataTo(pixelData);

                for (int i = 0; i < 10; i++)
                {
                    Debug.WriteLineIf(Game1.game.debugging, pixelData[i]);
                }

                // Convert the Depth Frame
                // Create a texture and assign the realigned pixels
                //
                depthVideo = new Texture2D(Game1.game.graphics.GraphicsDevice, depthVideoFrame.Width, depthVideoFrame.Height);
                depthVideo.SetData(ConvertDepthFrame(pixelData, kinect.DepthStream));
            }

            //
            // Skeleton Frame
            //
            using (SkeletonFrame skeletonFrame = imageFrames.OpenSkeletonFrame())
            {
                if (skeletonFrame != null)
                {
                    if ((skeletonData == null) || (skeletonData.Length != skeletonFrame.SkeletonArrayLength))
                    {
                        skeletonData = new Skeleton[skeletonFrame.SkeletonArrayLength];
                    }

                    //Copy the skeleton data to our array
                    skeletonFrame.CopySkeletonDataTo(skeletonData);
                }
            }

            if (skeletonData != null)
            {
                foreach (Skeleton skel in skeletonData)
                {
                    if (skel.TrackingState == SkeletonTrackingState.Tracked)
                    {
                        skeleton = skel;
                    }
                }
            }
        }

        public void DrawSkeleton(SpriteBatch spriteBatch, Vector2 resolution, Texture2D img)
        {
            if (skeleton != null)
            {
                foreach (Joint joint in skeleton.Joints)
                {
                    var position = new Vector2((((0.5f * joint.Position.X) + 0.5f) * (resolution.X)),
                        (((-0.5f * joint.Position.Y) + 0.5f) * (resolution.Y)));
                    Joint rightHand = skeleton.Joints[JointType.HandRight];
                    Game1.game.handPosition = new Vector2(((((0.5f * rightHand.Position.X) + 0.5f) * (resolution.X)) - Game1.game.hand.Width / 2),
                        ((((-0.5f * rightHand.Position.Y) + 0.5f) * (resolution.Y))) - Game1.game.hand.Height / 2);
                    Game1.game.easterEggPosition =
                        new Vector3(((((0.5f * rightHand.Position.X) + 0.5f) * (resolution.X)) - Game1.game.hand.Width / 2),
                            ((((-0.5f * rightHand.Position.Y) + 0.5f) * (resolution.Y))) - Game1.game.hand.Height / 2, 0f);
                    spriteBatch.Draw(img,
                        new Rectangle(Convert.ToInt32(position.X), Convert.ToInt32(position.Y), 10, 10), Color.Red);
                }
            }
        }

        public byte[] ConvertDepthFrame(short[] depthFrame, DepthImageStream depthStream)
        {
            int RedIndex = 0, GreenIndex = 1, BlueIndex = 2, AlphaIndex = 3;

            var depthFrame32 = new byte[depthStream.FrameWidth * depthStream.FrameHeight * 4];

            for (int i16 = 0, i32 = 0; i16 < depthFrame.Length && i32 < depthFrame32.Length; i16++, i32 += 4)
            {
                int player = depthFrame[i16] & DepthImageFrame.PlayerIndexBitmask;
                int realDepth = depthFrame[i16] >> DepthImageFrame.PlayerIndexBitmaskWidth;

                // transform 13-bit depth information into an 8-bit intensity appropriate
                // for display (we disregard information in most significant bit)
                var intensity = (byte)(~(realDepth >> 4));

                depthFrame32[i32 + RedIndex] = intensity;
                depthFrame32[i32 + GreenIndex] = intensity;
                depthFrame32[i32 + BlueIndex] = intensity;
                depthFrame32[i32 + AlphaIndex] = 255;
            }

            return depthFrame32;
        }

    }
}
