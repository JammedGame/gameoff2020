using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyGameItem : MonoBehaviour
{
    public UnityEngine.UI.Text gameNameTextField;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetGameName(string gameName)
    {
        gameNameTextField.text = gameName;
    }
}
