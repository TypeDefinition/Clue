using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageRotator : MonoBehaviour {
    [SerializeField] private float angularVelocity = 30.0f;

    private void Update() {
        GetComponent<Image>().rectTransform.Rotate(0.0f, 0.0f, angularVelocity * Time.deltaTime);
    }
}