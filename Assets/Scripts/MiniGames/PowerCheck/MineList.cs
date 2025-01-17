using System.Collections.Generic;
using UnityEngine;

public class MineList
{
    // ����� ������ ���
    public int Length { get; private set; }
    // ������ ���
    public List<Mine> Minelist { get; private set; }

    public MineList(int length)
    {
        Length = length;
        Minelist = new List<Mine>();
    }

    // ������������� ������ ��� ���� � ������
    public void InitializeMines(GameObject prefab, float cooldown, System.Func<uint, float, GameObject, Mine> createMine)
    {
        for (int i = 0; i < Length; i++)
        {
            uint number = (uint)i;
            GameObject mineGameObject = Object.Instantiate(prefab);
            mineGameObject.SetActive(false);
            Mine newMine = createMine(number, cooldown, mineGameObject);
            Minelist.Add(newMine);
        }
    }

    // ������������� ������ ��� � ����� � ��������
    public void InitializeSpeedBuffMines(GameObject prefab, float cooldown, float speedbuff, float buffcooldown, int timebeforeexplosion, float radius)
    {
        for (int i = 0; i < Length; i++)
        {
            uint number = (uint)i;
            GameObject mineGameObject = Object.Instantiate(prefab);
            mineGameObject.SetActive(false);
            Mine newMine = new BuffSpeedMine(number, cooldown, mineGameObject, speedbuff, buffcooldown, timebeforeexplosion, radius);
            Minelist.Add(newMine);
        }
    }

    public Mine AddMine(GameObject prefab, float cooldown, System.Func<uint, float, GameObject, Mine> createMine)
    {
        // ������� ���������� ����� ��� ����� ����
        uint number = (uint)Minelist.Count;

        // ������� ����� ������� ������ ��� ����
        GameObject mineGameObject = Object.Instantiate(prefab);
        mineGameObject.SetActive(false);

        // ������� ������ ���� � ������� ���������� �������
        Mine newMine = createMine(number, cooldown, mineGameObject);

        // �������� ����� ���� � ������
        Minelist.Add(newMine);

        // ��������� ����� ������
        Length++;
        return Minelist[Minelist.Count - 1];
    }

    public Mine AddMine(GameObject prefab, float cooldown, float speedbuff, float buffcooldown, int timebeforeexplosion, float radius, System.Func<uint, float, GameObject, float, float, int, float, Mine> createMine)
    {
        // ������� ���������� ����� ��� ����� ����
        uint number = (uint)Minelist.Count;

        // ������� ����� ������� ������ ��� ����
        GameObject mineGameObject = Object.Instantiate(prefab);
        mineGameObject.SetActive(false);

        // ������� ������ ���� � ������� ���������� �������
        Mine newMine = createMine(number, cooldown, mineGameObject, speedbuff, buffcooldown, timebeforeexplosion, radius);

        // �������� ����� ���� � ������
        Minelist.Add(newMine);

        // ��������� ����� ������
        Length++;
        return Minelist[Minelist.Count - 1];
    }
}
