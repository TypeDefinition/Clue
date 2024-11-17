using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitor : MonoBehaviour {
    public void OnTransitScene(string scene) {
        SceneManager.LoadScene(scene);
    }
}