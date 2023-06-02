using UnityEngine;
using UnityEngine.EventSystems;

public class IndicatorScript : MonoBehaviour
{
    /// <summary>
    /// Enemy indicator. Creates rays from the enemy and sends it towards the player to check for collsion, if collision happens
    /// and enemy is not inside cambox a circle pops up to indicate where the ray comes from.
    /// </summary>
    public GameObject indicator;
    public GameObject Target;
    public GameObject Target2;
    public GameObject camBox;
    Renderer rd;
    int number;
    void Start()
    {
        rd = GetComponent<Renderer>();

    }


    void Update()
    {
        if(Target.activeSelf==true)
        {
            number= 1;
        }
        else 
        {
            number = 2;
        }
        if(number==1) 
        {
            if (rd.isVisible == false)
            {
                if (indicator.activeSelf == false)
                {
                    indicator.SetActive(true);
                }
                Vector2 direction = Target.transform.position - transform.position;
                // creates ray from enemie and aims towards the player but on a specific camboxlayer.
                RaycastHit2D ray = Physics2D.Raycast(transform.position, direction, (direction.x + 1000 + direction.y + 1000), 1 << 11);
                if (ray.collider != null)
                {
                    indicator.transform.position = ray.point;
                }
            }
            else
            {
                if (indicator.activeSelf == true)
                {
                    indicator.SetActive(false);

                }
            }
        }

        /// target 2
        if(number==2)
        {
            if (rd.isVisible == false)
            {
                if (indicator.activeSelf == false)
                {
                    indicator.SetActive(true);
                }
                Vector2 direction = Target2.transform.position - transform.position;

                RaycastHit2D ray = Physics2D.Raycast(transform.position, direction, (direction.x + 1000 + direction.y + 1000), 1 << 11);
                if (ray.collider != null)
                {
                    indicator.transform.position = ray.point;
                }
            }
            else
            {
                if (indicator.activeSelf == true)
                {
                    indicator.SetActive(false);
                }
            }
            //Debug.Log(ray.collider.gameObject.name + " was hit!");
        }
    }
       
}