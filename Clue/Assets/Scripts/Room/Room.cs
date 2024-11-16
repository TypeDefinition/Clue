using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using UnityEngine;

public enum RoomState {
    None = -1,

    EnterRoom = 0,
    PointAndClick,
    Listen,
    Interrogate,
    LeaveRoom,

    Num,
}

public class Room : MonoBehaviour {
    [Header("Video")]
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private VideoClip enterRoomClip;
    [SerializeField] private VideoClip pointAndClickClip;
    [SerializeField] private VideoClip leaveRoomClip;

    [Header("UI")]
    [SerializeField] private GameObject pointAndClickUI;
    [SerializeField] private GameObject interrogateUI;

    private Dialogue dialogue;
    private FiniteStateMachine fsm = new FiniteStateMachine();

    private void Awake() {
        // Initialise Finite State Machine
        fsm.SetNumStates((int)RoomState.Num);

        fsm.SetStateEntry((int)RoomState.EnterRoom, OnEnterEnterRoom);
        fsm.SetStateUpdate((int)RoomState.EnterRoom, OnUpdateEnterRoom);
        fsm.SetStateExit((int)RoomState.EnterRoom, OnExitEnterRoom);

        fsm.SetStateEntry((int)RoomState.PointAndClick, OnEnterPointAndClick);
        fsm.SetStateUpdate((int)RoomState.PointAndClick, OnUpdatePointAndClick);
        fsm.SetStateExit((int)RoomState.PointAndClick, OnExitPointAndClick);

        fsm.SetStateEntry((int)RoomState.Listen, OnEnterListen);
        fsm.SetStateUpdate((int)RoomState.Listen, OnUpdateListen);
        fsm.SetStateExit((int)RoomState.Listen, OnExitListen);

        fsm.SetStateEntry((int)RoomState.Interrogate, OnEnterInterrogate);
        fsm.SetStateUpdate((int)RoomState.Interrogate, OnUpdateInterrogate);
        fsm.SetStateExit((int)RoomState.Interrogate, OnExitInterrogate);

        fsm.SetStateEntry((int)RoomState.LeaveRoom, OnEnterLeaveRoom);
        fsm.SetStateUpdate((int)RoomState.LeaveRoom, OnUpdateLeaveRoom);
        fsm.SetStateExit((int)RoomState.LeaveRoom, OnExitLeaveRoom);
    }

    private void OnEnable() {
        GameEventSystem.GetInstance().SubscribeToEvent<Dialogue>(nameof(GameEventName.StartDialogue), OnStartDialogue);
        GameEventSystem.GetInstance().SubscribeToEvent(nameof(GameEventName.EndDialogue), OnEndDialogue);

        videoPlayer.loopPointReached += OnVideoFinish;
    }

    private void OnDisable() {
        GameEventSystem.GetInstance().UnsubscribeFromEvent<Dialogue>(nameof(GameEventName.StartDialogue), OnStartDialogue);
        GameEventSystem.GetInstance().UnsubscribeFromEvent(nameof(GameEventName.EndDialogue), OnEndDialogue);

        videoPlayer.loopPointReached -= OnVideoFinish;
    }

    private void Start() {
        pointAndClickUI.SetActive(false);
        interrogateUI.SetActive(false);

        // Update Finite State Machine
        fsm.ChangeState((int)RoomState.EnterRoom);
    }

    private void Update() {
        fsm.Update();
    }

    // Enter Room State
    private void OnEnterEnterRoom() {
        // Play video.
        videoPlayer.clip = enterRoomClip;
        videoPlayer.Play();
    }

    private void OnUpdateEnterRoom() {
    }

    private void OnExitEnterRoom() {
    }

    // Point & Click State
    private void OnEnterPointAndClick() {
        videoPlayer.isLooping = true;
        videoPlayer.clip = pointAndClickClip;
        videoPlayer.Play();

        pointAndClickUI.SetActive(true);
    }

    private void OnUpdatePointAndClick() {

    }

    private void OnExitPointAndClick() {
        videoPlayer.Stop();

        pointAndClickUI.SetActive(false);
    }

    // Dialog State
    private void OnEnterListen() {
        // Play video.
        videoPlayer.clip = dialogue.GetVideoClip();
        videoPlayer.isLooping = false;
        videoPlayer.Play();

        // Are there any clues? If yes, update our notes.
        if (0 != dialogue.GetClues().Length) {
            GameEventSystem.GetInstance().TriggerEvent<Clue[]>(nameof(GameEventName.FoundClues), dialogue.GetClues());
        }
    }

    private void OnUpdateListen() {
        Debug.Log("uwuiwuwu");
    }

    private void OnExitListen() {

    }

    // Interrogate State
    private void OnEnterInterrogate() {
        interrogateUI.SetActive(true);

        // Display dialog options.
        DialogueOption[] options = dialogue.GetOptions();
        for (int i = 0; i < options.Length; ++i) {
            string text = options[i].text;
            Dialogue nextDialogue = options[i].nextDialogue;
            interrogateUI.GetComponent<QuestionList>().AddButton(text,
                () => {
                    this.dialogue = nextDialogue;
                    fsm.ChangeState((int)RoomState.Listen);
                });
        }
    }

    private void OnUpdateInterrogate() {

    }

    private void OnExitInterrogate() {
        interrogateUI.GetComponent<QuestionList>().ClearButtons();
        interrogateUI.SetActive(false);
    }

    // Leave Room State
    private void OnEnterLeaveRoom() {
        // Play video.
        videoPlayer.clip = leaveRoomClip;
        videoPlayer.Play();
    }

    private void OnUpdateLeaveRoom() {
    }

    private void OnExitLeaveRoom() {
    }

    // Video Player Callback
    private void OnVideoFinish(UnityEngine.Video.VideoPlayer vp) {
        RoomState currentState = (RoomState)fsm.GetCurrentState();
        switch (currentState) {
            case RoomState.EnterRoom:
                fsm.ChangeState((int)RoomState.PointAndClick);
                break;
            case RoomState.Listen:
                fsm.ChangeState((int)RoomState.Interrogate);
                break;
            case RoomState.LeaveRoom:
                SceneManager.LoadScene("MapScene");
                break;
        }
    }

    // Event Callbacks
    private void OnStartDialogue(Dialogue dialogue) {
        this.dialogue = dialogue;
        fsm.ChangeState((int)RoomState.Listen);
    }

    private void OnEndDialogue() {
        fsm.ChangeState((int)RoomState.PointAndClick);
    }
}