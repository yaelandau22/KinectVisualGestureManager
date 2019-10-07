using System.Collections;
using System.Collections.Generic;
using Microsoft.Kinect.VisualGestureBuilder;
using UnityEngine;
using Windows.Kinect;
using System.IO;



public class CustomGestureManager : MonoBehaviour
{
    //VisualGestureBuilderDatabase _gestureDatabase;
    VisualGestureBuilderFrameSource _gestureFrameSource;
    VisualGestureBuilderFrameReader _gestureFrameReader;
    KinectSensor _kinect;
    Gesture Lean_Left;
    LinkedList<Gesture> trackedGestures = new LinkedList<Gesture>();
        private string[] databaseArray = {  
                                        @"GestureDB\Lean.gbd",
                                        //Left Hand
                                        //,@"GestureDB\LeftHandLeftToRight.gbd"
                                        //,@"GestureDB\LeftHandRightToLeft.gbd"
                                        //Right Hand
                                        @"GestureDB\RightHandLeftToRight.gbd"
                                        ,@"GestureDB\RightHandRightToLeft.gbd" //
                                        };
    private string[] databaseGestureNameArray = { 
                                                "Lean_Left"
                                                ,"Lean_Right"
                                                //Swipe Right
                                                    ,"SwipeRight_Left"
                                                ,"SwipeRight_Right"
                                                //Swipe Left
                                                ,"SwipeLeft_Left"
                                                ,"SwipeLeft_Right"
                                                };

    public void SetTrackingId(ulong id)
    {
        _gestureFrameReader.IsPaused = false;
        _gestureFrameSource.TrackingId = id;
        _gestureFrameReader.FrameArrived += _gestureFrameReader_FrameArrived;
    }
 
    // Use this for initialization
    void Start ()
    {
        _kinect = KinectSensor.GetDefault();
        _gestureFrameSource = VisualGestureBuilderFrameSource.Create(_kinect, 0);

        VisualGestureBuilderDatabase _gestureDatabase = null;
        foreach (var dbName in databaseArray)
        {
            var databasePath = Path.Combine(Application.streamingAssetsPath, dbName);
            _gestureDatabase = VisualGestureBuilderDatabase.Create(databasePath);
    
            foreach (var gesture in _gestureDatabase.AvailableGestures)
            {
                _gestureFrameSource.AddGesture(gesture);
                trackedGestures.AddLast(gesture);

                //  if (gesture.Name == "Lean_Left")
                // {
                //     Lean_Left = gesture;
                // }
            }
        }

        _gestureFrameReader = _gestureFrameSource.OpenReader();
        _gestureFrameReader.IsPaused = true;
    }
 
    void _gestureFrameReader_FrameArrived(object sender, VisualGestureBuilderFrameArrivedEventArgs e)
    {
        VisualGestureBuilderFrameReference frameReference = e.FrameReference;
        using (VisualGestureBuilderFrame frame = frameReference.AcquireFrame())
        {
            if (frame != null && frame.DiscreteGestureResults != null)
            { 
                DiscreteGestureResult result = null;
 
                if (frame.DiscreteGestureResults.Count > 0)
                {
                    foreach (Gesture gesture in trackedGestures)
                    {
                        result = frame.DiscreteGestureResults[gesture];

                        if (result != null)
                        {

                            if (result.Detected == true)
                            {
                                //if(result.Confidence > 0.60f)
                                {
                                    Debug.Log("Detected: " + gesture.Name);
                                    //Debug.Log("result confidence: " +result.Confidence);
                                }

                            }
                        }
                    }
                }
            }
        }
    }
}
