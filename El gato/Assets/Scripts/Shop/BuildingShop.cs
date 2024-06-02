using System.Collections;
using TMPro;
using UnityEngine;

public class BuildingShop : MonoBehaviour
{
    #region Variables
    [SerializeField]
    BuildState buildState;

    [SerializeField]
    GameObject potatoT, cornT, eggT;

    public bool sellModeSwitch = false;

    public Transform towerHidePlace;

    public GameObject towershowP, towershowC, towershowE;

    public bool showP, showC, showE;

    public GameObject showObject;

    [SerializeField]
    GameObject sellModeText;

    [SerializeField]
    GameObject moneyWarning;

    public int money;

    [SerializeField]
    TMP_Text moneyAmount;
    #endregion

    /// <summary>
    /// Enum for states of shop building
    /// </summary>
    public enum TowerIndicators
    {
        Tower1,
        Tower2,
        Tower3
    }

    public TowerIndicators TowerToShow;

    //Start money setter
    private void Start()
    {
        money = 300;
    }

    private void Update()
    {
        moneyAmount.text = money.ToString();
    }

    /// <summary>
    /// Swtich case for GameObject assignment for tower placement
    /// </summary>
    void TowerTypeIndicator()
    {
        switch (TowerToShow)
        {
            case TowerIndicators.Tower1:
                Debug.Log("Tower1");

                buildState.CurrentTowerToPlace = potatoT;

                break;
            case TowerIndicators.Tower2:
                Debug.Log("Tower2");

                buildState.CurrentTowerToPlace = eggT;

                break;
            case TowerIndicators.Tower3:
                Debug.Log("Tower3");

                buildState.CurrentTowerToPlace = cornT;

                break;
        }
    }

    /// <summary>
    /// assigns the "Potato Tower" for placement
    /// </summary>
    public void TowerButton1()
    {
        if (!sellModeSwitch)
        {
            if (money >= 200)
            {
                if (!showP)
                {
                    if (showObject != null)
                    {
                        Destroy(showObject);
                    }
                    TowerToShow = TowerIndicators.Tower1;
                    showP = true;
                    showObject = Instantiate(towershowP, buildState.placementLocation, Quaternion.identity);

                    TowerTypeIndicator();
                }
                else
                {
                    if (showObject != null)
                    {
                        Destroy(showObject);
                    }

                    buildState.CurrentTowerToPlace = null;

                    showP = false;

                }

            }
            else
            {
                StartCoroutine(NoMannee(2));
            }
        }
    }

    /// <summary>
    /// assigns the "Egg Tower" for placement
    /// </summary>
    public void TowerButton2()
    {
        if (!sellModeSwitch)
        {
            if (money >= 350)
            {
                if (!showE)
                {
                    if (showObject != null)
                    {
                        Destroy(showObject);
                    }
                    TowerToShow = TowerIndicators.Tower2;
                    showE = true;
                    showObject = Instantiate(towershowE, buildState.placementLocation, Quaternion.identity);

                    TowerTypeIndicator();
                }
                else
                {
                    if (showObject != null)
                    {
                        Destroy(showObject);
                    }

                    buildState.CurrentTowerToPlace = null;

                    showE = false;

                }

            }
            else
            {
                StartCoroutine(NoMannee(2));
            }
        }
    }

    /// <summary>
    /// assigns the "Corn Tower" for placement
    /// </summary>
    public void TowerButton3()
    {

        if (!sellModeSwitch)
        {
            if (money >= 500)
            {
                if (!showC)
                {
                    if (showObject != null)
                    {
                        Destroy(showObject);
                    }
                    TowerToShow = TowerIndicators.Tower3;
                    showC = true;
                    showObject = Instantiate(towershowC, buildState.placementLocation, Quaternion.identity);

                    TowerTypeIndicator();
                }
                else
                {
                    if (showObject != null)
                    {
                        Destroy(showObject);
                    }

                    buildState.CurrentTowerToPlace = null;

                    showC = false;
                }
            }

            else
            {
                StartCoroutine(NoMannee(2));
            }
        }
    }

    /// <summary>
    /// Function for the sell button. Sets current towers to null and sell mode to active/inactive
    /// </summary>
    public void TowerSellMode()
    {
        buildState.CurrentTowerToPlace = null;

        if (sellModeText.activeInHierarchy == true)
        {
            sellModeText.SetActive(false);
        }
        else
        {
            sellModeText.SetActive(true);
        }

        sellModeSwitch = !sellModeSwitch;
    }

    IEnumerator NoMannee(float waitTime)
    {
        moneyWarning.SetActive(true);
        yield return new WaitForSeconds(waitTime);
        moneyWarning.SetActive(false);
    }
}
