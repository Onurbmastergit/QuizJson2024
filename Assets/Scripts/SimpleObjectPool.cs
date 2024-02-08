using UnityEngine;
using System.Collections.Generic;

// Uma classe de pool de objetos muito simples
public class SimpleObjectPool : MonoBehaviour
{
    // O prefab que este pool de objetos retorna instâncias
    public GameObject prefab;
    // Coleção de instâncias atualmente inativas do prefab
    private Stack<GameObject> inactiveInstances = new Stack<GameObject>();

    // Retorna uma instância do prefab
    public GameObject GetObject()
    {
        GameObject spawnedGameObject;

        // Se houver uma instância inativa do prefab pronta para ser retornada, retorna essa
        if (inactiveInstances.Count > 0)
        {
            // Remove a instância da coleção de instâncias inativas
            spawnedGameObject = inactiveInstances.Pop();
        }
        // Caso contrário, cria uma nova instância
        else
        {
            spawnedGameObject = (GameObject)GameObject.Instantiate(prefab);

            // Adiciona o componente PooledObject ao prefab para saber que veio deste pool
            PooledObject pooledObject = spawnedGameObject.AddComponent<PooledObject>();
            pooledObject.pool = this;
        }

        // Ativa a instância
        spawnedGameObject.SetActive(true);

        // Retorna uma referência à instância
        return spawnedGameObject;
    }

    // Retorna uma instância do prefab para o pool
    public void ReturnObject(GameObject toReturn)
    {
        PooledObject pooledObject = toReturn.GetComponent<PooledObject>();

        // Se a instância veio deste pool, retorna-a para o pool
        if (pooledObject != null && pooledObject.pool == this)
        {
            // Desativa a instância
            toReturn.SetActive(false);

            // Adiciona a instância à coleção de instâncias inativas
            inactiveInstances.Push(toReturn);
        }
        // Caso contrário, apenas a destrói
        else
        {
            Debug.LogWarning(toReturn.name + " foi retornada para um pool de onde não foi instanciada! Destruindo.");
            Destroy(toReturn);
        }
    }
}

// Um componente que simplesmente identifica o pool de onde um GameObject veio
public class PooledObject : MonoBehaviour
{
    public SimpleObjectPool pool;
}