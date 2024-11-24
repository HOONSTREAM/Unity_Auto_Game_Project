using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public interface IPool
{
    Transform parentTransform { get; set; }
    Queue<GameObject> pool { get; set; }
    GameObject Get(Action<GameObject> action = null);

    void Return(GameObject obj, Action<GameObject> action = null);

}


public class Object_Pool : IPool
{
    public Queue<GameObject> pool { get; set; } = new Queue<GameObject>();

    public Transform parentTransform { get; set; }

    /// <summary>
    /// ���� ������Ʈ Ǯ����ü�� queue���� ������, ������Ʈ�� Ȱ��ȭ ��ŵ�ϴ�.
    /// �׸���, Ư�� �׼�(���)�� �����Ǿ� �ִٸ� �����ŵ�ϴ�.
    /// </summary>
    /// <param name="action"></param>
    /// <returns></returns>
    public GameObject Get(Action<GameObject> action = null)
    {
        GameObject obj = pool.Dequeue();

        obj.gameObject.SetActive(true);

        if(action != null)
        {
            action?.Invoke(obj); // Get �޼��忡�� action.Invoke(obj)�� ȣ��Ǹ�, obj�� Ǯ���� ���� ������Ʈ�Դϴ�.
        }

        return obj;
    }

    /// <summary>
    /// ���� ������Ʈ Ǯ����ü�� queue�� �ְ�, ������Ʈ�� ��Ȱ��ȭ ��ŵ�ϴ�.
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="action"></param>
    public void Return(GameObject obj, Action<GameObject> action = null)
    {
        pool.Enqueue(obj);
        obj.transform.parent = parentTransform;
        obj.gameObject.SetActive(false);

        if(action != null) 
        {
            action?.Invoke(obj);
        }
        
    }

}
public class Pool_Manager 
{
    // IPool �������̽��� value�� ��ȯ�ϴ� ��ųʸ��� new�� �����մϴ�.
    public Dictionary<string, IPool> m_pool_Dictionary = new Dictionary<string, IPool>();

    /// <summary>
    /// ���̽� �Ŵ��� ������Ʈ�� Ʈ�������̸�, ��� �Ŵ����� ���̽� �Ŵ��� ������Ʈ ���Ͽ� ��ġ ��ŵ�ϴ�.
    /// </summary>
    private Transform base_manger_obj = null;

    /// <summary>
    /// Base_Manager ������Ʈ ���Ͽ� ��ġ�Ҽ� �ֵ��� �մϴ�. (���̾��Ű ����)
    /// </summary>
    /// <param name="T"></param>
    public void Initialize(Transform T)
    {
        base_manger_obj = T;
    }
    
    public IPool Pooling_OBJ (string path)
    {
        // ��ųʸ� Ű�� �˻��ؼ� path Ű�� ������, Pool ������Ʈ�� �߰���ŵ�ϴ�.
        if (m_pool_Dictionary.ContainsKey(path) == false)
        {
            Add_Pool(path);
        }

        //��ųʸ� Ű�� ����������, Queue�� ī��Ʈ�� 0�̸�, ���ο� ������Ʈ�� ���� Queue�� �߰��մϴ�.
        if (m_pool_Dictionary[path].pool.Count <= 0)
        {
            Add_Queue(path);
        }

        return m_pool_Dictionary[path]; // IPool �������̽��� ��ȯ
    }

    /// <summary>
    /// ���ο� Pool�� �����մϴ�.
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    private GameObject Add_Pool(string path)
    {
        GameObject obj = new GameObject(path + "@POOL");
        obj.transform.SetParent(base_manger_obj);
        Object_Pool T_Component = new Object_Pool();

        m_pool_Dictionary.Add(path, T_Component);

        T_Component.parentTransform = obj.transform;

        return obj;
    }

    /// <summary>
    /// ���ο� ������Ʈ�� �����ϰ�, �����Ͽ�, Queue�� ����ֽ��ϴ�.
    /// </summary>
    /// <param name="path"></param>
    private void Add_Queue(string path)
    {
        var go = Base_Manager.instance.Instantiate_Path(path);
        go.transform.parent = m_pool_Dictionary[path].parentTransform;

        m_pool_Dictionary[path].Return(go);
    }

    public void Clear_Pool()
    {
        foreach (var poolEntry in m_pool_Dictionary)
        {
            
            var pool = poolEntry.Value;

            while (pool.pool.Count > 0)
            {
                var obj = pool.pool.Dequeue();
                UnityEngine.Object.Destroy(obj);
            }

            if (pool.parentTransform != null)
            {
                UnityEngine.Object.Destroy(pool.parentTransform.gameObject);
            }
        }

        m_pool_Dictionary.Clear();
        Debug.Log("All pools cleared and reset.");
    }

}
