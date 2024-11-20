using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class CrimeScene : MonoBehaviour {
    [Header("Video")]
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private Investigation investigation;

    private GameInput gameInput;

    private void Awake() {
        gameInput = new GameInput();
    }

    private void OnEnable() {
        videoPlayer.loopPointReached += OnVideoFinish;

        // Input
        gameInput.Enable();
        gameInput.Game.SkipVideo.performed += OnSkipVideo;
    }

    private void OnDisable() {
        videoPlayer.loopPointReached -= OnVideoFinish;

        // Input
        gameInput.Disable();
        gameInput.Game.SkipVideo.performed -= OnSkipVideo;
    }

    private void Start() {
        if (0 != investigation.GetClues().Length) {
            GameEventSystem.GetInstance().TriggerEvent<Clue[]>(nameof(GameEventName.FoundClues), investigation.GetClues());
        }

        // Skip the video if there is none.
        if (investigation.GetVideoClip() == null) {
            OnVideoFinish(videoPlayer);
            return;
        }

        // Play video.
        videoPlayer.clip = investigation.GetVideoClip();
        videoPlayer.isLooping = false;
        videoPlayer.Play();
    }

    private void Update() {
    }

    // Input Callbacks
    private void OnSkipVideo(InputAction.CallbackContext context) {
        OnVideoFinish(videoPlayer);
    }

    // Video Player Callback
    private void OnVideoFinish(UnityEngine.Video.VideoPlayer vp) {
        SceneManager.LoadScene("MapScene");
    }
}