using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using UnityEngine.InputSystem;
using UnityEngine;

public enum RoomState {
    None = -1,

    EnterRoom = 0,
    PointAndClick,
    StartConversation,
    Listen,
    Interrogate,
    EndConversation,
    Investigate,
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

    private FiniteStateMachine fsm = new FiniteStateMachine();
    private GameInput gameInput;
    private Conversation conversation;
    private Dialogue dialogue;
    private Investigation investigation;

    private void Awake() {
        gameInput = new GameInput();

        // Initialise number of states.
        fsm.SetNumStates((int)RoomState.Num);

        // Enter Room
        fsm.SetStateEntry((int)RoomState.EnterRoom, EnterEnterRoom);

        // Point & Click
        fsm.SetStateEntry((int)RoomState.PointAndClick, EnterPointAndClick);
        fsm.SetStateExit((int)RoomState.PointAndClick, ExitPointAndClick);

        // Start Conversation
        fsm.SetStateEntry((int)RoomState.StartConversation, EnterStartConversation);

        // Listen
        fsm.SetStateEntry((int)RoomState.Listen, EnterListen);

        // Interrogate
        fsm.SetStateEntry((int)RoomState.Interrogate, EnterInterrogate);
        fsm.SetStateExit((int)RoomState.Interrogate, ExitInterrogate);

        // End Conversation
        fsm.SetStateEntry((int)RoomState.EndConversation, EnterEndConversation);

        // Investigate
        fsm.SetStateEntry((int)RoomState.Investigate, EnterInvestigate);

        // Leave Room
        fsm.SetStateEntry((int)RoomState.LeaveRoom, EnterLeaveRoom);
    }

    private void OnEnable() {
        videoPlayer.loopPointReached += OnVideoFinish;

        GameEventSystem.GetInstance().SubscribeToEvent<Conversation>(nameof(GameEventName.StartConversation), OnStartConversation);
        GameEventSystem.GetInstance().SubscribeToEvent(nameof(GameEventName.EndConversation), OnEndConversation);
        GameEventSystem.GetInstance().SubscribeToEvent<Investigation>(nameof(GameEventName.StartInvestigation), OnStartInvestigation);

        // Input
        gameInput.Enable();
        gameInput.Game.SkipVideo.performed += OnSkipVideo;
    }

    private void OnDisable() {
        videoPlayer.loopPointReached -= OnVideoFinish;

        GameEventSystem.GetInstance().UnsubscribeFromEvent<Conversation>(nameof(GameEventName.StartConversation), OnStartConversation);
        GameEventSystem.GetInstance().UnsubscribeFromEvent(nameof(GameEventName.EndConversation), OnEndConversation);
        GameEventSystem.GetInstance().UnsubscribeFromEvent<Investigation>(nameof(GameEventName.StartInvestigation), OnStartInvestigation);

        // Input
        gameInput.Disable();
        gameInput.Game.SkipVideo.performed -= OnSkipVideo;
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

    private void LateUpdate() {
        fsm.LateUpdate();
    }

    // Enter Room State
    private void EnterEnterRoom() {
        if (enterRoomClip == null) {
            OnVideoFinish(videoPlayer);
            return;
        }

        // Play video.
        videoPlayer.clip = enterRoomClip;
        videoPlayer.Play();
    }

    // Point & Click State
    private void EnterPointAndClick() {
        if (pointAndClickClip != null) {
            videoPlayer.isLooping = true;
            videoPlayer.clip = pointAndClickClip;
            videoPlayer.Play();
        }

        pointAndClickUI.SetActive(true);
    }

    private void ExitPointAndClick() {
        videoPlayer.Stop();

        pointAndClickUI.SetActive(false);
    }

    // Start Conversation State
    private void EnterStartConversation() {
        // Skip video if there is none.
        if (conversation.GetStartVideoClip() == null) {
            OnVideoFinish(videoPlayer);
            return;
        }

        // Play video.
        videoPlayer.clip = conversation.GetStartVideoClip();
        videoPlayer.isLooping = false;
        videoPlayer.Play();
    }

    // End Conversation State
    private void EnterEndConversation() {
        // Skip video if there is none.
        if (conversation.GetEndVideoClip() == null) {
            OnVideoFinish(videoPlayer);
            return;
        }

        // Play video.
        videoPlayer.clip = conversation.GetEndVideoClip();
        videoPlayer.isLooping = false;
        videoPlayer.Play();
    }

    // Dialog State
    private void EnterListen() {
        // Are there any clues? If yes, update our notes.
        if (0 != dialogue.GetClues().Length) {
            GameEventSystem.GetInstance().TriggerEvent<Clue[]>(nameof(GameEventName.FoundClues), dialogue.GetClues());
        }

        // Skip video if there is none.
        if (dialogue.GetVideoClip() == null) {
            OnVideoFinish(videoPlayer);
            return;
        }

        // Play video.
        videoPlayer.clip = dialogue.GetVideoClip();
        videoPlayer.isLooping = false;
        videoPlayer.Play();
    }

    // Interrogate State
    private void EnterInterrogate() {
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

    private void ExitInterrogate() {
        interrogateUI.GetComponent<QuestionList>().ClearButtons();
        interrogateUI.SetActive(false);
    }

    // Investigate State
    private void EnterInvestigate() {
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

    // Leave Room State
    private void EnterLeaveRoom() {
        // Skip video if there is none.
        if (leaveRoomClip == null) {
            OnVideoFinish(videoPlayer);
            return;
        }

        // Play video.
        videoPlayer.clip = leaveRoomClip;
        videoPlayer.Play();
    }

    // Video Player Callback
    private void OnVideoFinish(UnityEngine.Video.VideoPlayer vp) {
        RoomState currentState = (RoomState)fsm.GetCurrentState();
        switch (currentState) {
            case RoomState.EnterRoom:
                fsm.ChangeState((int)RoomState.PointAndClick);
                break;
            case RoomState.StartConversation:
                fsm.ChangeState((int)RoomState.Listen);
                break;
            case RoomState.Listen:
                fsm.ChangeState((int)RoomState.Interrogate);
                break;
            case RoomState.EndConversation:
                fsm.ChangeState((int)RoomState.PointAndClick);
                break;
            case RoomState.Investigate:
                fsm.ChangeState((int)RoomState.PointAndClick);
                break;
            case RoomState.LeaveRoom:
                SceneManager.LoadScene("MapScene");
                break;
            default:
                break;
        }
    }

    // Event Callbacks
    private void OnStartConversation(Conversation conversation) {
        this.conversation = conversation;
        this.dialogue = conversation.GetInitialDialogue();
        fsm.ChangeState((int)RoomState.StartConversation);
    }

    private void OnEndConversation() {
        fsm.ChangeState((int)RoomState.EndConversation);
    }

    private void OnStartInvestigation(Investigation investigation) {
        this.investigation = investigation;
        fsm.ChangeState((int)RoomState.Investigate);
    }

    // Input Callbacks
    private void OnSkipVideo(InputAction.CallbackContext context) {
        OnVideoFinish(videoPlayer);
    }
}