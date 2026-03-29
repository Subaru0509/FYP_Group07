using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatToolTip_UI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI statDescription;

    public void ShowStatToolTip(string _statDescription)
    {
        statDescription.text = _statDescription;

        gameObject.SetActive(true);
    }

    public void HideStatToolTip()
    {
        statDescription.text = "";
        gameObject.SetActive(false);
    }
}
