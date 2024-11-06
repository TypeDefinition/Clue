using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Map : MonoBehaviour {
    private void Start() {

    }

    private void Update() {

    }

    public void OnRoomButtonClick(string scene) {
        SceneManager.LoadScene(scene);
    }
}