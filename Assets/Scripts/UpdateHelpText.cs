using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpdateHelpText : MonoBehaviour
{
    [SerializeField] private TMP_Text helpText;

    public void UpdateText(string newText)
    {
        helpText.text = newText;
    }

    public void ClearText()
    {
        helpText.text = string.Empty;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
