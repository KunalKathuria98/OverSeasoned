﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customer : MonoBehaviour
{
    //public GameObject dish;
    public float totaltime;
    public bool completed;
    public static Customer instance;
    public string food;
    public int spiceLevel;
    public float timeleft;

    public GameObject DishStation;
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        completed = false;
        spiceLevel = Random.Range(0, 17);
        timeleft = totaltime;
    }

    // Update is called once per frame
    void Update()
    {
        int sec = Mathf.FloorToInt(timeleft % 60);
        this.transform.parent.GetComponent<Table>().OrderText.text =
            $"Customer Order:\n {food} w\\ Spice Level: {spiceLevel}\n{sec:D2}";
        timeleft -= Time.deltaTime;
        if (timeleft < 0)
        {
            //if(completed) RespawnDish();
            RespawnDish();

            CustomerLeft();
            timeleft = totaltime;
        }
        //CheckDish();

    }

    void CustomerLeft()
    {
        if (transform.parent.GetComponent<Table>().dish == null)
        {
            transform.parent.GetComponent<Table>().OnScoreEvent.Invoke(-200);
        }
        this.transform.parent.GetComponent<Table>().OrderText.text = "No Customer";
        SpawnCustomer.instance.numberOfCustomers -= 1;
        Destroy(this.gameObject);
    }

    void RespawnDish()
    {
        Destroy(this.transform.parent.gameObject.GetComponent<Table>().dish);
        GameObject.Find("DishStation").GetComponent<DishStation>().spawnDish(1f);
    }
//    void CheckDish()
//    {
//        Debug.DrawRay(transform.position, direction, Color.green);
//
//        if (Physics.SphereCast(sphereCastPos, radius, direction, out RaycastHit hitInfo, 3f))
//        {
//            if(hitInfo.collider.CompareTag("Dish"))
//            {   
//                print("success");
//                hitInfo.collider.gameObject.GetComponent<Dish>().checkCurry();
//                completed = hitInfo.collider.gameObject.GetComponent<Dish>().hasAll;
//                hitInfo.collider.gameObject.GetComponent<Dish>().clear();
//            }
//        }
//    }
}
