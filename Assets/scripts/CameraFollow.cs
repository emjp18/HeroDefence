using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] float cameraSpeed = 5.0f;
    [SerializeField] float upOffset = 5.0f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newPos = new Vector3(player.position.x, player.position.y + upOffset,-10);
        transform.position = Vector3.Slerp(transform.position, newPos, cameraSpeed * Time.deltaTime);
    }
}
/* CODE DOCUMENTATION
 Much of this is from youtube tutorials.
Applies this script to the main camera so that it stays focused on the player character. Serialize field is used instead of public.
Future update may include an ability to zoom in and out by changing fov. Also it should be clamped to the borders of the map somehow.
 Perhaps you could also change the target to be something else than the player for cinematic purproses.
 */