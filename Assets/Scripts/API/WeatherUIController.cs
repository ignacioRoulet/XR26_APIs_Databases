using UnityEngine;
using UnityEngine.UI;
using TMPro;
using WeatherApp.Services;
using WeatherApp.Data;

namespace WeatherApp.UI
{
    /// <summary>
    /// UI Controller for the Weather Application
    /// Students will connect this to the API client and handle user interactions
    /// </summary>
    public class WeatherUIController : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private TMP_InputField cityInputField;
        [SerializeField] private Button getWeatherButton;
        [SerializeField] private TextMeshProUGUI weatherDisplayText;
        [SerializeField] private TextMeshProUGUI statusText;
        
        [Header("API Client")]
        [SerializeField] private WeatherApiClient apiClient;
        
        private void Start()
        {
            Debug.Log("WeatherUIController started");
            // Set up button click listener
            getWeatherButton.onClick.AddListener(OnGetWeatherClicked);

            // Initialize UI state
            SetStatusText("Enter a city name and click Get Weather");

        }
        
        /// TODO: Students will implement this method
        private async void OnGetWeatherClicked()
        {
            Debug.Log("Get Weather button clicked");
            // Get city name from input field
            string cityName = cityInputField.text;
            
            // Validate input
            if (string.IsNullOrWhiteSpace(cityName))
            {
                SetStatusText("Please enter a city name");
                return;
            }
            
            // Disable button and show loading state
            getWeatherButton.interactable = false;
            SetStatusText("Loading weather data...");
            weatherDisplayText.text = "";
            
            try
            {
                // DONE: Call API client to get weather data
                WeatherData weatherData = await apiClient.GetWeatherDataAsync(cityName);
                
                // DONE: Handle the response
                if (weatherData != null && weatherData.IsValid)
                {
                    DisplayWeatherData(weatherData);
                    SetStatusText("Weather data loaded successfully");
                }
                else
                {
                    SetStatusText("Can't retrieve weather data");
                }
                
                
            }
            catch (System.Exception ex)
            {
                // Handle exceptions
                Debug.LogError($"Error getting weather data: {ex.Message}");
                SetStatusText("An error occurred. Please try again.");
            }
            finally
            {
                // Re-enable button
                getWeatherButton.interactable = true;
            }
        }
        
        /// TODO: Students will implement this method
        private void DisplayWeatherData(WeatherData weatherData)
        {
            // TODO: Format and display weather information
            // Example format:
            // City: London
            // Temperature: 15.2°C (Feels like: 14.1°C)
            // Description: Clear sky
            // Humidity: 65%
            // Pressure: 1013 hPa

            string displayText = "";
            
            // DONE: Add more weather details
            if (weatherData.Main != null)
            {
                displayText += $"The temperature is {weatherData.Main.Temperature} kelvin\n";
                displayText += $"It feels like the weather is {weatherData.Main.FeelsLike} kelvin\n";
                displayText += $"Humidity is {weatherData.Main.Humidity} %\n";
                displayText += $"Presure is {weatherData.Main.Pressure} hPa";

            }
            
            weatherDisplayText.text = displayText;
        }
        
        private void SetStatusText(string message)
        {
            if (statusText != null)
            {
                statusText.text = message;
            }
        }
        
        public void ClearDisplay()
        {
            weatherDisplayText.text = "";
            cityInputField.text = "";
            SetStatusText("Enter a city name and click Get Weather");
        }
    }
}