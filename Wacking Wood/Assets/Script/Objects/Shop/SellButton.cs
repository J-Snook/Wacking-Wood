using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SellButton : MonoBehaviour, IInteractSystem
{
    [SerializeField] private LogStorage _logStorage;
    [SerializeField] private float sellPrice;
    private string text
    {
        get
        {
            if (_logStorage.storedLogs.Count > 0)
            {
                return "Press F to Sell "+ _logStorage.storedLogs.Count + " Logs";
            }
            return string.Empty;
        }
    }

    public string promptText => text;

    public void Interact(InteractionSystem player)
    {
        if (_logStorage != null && _logStorage.storedLogs.Count > 0)
        {
            PlayerAttributes playerAttributes = player.GetComponent<PlayerAttributes>();
            for(int i = _logStorage.storedLogs.Count-1; i>=0; i--)
            {
                Destroy(_logStorage.storedLogs[i].gameObject);
                playerAttributes.UpdateCash(10f);
                _logStorage.storedLogs.RemoveAt(i);
            }
        }
    }

    private float TotalVolumeCalculate()
    {
        float totalVolume = 0f;
        foreach (LogPickup log in _logStorage.storedLogs)
        {
            totalVolume += (log.transform.localScale.x*log.transform.localScale.z*Mathf.PI) * log.transform.localScale.y;
        }
        return totalVolume;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
