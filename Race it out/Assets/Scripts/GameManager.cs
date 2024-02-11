using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject[] VehicleList;
    [SerializeField] GameObject carSpawnPos;
    [SerializeField] LapComplete lapComplete;
    int vehicleNo;
    private void Awake()
    {
        vehicleNo = PlayerPrefs.GetInt("VehilcleNo");
    }

    private void Start()
    {
        if(carSpawnPos != null)
        {
            GameObject car = Instantiate(VehicleList[vehicleNo], carSpawnPos.transform.position,carSpawnPos.transform.rotation,carSpawnPos.transform);
        }
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void Retry()
    {
        Debug.Log(SceneManager.GetActiveScene().buildIndex);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadCarSelect()
    {
        SceneManager.LoadScene("Car Select");
    }
    
    public void LoadTrackSelect()
    {
        SceneManager.LoadScene("Track Select");
    }


    public void LoadTrack(string trackName)
    {
        SceneManager.LoadScene(trackName);
    }

    public void SetVehicleNo(int no)
    {
        PlayerPrefs.SetInt("VehilcleNo", no);
        LoadTrackSelect();
    }

    public void Quit()
    {
        Application.Quit();
    }
}
