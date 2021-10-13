using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuInputHandler : MonoBehaviour
{
    public GameObject[] menuItems;
    public bool[] menuItemSelectedStatus;
    public bool horizontal;
    bool startingFromFirstMenuItem, startingFromLastMenuItem;
    bool positiveInput, positiveMovement, negativeInput, negativeMovement;
    float horizontalInput, verticalInput;

    void Start()
    {
        startingFromFirstMenuItem = true;
        startingFromLastMenuItem = true;
        menuItemSelectedStatus = new bool[menuItems.Length];
    }

    void Update()
    {
        CheckControllerInput();
        UpdateUI();
    }

    public void CheckControllerInput()
    {
        if (horizontal)
        {
            horizontalInput = Input.GetAxisRaw("Horizontal Xbox");
            positiveMovement = Input.GetKeyDown(KeyCode.RightArrow);
            negativeMovement = Input.GetKeyDown(KeyCode.LeftArrow);
        }
        else
        {
            verticalInput = Input.GetAxisRaw("Vertical Xbox");
            positiveMovement = Input.GetKeyDown(KeyCode.DownArrow);
            negativeMovement = Input.GetKeyDown(KeyCode.UpArrow);
        }

        if (positiveMovement || ((horizontalInput > 0 || verticalInput < 0) && !positiveInput))
        {
            if (startingFromFirstMenuItem)
            {
                startingFromLastMenuItem = false;
                StartCoroutine(Axis("positive"));
                startingFromFirstMenuItem = false;
                menuItemSelectedStatus[0] = true;
            }
            else
            {
                for (int i = 0; i < menuItems.Length; i++)
                {
                    if (menuItemSelectedStatus[i])
                    {
                        StartCoroutine(Axis("positive"));
                        menuItemSelectedStatus[i] = false;

                        if (i < menuItems.Length - 1)
                            menuItemSelectedStatus[i + 1] = true;
                        else
                            menuItemSelectedStatus[0] = true;

                        break;
                    }
                }
            }
        }

        if (negativeMovement || ((horizontalInput < 0 || verticalInput > 0) && !negativeInput))
        {
            if (startingFromLastMenuItem)
            {
                startingFromFirstMenuItem = false;
                StartCoroutine(Axis("negative"));
                startingFromLastMenuItem = false;
                menuItemSelectedStatus[menuItems.Length - 1] = true;
            }
            else
            {
                for (int i = menuItems.Length - 1; i >= 0; i--)
                {
                    if (menuItemSelectedStatus[i])
                    {
                        StartCoroutine(Axis("negative"));
                        menuItemSelectedStatus[i] = false;

                        if (i > 0)
                            menuItemSelectedStatus[i - 1] = true;
                        else
                            menuItemSelectedStatus[menuItemSelectedStatus.Length - 1] = true;

                        break;
                    }
                }
            }
        }
    }

    public void UpdateUI()
    {
        for (int i = 0; i < menuItemSelectedStatus.Length; i++)
            if (menuItemSelectedStatus[i])
                menuItems[i].GetComponent<Button>().Select();
    }

    public IEnumerator Axis(string direction)
    {
        if (direction == "positive")
        {
            positiveInput = true;
            yield return new WaitForSeconds(0.2f);
            positiveInput = false;
        }
        if (direction == "negative")
        {
            negativeInput = true;
            yield return new WaitForSeconds(0.2f);
            negativeInput = false;
        }
    }
}

