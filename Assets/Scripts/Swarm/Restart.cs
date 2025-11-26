using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Restart : MonoBehaviour
{
    private const string SceneToRestart = "EnemySwarm";

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            Debug.Log($"Restarting scene: {SceneToRestart}");
            SceneManager.LoadScene(SceneToRestart);
        }
    }
}
