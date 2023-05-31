using UnityEngine;
using UnityEngine.EventSystems;

public class IndicatorScript : MonoBehaviour
{

    public GameObject indicator;
    public GameObject Target;
    public GameObject Target2;
    public GameObject camBox;
    Renderer rd;
    int number;
    //Ray2D rays;
    void Start()
    {
        rd = GetComponent<Renderer>();
        //rays= new Ray2D(transform.position, transform.forward);
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
                    Debug.Log("testarIndiCatorUPPE");
                }
                Vector2 direction = Target.transform.position - transform.position;

                RaycastHit2D ray = Physics2D.Raycast(transform.position, direction, (direction.x + 1000 + direction.y + 1000), 1 << 11);
                if (ray.collider != null)
                {
                    indicator.transform.position = ray.point;
                    Debug.Log("testar RAY");
                }
            }
            else
            {
                if (indicator.activeSelf == true)
                {
                    indicator.SetActive(false);
                    Debug.Log("testarIndiCatorNERE");

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
                    Debug.Log("testarIndiCatorUPPE");
                }
                Vector2 direction = Target2.transform.position - transform.position;

                RaycastHit2D ray = Physics2D.Raycast(transform.position, direction, (direction.x + 1000 + direction.y + 1000), 1 << 11);
                if (ray.collider != null)
                {
                    indicator.transform.position = ray.point;
                    Debug.Log("testar RAY");
                }
            }
            else
            {
                if (indicator.activeSelf == true)
                {
                    indicator.SetActive(false);
                    Debug.Log("testarIndiCatorNERE");

                }
            }
            //Debug.Log(ray.collider.gameObject.name + " was hit!");
        }
    }
       
}