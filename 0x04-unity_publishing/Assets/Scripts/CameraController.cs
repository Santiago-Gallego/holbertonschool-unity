using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>Represents the camera that follows the player.</summary>
public class CameraController : MonoBehaviour
{
    /// <summary>Which player object the camera follows.</summary>
    public GameObject player;

    /// <summary>Move the camera to the player's position each frame.</summary>
    protected void Update() {
        Vector3 playerPos;
        playerPos = this.player.transform.position;
        this.transform.position = new Vector3(playerPos.x - 1, playerPos.y + 24.75f, playerPos.z - 9);
    }
}
