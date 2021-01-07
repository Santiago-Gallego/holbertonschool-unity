using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


/// <summary>Contains event handlers for the main menu.</summary>
public class MainMenu : MonoBehaviour
{
    /// <summary>Whether to use colorblind-accessible plane colors.</summary>
    public Toggle colorblindMode;
    /// <summary>The material used by the goal.</summary>
    public Material goalMat;
    /// <summary>The material used by traps.</summary>
    public Material trapMat;

    /// <summary>Load the maze scene.</summary>
    public void PlayMaze() {
        if (this.colorblindMode.isOn) {
            this.goalMat.color = new Color32(255, 112, 0, 1);
            this.trapMat.color = Color.blue;
        } else {
            this.goalMat.color = Color.green;
            this.trapMat.color = Color.red; 
        }
        SceneManager.LoadScene("maze");
    }

    /// <summary>Quit the game.</summary>
    public void QuitMaze() {
        Debug.Log("Quit Game");
        Application.Quit();
    }
}
