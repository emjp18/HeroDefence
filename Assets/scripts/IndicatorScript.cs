using UnityEngine;

public class IndicatorScript : MonoBehaviour
{

    public GameObject indicator;
    public GameObject Target;

    Renderer rd;

    void Start()
    {
        rd = GetComponent<Renderer>();
    }


    void Update()
    {
        if (rd.isVisible == false)
        {
            if (indicator.activeSelf == false)
            {
                indicator.SetActive(true);
                Debug.Log("testarIndiCatorUPPE");
            }
            Vector2 direction = Target.transform.position - transform.position;

            RaycastHit2D ray = Physics2D.Raycast(transform.position, direction);

            if (ray.collider.gameObject.tag=="Cambox")
            {
                indicator.transform.position = ray.point;
                Debug.Log("testarIndiCatorNERE");
            }
            else
            {
                if (indicator.activeSelf == true)
                {
                    indicator.SetActive(false);
                    Debug.Log("testarIndiCatorNERE V2");
                }
            }
        }
    }
}