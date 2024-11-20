using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using UnityEngine;
using UnityEngine.InputSystem;

public class Intro : MonoBehaviour {
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private VideoClip introClip;
    [SerializeField] private string nextScene = "MapScene";

    private GameInput gameInput;

    private void Awake() {
        gameInput = new GameInput();
    }

    private void OnEnable() {
        // Input
        gameInput.Enable();
        gameInput.Game.SkipVideo.performed += OnSkipVideo;
    }

    private void OnDisable() {
        // Input
        gameInput.Disable();
        gameInput.Game.SkipVideo.performed -= OnSkipVideo;
    }


    private void Start() {
        videoPlayer.clip = introClip;
        videoPlayer.loopPointReached += OnVideoFinish;
        videoPlayer.Play();
    }

    private void Update() {

    }

    // Video Player Callback
    private void OnVideoFinish(UnityEngine.Video.VideoPlayer vp) {
        videoPlayer.loopPointReached -= OnVideoFinish;
        SceneManager.LoadScene(nextScene);
    }

    // Input Callbacks
    private void OnSkipVideo(InputAction.CallbackContext context) {
        OnVideoFinish(videoPlayer);
    }
}