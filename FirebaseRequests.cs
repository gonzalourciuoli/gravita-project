using System.Linq;
using System;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine.UI;
using TMPro;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class FirebaseRequests
{
    public static FirebaseRequests _instance;
    private static readonly object _lock = new object();
    public DatabaseReference firebaseReference;
    private FirebaseRequests()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            FirebaseApp.Create();
            firebaseReference = FirebaseDatabase.DefaultInstance.RootReference;
        });
    }

    // Definimos el método Instance, ya que queremos que la base de datos sea un Singleton (solo una)
    public static FirebaseRequests Instance
    {
        get
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new FirebaseRequests();
                    }
                }
            }
            return _instance;
        }
    }
}