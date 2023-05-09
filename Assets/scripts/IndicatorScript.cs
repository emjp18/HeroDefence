using UnityEngine;
using UnityEngine.EventSystems;

public class IndicatorScript : MonoBehaviour
{

    public GameObject indicator;
    public GameObject Target;
    public GameObject camBox;
    Renderer rd;
    //Ray2D rays;
    void Start()
    {
        rd = GetComponent<Renderer>();
        //rays= new Ray2D(transform.position, transform.forward);
    }


    void Update()
    {


        if (rd.isVisible == false )
        {
            if (indicator.activeSelf == false)
            {
                indicator.SetActive(true);
                Debug.Log("testarIndiCatorUPPE");
            }
            Vector2 direction = Target.transform.position - transform.position;

            RaycastHit2D ray = Physics2D.Raycast(transform.position, direction,(direction.x+100+direction.y+100),1<<11);
            if (ray.collider!= null)
            {
                indicator.transform.position = ray.point;
                Debug.Log("testar RAY");
            }

            //if (ray.collider.gameObject.tag=="Cambox")
            //{
            //    indicator.transform.position = ray.point;
            //    Debug.Log("testarIndiCatorNERE");
            //}
            //Debug.Log("Ray Pos :" + ray.point);
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