using System.Collections.Generic;
using Unity.Services.Analytics;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.Analytics;

public class AnalyticsManager : MonoBehaviour
{
    
    async void Start() //Initialize Unity Services
    {
        try
        {
            await UnityServices.InitializeAsync();
        }
        catch (ConsentCheckException e)
        {
            Debug.Log(e.ToString());
        }
    }
    
    public void ThrowGameEndEvent(int totalHits)
    {
        
        //Define Custom Parameters
        //"levelName"  is the name of the custom parameter
        Dictionary<string, object> parameters = new Dictionary<string, object> {
            {
                "totalHits",  totalHits
            }
        };

        // The ‘levelCompleted’ event will get cached locally 
        //"levelCompleted" is the name of the custom event
        //and sent during the next scheduled upload, within 1 minute
        AnalyticsService.Instance.CustomData("gameLose", parameters);

        // You can call Events.Flush() to send the event immediately
        AnalyticsService.Instance.Flush();
    }
    
}
