using System;
using UnityEngine;

namespace cowsins2D
{

    public class AuroraCameraController : CameraController
    {
        public void ChangeOrientation(OrientationEnum orientationEnum)
        {
            if (orientationEnum == OrientationEnum.Right)
            {
                SetCameraOffsetX(10);
            }
            else
            {
                SetCameraOffsetX(-10);
            }
        }
    }
}