using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocationLogger : MonoBehaviour
{
    public Text latitudeText;
    public Text longitudeText;
    public Text altitudeText;

    private void Start()
    {
        // Check if the device supports location services
        if (!Input.location.isEnabledByUser)
        {
            Debug.LogError("Location services are disabled on this device.");
            UpdateText("N/A", "N/A", "N/A");
            return;
        }

        // Start location services
        StartCoroutine(StartLocationServices());
    }

    private IEnumerator StartLocationServices()
    {
        Input.location.Start();

        // Wait until the location service initializes
        int maxWait = 20; // 20 seconds timeout
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        if (maxWait <= 0 || Input.location.status == LocationServiceStatus.Failed)
        {
            Debug.LogError("Unable to initialize location services.");
            UpdateText("Error", "Error", "Error");
            yield break;
        }

        // Successfully initialized
        Debug.Log("Location services started.");
        UpdateText(Input.location.lastData.latitude.ToString(),
                   Input.location.lastData.longitude.ToString(),
                   Input.location.lastData.altitude.ToString());
    }

    public void LogCoordinates()
    {
        if (Input.location.status == LocationServiceStatus.Running)
        {
            var data = Input.location.lastData;
            UpdateText(data.latitude.ToString(), data.longitude.ToString(), data.altitude.ToString());
            Debug.Log($"Latitude: {data.latitude}, Longitude: {data.longitude}, Altitude: {data.altitude}");
        }
        else
        {
            Debug.LogWarning("Location services are not running.");
        }
    }

    private void UpdateText(string latitude, string longitude, string altitude)
    {
        latitudeText.text = $"Latitude: {latitude}";
        longitudeText.text = $"Longitude: {longitude}";
        altitudeText.text = $"Altitude: {altitude}";
    }

    private void OnDestroy()
    {
        Input.location.Stop();
    }
}