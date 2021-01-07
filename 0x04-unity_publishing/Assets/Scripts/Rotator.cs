using UnityEngine;

public class Rotator : MonoBehaviour
{
    ///<summary>Rotates the game object at a constant rate.</summary>
    protected void Update() {
        this.transform.Rotate(0, Time.deltaTime * 48, 0, Space.World);
    }
}
