//================================================================
//							IMPORTANT
//================================================================
//				 Copyright LazyFridayStudio
// DO NOT SELL THIS CODE OR REDISTRIBUTE WITH INTENT TO SELL.
//
// Send an email to our support line for any questions or inquirys
// CONTACT: admin@lazyfridaystudio.com
//
// Alternatively join our Discord
// DISCORD: https://discord.gg/Z3tpMG
//
// Hope you enjoy the simple lazy Scene Camera 
// designed and made by lazyFridayStudio
//================================================================

#region namespaces

using System;
using System.Collections.Generic;
using UnityEngine;

#endregion

namespace LazyHelper.LazySceneCamera
{
    public class LazySceneCameraInfo : ScriptableObject
    {
        [System.Serializable]
        public class ViewItems
        {
            public string viewName = String.Empty;
            public string sceneName = String.Empty;
            public Vector3 viewPosition = Vector3.zero;
            public Quaternion viewRotation = Quaternion.identity;
        }

        public List<ViewItems> viewItems = new List<ViewItems>();
    }
}