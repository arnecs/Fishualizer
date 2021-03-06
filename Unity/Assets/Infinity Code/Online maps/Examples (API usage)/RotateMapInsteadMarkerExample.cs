﻿/*     INFINITY CODE 2013-2015      */
/*   http://www.infinity-code.com   */

using UnityEngine;

namespace InfinityCode.OnlineMapsExamples
{
    [AddComponentMenu("")]
    public class RotateMapInsteadMarkerExample : MonoBehaviour
    {
        private OnlineMapsMarker marker;

        private void Start()
        {
            // Create a new marker.
            marker = OnlineMaps.instance.AddMarker(new Vector2(), "Player");

            // Subscribe to UpdateBefore event.
            OnlineMaps.instance.OnUpdateBefore += OnUpdateBefore;
        }

        private void OnUpdateBefore()
        {
            // Update camera rotation
            OnlineMapsTileSetControl.instance.cameraRotation = new Vector2(30, marker.rotation * 360);
        }
    }
}