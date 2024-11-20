using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public enum OutroState {
    Arrest,
    Death,
    Credits,

    Num,
}

public class Outro : MonoBehaviour {
    [Header("References")]
    [SerializeField] private Conclusion conclusion;
    [SerializeField] private Answer answer;
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private VideoClip creditsClip;

    private FiniteStateMachine fsm = new FiniteStateMachine();

    private void Awake() {
        fsm.SetNumStates((int)OutroState.Num);
        fsm.SetStateEntry((int)OutroState.Arrest, EnterArrest);
        fsm.SetStateEntry((int)OutroState.Death, EnterDeath);
        fsm.SetStateEntry((int)OutroState.Credits, EnterCredits);
    }

    private void OnEnable() {
        videoPlayer.loopPointReached += OnVideoFinish;
    }

    private void OnDisable() {
        videoPlayer.loopPointReached -= OnVideoFinish;
    }

    private void Start() {
        if (IsMurdererCorrect() && !IsMurderWeaponCorrect()) {
            fsm.ChangeState((int)OutroState.Death);
        } else {
            fsm.ChangeState((int)OutroState.Arrest);
        }
    }

    private void Update() {
        fsm.Update();
    }

    private void LateUpdate() {
        fsm.LateUpdate();
    }

    private bool IsMurdererCorrect() {
        return answer.murderer == conclusion.GetMurderer();
    }

    private bool IsMurderWeaponCorrect() {
        return answer.murderWeapon == conclusion.GetMurderWeapon();
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

    // ************ Credits State ************
    private void EnterCredits() {
        videoPlayer.clip = creditsClip;
        videoPlayer.isLooping = false;
        videoPlayer.Play();
    }

    // ************ Video Finish Callback ************
    private void OnVideoFinish(UnityEngine.Video.VideoPlayer vp) {
        OutroState currentState = (OutroState)fsm.GetCurrentState();
        switch (currentState) {
            case OutroState.Arrest:
                if (IsMurdererCorrect()) {
                    fsm.ChangeState((int)OutroState.Credits);
                } else {
                    fsm.ChangeState((int)OutroState.Death);
                }
                break;
            case OutroState.Death:
                fsm.ChangeState((int)OutroState.Credits);
                break;
            case OutroState.Credits:
                SceneManager.LoadScene("StartScene");
                break;
            default:
                break;
        }
    }
}