using UnityEngine;

public class CubeDestroy : MonoBehaviour
{
    public int DestroyAfter; //The time after which the box will be destroyed. Change parameter in editor !

    private void Start() //Start is called on the frame when a script is enabled just before any of the Update methods are called the first time.
    {
        Destroy(gameObject, DestroyAfter); //The box is destroyed
    }
}
