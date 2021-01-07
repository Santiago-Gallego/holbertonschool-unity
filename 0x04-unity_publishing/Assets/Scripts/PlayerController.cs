using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>Represents the parts of the player object that the player controls.</summary>
public class PlayerController : MonoBehaviour
{
    /// <summary>Speed factor affecting the player's movement.</summary>
    public float speed = 24;

    /// <summary>The number of times the player can survive traps.</summary>
    public int health = 5;

    /// <summary>A <see cref="Text"/> element to update as health changes.</summary>
    public Text healthText;
    /// <summary>A <see cref="Image"/> element to update on loss or victory.</summary>
    public Image resultText;
    /// <summary>A <see cref="Text"/> element to update as the score changes.</summary>
    public Text scoreText;

    // tracks which directions are being held
    private readonly Stack<Directions> moveStack;

    // tracks collected coins
    private int score = 0;

    /// <summary>Which direction the player is moving.</summary>
    public Directions Direction { get { return this.moveStack.Peek(); } }

    /// <summary>The position of this object.</summary>
    public Vector3 Position
    {
        get { return this.GetComponent<Rigidbody>().position; }
        set { this.GetComponent<Rigidbody>().MovePosition(value); }
    }

    /// <summary>Construct an instance, initializing complex fields.</summary>
    public PlayerController() {
        this.moveStack = new Stack<Directions>(9);
        this.moveStack.Push(Directions.None);
    }

    /// <summary>Move player according to pressed directions.</summary>
    protected void FixedUpdate() {
        const float pythag = 0.7071067690849304f; // square root of 0.5
        Vector3 newPosition;

        this.GetComponent<Rigidbody>().velocity = Vector3.zero;
        while (this.Direction != Directions.None) {
            if (this.Direction == Directions.N && Input.GetKey(KeyCode.W))
                break;
            if (this.Direction == Directions.NE && Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.D))
                break;
            if (this.Direction == Directions.E && Input.GetKey(KeyCode.D))
                break;
            if (this.Direction == Directions.SE && Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.S))
                break;
            if (this.Direction == Directions.S && Input.GetKey(KeyCode.S))
                break;
            if (this.Direction == Directions.SW && Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.A))
                break;
            if (this.Direction == Directions.W && Input.GetKey(KeyCode.A))
                break;
            if (this.Direction == Directions.NW && Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.A))
                break;
            this.NextMove();
        }
        if (this.Direction == Directions.None)
            return;

        newPosition = this.Position;
        if (this.Direction == Directions.N)
            newPosition.z += this.speed / 100;
        else if (this.Direction == Directions.NE) {
            newPosition.x += this.speed * pythag / 100;
            newPosition.z += this.speed * pythag / 100;
        } else if (this.Direction == Directions.E)
            newPosition.x += this.speed / 100;
        else if (this.Direction == Directions.SE) {
            newPosition.x += this.speed * pythag / 100;
            newPosition.z -= this.speed * pythag / 100;
        } else if (this.Direction == Directions.S)
            newPosition.z -= this.speed / 100;
        else if (this.Direction == Directions.SW) {
            newPosition.x -= this.speed * pythag / 100;
            newPosition.z -= this.speed * pythag / 100;
        } else if (this.Direction == Directions.W)
            newPosition.x -= this.speed / 100;
        else if (this.Direction == Directions.NW) {
            newPosition.x -= this.speed * pythag / 100;
            newPosition.z += this.speed * pythag / 100;
        }
        this.Position = newPosition;
    }

    /// <summary>Wait some seconds before restarting the game on a loss.</summary>
    /// <param name="seconds">Number of seconds to wait.</param>
    /// <returns>A <see cref="WaitForSeconds"/> instance using <paramref name="seconds"/>.</returns>
    protected IEnumerator LoadScene(float seconds) {
        yield return new WaitForSeconds(seconds);
        SceneManager.LoadScene("maze");
    }

    /// <summary>Invalidate the current movement direction and returns the next one.</summary>
    /// <returns>The next direction the player is moving in.</returns>
    protected Directions NextMove() {
        if (this.Direction == Directions.None)
            return Directions.None;
        this.moveStack.Pop();
        return this.Direction;
    }

    /// <summary>Called when the player collides with a trigger.</summary>
    /// <param name="other">Collider touched by the player.</param>
    protected void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Pickup")) {
            this.score++;
            //Debug.Log(String.Format("Score: {0}", this.score));
            this.SetScoreText();
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("Trap")) {
            this.health--;
            //Debug.Log(String.Format("Health: {0}", this.health));
            this.SetHealthText();
        }
        else if (other.CompareTag("Goal")) {
            //Debug.Log("You win!");
            this.resultText.gameObject.SetActive(true);
            this.resultText.GetComponentInChildren<Text>().text = "You Win!";
            this.resultText.GetComponentInChildren<Text>().color = Color.black;
            this.resultText.color = Color.green;
            this.StartCoroutine(this.LoadScene(5));
        }
    }

    /// <summary>Update <see cref="healthText"/> with the player's health.</summary>
    public void SetHealthText() {
        this.healthText.text = String.Format("Health: {0}", this.health);
    }

    /// <summary>Update <see cref="scoreText"/> with the current score.</summary>
    public void SetScoreText() {
        this.scoreText.text = String.Format("Score: {0}", this.score);
    }

    /// <summary>Process movement key presses.</summary>
    protected void Update() {
        Directions old;
        if (Input.GetKeyDown(KeyCode.W)) {
            old = this.Direction;
            if (old != Directions.N && old != Directions.NE && old != Directions.NW)
                this.moveStack.Push(Directions.N);
            if (old == Directions.E || old == Directions.SE)
                this.moveStack.Push(Directions.NE);
            else if (old == Directions.W || old == Directions.SW)
                this.moveStack.Push(Directions.NW);
        }
        if (Input.GetKeyDown(KeyCode.D)) {
            old = this.Direction;
            if (old != Directions.E && old != Directions.NE && old != Directions.SE)
                this.moveStack.Push(Directions.E);
            if (old == Directions.N || old == Directions.NW)
                this.moveStack.Push(Directions.NE);
            else if (old == Directions.S || old == Directions.SW)
                this.moveStack.Push(Directions.SE);
        }
        if (Input.GetKeyDown(KeyCode.S)) {
            old = this.Direction;
            if (old != Directions.S && old != Directions.SE && old != Directions.SW)
                this.moveStack.Push(Directions.S);
            if (old == Directions.E || old == Directions.NE)
                this.moveStack.Push(Directions.SE);
            else if (old == Directions.W || old == Directions.NW)
                this.moveStack.Push(Directions.SW);
        }
        if (Input.GetKeyDown(KeyCode.A)) {
            old = this.Direction;
            if (old != Directions.W && old != Directions.NW && old != Directions.SW)
                this.moveStack.Push(Directions.W);
            if (old == Directions.N || old == Directions.NE)
                this.moveStack.Push(Directions.NW);
            else if (old == Directions.S || old == Directions.SE)
                this.moveStack.Push(Directions.SW);
        }

        if (this.health < 1) {
            //Debug.Log("Game Over!");
            //SceneManager.LoadScene("maze");
            this.resultText.gameObject.SetActive(true);
            this.resultText.GetComponentInChildren<Text>().text = "Game Over!";
            this.resultText.GetComponentInChildren<Text>().color = Color.white;
            this.resultText.color = Color.red;
            this.StartCoroutine(this.LoadScene(5));
        }
    }
}

public enum Directions
{
    None, N, NE, E, SE, S, SW, W, NW
}
