using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DevGate
{
    public class PopupContent : MonoBehaviour
    {

        [SerializeField]
        private Image lockImage;

        [SerializeField]
        private GameObject popupObject;

        void Awake()
        {
            
        }

        public virtual void Init()
        {
        }

        public virtual void Show()
        {
        }

        public virtual void Hide()
        {
        }
    }
}
