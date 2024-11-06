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
    Question,
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
    [SerializeField] private GameObject dialogUI;

    private DialogNode dialogNode;
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

        fsm.SetStateEntry((int)RoomState.Question, OnEnterQuestion);
        fsm.SetStateUpdate((int)RoomState.Question, OnUpdateQuestion);
        fsm.SetStateExit((int)RoomState.Question, OnExitQuestion);

        fsm.SetStateEntry((int)RoomState.LeaveRoom, OnEnterLeaveRoom);
        fsm.SetStateUpdate((int)RoomState.LeaveRoom, OnUpdateLeaveRoom);
        fsm.SetStateExit((int)RoomState.LeaveRoom, OnExitLeaveRoom);
    }

    private void OnEnable() {
        GameEventSystem.GetInstance().SubscribeToEvent<DialogNode>(nameof(GameEventName.StartDialog), OnStartDialog);
        GameEventSystem.GetInstance().SubscribeToEvent(nameof(GameEventName.EndDialog), OnEndDialog);

        videoPlayer.loopPointReached += OnVideoFinish;
    }

    private void OnDisable() {
        GameEventSystem.GetInstance().UnsubscribeFromEvent<DialogNode>(nameof(GameEventName.StartDialog), OnStartDialog);
        GameEventSystem.GetInstance().UnsubscribeFromEvent(nameof(GameEventName.EndDialog), OnEndDialog);

        videoPlayer.loopPointReached -= OnVideoFinish;
    }

    private void Start() {
        pointAndClickUI.SetActive(false);
        dialogUI.SetActive(false);

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
        videoPlayer.clip = dialogNode.clip;
        videoPlayer.isLooping = false;
        videoPlayer.Play();

        // Are there any clues? If yes, update our notes.
        if (dialogNode.clueRoom != string.Empty &&
            dialogNode.clueItem != string.Empty &&
            dialogNode.clueDesc != string.Empty) {
            GameEventSystem.GetInstance().TriggerEvent<string, string, string>(nameof(GameEventName.FoundClue), dialogNode.clueRoom, dialogNode.clueItem, dialogNode.clueDesc);
        }
    }

    private void OnUpdateListen() {
    }

    private void OnExitListen() {

    }

    // Options State
    private void OnEnterQuestion() {
        dialogUI.SetActive(true);

        // Display dialog options.
        for (int i = 0; i < dialogNode.options.Length; ++i) {
            string text = dialogNode.options[i].text;
            DialogNode node = dialogNode.options[i].node;
            dialogUI.GetComponent<QuestionList>().AddButton(text,
                () => {
                    this.dialogNode = node;
                    fsm.ChangeState((int)RoomState.Listen);
                });
        }
    }

    private void OnUpdateQuestion() {

    }

    private void OnExitQuestion() {
        dialogUI.GetComponent<QuestionList>().ClearButtons();
        dialogUI.SetActive(false);
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
                fsm.ChangeState((int)RoomState.Question);
                break;
            case RoomState.LeaveRoom:
                SceneManager.LoadScene("MapScene");
                break;
        }
    }

    // Event Callbacks
    private void OnStartDialog(DialogNode node) {
        this.dialogNode = node;
        fsm.ChangeState((int)RoomState.Listen);
    }

    private void OnEndDialog() {
        fsm.ChangeState((int)RoomState.PointAndClick);
    }
}