using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DevGate
{
    public class PopupContent : MonoBehaviour
    {

        [SerializeField]
        protected Image lockImage;

        [SerializeField]
        protected GameObject popupObject;

        protected void PlaySound(string path)
        {
            var clip = Resources.Load<AudioClip>(path);
            Sound.Play(clip);
        }

        protected void OnAwake()
        {
            SetVisible(false, 0f);
        }

        protected virtual void Show()
        {
            SetVisible(true, 0.3f);
        }

        public virtual void Hide()
        {
            SetVisible(false, 0.3f);
            Destroy(gameObject, 0.3f);
        }

        private void SetVisible(bool visible, float time)
        {
            foreach (Graphic item in GetComponentsInChildren<Graphic>())
                item.CrossFadeAlpha(visible ? 1 : 0, time, true);
        }
    }
}
