using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using UnityEngine;

public class Intro : MonoBehaviour {
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private VideoClip introClip;
    [SerializeField] private string nextScene = "MapScene";

    private void Start() {
        videoPlayer.clip = introClip;
        videoPlayer.loopPointReached += OnVideoFinish;
        videoPlayer.Play();
    }

    private void Update() {

    }

    private void OnVideoFinish(UnityEngine.Video.VideoPlayer vp) {
        videoPlayer.loopPointReached -= OnVideoFinish;
        SceneManager.LoadScene(nextScene);
    }
}