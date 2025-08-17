using UnityEngine;

namespace ConjurerDice {
    public static class SaveService {
        public static void Save<T>(string key, T data) { /* ES3.Save(key, data); */ }
        public static T Load<T>(string key, T def = default) { /* return ES3.Load<T>(key, def); */ return def; }
    }
}
