using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CooldownsAbility : MonoBehaviour
{
    /// <summary>
    /// Shows icon for the class abilitys and their cooldowns. Activates a black screen over the icons during cooldown.
    /// </summary>
    [SerializeField]List<GameObject> CDlist = new List<GameObject>();
    [SerializeField]ArcherWeapon archerScript;
    [SerializeField] PlayerMovement archerMoveScript;
    [SerializeField] PlayerMovement warriorMoveScript;
    [SerializeField] PlayerCombat warriorCombat;



    void Start()
    {

    }
    void Update()
    {
        // warrior cds
        if (CDlist[7].gameObject.activeSelf)
        {
            archerMoveScript.gameObject.SetActive(false);
            if (warriorCombat.shieldCD > 0)
            {
                CDlist[4].SetActive(true);
            }
            else
            {
                CDlist[4].SetActive(false);
            }
            if (warriorCombat.knockbackCD > 0)
            {
                CDlist[3].SetActive(true);
            }
            else
            {
                CDlist[3].SetActive(false);
            }
            if (warriorMoveScript.canDash == false)
            {
                CDlist[5].SetActive(true);
            }
            else
            {
                CDlist[5].SetActive(false);
            }
        }
        else
        {
            CDlist[11].SetActive(false);
            CDlist[12].SetActive(false);
            CDlist[13].SetActive(false);
            CDlist[5].SetActive(false);
            CDlist[3].SetActive(false);
            CDlist[4].SetActive(false);
        }
       

        // ArcherCDS
        if (CDlist[6].gameObject.activeSelf==true)
        {
            warriorMoveScript.gameObject.SetActive(false);
            if (archerScript.timerMultiShootingSkillCoolDown > 0)
            {
                CDlist[1].SetActive(true);

            }
            else
            {
    
                CDlist[1].SetActive(false);
            }
            if (archerScript.timerUnlimitedShootingCoolDown > 0)
            {
                CDlist[0].SetActive(true);
            }
            else
            {
                CDlist[0].SetActive(false);
            }
            if (archerMoveScript.canDash == false)
            {
                CDlist[2].SetActive(true);
            }
            else
            {
                CDlist[2].SetActive(false);
            }
        }
        else
        {
            CDlist[8].SetActive(false);
            CDlist[9].SetActive(false);
            CDlist[10].SetActive(false);
            CDlist[2].SetActive(false);
            CDlist[0].SetActive(false);
            CDlist[1].SetActive(false);
        }
    }
}
