using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Catan.Camera
{
    /// <summary>
    /// Class responsible for camera movement
    /// </summary>
    public class CameraControl : MonoBehaviour
    {
        public float sens = 0.02f;
        private Vector3 posprime;

        public int scrollradius = 0;
        public float sidesens = 10f;
        public float arrowkeysens = 5f;

        public UnityEngine.Camera cam;

        private Vector3 pos;

        public float camdist_min = 20f;
        public float camdist_max = 300f;
        public float camdist = 25f;
        public float camdistmultiplier = 10f;
        public float dynamiscrollcmultiplier = 0.10f;

        public float xBound0 = -3f;
        public float xBound1 = 30f;
        public float zBound0 = -25f;
        public float zBound1 = 25f;

        private void Start()
        {
            transform.position = new Vector3(transform.position.x, camdist, transform.position.z);
            transform.rotation = Quaternion.Euler(0, -90, 0);
            cam.transform.rotation = Quaternion.Euler(62, -90, 0);
            pos = transform.position;
            transform.position = pos;
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(2))
            {
                posprime = Input.mousePosition;
            }

            if (Input.GetMouseButton(2))
            {
                Vector3 d = Input.mousePosition - posprime;
                transform.Translate(-d.x * sens * camdist * dynamiscrollcmultiplier, 0, -d.y * sens * camdist * dynamiscrollcmultiplier);
                transform.position = new Vector3(Mathf.Clamp(transform.position.x, xBound0, xBound1), transform.position.y, Mathf.Clamp(transform.position.z, zBound0, zBound1));
                posprime = Input.mousePosition;
                pos = transform.position;
            }
            else if (Mathf.Abs(Input.GetAxis("Horizontal")) > 0 || Mathf.Abs(Input.GetAxis("Vertical")) > 0)
            {
                pos.x = Mathf.Clamp(pos.x - Input.GetAxis("Vertical") * Time.deltaTime * camdist * dynamiscrollcmultiplier * arrowkeysens, xBound0, xBound1);
                pos.z = Mathf.Clamp(pos.z + Input.GetAxis("Horizontal") * Time.deltaTime * camdist * dynamiscrollcmultiplier * arrowkeysens, zBound0, zBound1);
                transform.position = pos;
            }

            if (Mathf.Abs(Input.GetAxis("Mouse ScrollWheel")) > 0)
            {
                camdist -= Input.GetAxis("Mouse ScrollWheel") * camdistmultiplier * camdist * dynamiscrollcmultiplier;
                camdist = Mathf.Clamp(camdist, camdist_min, camdist_max);

                pos.y = camdist;
            }

            transform.position = Vector3.Lerp(transform.position, pos, 0.05f);
        }
    }
}