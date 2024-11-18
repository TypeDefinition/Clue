using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public enum OutroState {
    Arrest,
    Death,

    Num,
}

public class Outro : MonoBehaviour {
    [Header("References")]
    [SerializeField] private Conclusion conclusion;
    [SerializeField] private Answer answer;
    [SerializeField] private VideoPlayer videoPlayer;

    private FiniteStateMachine fsm = new FiniteStateMachine();

    private void Awake() {
        fsm.SetNumStates((int)OutroState.Num);
        fsm.SetStateEntry((int)OutroState.Arrest, EnterArrest);
        fsm.SetStateEntry((int)OutroState.Death, EnterDeath);
    }

    private void OnEnable() {
        videoPlayer.loopPointReached += OnVideoFinish;
    }

    private void OnDisable() {
        videoPlayer.loopPointReached -= OnVideoFinish;
    }

    private void Start() {
        fsm.ChangeState((int)OutroState.Arrest);
    }

    private void Update() {
        fsm.Update();
    }

    private void LateUpdate() {
        fsm.LateUpdate();
    }

    private bool IsAnswerCorrect() {
        return (answer.murderWeapon == conclusion.GetMurderWeapon()) && (answer.murderer == conclusion.GetMurderer());
    }

    // ************ Arrest State ************
    private void EnterArrest() {
        videoPlayer.clip = conclusion.GetArrestClip(answer.murderer);
        videoPlayer.isLooping = false;
        videoPlayer.Play();
    }

    // ************ Death State ************
    private void EnterDeath() {
        videoPlayer.clip = conclusion.GetDeathClip();
        videoPlayer.isLooping = false;
        videoPlayer.Play();
    }

    // ************ Video Finish Callback ************
    private void OnVideoFinish(UnityEngine.Video.VideoPlayer vp) {
        OutroState currentState = (OutroState)fsm.GetCurrentState();
        switch (currentState) {
            case OutroState.Arrest:
                if (IsAnswerCorrect()) {
                    SceneManager.LoadScene("StartScene");
                } else {
                    fsm.ChangeState((int)OutroState.Death);
                }
                break;
            case OutroState.Death:
                SceneManager.LoadScene("StartScene");
                break;
            default:
                break;
        }
    }
}