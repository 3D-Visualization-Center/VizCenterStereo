using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Vizcenter1.VizCenterStereo {
    public class SimpleStereo : MonoBehaviour {
        [Tooltip("Distance to render between eyes to create 3D vision.")]
        public float ipd = .5f;
        int inversion = 1;
        bool stereoActive = true;

        [Tooltip("Defaults to main camera at runtime. No need to set this unless desired.")]
        [SerializeField] Transform cam;
        [Tooltip("Menu parent for toggling the active state of the stereo system. Do NOT make this same object as, or a parent of, this script.")]
        [SerializeField] GameObject IPDMenuParent;
        [Tooltip("Text Mesh Pro object to display current IPD text.")]
        [SerializeField] TextMeshProUGUI ipdText;
        [Tooltip("Slider object reference to populate IPD value at start. Ensure the slider has the 'OnValueChanged' property linked to this script.")]
        [SerializeField] Slider slider;

        // Start is called before the first frame update
        void Start()
        {
            if (!cam)
                cam = Camera.main.transform;
            StartCoroutine(ToggleEye());

            ipdText.text = "" + RoundFloat(ipd, 0.01f);
            slider.value = ipd;
        }

        public void SetIpd(float _ipd)
        {
            ipd = _ipd;
            ipdText.text = "" + RoundFloat(_ipd, 0.01f);
        }


        public void InvertStereo()
        {
            inversion = -inversion;
        }

        IEnumerator ToggleEye()
        {
            yield return null;
            if (stereoActive)
            {
                inversion *= -1;
                cam.localPosition += Vector3.right * ipd * inversion;
                StartCoroutine(ToggleEye());
            }
        }

        public void ToggleStereo()
        {
            if (stereoActive) //if stereo was previously active, stop it.
                StopCoroutine(ToggleEye());
            else //start it back up
                StartCoroutine(ToggleEye());

            stereoActive = !stereoActive; //change the active state to reflect that.
        }
        #region UI Management// UI Management Section
        public void ToggleMenu()
        {
            IPDMenuParent.SetActive(!IPDMenuParent.activeSelf);
        }

        public float RoundFloat(float val, float sigFig)
        {
            float result = val;
            result /= sigFig; // example: 24.235 / 0.01 = 2423.5
            float remainder = result % 1; // example: 2423.5 %1 = 0.5.
            result -= remainder; // 2423.5 - 0.5 = 2423
            if (remainder >= 0.5f)
                result += 1; //round up; example: 2423 -> 2424
            result *= sigFig; // example: 2424 * 0.01 = 24.24
            return result;
        }
        #endregion
    }
}

