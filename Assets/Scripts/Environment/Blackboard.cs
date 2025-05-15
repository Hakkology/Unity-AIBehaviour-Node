using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Blackboard : MonoBehaviour
{
    public float timeOfDay;
    public TextMeshProUGUI clock;

    public Stack<GameObject> Patrons = new Stack<GameObject>();

    public int openTime = 5;
    public int closeTime = 23;

    private static Blackboard _instance;
    public static Blackboard Instance 
    {
        get
        {
            return _instance;
        }
    }

    private void Awake()
    {
        // Eğer başka bir instance varsa kendini yok et
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // Bu instance'ı Singleton olarak ayarla
        _instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        clock.text = "00:00";
        StartCoroutine(nameof(UpdateClock));
    }

    IEnumerator UpdateClock(){
        while (true)
        {
            yield return new WaitForSeconds(1);
            timeOfDay = (timeOfDay + 1) % 24;
            if (timeOfDay == closeTime)
            {
                Patrons.Clear();
            }
            clock.text = $"{timeOfDay:00}:00";
        }
    }

    public bool RegisterPatron(GameObject patron){
        Patrons.Push(patron);
        return true;
    }

    public void DeregisterPatron(){
        //this.patron = null;
    }
}
