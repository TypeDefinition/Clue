using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Custom class to play a tiled sprite animation in the canvas.
[RequireComponent(typeof(Image))]
public class CanvasSpriteAnimation : MonoBehaviour {
    [SerializeField, Min(0.1f)] private float animationDuration = 1.0f;
    [SerializeField, Min(1)] private int numRows = 4;
    [SerializeField, Min(1)] private int numCols = 5;

    private int currentFrame = 0;
    private float animationTimer = 0.0f;

    public void Start() {

    }

    public void Update() {
        animationTimer += Time.deltaTime;
        animationTimer %= animationDuration;

        currentFrame = (int) ((animationTimer / animationDuration) * (float) (numRows * numCols));

        int row = currentFrame / numCols;
        int col = currentFrame % numCols;

        float uvWidth = 1.0f / (float)numCols;
        float uvHeight = 1.0f / (float)numRows;

        // GetComponent<Image>().uv = new Rect(row * uvHeight, col * uvWidth, row * uvHeight + uvHeight, col * uvWidth + uvWidth);
    }
}