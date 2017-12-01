using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace DevGate
{
    public class CameraPostEffectComponent : MonoBehaviour
    {
        public Material Material;

        private void OnRenderImage(RenderTexture src, RenderTexture dest)
        {
            Material.SetTexture("_MainTex", src);
            Graphics.Blit(src, dest, Material);
        }
    }
}
